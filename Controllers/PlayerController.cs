using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpinnerMS.Model;
using SpinnerMS.Services;

namespace SpinnerMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _player_service;

        public PlayerController(IPlayerService player_service)
        {
            _player_service = player_service;
        }

        [HttpPut("add")]
        public async Task<ActionResult<Player>> AddPlayer([FromBody] Player player)
        {
            var existing = await _player_service.GetPlayersAsync("SELECT * from c where c.email = @email", player.Email);

            var enumerable = existing.ToList();
            if (enumerable.Any())
            {
                return Ok(enumerable.First());
            }

            player.Id = Guid.NewGuid().ToString();


            await _player_service.AddPlayerAsync(player);

            return Ok(player);
        }

        [HttpPost("update")]
        public async Task<ActionResult> UpdatePlayer([FromBody] Player player)
        {
            var player_in_db = await _player_service.GetPlayerAsync(player.Id);

            if (player_in_db == null)
            {
                return NotFound($"No player found in database with ID {player.Id} to update.");
            }

            if (!string.IsNullOrEmpty(player.Name))
            {
                player_in_db.Name = player.Name;
            }


            await _player_service.UpdatePlayerAsync(player_in_db.Id, player_in_db);

            return Ok(player_in_db);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(string id)
        {
            var player = await _player_service.GetPlayerAsync(id);

            return Ok(player);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            var players = await _player_service.GetPlayersAsync("SELECT * from c");

            return Ok(players);
        }

        [HttpDelete("remove")]
        public async Task<ActionResult> RemoveAll()
        {
            await _player_service.RemoveAll();

            return Ok();
        }

        [HttpDelete("remove/{id}")]
        public async Task<ActionResult> RemovePlayer(string id)
        {
            var player_in_db = await _player_service.GetPlayerAsync(id);

            if (player_in_db == null)
            {
                return NotFound();
            }

            await _player_service.RemovePlayerAsync(id);

            return Ok();
        }
        
    }
}
