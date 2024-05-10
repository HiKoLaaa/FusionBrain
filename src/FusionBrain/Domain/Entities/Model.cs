namespace FusionBrain.Domain.Entities;

public class Model
{
    public int Id { get; }

    public string Name { get; }

    public Version Version { get; }

    public string Type { get; }

    public Model(
        int id,
        string name,
        Version version,
        string type)
    {
        Id = id;
        Name = name;
        Version = version;
        Type = type;
    }
}