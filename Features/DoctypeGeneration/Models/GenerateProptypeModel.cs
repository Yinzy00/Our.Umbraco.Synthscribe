using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Features.DoctypeGeneration.Models
{
    internal class GenerateProptypeModel: GenerateTypeBaseModel
    {
        public string DataTypeAlias { get; set; }
        public string Description { get; set; }
    }
}
