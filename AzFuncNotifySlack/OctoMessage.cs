using System;
using Newtonsoft.Json;

namespace Octopus
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class OctoMessage
    {
        [JsonProperty("Payload.Event.Message")]
        public string Message {get;set;}
        
        [JsonProperty("Payload.Event.SpaceId")]
        public string SpaceId {get;set;}

        [JsonProperty("Payload.Event.Username")]
        public string Username {get;set;}

        [JsonProperty("Payload.ServerUri")]
        public string ServerUri{get;set;}

        public string GetSpaceUrl(){
            return string.Format("{0}/app#/{1}",ServerUri,SpaceId);
        }        
    }
}