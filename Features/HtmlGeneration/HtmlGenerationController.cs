using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Our.Umbraco.Synthscribe.Features.HtmlGeneration.Models;
using Our.Umbraco.Synthscribe.Features.HtmlGeneration.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Controllers;

namespace Our.Umbraco.Synthscribe.Features.HtmlGeneration;
[PluginController("Synthscribe")]
public sealed class HtmlGenerationController : UmbracoApiController
{
    private readonly IHtmlGenerationService _htmlGenerationService;

    public HtmlGenerationController(IHtmlGenerationService htmlGenerationService)
    {
        _htmlGenerationService = htmlGenerationService;
    }
    [HttpGet]
    public async Task<IActionResult> Test()
    {
        return Ok("Ok!");
    }
    [HttpPost]
    public async Task<IActionResult> GenerateHtml(IFormFile formFile)
    {
        using MemoryStream ms = new();
        formFile.CopyTo(ms);
        var bytes = ms.ToArray();
        var base64 = $"data:{formFile.ContentType};base64,{Convert.ToBase64String(bytes)}";
        var response = await _htmlGenerationService.GenerateHtml(base64);

        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> GenerateRazor([FromBody] RazorGenerationRequestModel data)
    {
        var response = await _htmlGenerationService.GenerateRazor(data.Html, data.ViewModel);

        return Ok(response);
    }
}
