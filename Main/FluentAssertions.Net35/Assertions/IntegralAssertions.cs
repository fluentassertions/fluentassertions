using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that an integral number is in the correct state.
    /// </summary>
    [DebuggerNonUserCode]
    public class IntegralAssertions<T> : NumericAssertions<T>
    {
        protected internal IntegralAssertions(T value) : base(value)
        {
        }

        /// <summary>
        /// Asserts that the integral number value is exactly the same as the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        public AndConstraint<NumericAssertions<T>> Be(T expected)
        {
            return Be(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the integral number value is exactly the same as the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> Be(T expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(ReferenceEquals(Subject, expected) || (Subject.CompareTo(expected) == 0))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the integral number value is not the same as the <paramref name="unexpected"/> value.
        /// </summary>
        /// <param name="unexpected">The unexpected value.</param>
        public AndConstraint<NumericAssertions<T>> NotBe(T unexpected)
        {
            return NotBe(unexpected, String.Empty);
        }

        /// <summary>
        /// Asserts that the integral number value is not the same as the <paramref name="unexpected"/> value.
        /// </summary>
        /// <param name="unexpected">The unexpected value.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> NotBe(T unexpected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(unexpected) != 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect {0}{reason}.", unexpected);

            return new AndConstraint<NumericAssertions<T>>(this);
        }
    }
}