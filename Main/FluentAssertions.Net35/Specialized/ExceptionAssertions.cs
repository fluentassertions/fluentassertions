using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Specialized
{
    /// <summary>
    ///   Contains a number of methods to assert that an <see cref = "Exception" /> is in the correct state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ExceptionAssertions<TException> : ReferenceTypeAssertions<Exception, ExceptionAssertions<TException>>
        where TException : Exception
    {
        private static readonly Dictionary<ComparisonMode, ExceptionMessageAssertion> outerMessageAssertions =
            new Dictionary<ComparisonMode, ExceptionMessageAssertion>();

        private static readonly Dictionary<ComparisonMode, ExceptionMessageAssertion> innerMessageAssertions =
            new Dictionary<ComparisonMode, ExceptionMessageAssertion>();

        static ExceptionAssertions()
        {
            SetupMessageAssertionRules();
        }

        protected internal ExceptionAssertions(TException exception)
        {
            Subject = exception;
        }

        /// <summary>
        ///   Gets the exception object of the exception thrown.
        /// </summary>
        public TException And
        {
            get { return (TException) Subject; }
        }

        /// <summary>
        ///   Asserts that the thrown exception has a message that matches <paramref name = "expectedMessage" />
        ///   depending on the specified matching mode.
        /// </summary>
        /// <param name = "expectedMessage">
        ///   The expected message of the exception.
        /// </param>
        /// <param name = "reason">
        ///   A formatted phrase as is supported by <see cref = "string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "reasonArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "reason" />.
        /// </param>
        public virtual ExceptionAssertions<TException> WithMessage(string expectedMessage, string reason = "",
            params object[] reasonArgs)
        {
            return WithMessage(expectedMessage, ComparisonMode.Exact, reason, reasonArgs);
        }


        /// <summary>
        ///   Asserts that the thrown exception has a message that matches <paramref name = "expectedMessage" />
        ///   depending on the specified matching mode.
        /// </summary>
        /// <param name = "expectedMessage">
        ///   The expected message of the exception.
        /// </param>
        /// <param name = "comparisonMode">
        ///   Determines how the expected message is compared with the actual message.
        /// </param>
        /// <param name = "reason">
        ///   A formatted phrase as is supported by <see cref = "string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "reasonArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "reason" />.
        /// </param>
        public virtual ExceptionAssertions<TException> WithMessage(string expectedMessage, ComparisonMode comparisonMode,
            string reason = "", params object[] reasonArgs)
        {
            AssertionScope assertion = Execute.Assertion.BecauseOf(reason, reasonArgs).UsingLineBreaks;

            assertion.ForCondition(Subject != null).FailWith(
                "Expected exception with message {0}{reason}, but no exception was thrown.", expectedMessage);

            ExceptionMessageAssertion messageAssertion = outerMessageAssertions[comparisonMode];
            messageAssertion.Execute(Subject.Message, expectedMessage, reason, reasonArgs);

            return this;
        }

        /// <summary>
        ///   Asserts that the thrown exception contains an inner exception of type <typeparamref name = "TInnerException" />.
        /// </summary>
        /// <typeparam name = "TInnerException">The expected type of the inner exception.</typeparam>
        public ExceptionAssertions<TException> WithInnerException<TInnerException>()
        {
            return WithInnerException<TInnerException>(null, null);
        }

        /// <summary>
        ///   Asserts that the thrown exception contains an inner exception of type <typeparamref name = "TInnerException" />.
        /// </summary>
        /// <typeparam name = "TInnerException">The expected type of the inner exception.</typeparam>
        /// <param name = "reason">The reason why the inner exception should be of the supplied type.</param>
        /// <param name = "reasonArgs">The parameters used when formatting the <paramref name = "reason" />.</param>
        public virtual ExceptionAssertions<TException> WithInnerException<TInnerException>(string reason,
            params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected inner {0}{reason}, but no exception was thrown.", typeof (TInnerException));

            Execute.Assertion
                .ForCondition(Subject.InnerException != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected inner {0}{reason}, but the thrown exception has no inner exception.",
                    typeof (TInnerException));

            Execute.Assertion
                .ForCondition(Subject.InnerException != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected inner {0}{reason}, but the thrown exception has no inner exception.",
                    typeof (TInnerException));

            Execute.Assertion
                .ForCondition(Subject.InnerException is TInnerException)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected inner {0}{reason}, but found {1}.", typeof (TInnerException), Subject.InnerException);

            return this;
        }

        /// <summary>
        ///   Asserts that the thrown exception contains an inner exception with the <paramref name = "expectedInnerMessage" />.
        /// </summary>
        /// <param name = "expectedInnerMessage">The expected message of the inner exception.</param>
        /// <param name = "comparisonMode">Determines how the expected message is compared with the actual message.</param>
        public ExceptionAssertions<TException> WithInnerMessage(string expectedInnerMessage,
            ComparisonMode comparisonMode)
        {
            return WithInnerMessage(expectedInnerMessage, comparisonMode, null, null);
        }

        /// <summary>
        ///   Asserts that the thrown exception contains an inner exception with the <paramref name = "expectedInnerMessage" />.
        /// </summary>
        /// <param name = "expectedInnerMessage">The expected message of the inner exception.</param>
        /// <param name = "reason">
        ///   The reason why the message of the inner exception should match <paramref name = "expectedInnerMessage" />.
        /// </param>
        /// <param name = "reasonArgs">The parameters used when formatting the <paramref name = "reason" />.</param>
        public virtual ExceptionAssertions<TException> WithInnerMessage(string expectedInnerMessage, string reason = "",
            params object[] reasonArgs)
        {
            return WithInnerMessage(expectedInnerMessage, ComparisonMode.Exact, reason, reasonArgs);
        }

        /// <summary>
        ///   Asserts that the thrown exception contains an inner exception with the <paramref name = "expectedInnerMessage" />.
        /// </summary>
        /// <param name = "expectedInnerMessage">The expected message of the inner exception.</param>
        /// <param name = "comparisonMode">Determines how the expected message is compared with the actual message.</param>
        /// <param name = "reason">
        ///   The reason why the message of the inner exception should match <paramref name = "expectedInnerMessage" />.
        /// </param>
        /// <param name = "reasonArgs">The parameters used when formatting the <paramref name = "reason" />.</param>
        public virtual ExceptionAssertions<TException> WithInnerMessage(string expectedInnerMessage,
            ComparisonMode comparisonMode, string reason, params object[] reasonArgs)
        {
            AssertionScope assertion = Execute.Assertion
                .BecauseOf(reason, reasonArgs)
                .UsingLineBreaks;

            assertion
                .ForCondition(Subject != null)
                .FailWith("Expected inner exception{reason}, but no exception was thrown.");

            assertion
                .ForCondition(Subject.InnerException != null)
                .FailWith("Expected inner exception{reason}, but the thrown exception has no inner exception.");

            string subjectInnerMessage = Subject.InnerException.Message;

            ExceptionMessageAssertion messageAssertion = innerMessageAssertions[comparisonMode];
            messageAssertion.Execute(subjectInnerMessage, expectedInnerMessage, reason, reasonArgs);

            return this;
        }

        /// <summary>
        ///   Asserts that the exception matches a particular condition.
        /// </summary>
        /// <param name = "exceptionExpression">
        ///   The condition that the exception must match.
        /// </param>
        /// <param name = "reason">
        ///   A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        ///   start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name = "reasonArgs">
        ///   Zero or more values to use for filling in any <see cref = "string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public ExceptionAssertions<TException> Where(Expression<Func<TException, bool>> exceptionExpression,
            string reason = "", params object[] reasonArgs)
        {
            Func<TException, bool> condition = exceptionExpression.Compile();
            Execute.Assertion
                .ForCondition(condition((TException) Subject))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected exception where {0}{reason}, but the condition was not met by:\r\n\r\n{1}",
                    exceptionExpression.Body, Subject);

            return this;
        }

        private static void SetupMessageAssertionRules()
        {
            outerMessageAssertions[ComparisonMode.Exact] = new ExceptionMessageAssertion(
                "exception message",
                (subject, expectation, reason, reasonArgs) =>
                    subject.Should().Be(expectation, reason, reasonArgs));

            outerMessageAssertions[ComparisonMode.Equivalent] = new ExceptionMessageAssertion(
                "equivalent of exception message",
                (subject, expectation, reason, reasonArgs) =>
                    subject.Should().BeEquivalentTo(expectation, reason, reasonArgs));

            outerMessageAssertions[ComparisonMode.StartWith] = new ExceptionMessageAssertion(
                "exception message",
                (subject, expectation, reason, reasonArgs) =>
                    subject.Should().StartWith(expectation, reason, reasonArgs));

            outerMessageAssertions[ComparisonMode.StartWithEquivalent] = new ExceptionMessageAssertion(
                "exception message",
                (subject, expectation, reason, reasonArgs) =>
                    subject.Should().StartWithEquivalent(expectation, reason, reasonArgs));

            outerMessageAssertions[ComparisonMode.Substring] = new ExceptionMessageAssertion(
                "exception message to contain",
                (subject, expectation, reason, reasonArgs) =>
                    subject.Should().Contain(expectation, reason, reasonArgs));

            outerMessageAssertions[ComparisonMode.EquivalentSubstring] = new ExceptionMessageAssertion(
                "exception message to contain equivalent of",
                (subject, expectation, reason, reasonArgs) =>
                    subject.Should().ContainEquivalentOf(expectation, reason, reasonArgs));

            outerMessageAssertions[ComparisonMode.Wildcard] = new ExceptionMessageAssertion(
                "exception message",
                (subject, expectation, reason, reasonArgs) =>
                    subject.Should().Match(expectation, reason, reasonArgs));

            innerMessageAssertions[ComparisonMode.Exact] = new ExceptionMessageAssertion(
                "inner exception message",
                (subject, expectation, reason, reasonArgs) =>
                    subject.Should().Be(expectation, reason, reasonArgs));
            
            innerMessageAssertions[ComparisonMode.Equivalent] = new ExceptionMessageAssertion(
                "inner exception message",
                (subject, expectation, reason, reasonArgs) =>
                    subject.Should().BeEquivalentTo(expectation, reason, reasonArgs));

            innerMessageAssertions[ComparisonMode.Substring] = new ExceptionMessageAssertion(
                "inner exception message to contain",
                (subject, expectation, reason, reasonArgs) =>
                    subject.Should().Contain(expectation, reason, reasonArgs));            

            innerMessageAssertions[ComparisonMode.StartWith] = new ExceptionMessageAssertion(
                "inner exception message",
                (subject, expectation, reason, reasonArgs) =>
                    subject.Should().StartWith(expectation, reason, reasonArgs));            
            
            innerMessageAssertions[ComparisonMode.StartWithEquivalent] = new ExceptionMessageAssertion(
                "inner exception message",
                (subject, expectation, reason, reasonArgs) =>
                    subject.Should().StartWithEquivalent(expectation, reason, reasonArgs));            
            
            innerMessageAssertions[ComparisonMode.EquivalentSubstring] = new ExceptionMessageAssertion(
                "inner exception message to contain equivalent of",
                (subject, expectation, reason, reasonArgs) =>
                    subject.Should().ContainEquivalentOf(expectation, reason, reasonArgs));

            innerMessageAssertions[ComparisonMode.Wildcard] = new ExceptionMessageAssertion(
                "inner exception message",
                (subject, expectation, reason, reasonArgs) =>
                    subject.Should().Match(expectation, reason, reasonArgs));
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Context
        {
            get { return "exception"; }
        }

        private class ExceptionMessageAssertion
        {
            private readonly string subjectName;
            private readonly Action<string, string, string, object[]> assertion;

            public ExceptionMessageAssertion(string subjectName, Action<string, string, string, object[]> assertion)
            {
                this.subjectName = subjectName;
                this.assertion = assertion;
            }

            public void Execute(string actual, string expectation, string reason, params object[] reasonArgs)
            {
                using (var scope = new AssertionScope())
                {
                    scope.AddNonReportable("context", subjectName);
                    
                    assertion(actual, expectation, reason, reasonArgs);
                }
            }
        }
    }
}