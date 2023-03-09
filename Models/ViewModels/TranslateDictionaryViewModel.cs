using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Models.ViewModels
{
    public class TranslateDictionaryViewModel
    {
        [JsonProperty("dictionaryId")]
        public int DictionaryId { get; set; }
        [JsonProperty("languageTo")]
        public string LanguageTo { get; set; }
        [JsonProperty("overwrite")]
        public bool Overwrite { get; set; }
        [JsonProperty("translateDescendants")]
        public bool TranslateDescendants { get; set; }
    }
}
