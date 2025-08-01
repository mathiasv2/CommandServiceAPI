using System;
using System.Text.Json;
using CommandeServiceAPI.DTO;
using CommandeServiceAPI.Models;
using CommandeServiceAPI.Service;
using MassTransit;
using Shared.Messages;

namespace CommandeServiceAPI
{
	public class ProductConsumer : IConsumer<ProductDTO>
	{
        private ProductService _productService { get; set; }

        public ProductConsumer(ProductService productService)
        {
            _productService = productService;
        }

        public async Task Consume(ConsumeContext<ProductDTO> context)
        {
            Console.WriteLine("Consomme bien le message");
            var product = context.Message;

            ProductCacheDTO productToAdd = new ProductCacheDTO
            {

                ProductId = product.Id,
                ProductName = product.Name,
                ProductPriceAtOrder = product.Price
            };

            await _productService.CreateProductCache(productToAdd);
            
            var json = JsonSerializer.Serialize(product, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });
            Console.WriteLine($"\n Message reçu dans Commande : {json}\n");
            Console.ResetColor();
        }
    }
}

