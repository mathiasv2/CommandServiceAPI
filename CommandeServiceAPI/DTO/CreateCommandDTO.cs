using System;
using CommandeServiceAPI.Models;

namespace CommandeServiceAPI.DTO
{
	public class CreateCommandDTO
	{
		public List<CreateCommandProductDTO> ProductsOrdered { get; set; }
	}
}

