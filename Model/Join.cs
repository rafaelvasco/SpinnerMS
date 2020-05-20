using Newtonsoft.Json;

namespace SpinnerMS.Model
{
    public class Join
    {
        [JsonProperty("matchId")]
        public string MatchId { get; set; }

        [JsonProperty("playerId")]
        public string PlayerId { get; set; }

        [JsonProperty("serverPort")]
        public string ServerPort;
    }
}
