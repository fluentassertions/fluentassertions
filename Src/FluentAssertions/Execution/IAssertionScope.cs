using System;

namespace FluentAssertions.Execution
{
    public interface IAssertionScope : IDisposable
    {
        /// <summary>
        /// Allows to safely select the subject for successive assertions.
        /// </summary>
        /// <paramref name="selector">
        /// Selector which result is passed to successive calls to <see cref="ForCondition"/>.
        /// </paramref>
        GivenSelector<T> Given<T>(Func<T> selector);

        /// <summary>
        /// Specify the condition that must be satisfied.
        /// </summary>
        /// <param name="condition">
        /// If <c>true</c> the assertion will be treated as successful and no exceptions will be thrown.
        /// </param>
        IAssertionScope ForCondition(bool condition);

        /// <summary>
        /// Sets the failure message when the assertion is not met, or completes the failure message set to a prior call to
        /// <see cref="AssertionScope.WithExpectation"/>.
        /// </summary>
        /// <remarks>
        /// Messages may contain a few specialized placeholders. For instance, <em>{reason}</em> will be replaced with the reason
        /// of the assertion as passed to <see cref="AssertionScope.BecauseOf(string, object[])"/>.
        /// <para>
        /// Other named placeholders will be replaced with the <see cref="AssertionScope.Current"/> scope data passed through
        /// <see cref="AssertionScope.AddNonReportable"/> and <see cref="AssertionScope.AddReportable(string,string)"/>.
        /// </para>
        /// <para>
        /// Finally, a description of the current subject can be passed through the <em>{context:description}</em> placeholder.
        /// This is used in the message if no explicit context is specified through the <see cref="AssertionScope"/> constructor.
        /// </para>
        /// <para>
        /// If an expectation was set through a prior call to <see cref="AssertionScope.WithExpectation"/>, then the failure
        /// message is appended to that expectation.
        /// </para>
        /// </remarks>
        /// <param name="message">The format string that represents the failure message.</param>
        Continuation FailWith(string message);

        /// <summary>
        /// Sets the failure message when the assertion is not met, or completes the failure message set to a prior call to
        /// <see cref="AssertionScope.WithExpectation"/>.
        /// <paramref name="failReasonFunc"/> will not be called unless the assertion is not met.
        /// </summary>
        /// <param name="failReasonFunc">Function returning <see cref="FailReason"/> object on demand. Called only when the assertion is not met.</param>
        Continuation FailWith(Func<FailReason> failReasonFunc);

        /// <summary>
        /// Sets the failure message when the assertion is not met, or completes the failure message set to a prior call to
        /// <see cref="AssertionScope.WithExpectation"/>.
        /// </summary>
        /// <remarks>
        /// In addition to the numbered <see cref="string.Format(string,object[])"/>-style placeholders, messages may contain a
        /// few specialized placeholders as well. For instance, <em>{reason}</em> will be replaced with the reason of the
        /// assertion as passed to <see cref="AssertionScope.BecauseOf(string, object[])"/>.
        /// <para>
        /// Other named placeholders will be replaced with the <see cref="AssertionScope.Current"/> scope data passed through
        /// <see cref="AssertionScope.AddNonReportable"/> and <see cref="AssertionScope.AddReportable(string, string)"/>.
        /// </para>
        /// <para>
        /// Finally, a description of the current subject can be passed through the <em>{context:description}</em> placeholder.
        /// This is used in the message if no explicit context is specified through the <see cref="AssertionScope"/> constructor.
        /// </para>
        /// <para>
        /// Note that only 10 <paramref name="args"/> are supported in combination with a <em>{reason}</em>.
        /// </para>
        /// <para>
        /// If an expectation was set through a prior call to <see cref="AssertionScope.WithExpectation"/>, then the failure
        /// message is appended to that expectation.
        /// </para>
        /// </remarks>
        /// <param name="message">The format string that represents the failure message.</param>
        /// <param name="args">Optional arguments to any numbered placeholders.</param>
        Continuation FailWith(string message, params object[] args);

        /// <summary>
        /// Sets the failure message when the assertion is not met, or completes the failure message set to a prior call to
        /// <see cref="AssertionScope.WithExpectation"/>,
        /// but postpones evaluation of the formatting arguments until the assertion really fails.
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
        /// Note that only 10 <paramref name="argProviders"/> are supported in combination with a <em>{reason}</em>.
        /// </para>
        /// <para>
        /// If an expectation was set through a prior call to <see cref="AssertionScope.WithExpectation"/>, then the failure
        /// message is appended to that expectation.
        /// </para>
        /// </remarks>
        /// <param name="message">The format string that represents the failure message.</param>
        /// <param name="argProviders">Optional lazily evaluated arguments to any numbered placeholders</param>
        public Continuation FailWith(string message, params Func<object>[] argProviders);

        /// <summary>
        /// Specify the reason why you expect the condition to be <c>true</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase compatible with <see cref="string.Format(string,object[])"/> explaining why the condition should
        /// be satisfied. If the phrase does not start with the word <em>because</em>, it is prepended to the message.
        /// <para>
        /// If the format of <paramref name="because"/> or <paramref name="becauseArgs"/> is not compatible with
        /// <see cref="string.Format(string,object[])"/>, then a warning message is returned instead.
        /// </para>
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        IAssertionScope BecauseOf(string because, params object[] becauseArgs);

        /// <summary>
        /// Clears the expectation set by <see cref="WithExpectation"/>.
        /// </summary>
        // SMELL: It would be better to give the expectation an explicit scope, but that would be a breaking change.
        Continuation ClearExpectation();

        /// <summary>
        /// Sets the expectation part of the failure message when the assertion is not met.
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
        /// Note that only 10 <paramref name="args"/> are supported in combination with a <em>{reason}</em>.
        /// </para>
        /// </remarks>
        /// <param name="message">The format string that represents the failure message.</param>
        /// <param name="args">Optional arguments to any numbered placeholders.</param>
        IAssertionScope WithExpectation(string message, params object[] args);

        /// <summary>
        /// Defines the name of the subject in case this cannot be extracted from the source code.
        /// </summary>
        IAssertionScope WithDefaultIdentifier(string identifier);

        /// <summary>
        /// Forces the formatters, that support it, to add the necessary line breaks.
        /// </summary>
        /// <remarks>
        /// This is just shorthand for modifying the <see cref="AssertionScope.FormattingOptions"/> property.
        /// </remarks>
        IAssertionScope UsingLineBreaks { get; }

        /// <summary>
        /// Discards and returns the failures that happened up to now.
        /// </summary>
        string[] Discard();
    }
}
