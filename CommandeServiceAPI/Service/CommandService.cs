using System;
using CommandeServiceAPI.Database;
using CommandeServiceAPI.DTO;
using CommandeServiceAPI.Models;

namespace CommandeServiceAPI.Service
{
	public class CommandService
	{
        private CommandDbContext _dbContext { get; set; }

        public CommandService(CommandDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateCommande(CreateCommandDTO commandDTO)
        {
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

    }
}

