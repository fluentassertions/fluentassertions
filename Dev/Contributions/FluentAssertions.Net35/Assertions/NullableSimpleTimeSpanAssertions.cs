using System;
using System.Diagnostics;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="TimeSpan"/> is in the expected state.
    /// </summary>
    /// <remarks>
    /// You can use the <see cref="TimeSpanConversionExtensions"/> for a more fluent way of specifying a <see cref="TimeSpan"/>.
    /// </remarks>
    [DebuggerNonUserCode]
    public class NullableSimpleTimeSpanAssertions : SimpleTimeSpanAssertions
    {
        protected internal NullableSimpleTimeSpanAssertions(TimeSpan? value)
            : base(value)
        {
        }

        /// <summary>
        /// Asserts that a nullable <see cref="TimeSpan"/> value is not <c>null</c>.
        /// </summary>
        public AndConstraint<NullableSimpleTimeSpanAssertions> HaveValue()
        {
            return HaveValue(String.Empty);
        }

        /// <summary>
        /// Asserts that a nullable <see cref="TimeSpan"/> value is not <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>      
        public AndConstraint<NullableSimpleTimeSpanAssertions> HaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value{reason}.");

            return new AndConstraint<NullableSimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that a nullable <see cref="TimeSpan"/> value is <c>null</c>.
        /// </summary>
        public AndConstraint<NullableSimpleTimeSpanAssertions> NotHaveValue()
        {
            return NotHaveValue(String.Empty);
        }

        /// <summary>
        /// Asserts that a nullable <see cref="TimeSpan"/> value is <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>      
        public AndConstraint<NullableSimpleTimeSpanAssertions> NotHaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect a value{reason}, but found {0}.", Subject);

            return new AndConstraint<NullableSimpleTimeSpanAssertions>(this);
        }
    }
}