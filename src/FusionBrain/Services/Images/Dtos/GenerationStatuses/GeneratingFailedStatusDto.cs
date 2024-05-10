using System.Text.Json.Serialization;

namespace FusionBrain.Services.Images.Dtos.GenerationStatuses;

internal sealed class GeneratingFailedStatusDto
{
    [JsonPropertyName("model_status")]
    public string Status { get; init; } = null!;
}