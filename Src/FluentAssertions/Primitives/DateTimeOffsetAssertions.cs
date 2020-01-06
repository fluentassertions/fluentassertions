using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="DateTimeOffset"/> is in the expected state.
    /// </summary>
    /// <remarks>
    /// You can use the <see cref="FluentAssertions.Extensions.FluentDateTimeExtensions"/>
    /// for a more fluent way of specifying a <see cref="DateTimeOffset"/>.
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
        /// Asserts that the current <see cref="DateTimeOffset"/> is exactly equal to the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> Be(DateTimeOffset expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && (Subject.Value == expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the date and time} to be {0}{reason}, but it was {1}.",
                    expected, Subject ?? default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is not equal to the <paramref name="unexpected"/> value.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBe(DateTimeOffset unexpected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue || (Subject.Value != unexpected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect {context:the date and time} to be {0}{reason}, but it was.", unexpected);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is within the specified number of milliseconds (default = 20 ms)
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
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeCloseTo(DateTimeOffset nearbyTime, int precision = 20,
            string because = "",
            params object[] becauseArgs)
        {
            return BeCloseTo(nearbyTime, TimeSpan.FromMilliseconds(precision), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is within the specified time
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
        /// The maximum amount of time which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeCloseTo(DateTimeOffset nearbyTime, TimeSpan precision,
            string because = "",
            params object[] becauseArgs)
        {
            long distanceToMinInTicks = (nearbyTime - DateTimeOffset.MinValue).Ticks;
            DateTimeOffset minimumValue = nearbyTime.AddTicks(-Math.Min(precision.Ticks, distanceToMinInTicks));

            long distanceToMaxInTicks = (long)(DateTimeOffset.MaxValue - nearbyTime).Ticks;
            DateTimeOffset maximumValue = nearbyTime.AddTicks(Math.Min(precision.Ticks, distanceToMaxInTicks));

            Execute.Assertion
                .ForCondition(Subject.HasValue && (Subject.Value >= minimumValue) && (Subject.Value <= maximumValue))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the date and time} to be within {0} from {1}{reason}, but it was {2}.",
                    precision,
                    nearbyTime, Subject ?? default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is not within the specified number of milliseconds (default = 20 ms)
        /// from the specified <paramref name="distantTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="NotBe(DateTimeOffset, string, object[])"/>.
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
        public AndConstraint<DateTimeOffsetAssertions> NotBeCloseTo(DateTimeOffset distantTime, int precision = 20, string because = "",
            params object[] becauseArgs)
        {
            return NotBeCloseTo(distantTime, TimeSpan.FromMilliseconds(precision), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is not within the specified time
        /// from the specified <paramref name="distantTime"/> value.
        /// </summary>
        /// <remarks>
        /// Use this assertion when, for example the database truncates datetimes to nearest 20ms. If you want to assert to the exact datetime,
        /// use <see cref="NotBe(DateTimeOffset, string, object[])"/>.
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
        public AndConstraint<DateTimeOffsetAssertions> NotBeCloseTo(DateTimeOffset distantTime, TimeSpan precision, string because = "",
            params object[] becauseArgs)
        {
            long distanceToMinInTicks = (distantTime - DateTimeOffset.MinValue).Ticks;
            DateTimeOffset minimumValue = distantTime.AddTicks(-Math.Min(precision.Ticks, distanceToMinInTicks));

            long distanceToMaxInTicks = (DateTimeOffset.MaxValue - distantTime).Ticks;
            DateTimeOffset maximumValue = distantTime.AddTicks(Math.Min(precision.Ticks, distanceToMaxInTicks));

            Execute.Assertion
                .ForCondition(Subject.HasValue && ((Subject.Value < minimumValue) || (Subject.Value > maximumValue)))
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Did not expect {context:the date and time} to be within {0} from {1}{reason}, but it was {2}.",
                    precision,
                    distantTime, Subject ?? default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is before the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTimeOffset"/> that the current value is expected to be before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeBefore(DateTimeOffset expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) < 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the date and time} to be before {0}{reason}, but it was {1}.", expected,
                    Subject ?? default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/>  is not before the specified value.
        /// </summary>
        /// <param name="unexpected">The <see cref="DateTimeOffset"/>  that the current value is not expected to be before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBeBefore(DateTimeOffset unexpected, string because = "",
            params object[] becauseArgs)
        {
            return BeOnOrAfter(unexpected, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is either on, or before the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTimeOffset"/> that the current value is expected to be on or before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeOnOrBefore(DateTimeOffset expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) <= 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the date and time} to be on or before {0}{reason}, but it was {1}.", expected,
                    Subject ?? default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is neither on, nor before the specified value.
        /// </summary>
        /// <param name="unexpected">The <see cref="DateTimeOffset"/> that the current value is not expected to be on nor before.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBeOnOrBefore(DateTimeOffset unexpected, string because = "",
            params object[] becauseArgs)
        {
            return BeAfter(unexpected, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is after the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTimeOffset"/> that the current value is expected to be after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeAfter(DateTimeOffset expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) > 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the date and time} to be after {0}{reason}, but it was {1}.", expected,
                    Subject ?? default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is not after the specified value.
        /// </summary>
        /// <param name="unexpected">The <see cref="DateTimeOffset"/> that the current value is not expected to be after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBeAfter(DateTimeOffset unexpected, string because = "",
            params object[] becauseArgs)
        {
            return BeOnOrBefore(unexpected, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is either on, or after the specified value.
        /// </summary>
        /// <param name="expected">The <see cref="DateTimeOffset"/> that the current value is expected to be on or after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeOnOrAfter(DateTimeOffset expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.CompareTo(expected) >= 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the date and time} to be on or after {0}{reason}, but it was {1}.", expected,
                    Subject ?? default(DateTimeOffset?));

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/>  is neither on, nor after the specified value.
        /// </summary>
        /// <param name="unexpected">The <see cref="DateTimeOffset"/>  that the current value is expected not to be on nor after.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBeOnOrAfter(DateTimeOffset unexpected, string because = "",
            params object[] becauseArgs)
        {
            return BeBefore(unexpected, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> year.
        /// </summary>
        /// <param name="expected">The expected year of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveYear(int expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the year part of {context:the date} to be {0}{reason}, ", expected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.")
                .Then
                .ForCondition(Subject.Value.Year == expected)
                .FailWith("but it was {0}.", Subject.Value.Year)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> does not have the <paramref name="unexpected"/> year.
        /// </summary>
        /// <param name="unexpected">The year that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotHaveYear(int unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Did not expect the year part of {context:the date} to be {0}{reason}, ", unexpected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.")
                .Then
                .ForCondition(Subject.Value.Year != unexpected)
                .FailWith("but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> month.
        /// </summary>
        /// <param name="expected">The expected month of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveMonth(int expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the month part of {context:the date} to be {0}{reason}, ", expected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.")
                .Then
                .ForCondition(Subject.Value.Month == expected)
                .FailWith("but it was {0}.", Subject.Value.Month)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> does not have the <paramref name="unexpected"/> month.
        /// </summary>
        /// <param name="unexpected">The month that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotHaveMonth(int unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Did not expect the month part of {context:the date} to be {0}{reason}, ", unexpected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.")
                .Then
                .ForCondition(Subject.Value.Month != unexpected)
                .FailWith("but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> day.
        /// </summary>
        /// <param name="expected">The expected day of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveDay(int expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the day part of {context:the date} to be {0}{reason}, ", expected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.")
                .Then
                .ForCondition(Subject.Value.Day == expected)
                .FailWith("but it was {0}.", Subject.Value.Day)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> does not have the <paramref name="unexpected"/> day.
        /// </summary>
        /// <param name="unexpected">The day that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotHaveDay(int unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Did not expect the day part of {context:the date} to be {0}{reason}, ", unexpected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.")
                .Then
                .ForCondition(Subject.Value.Day != unexpected)
                .FailWith("but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> hour.
        /// </summary>
        /// <param name="expected">The expected hour of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveHour(int expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the hour part of {context:the time} to be {0}{reason}, ", expected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.")
                .Then
                .ForCondition(Subject.Value.Hour == expected)
                .FailWith("but it was {0}.", Subject.Value.Hour)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> does not have the <paramref name="unexpected"/> hour.
        /// </summary>
        /// <param name="unexpected">The hour that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotHaveHour(int unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Did not expect the hour part of {context:the time} to be {0}{reason}, ", unexpected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.")
                .Then
                .ForCondition(Subject.Value.Hour != unexpected)
                .FailWith("but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> minute.
        /// </summary>
        /// <param name="expected">The expected minutes of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveMinute(int expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the minute part of {context:the time} to be {0}{reason}, ", expected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.")
                .Then
                .ForCondition(Subject.Value.Minute == expected)
                .FailWith("but it was {0}.", Subject.Value.Minute)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> does not have the <paramref name="unexpected"/> minute.
        /// </summary>
        /// <param name="unexpected">The minute that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotHaveMinute(int unexpected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Did not expect the minute part of {context:the time} to be {0}{reason}, ", unexpected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.")
                .Then
                .ForCondition(Subject.Value.Minute != unexpected)
                .FailWith("but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> second.
        /// </summary>
        /// <param name="expected">The expected seconds of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveSecond(int expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the seconds part of {context:the time} to be {0}{reason}, ", expected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.")
                .Then
                .ForCondition(Subject.Value.Second == expected)
                .FailWith("but it was {0}.", Subject.Value.Second)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> does not have the <paramref name="unexpected"/> second.
        /// </summary>
        /// <param name="unexpected">The second that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotHaveSecond(int unexpected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Did not expect the seconds part of {context:the time} to be {0}{reason}, ", unexpected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.")
                .Then
                .ForCondition(Subject.Value.Second != unexpected)
                .FailWith("but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> offset.
        /// </summary>
        /// <param name="expected">The expected offset of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> HaveOffset(TimeSpan expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the offset of {context:the date} to be {0}{reason}, ", expected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.")
                .Then
                .ForCondition(Subject.Value.Offset == expected)
                .FailWith("but it was {0}.", Subject.Value.Offset)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> does not have the <paramref name="unexpected"/> offset.
        /// </summary>
        /// <param name="unexpected">The offset that should not be in the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotHaveOffset(TimeSpan unexpected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Did not expect the offset of {context:the date} to be {0}{reason}, ", unexpected)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.")
                .Then
                .ForCondition(Subject.Value.Offset != unexpected)
                .FailWith("but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Returns a <see cref="DateTimeOffsetRangeAssertions"/> object that can be used to assert that the current <see cref="DateTimeOffset"/>
        /// exceeds the specified <paramref name="timeSpan"/> compared to another <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTimeOffset"/> should exceed compared to another <see cref="DateTimeOffset"/>.
        /// </param>
        public DateTimeOffsetRangeAssertions BeMoreThan(TimeSpan timeSpan)
        {
            return new DateTimeOffsetRangeAssertions(this, Subject, TimeSpanCondition.MoreThan, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="DateTimeOffsetRangeAssertions"/> object that can be used to assert that the current <see cref="DateTimeOffset"/>
        /// is equal to or exceeds the specified <paramref name="timeSpan"/> compared to another <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTimeOffset"/> should be equal or exceed compared to
        /// another <see cref="DateTimeOffset"/>.
        /// </param>
        public DateTimeOffsetRangeAssertions BeAtLeast(TimeSpan timeSpan)
        {
            return new DateTimeOffsetRangeAssertions(this, Subject, TimeSpanCondition.AtLeast, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="DateTimeOffsetRangeAssertions"/> object that can be used to assert that the current <see cref="DateTimeOffset"/>
        /// differs exactly the specified <paramref name="timeSpan"/> compared to another <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTimeOffset"/> should differ exactly compared to another <see cref="DateTimeOffset"/>.
        /// </param>
        public DateTimeOffsetRangeAssertions BeExactly(TimeSpan timeSpan)
        {
            return new DateTimeOffsetRangeAssertions(this, Subject, TimeSpanCondition.Exactly, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="DateTimeOffsetRangeAssertions"/> object that can be used to assert that the current <see cref="DateTimeOffset"/>
        /// is within the specified <paramref name="timeSpan"/> compared to another <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time that the current <see cref="DateTimeOffset"/> should be within another <see cref="DateTimeOffset"/>.
        /// </param>
        public DateTimeOffsetRangeAssertions BeWithin(TimeSpan timeSpan)
        {
            return new DateTimeOffsetRangeAssertions(this, Subject, TimeSpanCondition.Within, timeSpan);
        }

        /// <summary>
        /// Returns a <see cref="DateTimeOffsetRangeAssertions"/> object that can be used to assert that the current <see cref="DateTimeOffset"/>
        /// differs at maximum the specified <paramref name="timeSpan"/> compared to another <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="timeSpan">
        /// The maximum amount of time that the current <see cref="DateTimeOffset"/> should differ compared to another <see cref="DateTimeOffset"/>.
        /// </param>
        public DateTimeOffsetRangeAssertions BeLessThan(TimeSpan timeSpan)
        {
            return new DateTimeOffsetRangeAssertions(this, Subject, TimeSpanCondition.LessThan, timeSpan);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> has the <paramref name="expected"/> date.
        /// </summary>
        /// <param name="expected">The expected date portion of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeSameDateAs(DateTimeOffset expected, string because = "",
            params object[] becauseArgs)
        {
            DateTime expectedDate = expected.Date;

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected the date part of {context:the date and time} to be {0}{reason}, ", expectedDate)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.", expectedDate)
                .Then
                .ForCondition(Subject.Value.Date == expectedDate)
                .FailWith("but it was {0}.", Subject.Value)
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="DateTimeOffset"/> is not the <paramref name="unexpected"/> date.
        /// </summary>
        /// <param name="unexpected">The date that is not to match the date portion of the current value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> NotBeSameDateAs(DateTimeOffset unexpected, string because = "",
            params object[] becauseArgs)
        {
            DateTime unexpectedDate = unexpected.Date;

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Did not expect the date part of {context:the date and time} to be {0}{reason}, ", unexpectedDate)
                .ForCondition(Subject.HasValue)
                .FailWith("but found a <null> DateTimeOffset.")
                .Then
                .ForCondition(Subject.Value.Date != unexpectedDate)
                .FailWith("but it was.")
                .Then
                .ClearExpectation();

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="DateTimeOffset"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeOneOf(params DateTimeOffset?[] validValues)
        {
            return BeOneOf(validValues, string.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="DateTimeOffset"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        public AndConstraint<DateTimeOffsetAssertions> BeOneOf(params DateTimeOffset[] validValues)
        {
            return BeOneOf(validValues.Cast<DateTimeOffset?>());
        }

        /// <summary>
        /// Asserts that the <see cref="DateTimeOffset"/> is one of the specified <paramref name="validValues"/>.
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
        public AndConstraint<DateTimeOffsetAssertions> BeOneOf(IEnumerable<DateTimeOffset> validValues, string because = "", params object[] becauseArgs)
        {
            return BeOneOf(validValues.Cast<DateTimeOffset?>(), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <see cref="DateTimeOffset"/> is one of the specified <paramref name="validValues"/>.
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
        public AndConstraint<DateTimeOffsetAssertions> BeOneOf(IEnumerable<DateTimeOffset?> validValues, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(validValues.Contains(Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the date and time} to be one of {0}{reason}, but it was {1}.", validValues, Subject);

            return new AndConstraint<DateTimeOffsetAssertions>(this);
        }
    }
}
