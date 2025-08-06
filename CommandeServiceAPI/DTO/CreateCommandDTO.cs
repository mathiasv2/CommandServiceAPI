using System;
using CommandeServiceAPI.Models;

namespace CommandeServiceAPI.DTO
{
	public class CreateCommandDTO
	{
		public List<CommandProductDTO> ProductsOrdered { get; set; }
		public DateTime OrderDate { get; set; }
	}
}

