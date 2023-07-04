using System.ComponentModel;
using Nuke.Common.Tooling;

[TypeConverter(typeof(TypeConverter<Configuration>))]
public class Configuration : Enumeration
{
    public static Configuration Debug { get; } = new() { Value = nameof(Debug) };

    public static Configuration Release { get; } = new() { Value = nameof(Release) };

    public static Configuration CI { get; } = new() { Value = nameof(CI) };

    public static implicit operator string(Configuration configuration)
    {
        return configuration.Value;
    }
}
