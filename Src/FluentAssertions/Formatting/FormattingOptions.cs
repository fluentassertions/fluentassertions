namespace FluentAssertionsAsync.Formatting;

public class FormattingOptions
{
    /// <summary>
    /// Indicates whether the formatter should use line breaks when the <see cref="IValueFormatter"/> supports it.
    /// </summary>
    /// <remarks>
    /// This value should not changed on <see cref="AssertionOptions.FormattingOptions"/> from within a unit test.
    /// See the <see href="https://fluentassertions.com/extensibility/#thread-safety">docs</see> on how to safely use it.
    /// </remarks>
    public bool UseLineBreaks { get; set; }

    /// <summary>
    /// Determines the depth until which the library should try to render an object graph.
    /// </summary>
    /// <remarks>
    /// This value should not changed on <see cref="AssertionOptions.FormattingOptions"/> from within a unit test.
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
    /// This value should not changed on <see cref="AssertionOptions.FormattingOptions"/> from within a unit test.
    /// See the <see href="https://fluentassertions.com/extensibility/#thread-safety">docs</see> on how to safely use it.
    /// </para>
    /// </remarks>
    public int MaxLines { get; set; } = 100;

    internal FormattingOptions Clone()
    {
        return new FormattingOptions
        {
            UseLineBreaks = UseLineBreaks,
            MaxDepth = MaxDepth,
            MaxLines = MaxLines
        };
    }
}
