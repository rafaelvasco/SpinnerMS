
using Newtonsoft.Json;

namespace SpinnerMS.Model
{
    public class Player
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("totalScore")]
        public int TotalScore { get; set; }
    }
}
