namespace FusionBrain.Services.Images.Exceptions;

public sealed class ProcessingUnavailableException : Exception
{
    public string UnavailableStatus { get; }

    public ProcessingUnavailableException(string unavailableStatus) : base($"Processing is unavailable. Current status: {unavailableStatus}")
    {
        UnavailableStatus = unavailableStatus;
    }
}