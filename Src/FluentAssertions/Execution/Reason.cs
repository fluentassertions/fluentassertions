namespace FluentAssertions.Execution
{
    /// <summary>
    /// Represents the reason for a structural equivalency assertion.
    /// </summary>
    public class Reason
    {
        public Reason(string formattedMessage, object[] arguments)
        {
            FormattedMessage = formattedMessage;
            Arguments = arguments;
        }

        /// <summary>
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </summary>
        public string FormattedMessage { get; set; }

        /// <summary>
        /// Zero or more objects to format using the placeholders in <see cref="Reason.FormattedMessage" />.
        /// </summary>
        public object[] Arguments { get; set; }
    }
}
