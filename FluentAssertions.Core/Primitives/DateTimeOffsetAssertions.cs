using System;
using System.Diagnostics;

using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is in the expected state.
    /// </summary>
    /// <remarks>
    /// You can use the <see cref="FluentDateTimeExtensions"/> for a more fluent way of specifying a <see cref="DateTime"/>.
    /// </remarks>
    [DebuggerNonUserCode]
    public class DateTimeOffsetAssertions
    {
        public DateTimeOffsetAssertions(DateTimeOffset? value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public DateTimeOffset? Subject { get; private set; }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is exactly equal to the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> Be(DateTime expected, string because = "", params object[] reasonArgs)
        {
            return Be(expected.ToDateTimeOffset(), because, reasonArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is exactly equal to the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> Be(DateTimeOffset expected, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && (Subject.Value == expected))
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected {context:date and time} to be {0}{reason}, but found {1}.",
                    expected, Subject.HasValue ? Subject.Value : default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is not equal to the <paramref name="unexpected"/> value.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBe(DateTime unexpected, string because = "",
            params object[] reasonArgs)
        {
            return NotBe(unexpected.ToDateTimeOffset(), because, reasonArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is not equal to the <paramref name="unexpected"/> value.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBe(DateTimeOffset unexpected, string because = "",
            params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue || (Subject.Value != unexpected))
                .BecauseOf(because, reasonArgs)
                .FailWith("Did not expect {context:date and time} to be {0}{reason}.", unexpected);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is within the specified number of milliseconds (default = 20 ms)
        /// from the specified <paramref name="nearbyTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="Be(DateTime, string, object[])"/>.
        /// </remarks>
        /// <param name="nearbyTime">
        /// The expected time to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of milliseconds which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeCloseTo(DateTime nearbyTime, int precision = 20, string because = "",
            params object[] reasonArgs)
        {
            return BeCloseTo(nearbyTime.ToDateTimeOffset(), precision, because, reasonArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is within the specified number of milliseconds (default = 20 ms)
        /// from the specified <paramref name="nearbyTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="Be(DateTimeOffset, string, object[])"/>.
        /// </remarks>
        /// <param name="nearbyTime">
        /// The expected time to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of milliseconds which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeCloseTo(DateTimeOffset nearbyTime, int precision = 20, string because = "",
            params object[] reasonArgs)
        {
            DateTimeOffset minimumValue = nearbyTime.AddMilliseconds(-precision);
            DateTimeOffset maximumValue = nearbyTime.AddMilliseconds(precision);

            Execute.Assertion
                .ForCondition(Subject.HasValue && (Subject.Value >= minimumValue) && (Subject.Value <= maximumValue))
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected {context:date and time} to be within {0} ms from {1}{reason}, but found {2}.", precision,
                    nearbyTime, Subject.HasValue ? Subject.Value : default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is before the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> or <see cref="DateTimeOffset"/> that the current value is expected to be before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeBefore(DateTime expected, string because = "",
            params object[] reasonArgs)
        {
            return BeBefore(expected.ToDateTimeOffset(), because, reasonArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is before the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> or <see cref="DateTimeOffset"/> that the current value is expected to be before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeBefore(DateTimeOffset expected, string because = "",
            params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) < 0)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected a {context:date and time} before {0}{reason}, but found {1}.", expected,
                    Subject.HasValue ? Subject.Value : default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is either on, or before the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> or <see cref="DateTimeOffset"/> that the current value is expected to be on or before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeOnOrBefore(DateTime expected, string because = "",
            params object[] reasonArgs)
        {
            return BeOnOrBefore(expected.ToDateTimeOffset(), because, reasonArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is either on, or before the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> or <see cref="DateTimeOffset"/> that the current value is expected to be on or before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeOnOrBefore(DateTimeOffset expected, string because = "",
            params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) <= 0)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected a {context:date and time} on or before {0}{reason}, but found {1}.", expected,
                    Subject.HasValue ? Subject.Value : default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is after the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> or <see cref="DateTimeOffset"/> that the current value is expected to be after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeAfter(DateTime expected, string because = "",
            params object[] reasonArgs)
        {
            return BeAfter(expected.ToDateTimeOffset(), because, reasonArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is after the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> or <see cref="DateTimeOffset"/> that the current value is expected to be after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeAfter(DateTimeOffset expected, string because = "",
            params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) > 0)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected a {context:date and time} after {0}{reason}, but found {1}.", expected,
                    Subject.HasValue ? Subject.Value : default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is either on, or after the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> or <see cref="DateTimeOffset"/> that the current value is expected to be on or after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeOnOrAfter(DateTime expected, string because = "",
            params object[] reasonArgs)
        {
            return BeOnOrAfter(expected.ToDateTimeOffset(), because, reasonArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> is either on, or after the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/> or <see cref="DateTimeOffset"/> that the current value is expected to be on or after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeOnOrAfter(DateTimeOffset expected, string because = "",
            params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) >= 0)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected a {context:date and time} on or after {0}{reason}, but found {1}.", expected,
                    Subject.HasValue ? Subject.Value : default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> has the <paramref name="expected"/> year.
        /// </summary>
        /// <param name="expected">The expected year of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveYear(int expected, string because = "", params object[] reasonArgs)
        {
            bool success = Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected year {0}{reason}, but found a <null> DateTime or DateTimeOffset.", expected);

            if (success)
            {
                Execute.Assertion
                    .ForCondition(Subject.Value.Year == expected)
                    .BecauseOf(because, reasonArgs)
                    .FailWith("Expected year {0}{reason}, but found {1}.", expected,
                        Subject.Value.Year);
            }

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> has the <paramref name="expected"/> month.
        /// </summary>
        /// <param name="expected">The expected month of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveMonth(int expected, string because = "", params object[] reasonArgs)
        {
            bool success = Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected month {0}{reason}, but found a <null> DateTime or DateTimeOffset.", expected);

            if (success)
            {
                Execute.Assertion
                    .ForCondition(Subject.Value.Month == expected)
                    .BecauseOf(because, reasonArgs)
                    .FailWith("Expected month {0}{reason}, but found {1}.", expected, Subject.Value.Month);
            }
            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> has the <paramref name="expected"/> day.
        /// </summary>
        /// <param name="expected">The expected day of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveDay(int expected, string because = "", params object[] reasonArgs)
        {
            bool success = Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected day {0}{reason}, but found a <null> DateTime or DateTimeOffset.", expected);

            if (success)
            {
                Execute.Assertion
                    .ForCondition(Subject.Value.Day == expected)
                    .BecauseOf(because, reasonArgs)
                    .FailWith("Expected day {0}{reason}, but found {1}.", expected, Subject.Value.Day);
            }

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> has the <paramref name="expected"/> hour.
        /// </summary>
        /// <param name="expected">The expected hour of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveHour(int expected, string because = "", params object[] reasonArgs)
        {
            bool success = Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected hour {0}{reason}, but found a <null> DateTime or DateTimeOffset.", expected);

            if (success)
            {
                Execute.Assertion
                    .ForCondition(Subject.Value.Hour == expected)
                    .BecauseOf(because, reasonArgs)
                    .FailWith("Expected hour {0}{reason}, but found {1}.", expected, Subject.Value.Hour);
            }

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> has the <paramref name="expected"/> minute.
        /// </summary>
        /// <param name="expected">The expected minutes of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveMinute(int expected, string because = "", params object[] reasonArgs)
        {
            bool success = Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected minute {0}{reason}, but found a <null> DateTime or DateTimeOffset.", expected);

            if (success)
            {
                Execute.Assertion
                    .ForCondition(Subject.Value.Minute == expected)
                    .BecauseOf(because, reasonArgs)
                    .FailWith("Expected minute {0}{reason}, but found {1}.", expected, Subject.Value.Minute);
            }

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> has the <paramref name="expected"/> second.
        /// </summary>
        /// <param name="expected">The expected seconds of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveSecond(int expected, string because = "", params object[] reasonArgs)
        {
            bool success = Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected second {0}{reason}, but found a <null> DateTime or DateTimeOffset.", expected);

            if (success)
            {
                Execute.Assertion
                    .ForCondition(Subject.Value.Second == expected)
                    .BecauseOf(because, reasonArgs)
                    .FailWith("Expected second {0}{reason}, but found {1}.", expected, Subject.Value.Second);
            }

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpanAssertions"/> object that can be used to assert that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/>
        /// exceeds the specified <paramref name="timeSpan"/> compared to another <see cref="DateTime"/> or <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> should exceed compared to another <see cref="DateTime"/> or <see cref="DateTimeOffset"/>.
        /// </param>
        public TimeSpanAssertions BeMoreThan(TimeSpan timeSpan)
        {
            return new TimeSpanAssertions(this, Subject, TimeSpanCondition.MoreThan, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpanAssertions"/> object that can be used to assert that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/>
        /// is equal to or exceeds the specified <paramref name="timeSpan"/> compared to another <see cref="DateTime"/> or <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> should be equal or exceed compared to
        /// another <see cref="DateTime"/>.
        /// </param>
        public TimeSpanAssertions BeAtLeast(TimeSpan timeSpan)
        {
            return new TimeSpanAssertions(this, Subject, TimeSpanCondition.AtLeast, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpanAssertions"/> object that can be used to assert that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/>
        /// differs exactly the specified <paramref name="timeSpan"/> compared to another <see cref="DateTime"/> or <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> should differ exactly compared to another <see cref="DateTime"/> or <see cref="DateTimeOffset"/>.
        /// </param>
        public TimeSpanAssertions BeExactly(TimeSpan timeSpan)
        {
            return new TimeSpanAssertions(this, Subject, TimeSpanCondition.Exactly, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpanAssertions"/> object that can be used to assert that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/>
        /// is within the specified <paramref name="timeSpan"/> compared to another <see cref="DateTime"/> or <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> should be within another <see cref="DateTime"/> or <see cref="DateTimeOffset"/>.
        /// </param>
        public TimeSpanAssertions BeWithin(TimeSpan timeSpan)
        {
            return new TimeSpanAssertions(this, Subject, TimeSpanCondition.Within, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpanAssertions"/> object that can be used to assert that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/>  
        /// differs at maximum the specified <paramref name="timeSpan"/> compared to another <see cref="DateTime"/> or <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The maximum amount of time that the current <see cref="DateTime"/> or <see cref="DateTimeOffset"/> should differ compared to another <see cref="DateTime"/> or <see cref="DateTimeOffset"/>.
        /// </param>
        public TimeSpanAssertions BeLessThan(TimeSpan timeSpan)
        {
            return new TimeSpanAssertions(this, Subject, TimeSpanCondition.LessThan, timeSpan);
        }
    }
}