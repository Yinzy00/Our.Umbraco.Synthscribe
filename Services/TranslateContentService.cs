using Newtonsoft.Json;
using NUglify;
using OpenAi.Models.ViewModels;
using Our.Umbraco.Synthscribe.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;
using static Umbraco.Cms.Core.Constants;

namespace Our.Umbraco.Synthscribe.Services
{
    internal class TranslateContentService : TranslateUmbracoService, ITranslateContentService
    {
        private readonly ILocalizationService _localizationService;
        private readonly IContentService _contentService;
        private readonly ITranslationService _translationService;
        private readonly IUmbracoContextFactory _umbracoContextFactory;

        private readonly IUmbracoContext _context;

        public TranslateContentService(IContentService contentService, ILocalizationService localizationService, ITranslationService translationService, IUmbracoContextFactory umbracoContextFactory)
            : base(localizationService)
        {
            _contentService = contentService;
            _translationService = translationService;
            _umbracoContextFactory = umbracoContextFactory;
            _localizationService = localizationService;

            _context = umbracoContextFactory.EnsureUmbracoContext().UmbracoContext;

        }

        /// <summary>
        /// Translate all pages
        /// </summary>
        /// <param name="destinationLanguage">Lagnuage to translate to</param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public async Task TranslateAllContent(string destinationLanguage = null, bool overwrite = false)
        {
            //Get all roots
            var rootList = _context?.Content?.GetAtRoot();

            if (rootList != null)
            {
                //Translate all roots
                foreach (var root in rootList)
                {
                    await TranslateContent(root.Id, destinationLanguage, overwrite, translateDescendants: true);
                }
            }
        }

        /// <summary>
        /// Translate content page by id
        /// </summary>
        /// <param name="id">The page id</param>
        /// <param name="destinationLanguage">Language to translate to</param>
        /// <param name="overwrite">Should overwrite existing values?</param>
        /// <param name="translateDescendants">Translate descendants?</param>
        /// <returns></returns>
        public async Task TranslateContent(int id, string destinationLanguage = null, bool overwrite = false, bool translateDescendants = false)
        {
            var content = _contentService.GetById(id);

            if (content == null)
                return;

            var destLanguage = _localizationService.GetLanguageByIsoCode(destinationLanguage);

            var defaultName = content.GetCultureName(defaultLanguage.IsoCode);

            if (defaultName != null)
            {
                if(destinationLanguage == null)
                {
                    //Translate page name
                    foreach (var language in languages.Where(l => !l.IsDefault))
                    {
                        var currentName = content.GetCultureName(language.IsoCode);

                        if (!string.IsNullOrEmpty(currentName) && !overwrite)
                            continue;

                        var translatedName = await _translationService.Translate(defaultName, defaultLanguage, language);
                        content.SetCultureName(translatedName, language.IsoCode);
                    }
                }
                else
                {
                    if(destLanguage != null)
                    {
                        var currentName = content.GetCultureName(destinationLanguage);

                        if ((!string.IsNullOrEmpty(currentName) && overwrite) || string.IsNullOrEmpty(currentName))
                        {
                            var translatedName = await _translationService.Translate(defaultName, defaultLanguage, destLanguage);
                            content.SetCultureName(translatedName, destinationLanguage);

                        }
                    }
                }
            }

            //Translate page content
            content.Properties = await HandleProperties(content.Properties, destLanguage, overwrite);

            //Save translated content
            _contentService.Save(content);
        }

        /// <summary>
        /// Translate blocklist values
        /// </summary>
        /// <param name="value">The string value</param>
        /// <param name="language">The language to translate to</param>
        /// <returns></returns>
        private async Task<string> TranslateBlockListValue(string value, ILanguage language)
        {
            //Parse blocklist value
            var obj = JsonConvert.DeserializeObject<BlockListViewModel>(value);

            if (obj == null || obj.ContentData == null)
                return null;

            //For each block
            foreach (var t in obj.ContentData)
            {
                //For each property of the block
                foreach (var p in t.Properties())
                {
                    //If not Umbraco Data
                    if (p.Name != "contentTypeKey" && p.Name != "udi")
                    {
                        //Translate property content
                        var translatedData = await _translationService.Translate(p.Value.ToString(), defaultLanguage, language);

                        if (translatedData != null)
                            p.Value = translatedData;
                    }
                }

            }

            //Serialize back to json
            var translatedValue = JsonConvert.SerializeObject(obj);

            return translatedValue;
        }
        /// <summary>
        /// Translate textbox value
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="language">language to translate to</param>
        /// <returns></returns>
        private async Task<string> TranslateTextBoxValue(string value, ILanguage language)
        {
            //Value of textbox is a string, translate the string
            var translatedValue = await _translationService.Translate(value, defaultLanguage, language);

            return translatedValue;
        }
        /// <summary>
        /// Translate multi string values
        /// </summary>
        /// <param name="value">String value of the content</param>
        /// <param name="language">The language to translate to</param>
        /// <returns></returns>
        private async Task<string> TranslateMultipleTextStringValue(string value, ILanguage language)
        {
            var translatedValue = "";
            var items = value.Split("\r\n");

            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(translatedValue))
                    translatedValue += "\r\n";

                var translatedItem = await _translationService.Translate(item, defaultLanguage, language);
                translatedValue += translatedItem;
            }

            return translatedValue;
        }
        /// <summary>
        /// Translate multi url picker values
        /// </summary>
        /// <param name="value">The string value of the content</param>
        /// <param name="language">The langage to translate to</param>
        /// <returns></returns>
        private async Task<string> TranslateMultiUrlPicker(string value, ILanguage language)
        {
            var items = JsonConvert.DeserializeObject<List<MultiUrlPickerViewModel>>(value);

            if (items == null) return null;

            foreach (var i in items)
            {
                if (i == null || i.Name == null) continue;

                var translatedValue = await _translationService.Translate(i.Name, defaultLanguage, language);

                i.Name = translatedValue;
            }

            return JsonConvert.SerializeObject(items);
        }
        /// <summary>
        /// Handle property translation
        /// </summary>
        /// <param name="properties">List of properties</param>
        /// <param name="where">Filter on properties to translate</param>
        /// <returns></returns>
        private async Task<IPropertyCollection> HandleProperties(IPropertyCollection properties, ILanguage languageTo = null, bool overwrite = false, Func<IProperty, bool> where = null)
        {
            //Translate each property
            foreach (var property in properties.Where(where != null ? where : i => true))
            {
                var source = property.GetValue(defaultLanguage.IsoCode)?.ToString();
                if (source != null)
                {
                    if(languageTo == null)
                    {
                        //In each language
                        foreach (var language in languages.Where(l => !l.IsDefault))
                        {
                            await HandleProperty(property, source, language, overwrite);
                        }
                    }
                    else
                    {
                        await HandleProperty(property, source, languageTo, overwrite);
                    }
                }
            }

            return properties;
        }

        private async Task HandleProperty(IProperty property, string source, ILanguage languageTo = null, bool overwrite = false)
        {
            var currentValue = property.GetValue(languageTo.IsoCode)?.ToString();

            if (currentValue == null ||  currentValue != null && overwrite)
            {
                string translatedValue = null;
                //Check propertyeditor & handle translation
                switch (property.PropertyType.PropertyEditorAlias)
                {
                    case PropertyEditors.Aliases.BlockList:
                        translatedValue = await TranslateBlockListValue(source, languageTo);
                        break;
                    case PropertyEditors.Aliases.TextBox:
                        translatedValue = await TranslateTextBoxValue(source, languageTo);
                        break;
                    case PropertyEditors.Aliases.TextArea:
                        translatedValue = await TranslateTextBoxValue(source, languageTo);
                        break;
                    case PropertyEditors.Aliases.TinyMce:
                        translatedValue = await TranslateTextBoxValue(source, languageTo);
                        break;
                    case PropertyEditors.Aliases.MultiUrlPicker:
                        translatedValue = await TranslateMultiUrlPicker(source, languageTo);
                        break;
                    case PropertyEditors.Aliases.MultipleTextstring:
                        translatedValue = await TranslateMultipleTextStringValue(source, languageTo);
                        break;
                    default:
                        break;
                }

                //Set property value in language
                if (translatedValue != null)
                    property.SetValue(translatedValue, languageTo.IsoCode);
            }

            return;
        }

    }
}
