using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.Synthscribe.Features.Translation.Content.Service;
using Our.Umbraco.Synthscribe.Features.Translation.Dictionary.Service;
using Our.Umbraco.Synthscribe.Features.Translation.Service;
using Our.Umbraco.Synthscribe.NotificationHandlers;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace Our.Umbraco.Synthscribe.Features.Translation
{
    internal class TranslationComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddTransient<ITranslationService, TranslationService>();
            builder.Services.AddTransient<ITranslateDictionaryService, TranslateDictionaryService>();
            builder.Services.AddTransient<ITranslateContentService, TranslateContentService>();

            //Add translate button to action menu
            builder.AddNotificationHandler<MenuRenderingNotification, MenuRenderingNotificationHandler>();
        }
    }
}
