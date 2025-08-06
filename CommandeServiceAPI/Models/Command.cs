using System;
namespace CommandeServiceAPI.Models
{
	public class Command
	{
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        public ICollection<Command_Product> Command_Products { get; set; }
    }
}

