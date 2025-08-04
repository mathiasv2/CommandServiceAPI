using System;
using CommandeServiceAPI.Service;
using MassTransit;
using Shared.Messages.ProductDTO;

namespace CommandeServiceAPI.Consumers
{
	public class DeleteProductConsumer : IConsumer<DeleteProductDTO>
	{
        private ProductService _productService { get; set; }

        public async Task Consume(ConsumeContext<DeleteProductDTO> context)
        {
            var product = context.Message;

            await _productService.RemoveProductCache(product.Id);
        }
    }
}

