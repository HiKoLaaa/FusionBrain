namespace FusionBrain.Services.Images.Exceptions;

public sealed class ProcessingStoppedException : Exception
{
    public ProcessingStoppedException(int processedAttempts) : base($"Processing stopped due to exhaustion of attempts in the amount of {processedAttempts}")
    {
    }
}