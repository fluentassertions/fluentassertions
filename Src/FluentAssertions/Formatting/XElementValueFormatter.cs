using System;
using System.Xml.Linq;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Formatting;

public class XElementValueFormatter : IValueFormatter
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
        return value is XElement;
    }

    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        var element = (XElement)value;

        formattedGraph.AddFragment(element.HasElements
            ? FormatElementWithChildren(element)
            : FormatElementWithoutChildren(element));
    }

    private static string FormatElementWithoutChildren(XElement element)
    {
        return element.ToString().EscapePlaceholders();
    }

    private static string FormatElementWithChildren(XElement element)
    {
        string[] lines = SplitIntoSeparateLines(element);

        // Can't use env.newline because the input doc may have unix or windows style
        // line-breaks
        string firstLine = lines[0].RemoveNewLines();
        string lastLine = lines[^1].RemoveNewLines();

        string formattedElement = firstLine + "…" + lastLine;
        return formattedElement.EscapePlaceholders();
    }

    private static string[] SplitIntoSeparateLines(XElement element)
    {
        string formattedXml = element.ToString();
        return formattedXml.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
    }
}
