using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.Synthscribe.Features.HtmlGeneration.Services;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Our.Umbraco.Synthscribe.Features.HtmlGeneration;
internal sealed class HtmlGenerationComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddTransient<IHtmlGenerationService, HtmlGenerationService>();
    }
}
