using Newtonsoft.Json;

namespace QuestSystem.Resolvers
{
    [JsonObject(MemberSerialization.OptIn)]
    public class JSONContainer
    {
        [JsonProperty("TypeIndex")]
        public int TypeIndex;

        [JsonProperty("JSON")]
        public string JSON;
    }
}