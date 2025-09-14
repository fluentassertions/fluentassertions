#if NET6_0_OR_GREATER

using System.Text.Json.Nodes;

namespace FluentAssertions.Formatting;

public class JsonNodeFormatter : IValueFormatter
{
    /// <inheritdoc />
    public bool CanHandle(object value) => value is JsonNode;

    /// <inheritdoc />
    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        var node = (JsonNode)value;

        if (context.UseLineBreaks)
        {
            formattedGraph.AddFragmentOnNewLine(node.ToString());
        }
        else
        {
            formattedGraph.AddFragment(node.ToJsonString());
        }
    }
}

#endif
