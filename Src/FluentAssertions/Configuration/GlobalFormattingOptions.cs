using FluentAssertions.Formatting;

namespace FluentAssertions.Configuration;

public class GlobalFormattingOptions : FormattingOptions
{
    public string ValueFormatterAssembly { get; set; }

    public bool DetectValueFormatters { get; set; }
}
