using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using SpinnerMS.Model;
using SpinnerMS.Utils;

namespace SpinnerMS.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly Container _container;

        public PlayerService(
            CosmosClient db_client,
            string database_name,
            string container_name)
        {
            _container = db_client.GetContainer(database_name, container_name);
        }

        public async Task AddPlayerAsync(Player player)
        {
            await _container.CreateItemAsync(player, new PartitionKey(player.Id));
        }

        public async Task<Player> GetPlayerAsync(string id)
        {
            try
            {
                ItemResponse<Player> response = await _container.ReadItemAsync<Player>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException e) when(e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task RemovePlayerAsync(string id)
        {
            await _container.DeleteItemAsync<Player>(id, new PartitionKey(id));
        }

        public async Task RemoveAll()
        {
            var items = await GetPlayersAsync("SELECT * from c");

            foreach (var player in items)
            {
                await RemovePlayerAsync(player.Id);
            }
        }

        public async Task<IEnumerable<Player>> GetPlayersAsync(string query_str, params object[] query_params)
        {
            var query = _container.GetItemQueryIterator<Player>(DbUtils.BuildQueryDefinition(query_str, query_params));
            var results = new List<Player>();
            while (query.HasMoreResults)
            {
                var current_result_Set = await query.ReadNextAsync();

                foreach (var player in current_result_Set)
                {
                    results.Add(player);
                }
            }

            return results;
        }

        public async Task UpdatePlayerAsync(string id, Player player)
        {
            await _container.UpsertItemAsync(player, new PartitionKey(id));
        }
       
    }
}
