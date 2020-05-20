using System.Collections.Generic;
using System.Threading.Tasks;
using SpinnerMS.Model;

namespace SpinnerMS.Services
{
    public interface IPlayerService
    {
        Task<IEnumerable<Player>> GetPlayersAsync(string query, params object[] @params);

        Task<Player> GetPlayerAsync(string id);
        Task AddPlayerAsync(Player player);
        Task UpdatePlayerAsync(string id, Player player);
        Task RemovePlayerAsync(string id);

        Task RemoveAll();
    }
}
