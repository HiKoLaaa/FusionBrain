using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using FusionBrain.Configurations;
using FusionBrain.Domain.Entities;
using FusionBrain.Domain.Primitives;
using FusionBrain.Extensions;
using FusionBrain.Services.Abstractions.Images;
using FusionBrain.Services.Images.Dtos;
using FusionBrain.Services.Images.Dtos.GeneratingImages;
using FusionBrain.Services.Images.Dtos.GenerationStatuses;
using FusionBrain.Services.Images.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FusionBrain.Services.Images;

public sealed class ImageGeneratorClient : IImageGeneratorClient
{
    private readonly HttpClient _httpClient;

    private readonly ILogger<ImageGeneratorClient> _logger;

    private readonly ImageGeneratorSettings _settings;

    public ImageGeneratorClient(HttpClient httpClient, IOptions<ImageGeneratorSettings> settings, ILogger<ImageGeneratorClient> logger)
    {
        _settings = settings.Value;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Model>> GetModelsAsync(CancellationToken cancellationToken)
    {
        const string relativeUrl = "key/api/v1/models";

        var response = await MakeRequest<IEnumerable<ModelDto>>(httpClient => httpClient.GetAsync(relativeUrl, cancellationToken)).ConfigureAwait(false);

        return response!.Select(
            dto =>
                new Model(
                    dto.Id,
                    dto.Name,
                    new Version(dto.Version.ToString(CultureInfo.InvariantCulture)),
                    dto.Type));
    }

    public async Task<GenerationStatus> StartGenerateAsync(GeneratingImage generatingImage, CancellationToken cancellationToken)
    {
        var generatingStatus = await StartInitialGenerationImageAsync(generatingImage, cancellationToken);

        return new GenerationStatus(generatingStatus.ProcessingId, generatingStatus.Status, generatingStatus.Images?.ToBytes().ToArray());
    }

    public async Task<IEnumerable<GeneratedImage>> GenerateAsync(GeneratingImage generatingImage, CancellationToken cancellationToken)
    {
        var generatingStatus = await StartInitialGenerationImageAsync(generatingImage, cancellationToken).ConfigureAwait(false);
        var attempts = _settings.CheckGeneration.Attempts;
        while (attempts > 0)
        {
            if (generatingStatus.Status is ProcessingStatus.Fail)
                throw new ProcessingFailedException(generatingStatus.Error!);

            if (generatingStatus.Status is ProcessingStatus.Done)
                return generatingStatus.Images!.ToBytes().ToGeneratedImages(generatingStatus.ProcessingId);

            cancellationToken.ThrowIfCancellationRequested();

            generatingStatus = await GetIntermediateGeneratingStatusAsync(
                    generatingImage.Model,
                    generatingStatus.ProcessingId,
                    cancellationToken)
                .ConfigureAwait(false);

            attempts--;

            await Task.Delay(_settings.CheckGeneration.Delay, cancellationToken);
        }

        throw new ProcessingStoppedException(_settings.CheckGeneration.Attempts);
    }

    public async Task<GenerationStatus> GetGenerationStatusAsync(Model model, Guid processingId, CancellationToken cancellationToken)
    {
        var generatingStatus = await GetIntermediateGeneratingStatusAsync(model, processingId, cancellationToken);

        return new GenerationStatus(generatingStatus.ProcessingId, generatingStatus.Status, generatingStatus.Images?.ToBytes().ToArray());
    }

    private async Task<GeneratingSuccessStatusDto> StartInitialGenerationImageAsync(GeneratingImage generatingImage, CancellationToken cancellationToken)
    {
        var relativeUrl = $"/key/api/v1/{generatingImage.Model.Type.ToLower()}/run";

        var generatingImageDto = new GeneratingImageParametersDto
        {
            Width = generatingImage.Size.Width,
            Height = generatingImage.Size.Height,
            NumberOfImages = generatingImage.NumberOfImages,
            Mode = generatingImage.Mode,
            ExcludingResultQuery = generatingImage.ExcludingResultQuery,
            Style = generatingImage.Style,
            Parameters = new GeneratingImageParameterQueryDto
            {
                Query = generatingImage.Query
            }
        };

        var requestContent = new MultipartFormDataContent();
        requestContent.Add(new StringContent(JsonSerializer.Serialize(generatingImageDto), Encoding.UTF8, "application/json"), "params");
        requestContent.Add(new StringContent(generatingImage.Model.Id.ToString()), "model_id");

        var generatingStatus = await MakeRequest(
                httpClient => httpClient.PostAsync(relativeUrl, requestContent, cancellationToken),
                ParseGeneratingSuccessStatusDto)
            .ConfigureAwait(false);

        return generatingStatus;
    }

    private async Task<GeneratingSuccessStatusDto> GetIntermediateGeneratingStatusAsync(Model model, Guid processingId, CancellationToken cancellationToken)
    {
        var relativeUrl = $"key/api/v1/{model.Type.ToLower()}/status";

        var generatingStatus = await MakeRequest(
                httpClient => httpClient.GetAsync($"{relativeUrl}/{processingId}", cancellationToken),
                ParseGeneratingSuccessStatusDto)
            .ConfigureAwait(false);

        return generatingStatus;
    }

    private static GeneratingSuccessStatusDto ParseGeneratingSuccessStatusDto(string responseContent)
    {
        try
        {
            var generatingSuccessStatus = JsonSerializer.Deserialize<GeneratingSuccessStatusDto>(responseContent)!;

            return generatingSuccessStatus;
        }
        catch (JsonException)
        {
            var generationFailedStatus = JsonSerializer.Deserialize<GeneratingFailedStatusDto>(responseContent)!;

            throw new ProcessingUnavailableException(generationFailedStatus.Status);
        }
    }

    private async Task<TResponse> MakeRequest<TResponse>(
        Func<HttpClient, Task<HttpResponseMessage>> request,
        Func<string, TResponse>? parseResult = null)
        where TResponse : class
    {
        try
        {
            var response = await request(_httpClient).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return parseResult?.Invoke(responseContent) ?? JsonSerializer.Deserialize<TResponse>(responseContent)!;
        }
        catch (HttpRequestException e) when (e.StatusCode is HttpStatusCode.Unauthorized)
        {
            _logger.LogError(
                e,
                "{ApiKeyName} or {SecretKeyName} into configuration are invalid",
                nameof(_settings.Authentication.ApiKey),
                nameof(_settings.Authentication.SecretKey));

            throw;
        }
    }
}
