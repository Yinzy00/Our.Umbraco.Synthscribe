using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Our.Umbraco.Synthscribe.Features.DoctypeGeneration.Models;
using Our.Umbraco.Synthscribe.General.Models.Interrfaces;
using Our.Umbraco.Synthscribe.OpenAi.Models;
using Our.Umbraco.Synthscribe.OpenAi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Persistence.Repositories;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Services.Implement;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;
using static Umbraco.Cms.Core.PropertyEditors.ImageCropperConfiguration;

namespace Our.Umbraco.Synthscribe.Features.DoctypeGeneration.Service
{
    internal class GenerateDoctypeService : IGenerateDoctypeService
    {
        private readonly ILogger<IGenerateDoctypeService> _logger;
        private readonly IContentTypeService _contentTypeService;
        private readonly IDataTypeService _dataTypeService;
        private readonly IShortStringHelper _shortStringHelper;
        private readonly IChatGptService _chatGptService;
        private readonly IConfigurationEditorJsonSerializer _configurationEditorJsonSerializer;

        public GenerateDoctypeService(IContentTypeService contentTypeService, 
            IDataTypeService dataTypeService, 
            IShortStringHelper shortStringHelper, 
            IChatGptService chatGptService, 
            IConfigurationEditorJsonSerializer configurationEditorJsonSerializer,
            ILogger<IGenerateDoctypeService> logger)
        {
            _contentTypeService = contentTypeService;
            _dataTypeService = dataTypeService;
            _shortStringHelper = shortStringHelper;
            _chatGptService = chatGptService;
            _configurationEditorJsonSerializer = configurationEditorJsonSerializer;
            _logger = logger;
        }

        public async Task<IResponseModel> GenerateDoctype(string context)
        {
            try
            {
                //Handle context data using ai
                DoctypeViewModel model = await HandleContext(context);

                //Create doctype
                var contentType = await CreateDoctype(model.Name, model.Icon);

                //Add properties to doctype
                contentType = await HandleProperties(contentType, model);

                //Save doctype
                _contentTypeService.Save(contentType);

                return new ResponseModel(true, "Doctype has been created!");
            }
            catch (Exception e)
            {

                return new ResponseModel(false, e?.Message);
            }

        }

        private async Task<DoctypeViewModel> HandleContext(string context)
        {
            //return JsonConvert.DeserializeObject<GenerateDoctypeModel>("{\"Name\":\"HomePage\",\"Icon\":\"icon-home\",\"Properties\":[{\"DataTypeAlias\":\"Textstring\",\"Name\":\"Title\",\"Description\":\"The title of the home page.\"},{\"DataTypeAlias\":\"Textstring\",\"Name\":\"Intro\",\"Description\":\"The introduction text of the home page.\"},{\"DataTypeAlias\":\"Contentstring\",\"Name\":\"BlockList\",\"Description\":\"A list of content blocks on the home page.\"},{\"DataTypeAlias\":\"Textstring\",\"Name\":\"FooterText\",\"Description\":\"The footer text of the home page.\"}]}");
            var response = await _chatGptService.CreateCompletion(new ChatGptCompletion()
            {
                Messages = new List<ChatGptCompletionMessage>()
                {
                    new ChatGptCompletionMessage()
                    {
                        Role = ChatGptRoles.system.ToString(),
                        Content = "Act as a Umbraco tool that converts human language text into json objects. [Return only the main response. Remove pre-text and post-text]"
                    },
                    new ChatGptCompletionMessage()
                    {
                        Role = ChatGptRoles.user.ToString(),
                        Content = $"[Return only the main response. Remove pre-text and post-text] Generate a json object in format [{{\"Icon\":\"string (Umbraco icon alias)\",\"Properties\":[{{\"DataTypeAlias\":\"string\",\"Name\":\"string\",\"Description\":\"string\"}}],\"Name\":\"string\"}}] based on [{context}], add relevant icon (if none found use icon-document), names and descriptions into the properties."
                    }
                }
            });

            _logger.LogInformation($"Generate doctype: {response}");

            var vm = JsonConvert.DeserializeObject<List<DoctypeViewModel>>(response);

            return vm.FirstOrDefault();
        }
        private async Task<ContentType> CreateDoctype(string name, string icon = "icon-document", bool isElementType = false)
        {
            var alias = GetAlias(name);


            ContentType contentType = new(_shortStringHelper, -1);
            contentType.Name = name;
            contentType.Alias = alias;
            contentType.Icon = icon;
            contentType.IsElement = isElementType;

            return contentType;

            //var key = Guid.NewGuid();
            //var attempt = _contentTypeService.CreateContainer(-1, key, "My DocType 2");



            //if (attempt.Success)
            //{
            //    var operationResult = attempt.Result;
            //    var entityContainer = operationResult.Entity;

            //    operationResult.Entity.AddingEntity();

            //    var saveResult = _contentTypeService.SaveContainer(entityContainer);

            //    if (saveResult.Success)
            //    {
            //        //_contentTypeRepository.Get(entityContainer.Key);

            //        var docType = _contentTypeService.Get(key);

            //        var docTypes = _contentTypeService.GetAll();

            //        var types = _dataTypeService.GetAll();
            //        var type = _dataTypeService.GetByEditorAlias("TextString").FirstOrDefault();
            //        if(type != null)
            //        {
            //            PropertyType propertyType = new PropertyType(_shortStringHelper, type, "Title");
            //            docType.AddPropertyType(propertyType);
            //        }

            //        _contentTypeService.Save(docType);

            //        //var doctype = _contentTypeRepository.Get(entityContainer.Key);
            //        //var a = _contentTypeService.Get(entityContainer.Key);
            //        //doctype.Tabs

            //        return new ResponseModel()
            //        {
            //            Succes = true,
            //            Message = "Doctype has been created."
            //        };
            //    }

            //    message = saveResult.Exception?.Message;
            //}
            //else
            //{
            //    var exception = attempt.Exception;
            //    message = exception.Message;
            //}
        }
        private async Task<ContentType> HandleProperties(ContentType contentType, DoctypeViewModel contextModel)
        {
            var empty = _dataTypeService.GetDataType("Synthscribe.Empty");
            if(empty == null)
            {
                var label = _dataTypeService?.GetByEditorAlias(Constants.PropertyEditors.Aliases.Label)?.FirstOrDefault()?.Editor;
                if (label == null)
                    return contentType;

                DataType dt = new(label, _configurationEditorJsonSerializer);
                dt.Name = "Synthscribe.Empty";

                _dataTypeService.Save(dt);

                empty = _dataTypeService.GetDataType("Synthscribe.Empty");
            }
            foreach (var prop in contextModel.Properties)
            {
                if (prop == null)
                    continue;

                //Get property dataType
                //var abc = _dataTypeService.GetAll().Where(a => a.Name?.ToLower()?.Contains("string")??false);
                var datatype = _dataTypeService.GetDataType(prop.DataTypeAlias);

                if (datatype == null)
                    datatype = empty;

                //Create propertType
                var propType = new PropertyType(_shortStringHelper, datatype);
                propType.Name = prop.Name;
                propType.Alias = GetAlias(prop.Name);

                //Add propertyType to contentType
                contentType.AddPropertyType(propType);
            }

            return contentType;
        }

        private string GetAlias(string name)
        {
            var alias = name.ToSafeAlias(_shortStringHelper);
            alias = Char.ToLowerInvariant(alias[0]) + alias.Substring(1);

            return alias;
        }
    }
}
