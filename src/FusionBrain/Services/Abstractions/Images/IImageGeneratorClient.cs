using FusionBrain.Domain.Entities;

namespace FusionBrain.Services.Abstractions.Images;

public interface IImageGeneratorClient
{
    /// <summary>
    /// Get available image generation models for generating
    /// </summary>
    /// <param name="cancellationToken">Token for cancelling operation</param>
    /// <returns>Enumerable of models</returns>
    Task<IEnumerable<Model>> GetModelsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Starts image generation process
    /// </summary>
    /// <param name="generatingImage">Image parameters for generation</param>
    /// <param name="cancellationToken">Token for cancelling operation</param>
    /// <exception cref="FusionBrain.Services.Images.Exceptions.ProcessingUnavailableException">Generation currently unavailable, e.g. technical work</exception>
    /// <returns>Generation status</returns>
    Task<GenerationStatus> StartGenerateAsync(GeneratingImage generatingImage, CancellationToken cancellationToken);

    /// <summary>
    /// Starts and periodically getting image generation status
    /// </summary>
    /// <param name="generatingImage">Image parameters for generation</param>
    /// <param name="cancellationToken">Token for cancelling operation</param>
    /// <exception cref="FusionBrain.Services.Images.Exceptions.ProcessingUnavailableException">Generation currently unavailable, e.g. technical work</exception>
    /// <exception cref="FusionBrain.Services.Images.Exceptions.ProcessingStoppedException">Generation was stopped due exhaustion of attempts</exception>
    /// <exception cref="FusionBrain.Services.Images.Exceptions.ProcessingFailedException">Generation was failed</exception>
    /// <exception cref="OperationCanceledException">Image generating was cancelled</exception>
    /// <returns>Enumerable of generated images</returns>
    Task<IEnumerable<GeneratedImage>> GenerateAsync(GeneratingImage generatingImage, CancellationToken cancellationToken);

    /// <summary>
    /// Get intermediate image generation status
    /// </summary>
    /// <param name="model">Image generation model</param>
    /// <param name="processingId">Image generation id</param>
    /// <param name="cancellationToken">Token for cancelling operation</param>
    /// <exception cref="FusionBrain.Services.Images.Exceptions.ProcessingUnavailableException">Generation currently unavailable, e.g. technical work</exception>
    /// <returns>Generation status</returns>
    Task<GenerationStatus> GetGenerationStatusAsync(Model model, Guid processingId, CancellationToken cancellationToken);
}
