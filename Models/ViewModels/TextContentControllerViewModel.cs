using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Models.ViewModels
{
    public class GenerateTextViewModel
    {
        [JsonProperty("context")]
        public string Context { get; set; }
    }
}
