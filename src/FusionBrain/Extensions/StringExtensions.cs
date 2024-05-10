namespace FusionBrain.Extensions;

internal static class StringExtensions
{
    public static IEnumerable<byte[]> ToBytes(this IEnumerable<string> values) => values.Select(Convert.FromBase64String);
}