using System.Text.Json.Serialization;
using FusionBrain.Domain.Primitives;
using FusionBrain.Services.Images.Dtos.Serializers;

namespace FusionBrain.Services.Images.Dtos.GeneratingImages;

internal sealed class GeneratingImageParametersDto
{
    [JsonPropertyName("width")]
    public int Width { get; init; }

    [JsonPropertyName("height")]
    public int Height { get; init; }

    [JsonPropertyName("num_images")]
    public int NumberOfImages { get; init; }

    [JsonPropertyName("negativePromptUnclip")]
    public string? ExcludingResultQuery { get; init; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(UpperCaseJsonStringEnumConverter))]
    public GenerationMode Mode { get; init; }

    [JsonPropertyName("style")]
    [JsonConverter(typeof(UpperCaseJsonStringEnumConverter))]
    public Style Style { get; init; }

    [JsonPropertyName("generateParams")]
    public GeneratingImageParameterQueryDto? Parameters { get; init; }
}