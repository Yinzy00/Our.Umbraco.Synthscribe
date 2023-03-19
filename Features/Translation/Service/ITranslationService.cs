using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;

namespace Our.Umbraco.Synthscribe.Features.Translation.Service
{
    public interface ITranslationService
    {
        Task<string> Translate(string text, ILanguage sourceLanguage, ILanguage targetLanguage);
    }
}
