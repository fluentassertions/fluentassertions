namespace FluentAssertions.Execution
{
    /// <summary>
    /// Represents assertion fail reason. Contains the message and arguments for
    /// message's numbered placeholders.
    /// </summary>
    /// <remarks>
    /// In addition to the numbered <see cref="string.Format(string,object[])"/>-style placeholders, messages may contain a few
    /// specialized placeholders as well. For instance, {reason} will be replaced with the reason of the assertion as passed
    /// to <see cref="FluentAssertions.Execution.AssertionScope.BecauseOf"/>. Other named placeholders will be replaced with
    /// the <see cref="FluentAssertions.Execution.AssertionScope.Current"/> scope data passed through
    /// <see cref="FluentAssertions.Execution.AssertionScope.AddNonReportable"/> and
    /// <see cref="FluentAssertions.Execution.AssertionScope.AddReportable"/>. Finally, a description of the
    /// current subject can be passed through the {context:description} placeholder. This is used in the message if no
    /// explicit context is specified through the <see cref="AssertionScope"/> constructor.
    /// Note that only 10 arguments are supported in combination with a {reason}.
    /// </remarks>
    public class FailReason
    {
        public FailReason(string message, params object[] args)
        {
            Message = message;
            Args = args;
        }

        /// <summary>
        /// Message to be displayed in case of failed assertion. May contain
        /// numbered <see cref="string.Format(string,object[])"/>-style placeholders as well
        /// as specialized placeholders.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Arguments for the numbered <see cref="string.Format(string,object[])"/>-style placeholders
        /// of <see cref="Message"/>.
        /// </summary>
        public object[] Args { get; }
    }
}
