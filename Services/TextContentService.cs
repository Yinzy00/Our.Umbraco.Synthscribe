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
                        Content = "Your only purpose is generating text for a website. Generate text in the language of the given context."
                    },
                    new ChatGptCompletionMessage()
                    {
                        Role = ChatGptRoles.user.ToString(),
                        Content = $"Context: \"{context}\""
                    }
                }
            });

            return response??"No return value...";
        }
    }
}
