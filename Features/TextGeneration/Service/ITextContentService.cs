using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Features.TextGeneration.Service
{
    public interface ITextContentService
    {
        public Task<string> GetText(string context);
    }
}
