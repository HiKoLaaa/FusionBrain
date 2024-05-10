namespace FusionBrain.Domain.ValueObjects;

public readonly record struct Size(int Width, int Height)
{
    public static Size Size1024X1024 = new(1024, 1024);
}