using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Pkcs;
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
using static Umbraco.Cms.Core.Constants;
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
                contentType = await AddProperties(contentType, model);

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
                Messages = new()
                {
                    new ChatGptCompletionTextMessage()
                    {
                        Role = ChatGptRoles.system.ToString(),
                        Content = "Act as a Umbraco tool that converts human language text into json objects. [Return only the main response. Remove pre-text and post-text]"
                    },
                    new ChatGptCompletionTextMessage()
                    {
                        Role = ChatGptRoles.user.ToString(),
                        Content = $"[Return only the main response. Remove pre-text and post-text] Generate a json object in format [{{\"Name\":\"string\",\"Properties\":[{{\"Name\":\"string\",\"Description\":\"string\",\"propertyTypeInfo\":\"string, info to choose wich property type to use for the property\"}}]}}] based on [{context}], add relevant names and descriptions into the properties."
                    }
                }
            });

            _logger.LogInformation($"Generate doctype: {response}");

            var vms = JsonConvert.DeserializeObject<List<DoctypeViewModel>>(response);

            var vm = vms.FirstOrDefault();

            if (vm != null)
            {
                var relevantIcon = await HandleIcon(vm.Name);

                vm.Icon = relevantIcon;

                vm.Properties = await HandleProperties(vm.Properties);
            }

            return vm;
        }

        private async Task<string> HandleIcon(string context)
        {

            var response = await _chatGptService.CreateCompletion(new ChatGptCompletion()
            {
                Messages = new()
                {
                    new ChatGptCompletionTextMessage()
                    {
                        Role = ChatGptRoles.system.ToString(),
                        Content = "Act as a tool that returns best fitting icon name based on page name. [Return only the main response. Remove pre-text and post-text]"
                    },
                    new ChatGptCompletionTextMessage()
                    {
                        Role = ChatGptRoles.user.ToString(),
                        Content = $"[Return only the main response. Remove pre-text and post-text] Generate a json object in format [{{\"icon\":\"icon name\"}}] based on page name: \"{context}\". List of possible icon names: [\"zoom-out\",\"truck\",\"zoom-in\",\"zip\",\"axis-rotation\",\"yen-bag\",\"axis-rotation-2\",\"axis-rotation-3\",\"wrench\",\"wine-glass\",\"wrong\",\"windows\",\"window-sizes\",\"window-popin\",\"wifi\",\"width\",\"weight\",\"war\",\"wand\",\"wallet\",\"wall-plug\",\"voice\",\"video\",\"vcard\",\"utilities\",\"users\",\"users-alt\",\"user\",\"user-glasses\",\"user-females\",\"user-females-alt\",\"user-female\",\"usb\",\"usb-connector\",\"unlocked\",\"universal\",\"undo\",\"umbrella\",\"umb-deploy\",\"umb-contour\",\"umb-settings\",\"umb-users\",\"umb-media\",\"umb-content\",\"umb-developer\",\"umb-members\",\"umb-translation\",\"tv\",\"tv-old\",\"trophy\",\"tree\",\"trash\",\"trash-alt\",\"trash-alt-2\",\"train\",\"trafic\",\"traffic-alt\",\"top\",\"tools\",\"timer\",\"time\",\"t-shirt\",\"tab-key\",\"tab\",\"tactics\",\"tag\",\"tags\",\"takeaway-cup\",\"target\",\"temperatrure-alt\",\"temperature\",\"terminal\",\"theater\",\"theif\",\"thought-bubble\",\"thumb-down\",\"thumb-up\",\"thumbnail-list\",\"thumbnails-small\",\"thumbnails\",\"ticket\",\"sync\",\"sweatshirt\",\"sunny\",\"stream\",\"store\",\"stop\",\"stop-hand\",\"stop-alt\",\"stamp\",\"stacked-disks\",\"ssd\",\"squiggly-line\",\"sprout\",\"split\",\"split-alt\",\"speed-gauge\",\"speaker\",\"sound\",\"spades\",\"sound-waves\",\"shipping-box\",\"shipping\",\"shoe\",\"shopping-basket-alt-2\",\"shopping-basket\",\"shopping-basket-alt\",\"shorts\",\"shuffle\",\"sience\",\"simcard\",\"single-note\",\"sitemap\",\"sleep\",\"slideshow\",\"smiley-inverted\",\"smiley\",\"snow\",\"sound-low\",\"sound-medium\",\"sound-off\",\"shift\",\"shield\",\"sharing-iphone\",\"share\",\"share-alt\",\"share-alt-2\",\"settings\",\"settings-alt\",\"settings-alt-2\",\"server\",\"server-alt\",\"sensor\",\"security-camera\",\"search\",\"scull\",\"script\",\"script-alt\",\"screensharing\",\"school\",\"scan\",\"refresh\",\"remote\",\"remove\",\"repeat-one\",\"repeat\",\"resize\",\"reply-arrow\",\"return-to-top\",\"right-double-arrow\",\"road\",\"roadsign\",\"rocket\",\"rss\",\"ruler-alt\",\"ruler\",\"sandbox-toys\",\"satellite-dish\",\"save\",\"safedial\",\"safe\",\"redo\",\"printer-alt\",\"planet\",\"paste-in\",\"os-x\",\"navigation-left\",\"message\",\"lock\",\"layers-alt\",\"record\",\"print\",\"plane\",\"partly-cloudy\",\"ordered-list\",\"navigation-last\",\"message-unopened\",\"location-nearby\",\"laptop\",\"reception\",\"price-yen\",\"piracy\",\"parental-control\",\"operator\",\"navigation-horizontal\",\"message-open\",\"lab\",\"location-near-me\",\"receipt-yen\",\"price-pound\",\"pin-location\",\"parachute-drop\",\"old-phone\",\"merge\",\"navigation-first\",\"locate\",\"keyhole\",\"receipt-pound\",\"price-euro\",\"piggy-bank\",\"paper-plane\",\"old-key\",\"navigation-down\",\"megaphone\",\"loading\",\"keychain\",\"receipt-euro\",\"price-dollar\",\"pie-chart\",\"paper-plane-alt\",\"notepad\",\"navigation-bottom\",\"meeting\",\"keyboard\",\"load\",\"receipt-dollar\",\"previous\",\"pictures\",\"notepad-alt\",\"paper-bag\",\"name-badge\",\"medicine\",\"list\",\"key\",\"receipt-alt\",\"previous-media\",\"pictures-alt\",\"pants\",\"nodes\",\"music\",\"readonly\",\"presentation\",\"pictures-alt-2\",\"pannel-close\",\"next\",\"multiple-windows\",\"medical-emergency\",\"medal\",\"link\",\"linux-tux\",\"junk\",\"item-arrangement\",\"iphone\",\"lightning\",\"map\",\"multiple-credit-cards\",\"next-media\",\"panel-show\",\"picture\",\"power\",\"re-post\",\"rate\",\"rain\",\"radio\",\"radio-receiver\",\"radio-alt\",\"quote\",\"qr-code\",\"pushpin\",\"pulse\",\"projector\",\"play\",\"playing-cards\",\"playlist\",\"plugin\",\"podcast\",\"poker-chip\",\"poll\",\"post-it\",\"pound-bag\",\"power-outlet\",\"photo-album\",\"phone\",\"phone-ring\",\"people\",\"people-female\",\"people-alt\",\"people-alt-2\",\"pc\",\"pause\",\"path\",\"out\",\"outbox\",\"outdent\",\"page-add\",\"page-down\",\"page-remove\",\"page-restricted\",\"page-up\",\"paint-roller\",\"palette\",\"newspaper\",\"newspaper-alt\",\"network-alt\",\"navigational-arrow\",\"navigation\",\"navigation-vertical\",\"navigation-up\",\"navigation-top\",\"navigation-road\",\"navigation-right\",\"microscope\",\"mindmap\",\"molecular-network\",\"molecular\",\"mountain\",\"mouse-cursor\",\"mouse\",\"movie-alt\",\"map-marker\",\"movie\",\"map-loaction\",\"map-alt\",\"male-symbol\",\"male-and-female\",\"mailbox\",\"magnet\",\"loupe\",\"mobile\",\"logout\",\"log-out\",\"layers\",\"left-double-arrow\",\"layout\",\"legal\",\"lense\",\"library\",\"light-down\",\"light-up\",\"lightbulb-active\",\"lightbulb\",\"ipad\",\"invoice\",\"info\",\"infinity\",\"indent\",\"inbox\",\"inbox-full\",\"inactive-line\",\"imac\",\"hourglass\",\"home\",\"grid\",\"food\",\"favorite\",\"door-open-alt\",\"diagnostics\",\"contrast\",\"coins-dollar-alt\",\"circle-dotted-active\",\"cinema\",\"chip\",\"chip-alt\",\"chess\",\"checkbox\",\"checkbox-empty\",\"checkbox-dotted\",\"checkbox-dotted-active\",\"check\",\"chat\",\"chat-active\",\"chart\",\"chart-curve\",\"certificate\",\"categories\",\"cash-register\",\"car\",\"caps-lock\",\"candy\",\"circle-dotted\",\"circuits\",\"circus\",\"client\",\"clothes-hanger\",\"cloud-drive\",\"cloud-upload\",\"cloud\",\"cloudy\",\"clubs\",\"cocktail\",\"code\",\"coffee\",\"coin-dollar\",\"coin-pound\",\"coin-yen\",\"coin\",\"coins-alt\",\"console\",\"connection\",\"compress\",\"company\",\"command\",\"coin-euro\",\"combination-lock\",\"combination-lock-open\",\"comb\",\"columns\",\"colorpicker\",\"color-bucket\",\"coins\",\"coins-yen\",\"coins-yen-alt\",\"coins-pound\",\"coins-pound-alt\",\"coins-euro\",\"coins-euro-alt\",\"coins-dollar\",\"conversation-alt\",\"conversation\",\"coverflow\",\"credit-card-alt\",\"credit-card\",\"crop\",\"crosshair\",\"crown-alt\",\"crown\",\"cupcake\",\"curve\",\"cut\",\"dashboard\",\"defrag\",\"delete\",\"delete-key\",\"departure\",\"desk\",\"desktop\",\"donate\",\"dollar-bag\",\"documents\",\"document\",\"document-dashed-line\",\"dock-connector\",\"dna\",\"display\",\"disk-image\",\"disc\",\"directions\",\"directions-alt\",\"diploma\",\"diploma-alt\",\"dice\",\"diamonds\",\"diamond\",\"diagonal-arrow\",\"diagonal-arrow-alt\",\"door-open\",\"download-alt\",\"download\",\"drop\",\"eco\",\"economy\",\"edit\",\"eject\",\"employee\",\"energy-saving-bulb\",\"enter\",\"equalizer\",\"escape\",\"ethernet\",\"euro-bag\",\"exit-fullscreen\",\"eye\",\"facebook-like\",\"factory\",\"font\",\"folders\",\"folder\",\"folder-outline\",\"folder-open\",\"flowerpot\",\"flashlight\",\"flash\",\"flag\",\"flag-alt\",\"firewire\",\"firewall\",\"fire\",\"fingerprint\",\"filter\",\"filter-arrows\",\"files\",\"file-cabinet\",\"female-symbol\",\"footprints\",\"hammer\",\"hand-active-alt\",\"forking\",\"hand-active\",\"hand-pointer-alt\",\"hand-pointer\",\"handprint\",\"handshake\",\"handtool\",\"hard-drive\",\"help\",\"graduate\",\"gps\",\"help-alt\",\"height\",\"globe\",\"hearts\",\"globe-inverted-europe-africa\",\"headset\",\"globe-inverted-asia\",\"headphones\",\"globe-inverted-america\",\"hd\",\"globe-europe---africa\",\"hat\",\"globe-asia\",\"globe-alt\",\"hard-drive-alt\",\"glasses\",\"gift\",\"handtool-alt\",\"geometry\",\"game\",\"fullscreen\",\"fullscreen-alt\",\"frame\",\"frame-alt\",\"camera-roll\",\"bookmark\",\"bill\",\"baby-stroller\",\"alarm-clock\",\"adressbook\",\"add\",\"activity\",\"untitled\",\"camcorder\",\"calendar\",\"calendar-alt\",\"calculator\",\"bus\",\"burn\",\"bulleted-list\",\"bug\",\"brush\",\"brush-alt\",\"brush-alt-2\",\"browser-window\",\"briefcase\",\"brick\",\"brackets\",\"box\",\"box-open\",\"box-alt\",\"books\",\"billboard\",\"bills-dollar\",\"bills-euro\",\"bills-pound\",\"bills-yen\",\"bills\",\"binarycode\",\"binoculars\",\"bird\",\"birthday-cake\",\"blueprint\",\"block\",\"bluetooth\",\"boat-shipping\",\"bomb\",\"book-alt-2\",\"bones\",\"book-alt\",\"book\",\"bill-yen\",\"award\",\"bill-pound\",\"autofill\",\"bill-euro\",\"auction-hammer\",\"bill-dollar\",\"attachment\",\"bell\",\"article\",\"bell-off\",\"art-easel\",\"beer-glass\",\"arrow-up\",\"battery-low\",\"arrow-right\",\"battery-full\",\"arrow-left\",\"bars\",\"arrow-down\",\"barcode\",\"arrivals\",\"bar-chart\",\"application-window\",\"band-aid\",\"application-window-alt\",\"ball\",\"application-error\",\"badge-restricted\",\"app\",\"badge-remove\",\"anchor\",\"badge-count\",\"alt\",\"badge-add\",\"alert\",\"backspace\",\"alert-alt\"]. [Only return the json object!]"
                    }
                }
            });

            _logger.LogInformation($"Generate icon: {response}");

            var vms = JsonConvert.DeserializeObject<List<DoctypeIconViewModel>>(response);

            var vm = vms.FirstOrDefault();

            var icon = "";

            if(vm == null || string.IsNullOrEmpty(vm.Icon))
                icon = "icon-document";
            else
            {
                vm.Icon = $"icon-{vm.Icon}";
                if (SynthscribeConstants.UmbracoIconNames.Contains(vm.Icon))
                    icon = vm.Icon;
                else
                    icon = "icon-document";
            }

            return icon;
        }

        private async Task<List<GenerateProptypeViewModel>> HandleProperties(List<GenerateProptypeViewModel> properties)
        {
            foreach (var property in properties)
            {
                property.PropertyEditorAlias = await HandlePropertyEditor(property);
            }

            return properties;
        }

        private async Task<string> HandlePropertyEditor(GenerateProptypeViewModel property)
        {
            //TODO: Maybe put this next line on startup & put data in globally accessible list.
            var existingPropertyEditorAliases = (typeof(PropertyEditors.Aliases).GetFields().Select(field => field?.GetValue(null)?.ToString())).ToList();

            string editorsString = "";
            if (existingPropertyEditorAliases.Any())
                editorsString = string.Join(", ", existingPropertyEditorAliases);

            var response = await _chatGptService.CreateCompletion(new ChatGptCompletion()
            {
                Messages = new ()
                {
                    new ChatGptCompletionTextMessage()
                    {
                        Role = ChatGptRoles.system.ToString(),
                        Content = "Act as a Umbraco tool that returns best fitting property editor alias based on property name & description. [Return only the main response. Remove pre-text and post-text]"
                    },
                    new ChatGptCompletionTextMessage()
                    {
                        Role = ChatGptRoles.user.ToString(),
                        Content = $"[Return only the main response. Remove pre-text and post-text] Generate a json object in format \"[{{\"propertyEditorAlias\":\"Umbraco Property Editor Alias\"}}]\" based on property name \"{property.Name}\", description \"{property.Description}\" and extra information about the type \"{property.PropertyTypeInfo}\". List of possible aliasses: [{editorsString}] [Only return the json object!]"
                    }
                }
            }); 

            _logger.LogInformation($"Generate propertyeditor alias: {response}");

            var vms = JsonConvert.DeserializeObject<List<PropertyTypeAlias>>(response);

            var vm = vms.FirstOrDefault();

            if (vm == null || string.IsNullOrEmpty(vm.PropertyEditorAlias))
                return null;

            if (existingPropertyEditorAliases.FirstOrDefault(x => x == vm.PropertyEditorAlias)!=null)
                return vm.PropertyEditorAlias;
            else
                return "Umbraco.TextBox";

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
        private async Task<ContentType> AddProperties(ContentType contentType, DoctypeViewModel contextModel)
        {
            //If doesn't exist, create fallback data type
            var empty = _dataTypeService.GetDataType("Synthscribe.Empty");
            if(empty == null)
            {
                var fallbackDataType = _dataTypeService?.GetByEditorAlias(Constants.PropertyEditors.Aliases.Label)?.FirstOrDefault()?.Editor;
                if (fallbackDataType == null)
                    return contentType;

                DataType dt = new(fallbackDataType, _configurationEditorJsonSerializer);
                dt.Name = "Synthscribe.Empty";

                _dataTypeService.Save(dt);

                empty = _dataTypeService.GetDataType("Synthscribe.Empty");
            }

            //Iterate over list of property information
            foreach (var prop in contextModel.Properties)
            {
                if (prop == null)
                    continue;

                //Get relevant data type
                var relevantDataType = await GetRelevantDataType(prop);

                if (relevantDataType == null)
                    relevantDataType = empty;

                //Create propertType
                var propType = new PropertyType(_shortStringHelper, relevantDataType);
                propType.Name = prop.Name;
                propType.Alias = GetAlias(prop.Name);

                //Reserved name handl..
                if (propType.Alias == "name")
                    propType.Alias = $"{GetAlias(contextModel.Name)}Name";

                //Add propertyType to contentType
                contentType.AddPropertyType(propType);
            }

            return contentType;
        }

        private async Task<IDataType> GetRelevantDataType(GenerateProptypeViewModel vm)
        {
            //Get property propertyEditorAlias
            var selectedDataTypes = _dataTypeService.GetByEditorAlias(vm.PropertyEditorAlias);

            var names = selectedDataTypes.Select(dt => dt.Name);

            if (selectedDataTypes == null)
                return null;

            ////Check if names length > 0

            var response = await _chatGptService.CreateCompletion(new ChatGptCompletion()
            {
                Messages = new()
                {
                    new ChatGptCompletionTextMessage()
                    {
                        Role = ChatGptRoles.system.ToString(),
                        Content = "Act as a Umbraco tool that returns best fitting DataType name based on property name, description & propertyTypeAlias, choose from list of existing names [Return only the main response. Remove pre-text and post-text]"
                    },
                    new ChatGptCompletionTextMessage()
                    {
                        Role = ChatGptRoles.user.ToString(),
                        Content = $"[Return only the main response. Remove pre-text and post-text] Generate a json object in format {{\"dataTypeName\":\"string, data type name from the list of supported types\"}} based on this data: [name: {vm.Name}, description: {vm.Description}], here is a list of existing data types: {string.Join(", ", names)}, if non is relevant suggest a relevant name. [Only return the json object!]"
                    }
                }
            });

            var obj = JsonConvert.DeserializeObject<GenerateDataTypeViewmodel>(response);

            IDataType relevantDataType = null;

            if (!string.IsNullOrEmpty(obj.DataTypeName) && names.Contains(obj.DataTypeName))
            {
                relevantDataType = _dataTypeService.GetDataType(obj.DataTypeName);
            }

            if (relevantDataType == null)
            {
                //Create new
                var dtName = $"Synthscribe.{obj.DataTypeName??vm.Name}";

                //Check if doest exist
                relevantDataType = _dataTypeService.GetDataType(dtName);

                //If not, create
                if (relevantDataType == null)
                {
                    var editorToUse = _dataTypeService?.GetByEditorAlias(vm.PropertyEditorAlias)?.FirstOrDefault()?.Editor;
                    if (editorToUse == null) return null;

                    DataType newDt = new(editorToUse, _configurationEditorJsonSerializer);
                    newDt.Name = dtName;

                    _dataTypeService.Save(newDt);

                    relevantDataType = _dataTypeService.GetDataType(dtName);
                }
            }

            return relevantDataType;
        }

        private string GetAlias(string name)
        {
            var alias = name.ToSafeAlias(_shortStringHelper);
            alias = Char.ToLowerInvariant(alias[0]) + alias.Substring(1);

            return alias;
        }
    }
}
