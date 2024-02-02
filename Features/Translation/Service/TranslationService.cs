using Our.Umbraco.Synthscribe.OpenAi.Models;
using Our.Umbraco.Synthscribe.OpenAi.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;

namespace Our.Umbraco.Synthscribe.Features.Translation.Service
{
    internal class TranslationService : ITranslationService
    {
        private readonly IChatGptService _chatGptService;

        public TranslationService(IChatGptService chatGptService)
        {
            _chatGptService = chatGptService;
        }
        public async Task<string> Translate(string text, ILanguage sourceLanguage, ILanguage targetLanguage)
        {
            var response = await _chatGptService.CreateCompletion(new ChatGptCompletion()
            {
                Messages = new()
                {
                    new ChatGptCompletionTextMessage()
                    {
                        Role = ChatGptRoles.system.ToString(),
                        Content = "You're a translator. You can only return the translated value."
                    },
                    new ChatGptCompletionTextMessage()
                    {
                        Role = ChatGptRoles.user.ToString(),
                        Content = $"[Return only the main response. Remove pre-text and post-text] Translate \"{text}\" from {sourceLanguage.IsoCode} to {targetLanguage.IsoCode} and don't touch html tag names and only return the translated value and remove the quotes arround the text."
                    }
                }
            });

            return response;
        }
    }
}
