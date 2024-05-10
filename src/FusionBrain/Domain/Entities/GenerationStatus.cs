using FusionBrain.Domain.Primitives;

namespace FusionBrain.Domain.Entities;

public class GenerationStatus
{
    public Guid ProcessingId { get; }

    public ProcessingStatus Status { get; }

    public byte[][]? Images { get; }

    public GenerationStatus(Guid processingId, ProcessingStatus status, byte[][]? images)
    {
        ProcessingId = processingId;
        Status = status;
        Images = images;
    }
}