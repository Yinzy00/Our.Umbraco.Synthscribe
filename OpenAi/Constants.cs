using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.OpenAi
{
    internal static class Constants
    {
        public static string BaseUrl = "https://api.openai.com/v1/";

        public static class Gpt
        {
            public static string CompletionUrl = BaseUrl + "chat/completions";
        }
    }
}
