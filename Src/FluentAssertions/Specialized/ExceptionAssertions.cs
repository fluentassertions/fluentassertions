using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Equivalency.Steps;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;
using FluentAssertions.Primitives;

namespace FluentAssertions.Specialized;

/// <summary>
/// Contains a number of methods to assert that an <see cref="Exception" /> is in the correct state.
/// </summary>
[DebuggerNonUserCode]
public class ExceptionAssertions<TException> : ReferenceTypeAssertions<IEnumerable<TException>, ExceptionAssertions<TException>>
    where TException : Exception
{
    private readonly AssertionChain assertionChain;

    public ExceptionAssertions(IEnumerable<TException> exceptions, AssertionChain assertionChain)
        : base(exceptions, assertionChain)
    {
        this.assertionChain = assertionChain;
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
    public virtual ExceptionAssertions<TException> WithMessage(string expectedWildcardPattern,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .UsingLineBreaks
            .ForCondition(Subject.Any())
            .FailWith("Expected exception with message {0}{reason}, but no exception was thrown.", expectedWildcardPattern);

        AssertExceptionMessage(Subject.Select(exc => exc.Message), expectedWildcardPattern, because,
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
    public virtual ExceptionAssertions<TInnerException> WithInnerException<TInnerException>(
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
        where TInnerException : Exception
    {
        var expectedInnerExceptions = AssertInnerExceptions(typeof(TInnerException), because, becauseArgs);
        return new ExceptionAssertions<TInnerException>(expectedInnerExceptions.Cast<TInnerException>(), assertionChain);
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
    public ExceptionAssertions<Exception> WithInnerException(Type innerException,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(innerException);

        return new ExceptionAssertions<Exception>(AssertInnerExceptions(innerException, because, becauseArgs), assertionChain);
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
    public virtual ExceptionAssertions<TInnerException> WithInnerExceptionExactly<TInnerException>(
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
        where TInnerException : Exception
    {
        var exceptionExpression = AssertInnerExceptionExactly(typeof(TInnerException), because, becauseArgs);
        return new ExceptionAssertions<TInnerException>(exceptionExpression.Cast<TInnerException>(), assertionChain);
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
    public ExceptionAssertions<Exception> WithInnerExceptionExactly(Type innerException,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(innerException);

        return new ExceptionAssertions<Exception>(AssertInnerExceptionExactly(innerException, because, becauseArgs), assertionChain);
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
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(exceptionExpression);

        Func<TException, bool> condition = exceptionExpression.Compile();

        assertionChain
            .ForCondition(condition(SingleSubject))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected exception where {0}{reason}, but the condition was not met by:"
                        + Environment.NewLine + Environment.NewLine + "{1}.",
                exceptionExpression, Subject);

        return this;
    }

    private IEnumerable<Exception> AssertInnerExceptionExactly(Type innerException,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.Any(e => e.InnerException is not null))
            .FailWith("Expected inner {0}{reason}, but the thrown exception has no inner exception.", innerException);

        Exception[] expectedExceptions = Subject
            .Select(e => e.InnerException)
            .Where(e => e?.GetType() == innerException).ToArray();

        assertionChain
            .ForCondition(expectedExceptions.Length > 0)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected inner {0}{reason}, but found {1}.", innerException, SingleSubject.InnerException);

        return expectedExceptions;
    }

    private IEnumerable<Exception> AssertInnerExceptions(Type innerException,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.Any(e => e.InnerException is not null))
            .FailWith("Expected inner {0}{reason}, but the thrown exception has no inner exception.", innerException);

        Exception[] expectedInnerExceptions = Subject
            .Select(e => e.InnerException)
            .Where(e => e != null && e.GetType().IsSameOrInherits(innerException))
            .ToArray();

        assertionChain
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

                AssertionEngine.TestFramework.Throw(
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

    private void AssertExceptionMessage(IEnumerable<string> messages, string expectation,
        [StringSyntax("CompositeFormat")] string because, params object[] becauseArgs)
    {
        var results = new AssertionResultSet();

        foreach (string message in messages)
        {
            using (var scope = new AssertionScope())
            {
                var chain = AssertionChain.GetOrCreate();
                chain.OverrideCallerIdentifier(() => "exception message");
                chain.ReuseOnce();

                message.Should().MatchEquivalentOf(expectation, because, becauseArgs);

                results.AddSet(message, scope.Discard());
            }

            if (results.ContainsSuccessfulSet())
            {
                break;
            }
        }

        foreach (string failure in results.GetTheFailuresForTheSetWithTheFewestFailures())
        {
            assertionChain.FailWith("{0}", failure.AsNonFormattable());
        }
    }
}
