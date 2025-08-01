using System;
using CommandeServiceAPI.Database;
using CommandeServiceAPI.DTO;
using Shared.Messages;

namespace CommandeServiceAPI.Service
{
	public class ProductService
	{
        private CommandDbContext _dbContext { get; set; }

        public ProductService(CommandDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateProductCache(ProductCacheDTO productDTO)
        {
            await _dbContext.Products.AddAsync(productDTO);
            await _dbContext.SaveChangesAsync();
        }



    }
}

