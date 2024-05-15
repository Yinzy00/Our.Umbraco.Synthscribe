using Newtonsoft.Json;

namespace Our.Umbraco.Synthscribe.Features.ModelGeneration.Models;
public sealed class ModelGenerationRequestModel
{
    [JsonProperty("html")]
    public string Html { get; private set; }
}
