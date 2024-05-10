namespace FusionBrain.Configurations;

public sealed class CheckGenerationSetting
{
    private const int DefaultAttempts = 10;

    private static readonly TimeSpan DefaultDelay = TimeSpan.FromSeconds(10);

    public int Attempts { get; init; } = DefaultAttempts;

    public TimeSpan Delay { get; init; } = DefaultDelay;
}