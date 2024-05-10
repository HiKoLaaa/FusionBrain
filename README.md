# Non official [fusion brain](https://fusionbrain.ai/) API client.

___

### For actual official documentation please refer to https://fusionbrain.ai/docs/en/doc/api-dokumentaciya/

___

### Features

1. Getting available models - `GetModelsAsync`.
2. Generation image
    1. Auto polling result status - `GenerateImageAsync`
    2. Manual pooling result status - `StartGenerateAsync` + `GetGenerationStatusAsync`

### Steps for start

1. Configure `ImageGeneratorSettings`.
    1. `BaseUrl` and `Authentication` are mandatory.
    2. `CheckGeneration` are not mandatory with default values (`Attempts` = 10 + `Delay` = 10 seconds)
2. Create instance of `IImageGeneratorClient`.
    1. Use can use DI way or manually configuring, meanwhile you can
       use `ConfigureImageGeneratorHttpClient` `HttpClient` extension for simplify authentication configuration.
3. Get existing models by `GetModelsAsync` method
4. Use one of model for creating `GeneratingImage`
    1. Example:
       ```csharp
       var generatingImage = new GeneratingImage(
           query: "<YOUR REQUEST>",
           excludingResultQuery: "<YOUR NEGATIVE PROMPT>",
           size: Size.Size1024X1024,
           numberOfImages: 1, // At now supporting only 1 image per request.
           mode: GenerationMode.Generate, // At now supporting only 1 mode - "GENERATE".
           model: model, // Model from step 4
           style: Style.Anime // One of style, available styles by API - https://cdn.fusionbrain.ai/static/styles/api)
       ```

5. Make request with auto or manual pooling result.
    1. Auto.
        1. Use `IImageGeneratorClient.GenerateAsync()`, result will be collection of images with `processingId` and
           content byte array.
    2. Manual
        1. Use `IImageGeneratorClient.StartGenerateAsync()`, result will be status with processing id and array of by
           arrays of case `Done` status (one of which is image byte array content).
        2. Depending on your application's polling configuration use `IImageGeneratorClient.GetGenerationStatusAsync()`
           to get actual status generation.
           1. Additionally u can get `GeneratedImage` by `ToGeneratedImages` collection of byte array extension.
    3. At each request you _must have_ processing `Model` and sometimes `processingId`.
