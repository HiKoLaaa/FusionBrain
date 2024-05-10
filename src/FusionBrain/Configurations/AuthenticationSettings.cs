namespace FusionBrain.Configurations;

public sealed class AuthenticationSettings
{
    public string ApiKey { get; init; } = string.Empty;

    public string SecretKey { get; init; } = string.Empty;
}