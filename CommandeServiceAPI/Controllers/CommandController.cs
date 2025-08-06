using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandeServiceAPI.DTO;
using CommandeServiceAPI.Models;
using CommandeServiceAPI.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommandeServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : Controller
    {

        private readonly CommandService _commandService;

        public CommandController(CommandService commandService)
        {
            _commandService = commandService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCommand(CreateCommandDTO createCommandDTO)
        {
            await _commandService.CreateCommande(createCommandDTO);
            return Ok(new
            {
                Statut = "Commande créée en bdd",
                createCommandDTO
            });
        }

        [HttpGet("Get/{commandId}")]
        public async Task<IActionResult> GetCommand(int commandId)
        {
            var command = await _commandService.GetCompleteCommand(commandId);
            return Ok(command);
        }
    }
}

