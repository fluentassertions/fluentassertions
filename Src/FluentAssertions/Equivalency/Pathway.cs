using FluentAssertions.Common;

namespace FluentAssertions.Equivalency;

/// <summary>
/// Represents the path of a field or property in an object graph.
/// </summary>
public record Pathway
{
    public delegate string GetDescription(string pathAndName);

    private string path = string.Empty;
    private string name = string.Empty;
    private string pathAndName;

    private readonly GetDescription getDescription;

    public Pathway(string path, string name, GetDescription getDescription)
    {
        Path = path;
        Name = name;
        this.getDescription = getDescription;
    }

    /// <summary>
    /// Creates an instance of <see cref="Pathway"/> with the specified parent and name and a factory
    /// to provide a description for the path and name.
    /// </summary>
    public Pathway(Pathway parent, string name, GetDescription getDescription)
    {
        Path = parent.PathAndName;
        Name = name;
        this.getDescription = getDescription;
    }

    /// <summary>
    /// Gets the path of the field or property without the name.
    /// </summary>
    public string Path
    {
        get => path;
        private init
        {
            path = value;
            pathAndName = null;
        }
    }

    /// <summary>
    /// Gets the name of the field or property without the path.
    /// </summary>
    public string Name
    {
        get => name;
        private init
        {
            name = value;
            pathAndName = null;
        }
    }

    /// <summary>
    /// Gets the path and name of the field or property separated by dots.
    /// </summary>
    public string PathAndName => pathAndName ??= path.Combine(name);

    /// <summary>
    /// Gets the display representation of this path.
    /// </summary>
    public string Description => getDescription(PathAndName);

    public override string ToString() => Description;
}
