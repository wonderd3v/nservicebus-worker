using Newtonsoft.Json;

namespace SharedContracts
{
    public class QueueStatus
    {
        [JsonProperty]
        public long MessageCount { get; set; }
    }
}
