using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Our.Umbraco.Synthscribe.Features.TextGeneration.Service;
using Our.Umbraco.Synthscribe.Features.Translation.Content.Service;
using Our.Umbraco.Synthscribe.Features.Translation.Dictionary.Service;
using Our.Umbraco.Synthscribe.Features.Translation.Service;
using Our.Umbraco.Synthscribe.NotificationHandlers;
using Our.Umbraco.Synthscribe.OpenAi.Services;
using Our.Umbraco.Synthscribe.OpenAi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace Our.Umbraco.Synthscribe.Composers
{
    internal class RegisterComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            //Dep injection ChatpGpT service
            builder.Services.AddTransient<IChatGptService, ChatGptService>(sp =>
            {
                var config = sp.GetService<IConfiguration>();

                return new ChatGptService(config.GetValue<string>("OpenAi:apiKey"));
            });
        }
    }
}
