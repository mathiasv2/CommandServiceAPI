using System;
using CommandeServiceAPI.Service;
using MassTransit;
using Shared.Messages.ProductDTO;

namespace CommandeServiceAPI.Consumers
{
	public class UpdateProductConsumer : IConsumer<UpdateProductDTO>
	{
        private ProductService _productService { get; set; }

        public UpdateProductConsumer(ProductService productService)
        {
            _productService = productService;
        }

        public async Task Consume(ConsumeContext<UpdateProductDTO> context)
        {
            var product = context.Message;

            await _productService.UpdateProductCache(product);
        }
    }
}

