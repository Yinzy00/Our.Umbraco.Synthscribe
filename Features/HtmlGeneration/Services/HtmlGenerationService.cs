using Our.Umbraco.Synthscribe.OpenAi.Models;
using Our.Umbraco.Synthscribe.OpenAi.Services.Interfaces;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Our.Umbraco.Synthscribe.Features.HtmlGeneration.Services;

internal sealed class HtmlGenerationService : IHtmlGenerationService
{
    private readonly IChatGptService _chatGptService;

    public HtmlGenerationService(IChatGptService chatGptService)
    {
        _chatGptService = chatGptService;
    }
    public async Task<string> GenerateHtml(string base64)
    {
        if (string.IsNullOrEmpty(base64))
            return string.Empty;

        var response = await _chatGptService.CreateCompletion(new ChatGptCompletion()
        {
            Messages = new()
                {
                    new ChatGptCompletionTextMessage()
                    {
                        Role = ChatGptRoles.system.ToString(),
                        Content = "You are a tool that generates plain html for web design blocks. \nYou only returen html, no css. You also only return the html strictly for the given image, not the html, head & body tags. \nAlso dont return any plain text, strictly return the html. \nDon't put the html in markup a code block. \n\nUse these css classes for the base components: \nButton>.c-button\nInput>.c-input\n\nIf you add other classes, put them in the same format 'c-*'."
                    },
                    new ChatGptCompletionImageMessage()
                    {
                        Role = ChatGptRoles.user.ToString(),
                        Content = new()
                        {
                            new()
                            {
                                Type = ImageMessageType.text,
                                Value = "Give me html for this webdesign block. {\"name\": \"Hero\"}"
                            },
                            new()
                            {
                                Type = ImageMessageType.image_url,
                                Value = base64
                            }
                        }
                    }
                }
        });

        return response;
    }
}