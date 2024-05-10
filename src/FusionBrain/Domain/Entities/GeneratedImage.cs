namespace FusionBrain.Domain.Entities;

public class GeneratedImage
{
    public Guid ProcessingId { get; }

    public byte[] Content { get; }

    public GeneratedImage(Guid processingId, byte[] content)
    {
        ProcessingId = processingId;
        Content = content;
    }
}