using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Features.DoctypeGeneration.Models
{
    internal class GenerateProptypeViewModel: GenerateTypeBaseViewModel
    {
        [JsonProperty("propertyEditorAlias")]
        public string PropertyEditorAlias { get; set; } = null;
        [JsonProperty("propertyTypeInfo")]
        public string PropertyTypeInfo { get; set; }
        public string Description { get; set; }
    }
}
