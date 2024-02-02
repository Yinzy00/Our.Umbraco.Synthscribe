using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Features.HtmlGeneration.Services;

internal interface IHtmlGenerationService
{
    public Task<string> GenerateHtml(string base64);
}