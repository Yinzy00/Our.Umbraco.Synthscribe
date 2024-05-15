using Newtonsoft.Json;

namespace Our.Umbraco.Synthscribe.OpenAi.Models
{
    internal abstract class OpenAiCompletionBase(string? model = null, int? maxTokens = null)
    {
        [JsonProperty("model")]
        public string Model { get; internal set; } = model ?? "gpt-3.5-turbo";
        [JsonProperty("max_tokens")]
        public int MaxTokens { get; private set; } = maxTokens ?? 1000;
    }
}
