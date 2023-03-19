using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Features.DoctypeGeneration.Models
{
    internal class GenerateDoctypeModel: GenerateTypeBaseModel
    {
        public string Icon { get; set; }
        public List<GenerateProptypeModel> Properties { get; set; } = new List<GenerateProptypeModel>();
    }
}
