using System;
using System.Diagnostics;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="DateTime"/> is in the expected state.
    /// </summary>
    /// <remarks>
    /// You can use the <see cref="FluentDateTimeExtensions"/> for a more fluent way of specifying a <see cref="DateTime"/>.
    /// </remarks>
    [DebuggerNonUserCode]
    public class NullableDateTimeAssertions : DateTimeAssertions
    {
        protected internal NullableDateTimeAssertions(DateTime? expected)
            : base(expected)
        {
        }

        /// <summary>
        /// Asserts that a nullable <see cref="DateTime"/> value is not <c>null</c>.
        /// </summary>
        public AndConstraint<NullableDateTimeAssertions> HaveValue()
        {
            return HaveValue(string.Empty);
        }

        /// <summary>
        /// Asserts that a nullable <see cref="DateTime"/> value is not <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>      
        public AndConstraint<NullableDateTimeAssertions> HaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected variable to have a value{reason}, but found {0}", Subject);

            return new AndConstraint<NullableDateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that a nullable <see cref="DateTime"/> value is <c>null</c>.
        /// </summary>
        public AndConstraint<NullableDateTimeAssertions> NotHaveValue()
        {
            return NotHaveValue(string.Empty);
        }

        /// <summary>
        /// Asserts that a nullable <see cref="DateTime"/> value is <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>      
        public AndConstraint<NullableDateTimeAssertions> NotHaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect variable to have a value{reason}, but found {0}", Subject);
            
            return new AndConstraint<NullableDateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the value is equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        public AndConstraint<DateTimeAssertions> Be(DateTime? expected)
        {
            return Be(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the value is equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> Be(DateTime? expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<DateTimeAssertions>(this);
        }
    }
}