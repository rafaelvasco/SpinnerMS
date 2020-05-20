using Newtonsoft.Json;

namespace SpinnerMS.Model
{
    public class NewMatch
    {
        [JsonProperty("serverPlayerId")]
        public string ServerPlayerId { get; set; }
    }
}
