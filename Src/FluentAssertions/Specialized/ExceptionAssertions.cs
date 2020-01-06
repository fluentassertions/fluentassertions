using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;
using FluentAssertions.Primitives;

namespace FluentAssertions.Specialized
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref = "Exception" /> is in the correct state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ExceptionAssertions<TException> :
        ReferenceTypeAssertions<IEnumerable<TException>, ExceptionAssertions<TException>>
        where TException : Exception
    {
        #region Private Definitions

        private static readonly ExceptionMessageAssertion outerMessageAssertion = new ExceptionMessageAssertion();

        #endregion

        public ExceptionAssertions(IEnumerable<TException> exceptions) : base(exceptions)
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
        /// Asserts that the thrown exception has a message that matches <paramref name = "expectedWildcardPattern" />.
        /// </summary>
        /// <param name = "expectedWildcardPattern">
        /// The wildcard pattern with which the exception message is matched, where * and ? have special meanings.
        /// </param>
        /// <param name = "because">
        /// A formatted phrase as is supported by <see cref = "string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref = "because" />.
        /// </param>
        public virtual ExceptionAssertions<TException> WithMessage(string expectedWildcardPattern, string because = "",
            params object[] becauseArgs)
        {
            AssertionScope assertion = Execute.Assertion.BecauseOf(because, becauseArgs).UsingLineBreaks;

            assertion
                .ForCondition(Subject.Any())
                .FailWith("Expected exception with message {0}{reason}, but no exception was thrown.", expectedWildcardPattern);

            outerMessageAssertion.Execute(Subject.Select(exc => exc.Message).ToArray(), expectedWildcardPattern, because, becauseArgs);

            return this;
        }

        /// <summary>
        /// Asserts that the thrown exception contains an inner exception of type <typeparamref name = "TInnerException" />.
        /// </summary>
        /// <typeparam name = "TInnerException">The expected type of the inner exception.</typeparam>
        /// <param name = "because">The reason why the inner exception should be of the supplied type.</param>
        /// <param name = "becauseArgs">The parameters used when formatting the <paramref name = "because" />.</param>
        public virtual ExceptionAssertions<TInnerException> WithInnerException<TInnerException>(string because = null,
            params object[] becauseArgs)
            where TInnerException : Exception
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected inner {0}{reason}, but ", typeof(TInnerException))
                .ForCondition(Subject != null)
                .FailWith("no exception was thrown.")
                .Then
                .ForCondition(Subject.Any(e => e.InnerException != null))
                .FailWith("the thrown exception has no inner exception.")
                .Then
                .ClearExpectation();

            TInnerException[] expectedInnerExceptions = Subject
                .Select(e => e.InnerException)
                .OfType<TInnerException>()
                .ToArray();

            Execute.Assertion
                .ForCondition(expectedInnerExceptions.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected inner {0}{reason}, but found {1}.", typeof(TInnerException), SingleSubject.InnerException);

            return new ExceptionAssertions<TInnerException>(expectedInnerExceptions);
        }

        /// <summary>
        /// Asserts that the thrown exception contains an inner exception of the exact type <typeparamref name = "TInnerException" /> (and not a derived exception type).
        /// </summary>
        /// <typeparam name = "TInnerException">The expected type of the inner exception.</typeparam>
        /// <param name = "because">The reason why the inner exception should be of the supplied type.</param>
        /// <param name = "becauseArgs">The parameters used when formatting the <paramref name = "because" />.</param>
        public virtual ExceptionAssertions<TInnerException> WithInnerExceptionExactly<TInnerException>(string because = null,
            params object[] becauseArgs)
            where TInnerException : Exception
        {
            WithInnerException<TInnerException>(because, becauseArgs);

            TInnerException[] expectedExceptions = Subject
                .Select(e => e.InnerException)
                .OfType<TInnerException>()
                .Where(e => e?.GetType() == typeof(TInnerException)).ToArray();

            Execute.Assertion
                .ForCondition(expectedExceptions.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected inner {0}{reason}, but found {1}.", typeof(TInnerException), SingleSubject.InnerException);

            return new ExceptionAssertions<TInnerException>(expectedExceptions);
        }

        /// <summary>
        /// Asserts that the exception matches a particular condition.
        /// </summary>
        /// <param name = "exceptionExpression">
        /// The condition that the exception must match.
        /// </param>
        /// <param name = "because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name = "becauseArgs">
        /// Zero or more values to use for filling in any <see cref = "string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public ExceptionAssertions<TException> Where(Expression<Func<TException, bool>> exceptionExpression,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(exceptionExpression, nameof(exceptionExpression));

            Func<TException, bool> condition = exceptionExpression.Compile();
            Execute.Assertion
                .ForCondition(condition(SingleSubject))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected exception where {0}{reason}, but the condition was not met by:{1}{1}{2}.",
                    exceptionExpression.Body, Environment.NewLine, Subject);

            return this;
        }

        private TException SingleSubject
        {
            get
            {
                if (Subject.Count() > 1)
                {
                    string thrownExceptions = BuildExceptionsString(Subject);
                    Services.ThrowException(
                        string.Format(
                            "More than one exception was thrown.  FluentAssertions cannot determine which Exception was meant.{0}{1}",
                            Environment.NewLine, thrownExceptions));
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

        private class ExceptionMessageAssertion
        {
            public ExceptionMessageAssertion()
            {
                Context = "exception message";
            }

            public string Context { get; set; }

            public void Execute(IEnumerable<string> messages, string expectation, string because, params object[] becauseArgs)
            {
                using (new AssertionScope())
                {
                    var results = new AssertionResultSet();

                    foreach (string message in messages)
                    {
                        using (var scope = new AssertionScope())
                        {
                            scope.Context = Context;

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
                        AssertionScope.Current.FailWith(failure.Replace("{", "{{").Replace("}", "}}"));
                    }
                }
            }
        }
    }
}
