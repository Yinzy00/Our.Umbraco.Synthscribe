using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.Synthscribe.Features.TextGeneration.Service;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Our.Umbraco.Synthscribe.Features.TextGeneration
{
    internal class TextGenerationComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddTransient<ITextContentService, TextContentService>();
        }
    }
}
