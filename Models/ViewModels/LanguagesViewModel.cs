using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Models.ViewModels
{
    internal class LanguagesViewModel
    {
        [JsonProperty("defaultLanguage")]
        public string DefaultLanguage { get; set; }
        [JsonProperty("languages")]
        public List<string> Languages { get; set; } = new List<string>();
    }
}
