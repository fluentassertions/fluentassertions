using FluentAssertions.Common;
using FluentAssertions.Formatting;

namespace FluentAssertions.Configuration;

public class GlobalFormattingOptions : FormattingOptions
{
    private string valueFormatterAssembly;

    public string ValueFormatterAssembly
    {
        get => valueFormatterAssembly;
        set
        {
            valueFormatterAssembly = value;
            ValueFormatterDetectionMode = ValueFormatterDetectionMode.Specific;
        }
    }

    public ValueFormatterDetectionMode ValueFormatterDetectionMode { get; set; }

    internal new GlobalFormattingOptions Clone()
    {
        return new GlobalFormattingOptions
        {
            UseLineBreaks = UseLineBreaks,
            MaxDepth = MaxDepth,
            MaxLines = MaxLines,
            ScopedFormatters = [..ScopedFormatters],
            ValueFormatterAssembly = ValueFormatterAssembly,
            ValueFormatterDetectionMode = ValueFormatterDetectionMode
        };
    }
}
