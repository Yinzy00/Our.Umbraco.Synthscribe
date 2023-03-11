using Microsoft.AspNetCore.Mvc;
using Our.Umbraco.Synthscribe.Models.ViewModels;
using Our.Umbraco.Synthscribe.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Controllers;

namespace Our.Umbraco.Synthscribe.Controllers
{
    [PluginController("Synthscribe")]
    public class TranslationController: UmbracoAuthorizedApiController
    {
        private readonly ITranslateDictionaryService _translateDictionaryService;
        private readonly ILocalizationService _localizationService;
        public TranslationController(ITranslateDictionaryService translateDictionaryService, ILocalizationService localizationService)
        {
            _translateDictionaryService = translateDictionaryService;
            _localizationService = localizationService;

        }

        /// <summary>
        /// Get all languages except the default.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Languages()
        {
            var languages = _localizationService?.GetAllLanguages()?.Where(l=>!l.IsDefault);
            var defaultLanguage = _localizationService?.GetDefaultLanguageIsoCode();

            if (defaultLanguage == null)
                return NotFound("No default language found!");

            return Ok(new LanguagesViewModel()
            {
                Languages = languages?.Select(l => l.IsoCode)?.ToList(),
                DefaultLanguage = defaultLanguage
            });
        }

        [HttpPost]
        public async Task<ActionResult> TranslateDictionary(TranslateDictionaryViewModel vm)
        {
            if(vm.DictionaryId == null)
                return BadRequest();

            await _translateDictionaryService.TranslateDictionary((int)vm.DictionaryId, !string.IsNullOrEmpty(vm.LanguageTo) ? vm.LanguageTo : null, vm.Overwrite, vm.TranslateDescendants);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> TranslateAllDictionaries(TranslateDictionaryViewModel vm)
        {
            await _translateDictionaryService.TranslateAllDictionaries(vm.LanguageTo, vm.Overwrite);

            return Ok();
        }

        //[HttpPost]
        //public async Task<IActionResult> Translate(string text, string sourceLanguage, string destinationLanguage)
        //{
        //    return Ok();
        //}
    }
}
