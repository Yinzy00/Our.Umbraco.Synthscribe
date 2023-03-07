using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Our.Umbraco.Synthscribe.OpenAi.Services;
using Our.Umbraco.Synthscribe.OpenAi.Services.Interfaces;
using Our.Umbraco.Synthscribe.Services;
using Our.Umbraco.Synthscribe.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Our.Umbraco.Synthscribe.Composers
{
    internal class RegisterComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddTransient<IChatGptService, ChatGptService>(sp =>
            {
                var config = sp.GetService<IConfiguration>();

                return new ChatGptService(config.GetValue<string>("OpenAi:apiKey"));
            });
            builder.Services.AddTransient<ITextContentService, TextContentService>();
        }
    }
}
