using Newtonsoft.Json;

namespace SpinnerMS.Model
{
    public class MatchPlayer
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("player")]
        public Player Player { get; set; }
        
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("port")]
        public string Port { get; set; }
    }
}
