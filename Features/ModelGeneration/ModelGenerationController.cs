using Microsoft.AspNetCore.Mvc;
using Our.Umbraco.Synthscribe.Features.ModelGeneration.Models;
using Our.Umbraco.Synthscribe.Features.ModelGeneration.Services;
using System.Threading.Tasks;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Controllers;

namespace Our.Umbraco.Synthscribe.Features.ModelGeneration;
[PluginController("Synthscribe")]
public sealed class ModelGenerationController(IModelGenerationService modelGenerationService) 
    : UmbracoApiController
{
    private readonly IModelGenerationService _modelGenerationService = modelGenerationService;

    [HttpPost]
    public async Task<IActionResult> GenerateModel([FromBody] ModelGenerationRequestModel data)
    {
        var model = await _modelGenerationService.GenerateViewModel(data.Html);
        return Ok(model);
    }
}
