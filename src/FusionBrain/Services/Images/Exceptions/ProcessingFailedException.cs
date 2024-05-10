namespace FusionBrain.Services.Images.Exceptions;

public sealed class ProcessingFailedException : Exception
{
    public ProcessingFailedException(string failReason) : base($"Processing stopped due to error: {failReason}")
    {
    }
}