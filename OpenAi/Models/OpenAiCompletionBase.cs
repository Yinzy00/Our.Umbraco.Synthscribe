using Newtonsoft.Json;

namespace Our.Umbraco.Synthscribe.OpenAi.Models
{
    internal abstract class OpenAiCompletionBase
    {
        [JsonProperty("model")]
        public string Model { get; internal set; }
    }
}
