using System.Diagnostics.CodeAnalysis;
using FluentAssertions.Common;
using FluentAssertions.Formatting;

namespace FluentAssertions.Configuration;

public class GlobalFormattingOptions : FormattingOptions
{
    public string ValueFormatterAssembly
    {
        get;
        set
        {
            field = value;
            ValueFormatterDetectionMode = ValueFormatterDetectionMode.Specific;
        }
    }

    public ValueFormatterDetectionMode ValueFormatterDetectionMode { get; set; }

    [SuppressMessage("Class Design", "AV1010:Member hides inherited member")]
    internal new GlobalFormattingOptions Clone()
    {
        return new GlobalFormattingOptions
        {
            UseLineBreaks = UseLineBreaks,
            MaxDepth = MaxDepth,
            MaxLines = MaxLines,
            ScopedFormatters = [.. ScopedFormatters],
            ValueFormatterAssembly = ValueFormatterAssembly,
            ValueFormatterDetectionMode = ValueFormatterDetectionMode
        };
    }
}
