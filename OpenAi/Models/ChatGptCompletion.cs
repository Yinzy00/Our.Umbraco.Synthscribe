using Newtonsoft.Json;
using System.Collections.Generic;

namespace Our.Umbraco.Synthscribe.OpenAi.Models
{
    internal sealed class ChatGptCompletion : OpenAiCompletionBase
    {
        public ChatGptCompletion()
        {
            Model = "gpt-3.5-turbo";
        }

        [JsonProperty("messages")]
        public List<ChatGptCompletionMessage> Messages { get; set; } = new();
    }
}
