using System;
using System.Text.Json;
using CommandeServiceAPI.DTO;
using CommandeServiceAPI.Models;
using CommandeServiceAPI.Service;
using MassTransit;
using Shared.Messages;

namespace CommandeServiceAPI
{
	public class CreateProductConsumer : IConsumer<CreateProductDTO>
	{
        private ProductService _productService { get; set; }

        public CreateProductConsumer(ProductService productService)
        {
            _productService = productService;
        }

        public async Task Consume(ConsumeContext<CreateProductDTO> context)
        {
            Console.WriteLine("Consomme bien le message");
            var product = context.Message;

            ProductCacheDTO productToAdd = new ProductCacheDTO
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = product.Quantity,
                ProductPriceAtOrder = product.Price,
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

