using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;

namespace Our.Umbraco.Synthscribe.Features.DoctypeGeneration.Models
{
    internal class GenerateTypeBaseModel
    {
        private string name;
        public string Name { get => name ?? "NameWasEmpty"; set => name = value; }
    }
}
