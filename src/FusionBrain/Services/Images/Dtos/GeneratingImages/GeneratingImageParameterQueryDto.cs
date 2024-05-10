using System.Text.Json.Serialization;

namespace FusionBrain.Services.Images.Dtos.GeneratingImages;

internal sealed class GeneratingImageParameterQueryDto
{
    [JsonPropertyName("query")]
    public string? Query { get; init; }
}