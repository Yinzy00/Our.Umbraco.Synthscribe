using Microsoft.AspNetCore.Mvc;
using Our.Umbraco.Synthscribe.Features.Translation.Content.Service;
using Our.Umbraco.Synthscribe.Features.Translation.Dictionary.Service;
using Our.Umbraco.Synthscribe.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Extensions;

namespace Our.Umbraco.Synthscribe.Features.Translation
{
    [PluginController("Synthscribe")]
    public class TranslationController : UmbracoAuthorizedApiController
    {
        private readonly ITranslateDictionaryService _translateDictionaryService;
        private readonly ITranslateContentService _translateContentService;
        private readonly ILocalizationService _localizationService;
        public TranslationController(ITranslateDictionaryService translateDictionaryService, ILocalizationService localizationService, ITranslateContentService translateContentService)
        {
            _translateDictionaryService = translateDictionaryService;
            _localizationService = localizationService;
            _translateContentService = translateContentService;

        }

        /// <summary>
        /// Get all languages except the default.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Languages()
        {
            var languages = _localizationService?.GetAllLanguages()?.Where(l => !l.IsDefault);
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
        public async Task<IActionResult> TranslateDictionary(TranslationViewModel vm)
        {
            if (vm == null || vm.NodeId == null)
                HandleError();

            await _translateDictionaryService.TranslateDictionary((int)vm.NodeId, !string.IsNullOrEmpty(vm.LanguageTo) ? vm.LanguageTo : null, vm.Overwrite, vm.TranslateDescendants);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> TranslateAllDictionaries(TranslationViewModel vm)
        {
            if (vm == null)
                HandleError();

            await _translateDictionaryService.TranslateAllDictionaries(vm.LanguageTo, vm.Overwrite);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> TranslateContent(TranslationViewModel vm)
        {
            if (vm == null || vm.NodeId == null)
                HandleError();

            await _translateContentService.TranslateContent((int)vm.NodeId, vm.LanguageTo, vm.TranslateDescendants);

            return Ok();

        }

        [HttpPost]
        public async Task<IActionResult> TranslateAllContent(TranslationViewModel vm)
        {
            if (vm == null)
                HandleError();

            await _translateContentService.TranslateAllContent(vm.LanguageTo, vm.Overwrite);

            return Ok();
        }

        private IActionResult HandleError()
        {
            return BadRequest("Something went wrong!");
        }

        //[HttpPost]
        //public async Task<IActionResult> Translate(string text, string sourceLanguage, string destinationLanguage)
        //{
        //    return Ok();
        //}
    }
}
