using Microsoft.Extensions.Logging;
using Our.Umbraco.Synthscribe.OpenAi.Models;
using Our.Umbraco.Synthscribe.OpenAi.Services.Interfaces;
using Our.Umbraco.Synthscribe.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;

namespace Our.Umbraco.Synthscribe.Services
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
            var response = await _chatGptService.CreateCompletion(new OpenAi.Models.ChatGptCompletion()
            {
                Messages = new List<ChatGptCompletionMessage>()
                {
                    new ChatGptCompletionMessage()
                    {
                        Role = ChatGptRoles.system.ToString(),
                        Content = "You're a translator."
                    },
                    new ChatGptCompletionMessage()
                    {
                        Role = ChatGptRoles.user.ToString(),
                        Content = $"Translate \"{text}\" from {sourceLanguage.IsoCode} to {targetLanguage.IsoCode} and don't touch html tag names and only return the translated value and remove the quotes arround the text."
                    }
                }
            });

            return response;
        }
    }
}
