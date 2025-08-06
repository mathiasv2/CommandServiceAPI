using System;
namespace CommandeServiceAPI.DTO
{
	public class GetProductDTO
	{
        public string ProductName { get; set; }
        public int QuantityOrdered { get; set; }
        public int ProductPriceAtOrder { get; set; }
        public int TotalProductPrice => QuantityOrdered * ProductPriceAtOrder;
    }
}

