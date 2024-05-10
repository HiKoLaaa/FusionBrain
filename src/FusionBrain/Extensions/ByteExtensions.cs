using FusionBrain.Domain.Entities;

namespace FusionBrain.Extensions;

public static class ByteExtensions
{
    public static IEnumerable<GeneratedImage> ToGeneratedImages(this IEnumerable<byte[]> values, Guid processingId)
    {
        return values.Select(value => new GeneratedImage(processingId, value));
    }
}