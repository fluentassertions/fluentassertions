using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="DateTime"/> is in the expected state.
    /// </summary>
    /// <remarks>
    /// You can use the <see cref="FluentAssertions.Extensions.FluentDateTimeExtensions"/>
    /// for a more fluent way of specifying a <see cref="DateTime"/>.
    /// </remarks>
    [DebuggerNonUserCode]
    public class DateTimeAssertions
    {
        public DateTimeAssertions(DateTime? value)
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
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> Be(DateTime expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && (Subject.Value == expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:date and time} to be {0}{reason}, but found {1}.",
                    expected, Subject ?? default(DateTime?));

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> or <see cref="DateTime"/> is not equal to the <paramref name="unexpected"/> value.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> NotBe(DateTime unexpected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue || (Subject.Value != unexpected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:date and time} not to be {0}{reason}, but it is.", unexpected);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  is within the specified number of milliseconds (default = 20 ms)
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
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeCloseTo(DateTime nearbyTime, int precision = 20, string because = "",
            params object[] becauseArgs)
        {
            return BeCloseTo(nearbyTime, TimeSpan.FromMilliseconds(precision), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  is within the specified time
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
        /// The maximum amount of time which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeCloseTo(DateTime nearbyTime, TimeSpan precision, string because = "",
            params object[] becauseArgs)
        {
            long distanceToMinInTicks = (nearbyTime - DateTime.MinValue).Ticks;
            DateTime minimumValue = nearbyTime.AddTicks(-Math.Min(precision.Ticks, distanceToMinInTicks));

            long distanceToMaxInTicks = (DateTime.MaxValue - nearbyTime).Ticks;
            DateTime maximumValue = nearbyTime.AddTicks(Math.Min(precision.Ticks, distanceToMaxInTicks));

            Execute.Assertion
                .ForCondition(Subject.HasValue && (Subject.Value >= minimumValue) && (Subject.Value <= maximumValue))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the date and time} to be within {0} from {1}{reason}, but found {2}.",
                    precision,
                    nearbyTime, Subject ?? default(DateTime?));

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  is not within the specified number of milliseconds (default = 20 ms)
        /// from the specified <paramref name="distantTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="NotBe(DateTime, string, object[])"/>.
        /// </remarks>
        /// <param name="distantTime">
        /// The time to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of milliseconds which the two values must differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> NotBeCloseTo(DateTime distantTime, int precision = 20, string because = "",
            params object[] becauseArgs)
        {
            return NotBeCloseTo(distantTime, TimeSpan.FromMilliseconds(precision), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  is not within the specified time
        /// from the specified <paramref name="distantTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="NotBe(DateTime, string, object[])"/>.
        /// </remarks>
        /// <param name="distantTime">
        /// The time to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of time which the two values must differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> NotBeCloseTo(DateTime distantTime, TimeSpan precision, string because = "",
            params object[] becauseArgs)
        {
            long distanceToMinInTicks = (distantTime - DateTime.MinValue).Ticks;
            DateTime minimumValue = distantTime.AddTicks(-Math.Min(precision.Ticks, distanceToMinInTicks));

            long distanceToMaxInTicks = (DateTime.MaxValue - distantTime).Ticks;
            DateTime maximumValue = distantTime.AddTicks(Math.Min(precision.Ticks, distanceToMaxInTicks));

            Execute.Assertion
                .ForCondition(Subject.HasValue && ((Subject.Value < minimumValue) || (Subject.Value > maximumValue)))
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Did not expect {context:the date and time} to be within {0} from {1}{reason}, but it was {2}.",
                    precision,
                    distantTime, Subject ?? default(DateTime?));

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  is before the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/>  that the current value is expected to be before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeBefore(DateTime expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) < 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the date and time} to be before {0}{reason}, but found {1}.", expected,
                    Subject ?? default(DateTime?));

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  is not before the specified value.
        /// </summary>
        /// <param name="unexpected">The <see cref="DateTime"/>  that the current value is not expected to be before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> NotBeBefore(DateTime unexpected, string because = "",
            params object[] becauseArgs)
        {
            return BeOnOrAfter(unexpected, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  is either on, or before the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/>  that the current value is expected to be on or before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeOnOrBefore(DateTime expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) <= 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the date and time} to be on or before {0}{reason}, but found {1}.", expected,
                    Subject ?? default(DateTime?));

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  is neither on, nor before the specified value.
        /// </summary>
        /// <param name="unexpected">The <see cref="DateTime"/>  that the current value is not expected to be on nor before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> NotBeOnOrBefore(DateTime unexpected, string because = "",
            params object[] becauseArgs)
        {
            return BeAfter(unexpected, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  is after the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/>  that the current value is expected to be after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeAfter(DateTime expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) > 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the date and time} to be after {0}{reason}, but found {1}.", expected,
                    Subject ?? default(DateTime?));

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  is not after the specified value.
        /// </summary>
        /// <param name="unexpected">The <see cref="DateTime"/>  that the current value is not expected to be after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> NotBeAfter(DateTime unexpected, string because = "",
            params object[] becauseArgs)
        {
            return BeOnOrBefore(unexpected, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  is either on, or after the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTime"/>  that the current value is expected to be on or after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeOnOrAfter(DateTime expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) >= 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the date and time} to be on or after {0}{reason}, but found {1}.", expected,
                    Subject ?? default(DateTime?));

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  is neither on, nor after the specified value.
        /// </summary>
        /// <param name="unexpected">The <see cref="DateTime"/>  that the current value is expected not to be on nor after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> NotBeOnOrAfter(DateTime unexpected, string because = "",
            params object[] becauseArgs)
        {
            return BeBefore(unexpected, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> has the <paramref name="expected"/> year.
        /// </summary>
        /// <param name="expected">The expected year of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> HaveYear(int expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the year part of {context:the date} to be {0}{reason}", expected)
                .ForCondition(Subject.HasValue)
                .FailWith(", but found <null>.")
                .Then
                .ForCondition(Subject.Value.Year == expected)
                .FailWith(", but found {0}.", Subject.Value.Year)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> does not have the <paramref name="unexpected"/> year.
        /// </summary>
        /// <param name="unexpected">The year that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> NotHaveYear(int unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.HasValue)
                .FailWith("Did not expect the year part of {context:the date} to be {0}{reason}, but found a <null> DateTime.", unexpected)
                .Then
                .ForCondition(Subject.Value.Year != unexpected)
                .FailWith("Did not expect the year part of {context:the date} to be {0}{reason}, but it was.", unexpected,
                    Subject.Value.Year);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> has the <paramref name="expected"/> month.
        /// </summary>
        /// <param name="expected">The expected month of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> HaveMonth(int expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the month part of {context:the date} to be {0}{reason}", expected)
                .ForCondition(Subject.HasValue)
                .FailWith(", but found a <null> DateTime.")
                .Then
                .ForCondition(Subject.Value.Month == expected)
                .FailWith(", but found {0}.", Subject.Value.Month)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> does not have the <paramref name="unexpected"/> month.
        /// </summary>
        /// <param name="unexpected">The month that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> NotHaveMonth(int unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Did not expect the month part of {context:the date} to be {0}{reason}", unexpected)
                .ForCondition(Subject.HasValue)
                .FailWith(", but found a <null> DateTime.")
                .Then
                .ForCondition(Subject.Value.Month != unexpected)
                .FailWith(", but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  has the <paramref name="expected"/> day.
        /// </summary>
        /// <param name="expected">The expected day of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> HaveDay(int expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the day part of {context:the date} to be {0}{reason}", expected)
                .ForCondition(Subject.HasValue)
                .FailWith(", but found a <null> DateTime.")
                .Then
                .ForCondition(Subject.Value.Day == expected)
                .FailWith(", but found {0}.", Subject.Value.Day)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> does not have the <paramref name="unexpected"/> day.
        /// </summary>
        /// <param name="unexpected">The day that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> NotHaveDay(int unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Did not expect the day part of {context:the date} to be {0}{reason}", unexpected)
                .ForCondition(Subject.HasValue)
                .FailWith(", but found a <null> DateTime.")
                .Then
                .ForCondition(Subject.Value.Day != unexpected)
                .FailWith(", but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  has the <paramref name="expected"/> hour.
        /// </summary>
        /// <param name="expected">The expected hour of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> HaveHour(int expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the hour part of {context:the time} to be {0}{reason}", expected)
                .ForCondition(Subject.HasValue)
                .FailWith(", but found a <null> DateTime.")
                .Then
                .ForCondition(Subject.Value.Hour == expected)
                .FailWith(", but found {0}.", Subject.Value.Hour)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> does not have the <paramref name="unexpected"/> hour.
        /// </summary>
        /// <param name="unexpected">The hour that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> NotHaveHour(int unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Did not expect the hour part of {context:the time} to be {0}{reason}", unexpected)
                .ForCondition(Subject.HasValue)
                .FailWith(", but found a <null> DateTime.", unexpected)
                .Then
                .ForCondition(Subject.Value.Hour != unexpected)
                .FailWith(", but it was.", unexpected, Subject.Value.Hour)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  has the <paramref name="expected"/> minute.
        /// </summary>
        /// <param name="expected">The expected minutes of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> HaveMinute(int expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the minute part of {context:the time} to be {0}{reason}", expected)
                .ForCondition(Subject.HasValue)
                .FailWith(", but found a <null> DateTime.")
                .Then
                .ForCondition(Subject.Value.Minute == expected)
                .FailWith(", but found {0}.", Subject.Value.Minute)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> does not have the <paramref name="unexpected"/> minute.
        /// </summary>
        /// <param name="unexpected">The minute that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> NotHaveMinute(int unexpected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Did not expect the minute part of {context:the time} to be {0}{reason}", unexpected)
                .ForCondition(Subject.HasValue)
                .FailWith(", but found a <null> DateTime.", unexpected)
                .Then
                .ForCondition(Subject.Value.Minute != unexpected)
                .FailWith(", but it was.", unexpected, Subject.Value.Minute)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/>  has the <paramref name="expected"/> second.
        /// </summary>
        /// <param name="expected">The expected seconds of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> HaveSecond(int expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the seconds part of {context:the time} to be {0}{reason}", expected)
                .ForCondition(Subject.HasValue)
                .FailWith(", but found a <null> DateTime.")
                .Then
                .ForCondition(Subject.Value.Second == expected)
                .FailWith(", but found {0}.", Subject.Value.Second)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> does not have the <paramref name="unexpected"/> second.
        /// </summary>
        /// <param name="unexpected">The second that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> NotHaveSecond(int unexpected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Did not expect the seconds part of {context:the time} to be {0}{reason}", unexpected)
                .ForCondition(Subject.HasValue)
                .FailWith(", but found a <null> DateTime.")
                .Then
                .ForCondition(Subject.Value.Second != unexpected)
                .FailWith(", but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Returns a <see cref="DateTimeRangeAssertions"/> object that can be used to assert that the current <see cref="DateTime"/>
        /// exceeds the specified <paramref name="timeSpan"/> compared to another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTime"/>  should exceed compared to another <see cref="DateTime"/>.
        /// </param>
        public DateTimeRangeAssertions BeMoreThan(TimeSpan timeSpan)
        {
            return new DateTimeRangeAssertions(this, Subject, TimeSpanCondition.MoreThan, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="DateTimeRangeAssertions"/> object that can be used to assert that the current <see cref="DateTime"/>
        /// is equal to or exceeds the specified <paramref name="timeSpan"/> compared to another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTime"/>  should be equal or exceed compared to
        /// another <see cref="DateTime"/>.
        /// </param>
        public DateTimeRangeAssertions BeAtLeast(TimeSpan timeSpan)
        {
            return new DateTimeRangeAssertions(this, Subject, TimeSpanCondition.AtLeast, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="DateTimeRangeAssertions"/> object that can be used to assert that the current <see cref="DateTime"/>
        /// differs exactly the specified <paramref name="timeSpan"/> compared to another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTime"/>  should differ exactly compared to another <see cref="DateTime"/>.
        /// </param>
        public DateTimeRangeAssertions BeExactly(TimeSpan timeSpan)
        {
            return new DateTimeRangeAssertions(this, Subject, TimeSpanCondition.Exactly, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="DateTimeRangeAssertions"/> object that can be used to assert that the current <see cref="DateTime"/>
        /// is within the specified <paramref name="timeSpan"/> compared to another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTime"/>  should be within another <see cref="DateTime"/>.
        /// </param>
        public DateTimeRangeAssertions BeWithin(TimeSpan timeSpan)
        {
            return new DateTimeRangeAssertions(this, Subject, TimeSpanCondition.Within, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="DateTimeRangeAssertions"/> object that can be used to assert that the current <see cref="DateTime"/>
        /// differs at maximum the specified <paramref name="timeSpan"/> compared to another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The maximum amount of time that the current <see cref="DateTime"/>  should differ compared to another <see cref="DateTime"/>.
        /// </param>
        public DateTimeRangeAssertions BeLessThan(TimeSpan timeSpan)
        {
            return new DateTimeRangeAssertions(this, Subject, TimeSpanCondition.LessThan, timeSpan);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> has the <paramref name="expected"/> date.
        /// </summary>
        /// <param name="expected">The expected date portion of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeSameDateAs(DateTime expected, string because = "",
            params object[] becauseArgs)
        {
            DateTime expectedDate = expected.Date;

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the date part of {context:the date and time} to be {0}{reason}", expectedDate)
                .ForCondition(Subject.HasValue)
                .FailWith(", but found a <null> DateTime.", expectedDate)
                .Then
                .ForCondition(Subject.Value.Date == expectedDate)
                .FailWith(", but found {1}.", expectedDate, Subject.Value)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTime"/> is not the <paramref name="unexpected"/> date.
        /// </summary>
        /// <param name="unexpected">The date that is not to match the date portion of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> NotBeSameDateAs(DateTime unexpected, string because = "",
            params object[] becauseArgs)
        {
            DateTime unexpectedDate = unexpected.Date;

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Did not expect the date part of {context:the date and time} to be {0}{reason}", unexpectedDate)
                .ForCondition(Subject.HasValue)
                .FailWith(", but found a <null> DateTime.")
                .Then
                .ForCondition(Subject.Value.Date != unexpectedDate)
                .FailWith(", but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="DateTime"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeOneOf(params DateTime?[] validValues)
        {
            return BeOneOf(validValues, string.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="DateTime"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeOneOf(params DateTime[] validValues)
        {
            return BeOneOf(validValues.Cast<DateTime?>());
        }

        /// <summary>
        /// Asserts that the <see cref="DateTime"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeOneOf(IEnumerable<DateTime> validValues, string because = "", params object[] becauseArgs)
        {
            return BeOneOf(validValues.Cast<DateTime?>(), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <see cref="DateTime"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeOneOf(IEnumerable<DateTime?> validValues, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(validValues.Contains(Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:date and time} to be one of {0}{reason}, but found {1}.", validValues, Subject);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="DateTime"/> represents a value in the <paramref name="expectedKind"/>.
        /// </summary>
        /// <param name="expectedKind">
        /// The expected <see cref="DateTimeKind"/> that the current value must represent.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeAssertions> BeIn(DateTimeKind expectedKind, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:the date and time} to be in {0}{reason}", expectedKind)
                .ForCondition(Subject.HasValue)
                .FailWith(", but found a <null> DateTime.")
                .Then
                .ForCondition(Subject.Value.Kind == expectedKind)
                .FailWith(", but found {0}.", Subject.Value.Kind)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeAssertions>(this);
        }
    }
}
