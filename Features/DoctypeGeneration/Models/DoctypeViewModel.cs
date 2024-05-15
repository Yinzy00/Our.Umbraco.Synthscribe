using System.Collections.Generic;

namespace Our.Umbraco.Synthscribe.Features.DoctypeGeneration.Models
{
    internal class DoctypeViewModel: GenerateTypeBaseViewModel
    {
        public string Icon { get; set; } = null;
        public List<GenerateProptypeViewModel> Properties { get; set; } = [];
    }
}
