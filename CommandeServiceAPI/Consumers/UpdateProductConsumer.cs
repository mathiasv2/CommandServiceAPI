using System;
using CommandeServiceAPI.Service;
using MassTransit;

namespace CommandeServiceAPI.Consumers
{
	public class UpdateProductConsumer : IConsumer<UpdateProductConsumer>
	{
        private ProductService _productService { get; set; }

        public UpdateProductConsumer(ProductService productService)
        {
            _productService = productService;
        }

        public Task Consume(ConsumeContext<UpdateProductConsumer> context)
        {
            var product = context.Message;


        }
    }
}

