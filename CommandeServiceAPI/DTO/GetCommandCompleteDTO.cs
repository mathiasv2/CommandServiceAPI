using System;
namespace CommandeServiceAPI.DTO
{
	public class GetCommandCompleteDTO
	{
		public DateTime OrderDate { get; set; }
		public List<GetProductDTO> Products { get; set; }
		public int TotalAmount => Products.Sum(x => x.TotalProductPrice);

	}
}

