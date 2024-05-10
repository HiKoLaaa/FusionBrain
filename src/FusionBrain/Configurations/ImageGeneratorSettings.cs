namespace FusionBrain.Configurations;

public sealed class ImageGeneratorSettings
{
    public string BaseUrl { get; init; } = null!;

    public AuthenticationSettings Authentication { get; init; } = new();

    public CheckGenerationSetting CheckGeneration { get; init; } = new();
}