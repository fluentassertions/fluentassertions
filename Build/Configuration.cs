using System.ComponentModel;
using Nuke.Common.Tooling;

[TypeConverter(typeof(TypeConverter<Configuration>))]
public class Configuration : Enumeration
{
    public static readonly Configuration Debug = new() { Value = nameof(Debug) };

    public static readonly Configuration CI = new() { Value = nameof(CI) };

    public static implicit operator string(Configuration configuration)
    {
        return configuration.Value;
    }
}
