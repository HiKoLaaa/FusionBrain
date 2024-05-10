using System.Text.Json.Serialization;

namespace FusionBrain.Services.Images.Dtos;

internal sealed class ModelDto
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;

    [JsonPropertyName("version")]
    public float Version { get; init; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;
}