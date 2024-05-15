using Newtonsoft.Json;
using System.Collections.Generic;

namespace Our.Umbraco.Synthscribe.OpenAi.Models
{
    internal sealed class ChatGptCompletion(string model = null)
        : OpenAiCompletionBase(model)
    {

        [JsonProperty("messages")]
        public List<ChatGptCompletionMessage> Messages { get; set; } = [];
    }
}
