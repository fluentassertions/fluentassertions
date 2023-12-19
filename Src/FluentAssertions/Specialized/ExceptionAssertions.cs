using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Equivalency.Steps;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Formatting;
using FluentAssertionsAsync.Primitives;

namespace FluentAssertionsAsync.Specialized;

/// <summary>
/// Contains a number of methods to assert that an <see cref="Exception" /> is in the correct state.
/// </summary>
[DebuggerNonUserCode]
public class ExceptionAssertions<TException> : ReferenceTypeAssertions<IEnumerable<TException>, ExceptionAssertions<TException>>
    where TException : Exception
{
    public ExceptionAssertions(IEnumerable<TException> exceptions)
        : base(exceptions)
    {
    }

    /// <summary>
    /// Gets the exception object of the exception thrown.
    /// </summary>
    public TException And => SingleSubject;

    /// <summary>
    /// Gets the exception object of the exception thrown.
    /// </summary>
    public TException Which => And;

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => "exception";

    /// <summary>
    /// Asserts that the thrown exception has a message that matches <paramref name="expectedWildcardPattern" />.
    /// </summary>
    /// <param name="expectedWildcardPattern">
    /// The pattern to match against the exception message. This parameter can contain a combination of literal text and
    /// wildcard (* and ?) characters, but it doesn't support regular expressions.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <remarks>
    /// <paramref name="expectedWildcardPattern"/> can be a combination of literal and wildcard characters,
    /// but it doesn't support regular expressions. The following wildcard specifiers are permitted in
    /// <paramref name="expectedWildcardPattern"/>.
    /// <list type="table">
    /// <listheader>
    /// <term>Wildcard character</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>* (asterisk)</term>
    /// <description>Zero or more characters in that position.</description>
    /// </item>
    /// <item>
    /// <term>? (question mark)</term>
    /// <description>Exactly one character in that position.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public virtual ExceptionAssertions<TException> WithMessage(string expectedWildcardPattern, string because = "",
        params object[] becauseArgs)
    {
        AssertionScope assertion = Execute.Assertion.BecauseOf(because, becauseArgs).UsingLineBreaks;

        assertion
            .ForCondition(Subject.Any())
            .FailWith("Expected exception with message {0}{reason}, but no exception was thrown.", expectedWildcardPattern);

        ExceptionMessageAssertion.Execute(Subject.Select(exc => exc.Message), expectedWildcardPattern, because,
            becauseArgs);

        return this;
    }

    /// <summary>
    /// Asserts that the thrown exception contains an inner exception of type <typeparamref name="TInnerException" />.
    /// </summary>
    /// <typeparam name="TInnerException">The expected type of the inner exception.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public virtual ExceptionAssertions<TInnerException> WithInnerException<TInnerException>(string because = "",
        params object[] becauseArgs)
        where TInnerException : Exception
    {
        var expectedInnerExceptions = AssertInnerExceptions(typeof(TInnerException), because, becauseArgs);
        return new ExceptionAssertions<TInnerException>(expectedInnerExceptions.Cast<TInnerException>());
    }

    /// <summary>
    /// Asserts that the thrown exception contains an inner exception of type <paramref name="innerException" />.
    /// </summary>
    /// <param name="innerException">The expected type of the inner exception.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public ExceptionAssertions<Exception> WithInnerException(Type innerException, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(innerException);

        return new ExceptionAssertions<Exception>(AssertInnerExceptions(innerException, because, becauseArgs));
    }

    /// <summary>
    /// Asserts that the thrown exception contains an inner exception of the exact type <typeparamref name="TInnerException" /> (and not a derived exception type).
    /// </summary>
    /// <typeparam name="TInnerException">The expected type of the inner exception.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public virtual ExceptionAssertions<TInnerException> WithInnerExceptionExactly<TInnerException>(string because = "",
        params object[] becauseArgs)
        where TInnerException : Exception
    {
        var exceptionExpression = AssertInnerExceptionExactly(typeof(TInnerException), because, becauseArgs);
        return new ExceptionAssertions<TInnerException>(exceptionExpression.Cast<TInnerException>());
    }

    /// <summary>
    /// Asserts that the thrown exception contains an inner exception of the exact type <paramref name="innerException" /> (and not a derived exception type).
    /// </summary>
    /// <param name="innerException">The expected type of the inner exception.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public ExceptionAssertions<Exception> WithInnerExceptionExactly(Type innerException, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(innerException);

        return new ExceptionAssertions<Exception>(AssertInnerExceptionExactly(innerException, because, becauseArgs));
    }

    /// <summary>
    /// Asserts that the exception matches a particular condition.
    /// </summary>
    /// <param name="exceptionExpression">
    /// The condition that the exception must match.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="exceptionExpression"/> is <see langword="null"/>.</exception>
    public ExceptionAssertions<TException> Where(Expression<Func<TException, bool>> exceptionExpression,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(exceptionExpression);

        Func<TException, bool> condition = exceptionExpression.Compile();

        Execute.Assertion
            .ForCondition(condition(SingleSubject))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected exception where {0}{reason}, but the condition was not met by:"
                        + Environment.NewLine + Environment.NewLine + "{1}.",
                exceptionExpression, Subject);

        return this;
    }

    private IEnumerable<Exception> AssertInnerExceptionExactly(Type innerException, string because = "",
        params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.Any(e => e.InnerException is not null))
            .FailWith("Expected inner {0}{reason}, but the thrown exception has no inner exception.", innerException);

        Exception[] expectedExceptions = Subject
            .Select(e => e.InnerException)
            .Where(e => e?.GetType() == innerException).ToArray();

        Execute.Assertion
            .ForCondition(expectedExceptions.Length > 0)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected inner {0}{reason}, but found {1}.", innerException, SingleSubject.InnerException);

        return expectedExceptions;
    }

    private IEnumerable<Exception> AssertInnerExceptions(Type innerException, string because = "",
        params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.Any(e => e.InnerException is not null))
            .FailWith("Expected inner {0}{reason}, but the thrown exception has no inner exception.", innerException);

        Exception[] expectedInnerExceptions = Subject
            .Select(e => e.InnerException)
            .Where(e => e != null && e.GetType().IsSameOrInherits(innerException))
            .ToArray();

        Execute.Assertion
            .ForCondition(expectedInnerExceptions.Length > 0)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected inner {0}{reason}, but found {1}.", innerException, SingleSubject.InnerException);

        return expectedInnerExceptions;
    }

    private TException SingleSubject
    {
        get
        {
            if (Subject.Count() > 1)
            {
                string thrownExceptions = BuildExceptionsString(Subject);

                Services.ThrowException(
                    $"More than one exception was thrown.  FluentAssertions cannot determine which Exception was meant.{Environment.NewLine}{thrownExceptions}");
            }

            return Subject.Single();
        }
    }

    private static string BuildExceptionsString(IEnumerable<TException> exceptions)
    {
        return string.Join(Environment.NewLine,
            exceptions.Select(
                exception =>
                    "\t" + Formatter.ToString(exception)));
    }

    private static class ExceptionMessageAssertion
    {
        private const string Context = "exception message";

        public static void Execute(IEnumerable<string> messages, string expectation, string because, params object[] becauseArgs)
        {
            using var _ = new AssertionScope();
            var results = new AssertionResultSet();

            foreach (string message in messages)
            {
                using (var scope = new AssertionScope())
                {
                    scope.Context = new Lazy<string>(() => Context);

                    message.Should().MatchEquivalentOf(expectation, because, becauseArgs);

                    results.AddSet(message, scope.Discard());
                }

                if (results.ContainsSuccessfulSet())
                {
                    break;
                }
            }

            foreach (string failure in results.SelectClosestMatchFor())
            {
                string replacedCurlyBraces =
                    failure.EscapePlaceholders();

                AssertionScope.Current.FailWith(replacedCurlyBraces);
            }
        }
    }
}
