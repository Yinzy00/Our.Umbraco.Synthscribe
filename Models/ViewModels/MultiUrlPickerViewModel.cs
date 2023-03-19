using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAi.Models.ViewModels
{
    internal class MultiUrlPickerViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("queryString")]
        public string QueryString { get; set; }
    }
}
