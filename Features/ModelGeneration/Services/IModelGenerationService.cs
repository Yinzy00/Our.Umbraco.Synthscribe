using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Features.ModelGeneration.Services;
public interface IModelGenerationService
{
    /// <summary>
    /// Generates a viewmodel from html
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public Task<string> GenerateViewModel(string html);
}
