using System;
using CommandeServiceAPI.Database;
using CommandeServiceAPI.DTO;

namespace CommandeServiceAPI.Service
{
	public class CommandService
	{
        private CommandDbContext _dbContext { get; set; }

        public CommandService(CommandDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateCommande(CommandDTO commandDTO)
        {
            await _dbContext.Commands.AddAsync(commandDTO);
            await _dbContext.SaveChangesAsync();
        }

    }
}

