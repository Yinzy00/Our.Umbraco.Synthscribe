using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.Synthscribe.Features.ModelGeneration.Services;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Our.Umbraco.Synthscribe.Features.ModelGeneration;
internal sealed class ModelGenerationComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddTransient<IModelGenerationService, ModelGenerationService>();
    }
}
