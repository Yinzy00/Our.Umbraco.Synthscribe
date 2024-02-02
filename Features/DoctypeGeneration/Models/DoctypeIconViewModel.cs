using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Features.DoctypeGeneration.Models
{
    internal class DoctypeIconViewModel
    {
        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
