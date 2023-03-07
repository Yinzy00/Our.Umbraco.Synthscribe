using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.OpenAi.Models
{
    internal class ChatGptCompletion : OpenAiCompletionBase
    {
        public ChatGptCompletion()
        {
            Model = "gpt-3.5-turbo";
        }

        [JsonProperty("messages")]
        public List<ChatGptCompletionMessage> Messages { get; set; } = new();
    }
}
