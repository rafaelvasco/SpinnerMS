using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using SpinnerMS.Model;
using SpinnerMS.Utils;

namespace SpinnerMS.Services
{
    public class MatchService : IMatchService
    {
        private readonly Container _container;

        public MatchService(
            CosmosClient db_client,
            string database_name,
            string container_name)
        {
            _container = db_client.GetContainer(database_name, container_name);
        }

        public async Task AddMatchAsync(Match match)
        {
            await _container.CreateItemAsync(match, new PartitionKey(match.Id));
        }

        public async Task UpdateMatchAsync(string id, Match match)
        {
            await _container.UpsertItemAsync(match, new PartitionKey(id));
        }

        public async Task RemoveMatchAsync(string id)
        {
            await _container.DeleteItemAsync<Match>(id, new PartitionKey(id));
        }

        public async Task<Match> GetMatchAsync(string id)
        {
            try
            {
                ItemResponse<Match> response = await _container.ReadItemAsync<Match>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException e) when(e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Match>> GetMatchesAsync(string query_str, params object[] query_params)
        {
            var query = _container.GetItemQueryIterator<Match>(DbUtils.BuildQueryDefinition(query_str, query_params));
            var results = new List<Match>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }
       
    }
}
