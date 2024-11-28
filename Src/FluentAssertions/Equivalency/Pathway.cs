using System;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency;

public record Pathway
{
    private string path = string.Empty;
    private string name = string.Empty;

    private string pathAndName;

    private readonly Func<string> descriptionPrefix = () => string.Empty;

    public Pathway()
    {
    }

    public Pathway(Pathway parent, string name)
    {
        path = parent.PathAndName;
        this.name = name;
        descriptionPrefix = parent.descriptionPrefix;
    }

    public Pathway(string path, string name, Func<string> getDescriptionPrefix)
    {
        this.path = path;
        this.name = name;
        descriptionPrefix = getDescriptionPrefix;
    }

    public Pathway(Pathway original)
    {
        path = original.path;
        name = original.name;
        descriptionPrefix = original.descriptionPrefix;
    }

    public string Path
    {
        get => path;
        set
        {
            path = value;
            pathAndName = null;
        }
    }

    public string Name
    {
        get => name;
        set
        {
            name = value;
            pathAndName = null;
        }
    }

    public string Description => $"{descriptionPrefix().Combine(PathAndName)}";

    public string PathAndName => pathAndName ??= path.Combine(name);

    public override string ToString() => Description;
}
