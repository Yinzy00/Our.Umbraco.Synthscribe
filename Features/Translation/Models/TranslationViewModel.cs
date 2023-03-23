using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Features.Translation.Models
{
    public class TranslationViewModel
    {
        [JsonProperty("nodeId")]
        public int? NodeId { get; set; }
        [JsonProperty("languageTo")]
        public string LanguageTo { get; set; }
        [JsonProperty("overwrite")]
        public bool Overwrite { get; set; }
        [JsonProperty("translateDescendants")]
        public bool TranslateDescendants { get; set; }
    }
}
