using System.Text.Json.Serialization;
using FusionBrain.Domain.Primitives;
using FusionBrain.Services.Images.Dtos.Serializers;

namespace FusionBrain.Services.Images.Dtos.GenerationStatuses;

internal sealed class GeneratingSuccessStatusDto
{
    [JsonPropertyName("uuid")]
    public Guid ProcessingId { get; init; }

    [JsonPropertyName("status")]
    [JsonConverter(typeof(UpperCaseJsonStringEnumConverter))]
    public ProcessingStatus Status { get; init; }

    [JsonPropertyName("images")]
    public string[]? Images { get; init; }

    [JsonPropertyName("errorDescription")]
    public string? Error { get; set; }

    [JsonPropertyName("censored")]
    public bool IsCensored { get; set; }
}