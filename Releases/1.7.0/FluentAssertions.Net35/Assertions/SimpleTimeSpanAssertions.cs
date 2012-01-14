using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="TimeSpan"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class SimpleTimeSpanAssertions
    {
        protected internal SimpleTimeSpanAssertions(TimeSpan? value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public TimeSpan? Subject
        {
            get;
            private set;
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is greater than zero.
        /// </summary>
        public AndConstraint<SimpleTimeSpanAssertions> BePositive()
        {
            return BePositive(String.Empty);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is greater than zero.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BePositive(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(new TimeSpan()) > 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected positive value{reason}, but found {0}", Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is less than zero.
        /// </summary>
        public AndConstraint<SimpleTimeSpanAssertions> BeNegative()
        {
            return BeNegative(String.Empty);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is less than zero.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeNegative(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(new TimeSpan()) < 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected negative value{reason}, but found {0}", Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is equal to the 
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The expected time difference</param>
        public AndConstraint<SimpleTimeSpanAssertions> Be(TimeSpan expected)
        {
            return Be(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is equal to the 
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The expected time difference</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> Be(TimeSpan expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(expected) == 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", expected, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is not equal to the 
        /// specified <paramref name="unexpected"/> time.
        /// </summary>
        /// <param name="unexpected">The unexpected time difference</param>
        public AndConstraint<SimpleTimeSpanAssertions> NotBe(TimeSpan unexpected)
        {
            return NotBe(unexpected, String.Empty);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is not equal to the 
        /// specified <paramref name="unexpected"/> time.
        /// </summary>
        /// <param name="unexpected">The unexpected time difference</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> NotBe(TimeSpan unexpected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(unexpected) != 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect {0}{reason}.", unexpected);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is less than the 
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        public AndConstraint<SimpleTimeSpanAssertions> BeLessThan(TimeSpan expected)
        {
            return BeLessThan(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is less than the 
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeLessThan(TimeSpan expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(expected) < 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value less than {0}{reason}, but found {1}.", expected, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is less than or equal to the 
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        public AndConstraint<SimpleTimeSpanAssertions> BeLessOrEqualTo(TimeSpan expected)
        {
            return BeLessOrEqualTo(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is less than or equal to the 
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeLessOrEqualTo(TimeSpan expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(expected) <= 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value less or equal to {0}{reason}, but found {1}.", expected, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is greater than the 
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterThan(TimeSpan expected)
        {
            return BeGreaterThan(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is greater than the 
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterThan(TimeSpan expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(expected) > 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value greater than {0}{reason}, but found {1}.", expected, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is greater than or equal to the 
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterOrEqualTo(TimeSpan expected)
        {
            return BeGreaterOrEqualTo(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the time difference of the current <see cref="TimeSpan"/> is greater than or equal to the 
        /// specified <paramref name="expected"/> time.
        /// </summary>
        /// <param name="expected">The time difference to which the current value will be compared</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterOrEqualTo(TimeSpan expected, string reason,
            params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(expected) >= 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value greater or equal to {0}{reason}, but found {1}.", expected, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }
    }
}
