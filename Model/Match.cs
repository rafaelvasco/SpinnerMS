using Newtonsoft.Json;

namespace SpinnerMS.Model
{
    public class Match
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("server")]
        public MatchPlayer Server { get; set; }

        [JsonProperty("client")]
        public MatchPlayer Client { get; set; }
    }
}
