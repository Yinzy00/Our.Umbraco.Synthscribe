using Microsoft.AspNetCore.Mvc;
using Our.Umbraco.Synthscribe.Features.TextGeneration.Model;
using Our.Umbraco.Synthscribe.Features.TextGeneration.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace Our.Umbraco.Synthscribe.Features.TextGeneration
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
        public async Task<IActionResult> GenerateText([FromBody] GenerateTextViewModel vm)
        {
            var text = await _textContentService.GetText(vm.Context);

            if (text.StartsWith('"') && text.EndsWith('"'))
            {
                var end = text.Length - 2;
                if (end > 1)
                {
                    text = text.Substring(1, end);
                }
            }
            return Ok(text);
        }

    }
}
