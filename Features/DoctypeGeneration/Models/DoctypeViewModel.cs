using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Features.DoctypeGeneration.Models
{
    internal class DoctypeViewModel: GenerateTypeBaseViewModel
    {
        public string Icon { get; set; }
        public List<GenerateProptypeViewModel> Properties { get; set; } = new List<GenerateProptypeViewModel>();
    }
}
