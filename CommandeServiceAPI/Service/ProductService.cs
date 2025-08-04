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


        public async Task<ProductCache> GetProductCacheByProductId(int productId)
        {
            ProductCache? product = _dbContext.Products.FirstOrDefault(x => x.ProductId == productId) ;
            return product;
        }

        public async Task RemoveProductCache(int productId)
        {
            var product = await GetProductCacheByProductId(productId);
            _dbContext.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductCache(UpdateProductDTO productDTO)
        {
            ProductCache product = await GetProductCacheByProductId(productDTO.Id);

            product.ProductName  = productDTO.Name;
            product.ProductPriceAtOrder = productDTO.Price;


            await _dbContext.SaveChangesAsync();

        }




    }
}

