using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.OpenAi.Models
{
    internal class ChatGptCompletionResponse
    {
        public string id { get; set; }
        public string _object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public GptCompletionResponseUsage usage { get; set; }
        public List<GptCompletionResponseChoice> choices { get; set; }
    }

    internal class GptCompletionResponseUsage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }

    internal class GptCompletionResponseChoice
    {
        public GptCompletionResponseMessage message { get; set; }
        public string finish_reason { get; set; }
        public int index { get; set; }
    }

    internal class GptCompletionResponseMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }
}
