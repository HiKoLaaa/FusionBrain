using FusionBrain.Domain.Primitives;
using FusionBrain.Domain.ValueObjects;

namespace FusionBrain.Domain.Entities;

public class GeneratingImage
{
    public string Query { get; }

    public string? ExcludingResultQuery { get; }

    public Size Size { get; }

    public int NumberOfImages { get; }

    public GenerationMode Mode { get; }

    public Style Style { get; }

    public Model Model { get; }

    public GeneratingImage(
        string query,
        Size size,
        int numberOfImages,
        GenerationMode mode,
        Model model)
        : this(
            query,
            null,
            size,
            numberOfImages,
            mode,
            model,
            Style.Default)
    {
    }

    public GeneratingImage(
        string query,
        Size size,
        int numberOfImages,
        GenerationMode mode,
        Model model,
        Style style)
        : this(
            query,
            null,
            size,
            numberOfImages,
            mode,
            model,
            style)
    {
    }

    public GeneratingImage(
        string query,
        string excludingResultQuery,
        Size size,
        int numberOfImages,
        GenerationMode mode,
        Model model)
        : this(
            query,
            excludingResultQuery,
            size,
            numberOfImages,
            mode,
            model,
            Style.Default)
    {
    }

    public GeneratingImage(
        string query,
        string? excludingResultQuery,
        Size size,
        int numberOfImages,
        GenerationMode mode,
        Model model,
        Style style)
    {
        const int maxQueryLenght = 1000;

        if (query.Length > maxQueryLenght)
            throw new ArgumentException($"Lenght must be less than {maxQueryLenght}", nameof(query));

        if (!string.IsNullOrWhiteSpace(excludingResultQuery) && excludingResultQuery.Length > maxQueryLenght)
            throw new ArgumentException($"Lenght must be less than {maxQueryLenght}", nameof(excludingResultQuery));

        Query = query;
        ExcludingResultQuery = excludingResultQuery;
        Size = size;
        NumberOfImages = numberOfImages;
        Mode = mode;
        Model = model;
        Style = style;
    }
}