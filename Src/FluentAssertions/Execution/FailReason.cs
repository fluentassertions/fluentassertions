namespace FluentAssertionsAsync.Execution;

/// <summary>
/// Represents assertion fail reason. Contains the message and arguments for message's numbered placeholders.
/// </summary>
/// <remarks>
/// In addition to the numbered <see cref="string.Format(string,object[])"/>-style placeholders, messages may contain a
/// few specialized placeholders as well. For instance, <em>{reason}</em> will be replaced with the reason of the
/// assertion as passed to <see cref="AssertionScope.BecauseOf(string, object[])"/>.
/// <para>
/// Other named placeholders will be replaced with the <see cref="AssertionScope.Current"/> scope data passed through
/// <see cref="AssertionScope.AddNonReportable"/> and <see cref="AssertionScope.AddReportable(string,string)"/>.
/// </para>
/// <para>
/// Finally, a description of the current subject can be passed through the <em>{context:description}</em> placeholder.
/// This is used in the message if no explicit context is specified through the <see cref="AssertionScope"/> constructor.
/// </para>
/// <para>
/// Note that only 10 <c>args</c> are supported in combination with a <em>{reason}</em>.
/// </para>
/// </remarks>
public class FailReason
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FailReason"/> class.
    /// </summary>
    /// <remarks>
    /// <inheritdoc cref="FailReason"/>
    /// </remarks>
    public FailReason(string message, params object[] args)
    {
        Message = message;
        Args = args;
    }

    /// <summary>
    /// Message to be displayed in case of failed assertion. May contain numbered
    /// <see cref="string.Format(string,object[])"/>-style placeholders as well as specialized placeholders.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Arguments for the numbered <see cref="string.Format(string,object[])"/>-style placeholders of <see cref="Message"/>.
    /// </summary>
    public object[] Args { get; }
}
