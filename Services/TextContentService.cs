using Our.Umbraco.Synthscribe.OpenAi.Models;
using Our.Umbraco.Synthscribe.OpenAi.Services.Interfaces;
using Our.Umbraco.Synthscribe.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Services
{
    internal class TextContentService : ITextContentService
    {

        private readonly IChatGptService _chatGptService;

        public TextContentService(IChatGptService chatGptService)
        {
            _chatGptService = chatGptService;
        }

        public async Task<string> GetText(string context)
        {
            var response = await _chatGptService.CreateCompletion(new ChatGptCompletion()
            {
                Messages = new List<ChatGptCompletionMessage>()
                {
                    new ChatGptCompletionMessage()
                    {
                        Role = ChatGptRoles.system.ToString(),
                        Content = "You're a text generator for websites."
                    },
                    new ChatGptCompletionMessage()
                    {
                        Role = ChatGptRoles.user.ToString(),
                        Content = $"{context}, Return only the requested text. Return text in the language of the message. And return in 1 answer, no multiple option."
                    }
                }
            });

                return response;
        }
    }
}
