using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpinnerMS.Model;
using SpinnerMS.Services;

namespace SpinnerMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _match_service;
        private readonly IPlayerService _player_service;

        public MatchController(IMatchService match_service, IPlayerService player_service)
        {
            _match_service = match_service;
            _player_service = player_service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatches()
        {
            var matches = await _match_service.GetMatchesAsync("SELECT * FROM c");

            return Ok(matches);
        }

        [HttpPut("add")]
        public async Task<ActionResult> AddMatch([FromBody]NewMatch new_match)
        {
            var player = await _player_service.GetPlayerAsync(new_match.ServerPlayerId);
            var (ip, port) = Utils.RequestUtils.GetRemoteIpAndPort(Request);

            var match = new Match()
            {
                Id = Guid.NewGuid().ToString(),

                Server = new MatchPlayer()
                {
                    Id = Guid.NewGuid().ToString(),
                    Ip = ip,
                    Port = port,
                    Player = player
                }
            };

            await _match_service.AddMatchAsync(match);

            return Ok(match);

        }

        [HttpDelete("remove/{id}")]
        public async Task<ActionResult> RemoveMatch(string id)
        {
            await _match_service.RemoveMatchAsync(id);

            return Ok();
        }

        [HttpPost("join")]
        public async Task<ActionResult> Join([FromBody]Join join)
        {
            var player = await _player_service.GetPlayerAsync(join.PlayerId);
            var (ip, port) = Utils.RequestUtils.GetRemoteIpAndPort(Request);

            var match = await _match_service.GetMatchAsync(join.MatchId);

            if (match == null)
            {
                return Ok(null);
            }

            match.Client = new MatchPlayer()
            {
                Id = Guid.NewGuid().ToString(),
                Ip = ip,
                Port = port,
                Player = player,
            };

            await _match_service.UpdateMatchAsync(match.Id, match);

            return Ok(match);
        }

    }
}
