using System.Text.Json;
using System.Text.Json.Serialization;

namespace FusionBrain.Services.Images.Dtos.Serializers;

internal sealed class UpperCaseJsonStringEnumConverter : JsonStringEnumConverter
{
    public UpperCaseJsonStringEnumConverter() : base(JsonNamingPolicy.SnakeCaseUpper)
    {
    }
}