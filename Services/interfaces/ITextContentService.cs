using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Services.interfaces
{
    public interface ITextContentService
    {
        public Task<string> GetText(string context);
    }
}
