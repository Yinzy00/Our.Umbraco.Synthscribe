using Newtonsoft.Json;

namespace Our.Umbraco.Synthscribe.OpenAi.Models;
internal abstract class ChatGptCompletionMessage
{
    [JsonProperty("role")]
    public string Role { get; set; }
}
