using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Features.HtmlGeneration.Services;

public interface IHtmlGenerationService
{
    /// <summary>
    /// Generates html based on the base64 of an image
    /// </summary>
    /// <param name="base64"></param>
    /// <returns></returns>
    public Task<string> GenerateHtml(string base64);
    /// <summary>
    /// Generates razor based on html & a viewmodel
    /// </summary>
    /// <param name="html"></param>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public Task<string> GenerateRazor(string html, string viewModel);
}