using System;
using CommandeServiceAPI.Database;
using CommandeServiceAPI.DTO;
using CommandeServiceAPI.Models;
using Shared.Messages;
using Shared.Messages.ProductDTO;

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


        public async Task<ProductCache> GetProductCacheById(int productId)
        {
            ProductCache product = await _dbContext.Products.FindAsync(productId);
            return product;
        }

        public async Task RemoveProductCache(int productId)
        {
            var product = await GetProductCacheById(productId);
            _dbContext.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductCache(UpdateProductDTO productDTO)
        {
            ProductCache product = await GetProductCacheById(productDTO.Id);

            product.ProductName  = productDTO.Name;
            product.ProductPriceAtOrder = productDTO.Price;


            await _dbContext.SaveChangesAsync();

        }




    }
}

