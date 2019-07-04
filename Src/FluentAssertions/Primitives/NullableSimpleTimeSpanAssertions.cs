using System;
using System.Diagnostics;
using FluentAssertions.Execution;
using FluentAssertions.Localization;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="TimeSpan"/> is in the expected state.
    /// </summary>
    /// <remarks>
    /// You can use the <see cref="FluentAssertions.Extensions.FluentTimeSpanExtensions"/>
    /// for a more fluent way of specifying a <see cref="TimeSpan"/>.
    /// </remarks>
    [DebuggerNonUserCode]
    public class NullableSimpleTimeSpanAssertions : SimpleTimeSpanAssertions
    {
        public NullableSimpleTimeSpanAssertions(TimeSpan? value)
            : base(value)
        {
        }

        /// <summary>
        /// Asserts that a nullable <see cref="TimeSpan"/> value is not <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<NullableSimpleTimeSpanAssertions> HaveValue(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Object_ExpectedAValue);

            return new AndConstraint<NullableSimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that a nullable <see cref="TimeSpan"/> value is not <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<NullableSimpleTimeSpanAssertions> NotBeNull(string because = "", params object[] becauseArgs)
        {
            return HaveValue(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that a nullable <see cref="TimeSpan"/> value is <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<NullableSimpleTimeSpanAssertions> NotHaveValue(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Object_DidNotExpectAValue + Resources.Common_CommaButFoundX0Format, Subject);

            return new AndConstraint<NullableSimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that a nullable <see cref="TimeSpan"/> value is <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<NullableSimpleTimeSpanAssertions> BeNull(string because = "", params object[] becauseArgs)
        {
            return NotHaveValue(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the value is equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NullableSimpleTimeSpanAssertions> Be(TimeSpan? expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Object_ExpectedX0Format + Resources.Common_CommaButFoundX1Format, expected, Subject);

            return new AndConstraint<NullableSimpleTimeSpanAssertions>(this);
        }
    }
}
