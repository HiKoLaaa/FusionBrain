using System.Text.Json.Serialization;

namespace FusionBrain.Services.Images.Dtos.GeneratingImages;

internal sealed class GeneratingImageDto
{
    [JsonPropertyName("model_id")]
    public int ModelId { get; init; }

    [JsonPropertyName("params")]
    public GeneratingImageParametersDto? Parameters { get; init; }
}