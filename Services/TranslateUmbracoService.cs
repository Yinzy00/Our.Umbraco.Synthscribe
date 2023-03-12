using Our.Umbraco.Synthscribe.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace Our.Umbraco.Synthscribe.Services
{
    internal class TranslateUmbracoService
    {
        public readonly IEnumerable<ILanguage> languages;
        public readonly ILanguage defaultLanguage;
        public TranslateUmbracoService(ILocalizationService localizationService)
        {
            languages = localizationService.GetAllLanguages().Where(l => !l.IsDefault);
            defaultLanguage = localizationService.GetAllLanguages().First(l => l.IsDefault);
        }
    }
}
