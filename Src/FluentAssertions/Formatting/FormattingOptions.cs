using System.Collections.Generic;
using FluentAssertions.Execution;

namespace FluentAssertions.Formatting;

public class FormattingOptions
{
    internal List<IValueFormatter> ScopedFormatters { get; set; } = [];

    /// <summary>
    /// Indicates whether the formatter should use line breaks when the <see cref="IValueFormatter"/> supports it.
    /// </summary>
    /// <remarks>
    /// This property is not thread-safe and should not be modified through <see cref="AssertionConfiguration"/> from within a unit test.
    /// See the <see href="https://fluentassertions.com/extensibility/#thread-safety">docs</see> on how to safely use it.
    /// </remarks>
    public bool UseLineBreaks { get; set; }

    /// <summary>
    /// Determines the depth until which the library should try to render an object graph.
    /// </summary>
    /// <remarks>
    /// This property is not thread-safe and should not be modified through <see cref="AssertionConfiguration"/> from within a unit test.
    /// See the <see href="https://fluentassertions.com/extensibility/#thread-safety">docs</see> on how to safely use it.
    /// </remarks>
    /// <value>
    /// A depth of 1 will only the display the members of the root object.
    /// </value>
    public int MaxDepth { get; set; } = 5;

    /// <summary>
    /// Sets the maximum number of lines of the failure message.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Because of technical reasons, the actual output may be one or two lines longer.
    /// </para>
    /// <para>
    /// This property is not thread-safe and should not be modified through <see cref="AssertionConfiguration"/> from within a unit test.
    /// See the <see href="https://fluentassertions.com/extensibility/#thread-safety">docs</see> on how to safely use it.
    /// </para>
    /// </remarks>
    public int MaxLines { get; set; } = 100;

    /// <summary>
    /// Removes a scoped formatter that was previously added through <see cref="FormattingOptions.AddFormatter"/>.
    /// </summary>
    /// <param name="formatter">A custom implementation of <see cref="IValueFormatter"/></param>
    public void RemoveFormatter(IValueFormatter formatter)
    {
        ScopedFormatters.Remove(formatter);
    }

    /// <summary>
    /// Ensures a scoped formatter is included in the chain, which is executed before the static custom formatters and the default formatters.
    /// This also lasts only for the current <see cref="AssertionScope"/> until disposal.
    /// </summary>
    /// <param name="formatter">A custom implementation of <see cref="IValueFormatter"/></param>
    public void AddFormatter(IValueFormatter formatter)
    {
        if (!ScopedFormatters.Contains(formatter))
        {
            ScopedFormatters.Insert(0, formatter);
        }
    }

    internal FormattingOptions Clone()
    {
        return new FormattingOptions
        {
            UseLineBreaks = UseLineBreaks,
            MaxDepth = MaxDepth,
            MaxLines = MaxLines,
            ScopedFormatters = [.. ScopedFormatters],
        };
    }
}
