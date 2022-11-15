using Newtonsoft.Json;

namespace SampleExtensionReceiver.Models
{
    public record Response
    {
        [JsonProperty("status")]
        public string Status { get; init; } = "OK";

        [JsonProperty("data")]
        public Data Data { get; init; } = new();
    }

    public record Data
    {
        [JsonProperty("familyStatus")]
        public string FamilyStatus { get; init; } = "married";
    }
}