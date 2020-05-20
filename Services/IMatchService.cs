using System.Collections.Generic;
using System.Threading.Tasks;
using SpinnerMS.Model;

namespace SpinnerMS.Services
{
    public interface IMatchService
    {
        Task AddMatchAsync(Match match);
        Task UpdateMatchAsync(string id, Match match);
        Task RemoveMatchAsync(string id);
        Task<Match> GetMatchAsync(string id);
        Task<IEnumerable<Match>> GetMatchesAsync(string query, params object[] @params);
    }
}
