using Our.Umbraco.Synthscribe.OpenAi.Models;
using Our.Umbraco.Synthscribe.OpenAi.Services.Interfaces;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Features.ModelGeneration.Services;
internal sealed class ModelGenerationService : IModelGenerationService
{
    private readonly IChatGptService _chatGptService;

    public ModelGenerationService(IChatGptService chatGptService)
    {
        _chatGptService = chatGptService;
    }
    public async Task<string> GenerateViewModel(string html)
    {
        if (string.IsNullOrEmpty(html))
            return string.Empty;

        var response = await _chatGptService.CreateCompletion(new ChatGptCompletion()
        {
            Messages = [
                new ChatGptCompletionTextMessage()
                {
                    Role = ChatGptRoles.system.ToString(),
                    Content = "You are a tool that generates c# models based on html. \nStrictly return the c#, dont return any text.\n\nOur base models are:\n- ButtonViewModel\n- ImageViewModel\n- LinkViewModel"
                },
                new ChatGptCompletionTextMessage()
                {
                    Role = ChatGptRoles.user.ToString(),
                    Content = $"Give me a c# model for this html: {html}"
                }
            ]
        });

        return response;
    }
}
