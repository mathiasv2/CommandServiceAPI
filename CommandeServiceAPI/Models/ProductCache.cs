using System;
namespace CommandeServiceAPI.Models
{
	public class ProductCache
	{
        public int Id { get; set; }
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public  int Quantity  { get; set; }
        public int ProductPriceAtOrder { get; set; }

        public ICollection<Command_Product> Command_Products { get; set; }
    }
}


