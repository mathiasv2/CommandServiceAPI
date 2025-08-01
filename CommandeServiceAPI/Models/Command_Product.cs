using System;
namespace CommandeServiceAPI.Models
{
	public class Command_Product
	{
        public int CommandId { get; set; }
        public int ProductCacheId { get; set; }

        public int QuantityOrdered { get; set; }

        public Command Command { get; set; }
        public ProductCache ProductCache { get; set; }
    }
}

