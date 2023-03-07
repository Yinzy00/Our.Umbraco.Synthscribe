using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.OpenAi.Models
{
    internal class OpenAiCompletionBase
    {
        [JsonProperty("model")]
        public string Model { get; set; }
    }
}
