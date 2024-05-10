using FusionBrain.Configurations;

namespace FusionBrain.Extensions;

public static class HttpClientExtensions
{
    public static HttpClient ConfigureImageGeneratorHttpClient(this HttpClient httpClient, ImageGeneratorSettings settings)
    {
        const string xKeyHeader = "X-Key";
        const string xSecretHeader = "X-Secret";

        httpClient.BaseAddress = new Uri(settings.BaseUrl);

        httpClient.DefaultRequestHeaders.Add(xKeyHeader, $"Key {settings.Authentication.ApiKey}");
        httpClient.DefaultRequestHeaders.Add(xSecretHeader, $"Secret {settings.Authentication.SecretKey}");

        return httpClient;
    }
}