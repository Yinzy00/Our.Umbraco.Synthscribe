using Newtonsoft.Json;

namespace Our.Umbraco.Synthscribe.Features.HtmlGeneration.Models;
public sealed class RazorGenerationRequestModel
{
    [JsonProperty("html")]
    public string Html { get; private set; }
    [JsonProperty("viewModel")]
    public string ViewModel { get; private set; }
}
