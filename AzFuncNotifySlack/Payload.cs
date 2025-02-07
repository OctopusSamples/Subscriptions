using Newtonsoft.Json;

namespace Octopus
{
    public class Payload
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }
        
        [JsonProperty("username")]
        public string Username { get; set; }
        
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}