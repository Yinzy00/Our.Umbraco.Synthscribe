using NUglify.Helpers;
using Our.Umbraco.Synthscribe.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace Our.Umbraco.Synthscribe.Services
{
    internal class TranslateDictionaryService : TranslateUmbracoService, ITranslateDictionaryService
    {
        private readonly ILocalizationService _localizationService;
        private readonly ITranslationService _translationService;

        public TranslateDictionaryService(ILocalizationService localizationService, ITranslationService translationService)
            :base(localizationService)
        {
            _localizationService = localizationService;
            _translationService = translationService;
        }
        public async Task TranslateAllDictionaries(string destinationLanguage = null, bool overwrite = false)
        {
            IEnumerable<IDictionaryItem> allDictionaries = new List<IDictionaryItem>();
            allDictionaries = allDictionaries.Concat(_localizationService.GetRootDictionaryItems());

            IEnumerable<IDictionaryItem> descendants = new List<IDictionaryItem>();

            foreach (var di in allDictionaries)
            {
                descendants = descendants.Concat(_localizationService.GetDictionaryItemDescendants(di.Key));
            }

            allDictionaries = allDictionaries.Concat(descendants);

            foreach (var di in allDictionaries)
            {
                await TranslateSingleDictionary(di, destinationLanguage, overwrite);
            }
        }

        public async Task TranslateDictionary(int id, string destinationLanguage = null, bool overwrite = false, bool translateDescendants = false)
        {
            var di = _localizationService.GetDictionaryItemById(id);

            //Check if dictionary exists
            if (di == null)
                return;

            await TranslateSingleDictionary(di, destinationLanguage, overwrite);

            if (translateDescendants)
            {
                var descendants = _localizationService.GetDictionaryItemDescendants(di.Key);

                foreach (var descendant in descendants)
                {
                    if (descendant == null)
                        continue;

                    await TranslateSingleDictionary(descendant, destinationLanguage, overwrite);
                }
            }
        }

        private async Task<bool> TranslateSingleDictionary(IDictionaryItem di, string destinationLanguageIso = null, bool overwrite = false)
        {

            var defaultValue = di.GetDefaultValue();

            //Check if there is a value for the default language
            if (defaultValue == null)
                return false;

            if (!string.IsNullOrEmpty(destinationLanguageIso))
            {
                //Translate to specific language

                var language = _localizationService.GetLanguageByIsoCode(destinationLanguageIso);

                var currentTranslation = di.GetTranslatedValue(language.Id);

                if (!string.IsNullOrEmpty(currentTranslation) && !overwrite)
                    return false;

                var tranlatedValue = await _translationService.Translate(defaultValue, defaultLanguage, language);

                if (string.IsNullOrEmpty(tranlatedValue))
                    return false;

                //Update dictionary value
                _localizationService.AddOrUpdateDictionaryValue(di, language, tranlatedValue);

                _localizationService.Save(di);

                return true;
            }
            else
            {
                //Translate to all languages

                bool changed = false;

                foreach (var language in languages)
                {
                    var currentTranslation = di.GetTranslatedValue(language.Id);
                    if (!string.IsNullOrEmpty(currentTranslation) && !overwrite)
                        continue;

                    var translatedValue = await _translationService.Translate(defaultValue, defaultLanguage, language);

                    if (translatedValue != null)
                    {
                        _localizationService.AddOrUpdateDictionaryValue(di, language, translatedValue);
                        changed = true;
                    }
                }

                if (changed)
                    _localizationService.Save(di);

                return changed;
            }
        }
    }
}
