using System;
using CommandeServiceAPI.Database;
using CommandeServiceAPI.DTO;
using CommandeServiceAPI.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Messages.ProductDTO;

namespace CommandeServiceAPI.Service
{
	public class CommandService
	{
        private CommandDbContext _dbContext { get; set; }
        private ProductService _productService { get; set; }
        private IPublishEndpoint _publishEndpoint { get; set; }

        public CommandService(CommandDbContext dbContext, ProductService productService, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _productService = productService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task CreateCommande(CreateCommandDTO commandDTO)
        {
            if (!IsListProductsQuantitySuperiorToStock(commandDTO))
                throw new Exception("Pas la quantité nécessaire en base de données");

            Command command = new Command
            {
                OrderDate = DateTime.Today
            };

            await _dbContext.Commands.AddAsync(command);
            await _dbContext.SaveChangesAsync();

            List<Command_Product> command_Product = commandDTO.ProductsOrdered.Select(x => new Command_Product
            {
                CommandId = command.Id,
                ProductCacheId = x.ProductId,
                QuantityOrdered = x.QuantityOrdered
            }).ToList();

            foreach (var item in commandDTO.ProductsOrdered)
            {
                await _productService.ReduceQuantityInDb(item.ProductId, item.QuantityOrdered);
            };

            await _dbContext.Command_Products.AddRangeAsync(command_Product);
            await _dbContext.SaveChangesAsync();

            foreach (var item in command_Product)
            {
                UpdateProductQuantityDTO updateProduct = new UpdateProductQuantityDTO
                {
                    ProductId = item.ProductCacheId,
                    Quantity = item.QuantityOrdered
                };

                await _publishEndpoint.Publish(updateProduct);
            }
        }

        public bool IsQuantityCommandedSuperiorToStock(int productId, int quantity)
        {
            ProductCache? product = GetProductById(productId);
            return product.Quantity >= quantity;
        }

        public bool IsListProductsQuantitySuperiorToStock(CreateCommandDTO commandDTO)
        {
            foreach (var item in commandDTO.ProductsOrdered)
            {
                if (!IsQuantityCommandedSuperiorToStock(item.ProductId, item.QuantityOrdered))
                    return false;
            }

            return true;
        }

        public ProductCache GetProductById(int productId)
        {
            ProductCache? product = _dbContext.Products.FirstOrDefault(x => x.ProductId == productId);
            if (product == null)
                throw new Exception("Product n'existe pas en base de données");
            return product;
        }

        public async Task<Command?> GetCommand(int commandId)
        {
            Command? command = await _dbContext.Commands
            .Where(c => c.Id == commandId) 
            .Include(c => c.Command_Products)
                .ThenInclude(cp => cp.ProductCache)
            .FirstOrDefaultAsync();

            if (command == null)
                throw new Exception("Commande n'existe pas");

            return command;
        }

        public async Task<GetCommandCompleteDTO?> GetCompleteCommand(int commandId)
        {
            Command? command = await GetCommand(commandId);
            if (command == null)
                return null;

            GetCommandCompleteDTO completeCommand = new GetCommandCompleteDTO
            {

                OrderDate = command.OrderDate,
                Products = command.Command_Products.Select(x => new DTO.GetProductDTO
                {
                    ProductName = x.ProductCache.ProductName,
                    ProductPriceAtOrder = x.ProductCache.ProductPriceAtOrder,
                    QuantityOrdered = x.QuantityOrdered,
                }).ToList()
            };

            return completeCommand;
        }

    }
}

