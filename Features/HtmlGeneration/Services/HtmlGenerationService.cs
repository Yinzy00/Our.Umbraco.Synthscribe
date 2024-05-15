using Our.Umbraco.Synthscribe.OpenAi.Models;
using Our.Umbraco.Synthscribe.OpenAi.Services.Interfaces;
using System.Threading.Tasks;

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
            return "Base64 is null or empty";

        var response = await _chatGptService.CreateCompletion(new ChatGptCompletion("gpt-4-vision-preview")
        {
            Messages =
                [
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
                ]
        });

        return response;
    }

    public async Task<string> GenerateRazor(string html, string viewModel)
    {
        if (string.IsNullOrEmpty(html) || string.IsNullOrEmpty(viewModel))
            return "Html or viewmodel is null or empty";

        var response = await _chatGptService.CreateCompletion(new ChatGptCompletion()
        {
            Messages = [
                new ChatGptCompletionTextMessage()
                {
                    Role = ChatGptRoles.system.ToString(),
                    Content = "You are a tool that generates razor code based on html and a viewmodel. \nReplace all text with properties from the viewModel.\nAlso add a using statement for the viewmodel using the namespace of the viewmodel.\nOnly return the viewmodel, don't ever return any plain text."
                },
                new ChatGptCompletionTextMessage()
                {
                    Role = ChatGptRoles.user.ToString(),
                    Content = $"Give me razor code based on this html: {html} ; and this viewModel: {viewModel} ;"
                }
            ]
        });

        return response;
    }
}