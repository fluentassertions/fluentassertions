using System;
using System.Diagnostics;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="DateTime"/> is in the expected state.
    /// </summary>
    /// <remarks>
    /// You can use the <see cref="FluentDateTimeExtensions"/> for a more fluent way of specifying a <see cref="DateTime"/>.
    /// </remarks>
    [DebuggerNonUserCode]
    public class DateTimeAssertions
    {
        protected internal DateTimeAssertions(DateTime? value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public DateTime? Subject { get; private set; }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> is exactly equal to the <paramref name="expected"/> value.
        /// </summary>
        public AndConstraint<DateTimeAssertions> Be(DateTime expected)
        {
            return Be(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> is exactly equal to the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> Be(DateTime expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected " + Verification.SubjectNameOr("DateTime") + " to be {0}{reason}, but found {1}.",
                    expected, Subject.Value);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> is not equal to the <paramref name="unexpected"/> value.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        public AndConstraint<DateTimeAssertions> NotBe(DateTime unexpected)
        {
            return NotBe(unexpected, String.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> is not equal to the <paramref name="unexpected"/> value.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> NotBe(DateTime unexpected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value != unexpected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect DateTime to be {0}{reason}.", unexpected);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> is before the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> that the current value is expected to be before.</param>
        public AndConstraint<DateTimeAssertions> BeBefore(DateTime expected)
        {
            return BeBefore(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> is before the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> that the current value is expected to be before.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeBefore(DateTime expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(expected) < 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a date/time before {0}{reason}, but found {1}.", expected, Subject.Value);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> is either on, or before the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> that the current value is expected to be on or before.</param>
        public AndConstraint<DateTimeAssertions> BeOnOrBefore(DateTime expected)
        {
            return BeOnOrBefore(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> is either on, or before the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> that the current value is expected to be on or before.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeOnOrBefore(DateTime expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(expected) <= 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a date/time on or before {0}{reason}, but found {1}.", expected, Subject.Value);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> is after the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> that the current value is expected to be after.</param>
        public AndConstraint<DateTimeAssertions> BeAfter(DateTime expected)
        {
            return BeAfter(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> is after the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> that the current value is expected to be after.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeAfter(DateTime expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(expected) > 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a date/time after {0}{reason}, but found {1}.", expected, Subject.Value);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> is either on, or after the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> that the current value is expected to be on or after.</param>
        public AndConstraint<DateTimeAssertions> BeOnOrAfter(DateTime expected)
        {
            return BeOnOrAfter(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> is either on, or after the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> that the current value is expected to be on or after.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeOnOrAfter(DateTime expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(expected) >= 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a date/time on or after {0}{reason}, but found {1}.", expected, Subject.Value);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> has the <paramref name="expected"/> year.
        /// </summary>
        /// <param name="expected">The expected year of the current value.</param>
        public AndConstraint<DateTimeAssertions> HaveYear(int expected)
        {
            return HaveYear(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> has the <paramref name="expected"/> year.
        /// </summary>
        /// <param name="expected">The expected year of the current value.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> HaveYear(int expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.Year == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected year {0}{reason}, but found {1}.", expected, Subject.Value.Year);
            
            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> has the <paramref name="expected"/> month.
        /// </summary>
        /// <param name="expected">The expected month of the current value.</param>
        public AndConstraint<DateTimeAssertions> HaveMonth(int expected)
        {
            return HaveMonth(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> has the <paramref name="expected"/> month.
        /// </summary>
        /// <param name="expected">The expected month of the current value.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> HaveMonth(int expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.Month == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected month {0}{reason}, but found {1}.", expected, Subject.Value.Month);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> has the <paramref name="expected"/> day.
        /// </summary>
        /// <param name="expected">The expected day of the current value.</param>
        public AndConstraint<DateTimeAssertions> HaveDay(int expected)
        {
            return HaveDay(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> has the <paramref name="expected"/> day.
        /// </summary>
        /// <param name="expected">The expected day of the current value.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> HaveDay(int expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.Day == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected day {0}{reason}, but found {1}.", expected, Subject.Value.Day);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> has the <paramref name="expected"/> hour.
        /// </summary>
        /// <param name="expected">The expected hour of the current value.</param>
        public AndConstraint<DateTimeAssertions> HaveHour(int expected)
        {
            return HaveHour(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> has the <paramref name="expected"/> hour.
        /// </summary>
        /// <param name="expected">The expected hour of the current value.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> HaveHour(int expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.Hour == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected hour {0}{reason}, but found {1}.", expected, Subject.Value.Hour);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> has the <paramref name="expected"/> minute.
        /// </summary>
        /// <param name="expected">The expected minutes of the current value.</param>
        public AndConstraint<DateTimeAssertions> HaveMinute(int expected)
        {
            return HaveMinute(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> has the <paramref name="expected"/> minute.
        /// </summary>
        /// <param name="expected">The expected minutes of the current value.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> HaveMinute(int expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.Minute == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected minute {0}{reason}, but found {1}.", expected, Subject.Value.Minute);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> has the <paramref name="expected"/> second.
        /// </summary>
        /// <param name="expected">The expected seconds of the current value.</param>
        public AndConstraint<DateTimeAssertions> HaveSecond(int expected)
        {
            return HaveSecond(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> has the <paramref name="expected"/> second.
        /// </summary>
        /// <param name="expected">The expected seconds of the current value.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> HaveSecond(int expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.Second == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected second {0}{reason}, but found {1}.", expected, Subject.Value.Second);

            return new AndConstraint<DateTimeAssertions>(this);
        }
        
        /// <summary>
        /// Returns a <see cref="TimeSpanAssertions"/> object that can be used to assert that the current <see cref="DateTime"/>
        /// exceeds the specified <paramref name="timeSpan"/> compared to another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTime"/> should exceed compared to another <see cref="DateTime"/>.
        /// </param>
        public TimeSpanAssertions BeMoreThan(TimeSpan timeSpan)
        {
            return new TimeSpanAssertions(this, Subject, TimeSpanCondition.MoreThan, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpanAssertions"/> object that can be used to assert that the current <see cref="DateTime"/>
        /// is equal to or exceeds the specified <paramref name="timeSpan"/> compared to another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTime"/> should be equal or exceed compared to
        /// another <see cref="DateTime"/>.
        /// </param>
        public TimeSpanAssertions BeAtLeast(TimeSpan timeSpan)
        {
            return new TimeSpanAssertions(this, Subject, TimeSpanCondition.AtLeast, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpanAssertions"/> object that can be used to assert that the current <see cref="DateTime"/>
        /// differs exactly the specified <paramref name="timeSpan"/> compared to another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTime"/> should differ exactly compared to another <see cref="DateTime"/>.
        /// </param>
        public TimeSpanAssertions BeExactly(TimeSpan timeSpan)
        {
            return new TimeSpanAssertions(this, Subject, TimeSpanCondition.Exactly, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpanAssertions"/> object that can be used to assert that the current <see cref="DateTime"/>
        /// is within the specified <paramref name="timeSpan"/> compared to another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTime"/> should be within another <see cref="DateTime"/>.
        /// </param>
        public TimeSpanAssertions BeWithin(TimeSpan timeSpan)
        {
            return new TimeSpanAssertions(this, Subject, TimeSpanCondition.Within, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpanAssertions"/> object that can be used to assert that the current <see cref="DateTime"/>
        /// differs at maximum the specified <paramref name="timeSpan"/> compared to another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The maximum amount of time that the current <see cref="DateTime"/> should differ compared to another <see cref="DateTime"/>.
        /// </param>
        public TimeSpanAssertions BeLessThan(TimeSpan timeSpan)
        {
            return new TimeSpanAssertions(this, Subject, TimeSpanCondition.LessThan, timeSpan);
        }
    }

    public enum TimeSpanCondition
    {
        MoreThan,
        AtLeast,
        Exactly,
        Within,
        LessThan
    }
}