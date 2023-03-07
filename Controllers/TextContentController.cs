using Microsoft.AspNetCore.Mvc;
using Our.Umbraco.Synthscribe.Models.ViewModels;
using Our.Umbraco.Synthscribe.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace Our.Umbraco.Synthscribe.Controllers
{
    [PluginController("Synthscribe")]
    public class TextContentController : UmbracoAuthorizedApiController
    {

        private readonly ITextContentService _textContentService;

        public TextContentController(ITextContentService textContentService)
        {
            _textContentService = textContentService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateText([FromBody]GenerateTextViewModel vm)
        {
            return Ok(await _textContentService.GetText(vm.Context));
        }

    }
}
