using Newtonsoft.Json;

namespace Our.Umbraco.Synthscribe.OpenAi.Models
{
    internal sealed class ChatGptCompletionTextMessage: ChatGptCompletionMessage
    {
        [JsonProperty("content")]
        public string Content { get; set; } = "";
    }
}
