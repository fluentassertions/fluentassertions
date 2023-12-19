using System.Xml;

namespace FluentAssertionsAsync.Formatting;

public class XmlReaderValueFormatter : IValueFormatter
{
    /// <summary>
    /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value for which to create a <see cref="string"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <see langword="false"/>.
    /// </returns>
    public bool CanHandle(object value)
    {
        return value is XmlReader;
    }

    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        var reader = (XmlReader)value;

        if (reader.ReadState == ReadState.Initial)
        {
            reader.Read();
        }

        var result = "\"" + reader.ReadOuterXml() + "\"";

        formattedGraph.AddFragment(result);
    }
}
