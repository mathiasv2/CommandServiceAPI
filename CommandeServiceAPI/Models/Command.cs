using System;
namespace CommandeServiceAPI.Models
{
	public class Command
	{
        public int Id { get; set; }
        public int Quantity { get; set; }

        public ICollection<Command_Product> Command_Products { get; set; }
    }
}

