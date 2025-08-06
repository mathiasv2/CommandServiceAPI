using System;
using CommandeServiceAPI.Database;
using CommandeServiceAPI.DTO;
using CommandeServiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandeServiceAPI.Service
{
	public class CommandService
	{
        private CommandDbContext _dbContext { get; set; }
        private ProductService _productService { get; set; }

        public CommandService(CommandDbContext dbContext, ProductService productService)
        {
            _dbContext = dbContext;
            _productService = productService;
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

        public async Task<Command> GetCommandComplete(int commandId)
        {
            Command? commandWithDetails = await _dbContext.Commands
            .Where(c => c.Id == commandId) 
            .Include(c => c.Command_Products)
                .ThenInclude(cp => cp.ProductCache)
            .FirstOrDefaultAsync();

            return commandWithDetails;
        }

    }
}

