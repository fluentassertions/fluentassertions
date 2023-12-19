#if NET6_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Primitives;

/// <summary>
/// Contains a number of methods to assert that a <see cref="TimeOnly"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class TimeOnlyAssertions : TimeOnlyAssertions<TimeOnlyAssertions>
{
    public TimeOnlyAssertions(TimeOnly? value)
        : base(value)
    {
    }
}

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
/// <summary>
/// Contains a number of methods to assert that a <see cref="TimeOnly"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class TimeOnlyAssertions<TAssertions>
    where TAssertions : TimeOnlyAssertions<TAssertions>
{
    public TimeOnlyAssertions(TimeOnly? value)
    {
        Subject = value;
    }

    /// <summary>
    /// Gets the object whose value is being asserted.
    /// </summary>
    public TimeOnly? Subject { get; }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/> is exactly equal to the <paramref name="expected"/> value.
    /// </summary>
    /// <param name="expected">The expected value</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> Be(TimeOnly expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject == expected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:time} to be {0}{reason}, but found {1}.",
                expected, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/> is exactly equal to the <paramref name="expected"/> value.
    /// </summary>
    /// <param name="expected">The expected value</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> Be(TimeOnly? expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject == expected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:time} to be {0}{reason}, but found {1}.",
                expected, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/> or <see cref="TimeOnly"/> is not equal to the <paramref name="unexpected"/> value.
    /// </summary>
    /// <param name="unexpected">The unexpected value</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBe(TimeOnly unexpected, string because = "",
        params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject != unexpected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:time} not to be {0}{reason}, but it is.", unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/> or <see cref="TimeOnly"/> is not equal to the <paramref name="unexpected"/> value.
    /// </summary>
    /// <param name="unexpected">The unexpected value</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBe(TimeOnly? unexpected, string because = "",
        params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject != unexpected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:time} not to be {0}{reason}, but it is.", unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/>  is within the specified time
    /// from the specified <paramref name="nearbyTime"/> value.
    /// </summary>
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="precision"/> is negative.</exception>
    public AndConstraint<TAssertions> BeCloseTo(TimeOnly nearbyTime, TimeSpan precision, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNegative(precision);

        TimeSpan? difference = Subject != null
            ? MinimumDifference(Subject.Value, nearbyTime)
            : null;

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:the time} to be within {0} from {1}{reason}, ", precision, nearbyTime)
            .ForCondition(Subject is not null)
            .FailWith("but found <null>.")
            .Then
            .ForCondition(Subject?.IsCloseTo(nearbyTime, precision) == true)
            .FailWith("but {0} was off by {1}.", Subject, difference)
            .Then
            .ClearExpectation();

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    private static TimeSpan? MinimumDifference(TimeOnly a, TimeOnly b)
    {
        var diff1 = a - b;
        var diff2 = b - a;

        return diff1 < diff2 ? diff1 : diff2;
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/>  is not within the specified time
    /// from the specified <paramref name="distantTime"/> value.
    /// </summary>
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="precision"/> is negative.</exception>
    public AndConstraint<TAssertions> NotBeCloseTo(TimeOnly distantTime, TimeSpan precision, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNegative(precision);

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Did not expect {context:the time} to be within {0} from {1}{reason}, ", precision, distantTime)
            .ForCondition(Subject is not null)
            .FailWith("but found <null>.")
            .Then
            .ForCondition(Subject?.IsCloseTo(distantTime, precision) == false)
            .FailWith("but it was {0}.", Subject)
            .Then
            .ClearExpectation();

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/>  is before the specified value.
    /// </summary>
    /// <param name="expected">The <see cref="TimeOnly"/>  that the current value is expected to be before.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeBefore(TimeOnly expected, string because = "",
        params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject < expected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:time} to be before {0}{reason}, but found {1}.", expected,
                Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/>  is not before the specified value.
    /// </summary>
    /// <param name="unexpected">The <see cref="TimeOnly"/>  that the current value is not expected to be before.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeBefore(TimeOnly unexpected, string because = "",
        params object[] becauseArgs)
    {
        return BeOnOrAfter(unexpected, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/>  is either on, or before the specified value.
    /// </summary>
    /// <param name="expected">The <see cref="TimeOnly"/>  that the current value is expected to be on or before.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeOnOrBefore(TimeOnly expected, string because = "",
        params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject <= expected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:time} to be on or before {0}{reason}, but found {1}.", expected,
                Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/>  is neither on, nor before the specified value.
    /// </summary>
    /// <param name="unexpected">The <see cref="TimeOnly"/>  that the current value is not expected to be on nor before.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeOnOrBefore(TimeOnly unexpected, string because = "",
        params object[] becauseArgs)
    {
        return BeAfter(unexpected, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/>  is after the specified value.
    /// </summary>
    /// <param name="expected">The <see cref="TimeOnly"/>  that the current value is expected to be after.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeAfter(TimeOnly expected, string because = "",
        params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject > expected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:time} to be after {0}{reason}, but found {1}.", expected,
                Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/>  is not after the specified value.
    /// </summary>
    /// <param name="unexpected">The <see cref="TimeOnly"/>  that the current value is not expected to be after.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeAfter(TimeOnly unexpected, string because = "",
        params object[] becauseArgs)
    {
        return BeOnOrBefore(unexpected, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/>  is either on, or after the specified value.
    /// </summary>
    /// <param name="expected">The <see cref="TimeOnly"/>  that the current value is expected to be on or after.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeOnOrAfter(TimeOnly expected, string because = "",
        params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject >= expected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:time} to be on or after {0}{reason}, but found {1}.", expected,
                Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/>  is neither on, nor after the specified value.
    /// </summary>
    /// <param name="unexpected">The <see cref="TimeOnly"/>  that the current value is expected not to be on nor after.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeOnOrAfter(TimeOnly unexpected, string because = "",
        params object[] becauseArgs)
    {
        return BeBefore(unexpected, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/> has the <paramref name="expected"/> hour.
    /// </summary>
    /// <param name="expected">The expected hour of the current value.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveHours(int expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected the hours part of {context:the time} to be {0}{reason}", expected)
            .ForCondition(Subject.HasValue)
            .FailWith(", but found <null>.")
            .Then
            .ForCondition(Subject.Value.Hour == expected)
            .FailWith(", but found {0}.", Subject.Value.Hour)
            .Then
            .ClearExpectation();

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/> does not have the <paramref name="unexpected"/> hour.
    /// </summary>
    /// <param name="unexpected">The hour that should not be in the current value.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHaveHours(int unexpected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.HasValue)
            .FailWith("Did not expect the hours part of {context:the time} to be {0}{reason}, but found a <null> TimeOnly.",
                unexpected)
            .Then
            .ForCondition(Subject.Value.Hour != unexpected)
            .FailWith("Did not expect the hours part of {context:the time} to be {0}{reason}, but it was.", unexpected,
                Subject.Value.Hour);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/> has the <paramref name="expected"/> minute.
    /// </summary>
    /// <param name="expected">The expected minute of the current value.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveMinutes(int expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected the minutes part of {context:the time} to be {0}{reason}", expected)
            .ForCondition(Subject.HasValue)
            .FailWith(", but found a <null> TimeOnly.")
            .Then
            .ForCondition(Subject.Value.Minute == expected)
            .FailWith(", but found {0}.", Subject.Value.Minute)
            .Then
            .ClearExpectation();

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/> does not have the <paramref name="unexpected"/> minute.
    /// </summary>
    /// <param name="unexpected">The minute that should not be in the current value.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHaveMinutes(int unexpected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Did not expect the minutes part of {context:the time} to be {0}{reason}", unexpected)
            .ForCondition(Subject.HasValue)
            .FailWith(", but found a <null> TimeOnly.")
            .Then
            .ForCondition(Subject.Value.Minute != unexpected)
            .FailWith(", but it was.")
            .Then
            .ClearExpectation();

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/>  has the <paramref name="expected"/> second.
    /// </summary>
    /// <param name="expected">The expected second of the current value.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveSeconds(int expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected the seconds part of {context:the time} to be {0}{reason}", expected)
            .ForCondition(Subject.HasValue)
            .FailWith(", but found a <null> TimeOnly.")
            .Then
            .ForCondition(Subject.Value.Second == expected)
            .FailWith(", but found {0}.", Subject.Value.Second)
            .Then
            .ClearExpectation();

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/> does not have the <paramref name="unexpected"/> second.
    /// </summary>
    /// <param name="unexpected">The second that should not be in the current value.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHaveSeconds(int unexpected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Did not expect the seconds part of {context:the time} to be {0}{reason}", unexpected)
            .ForCondition(Subject.HasValue)
            .FailWith(", but found a <null> TimeOnly.")
            .Then
            .ForCondition(Subject.Value.Second != unexpected)
            .FailWith(", but it was.")
            .Then
            .ClearExpectation();

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/>  has the <paramref name="expected"/> millisecond.
    /// </summary>
    /// <param name="expected">The expected millisecond of the current value.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveMilliseconds(int expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected the milliseconds part of {context:the time} to be {0}{reason}", expected)
            .ForCondition(Subject.HasValue)
            .FailWith(", but found a <null> TimeOnly.")
            .Then
            .ForCondition(Subject.Value.Millisecond == expected)
            .FailWith(", but found {0}.", Subject.Value.Millisecond)
            .Then
            .ClearExpectation();

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="TimeOnly"/> does not have the <paramref name="unexpected"/> millisecond.
    /// </summary>
    /// <param name="unexpected">The millisecond that should not be in the current value.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHaveMilliseconds(int unexpected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Did not expect the milliseconds part of {context:the time} to be {0}{reason}", unexpected)
            .ForCondition(Subject.HasValue)
            .FailWith(", but found a <null> TimeOnly.")
            .Then
            .ForCondition(Subject.Value.Millisecond != unexpected)
            .FailWith(", but it was.")
            .Then
            .ClearExpectation();

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the <see cref="TimeOnly"/> is one of the specified <paramref name="validValues"/>.
    /// </summary>
    /// <param name="validValues">
    /// The values that are valid.
    /// </param>
    public AndConstraint<TAssertions> BeOneOf(params TimeOnly?[] validValues)
    {
        return BeOneOf(validValues, string.Empty);
    }

    /// <summary>
    /// Asserts that the <see cref="TimeOnly"/> is one of the specified <paramref name="validValues"/>.
    /// </summary>
    /// <param name="validValues">
    /// The values that are valid.
    /// </param>
    public AndConstraint<TAssertions> BeOneOf(params TimeOnly[] validValues)
    {
        return BeOneOf(validValues.Cast<TimeOnly?>());
    }

    /// <summary>
    /// Asserts that the <see cref="TimeOnly"/> is one of the specified <paramref name="validValues"/>.
    /// </summary>
    /// <param name="validValues">
    /// The values that are valid.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeOneOf(IEnumerable<TimeOnly> validValues, string because = "", params object[] becauseArgs)
    {
        return BeOneOf(validValues.Cast<TimeOnly?>(), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the <see cref="TimeOnly"/> is one of the specified <paramref name="validValues"/>.
    /// </summary>
    /// <param name="validValues">
    /// The values that are valid.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeOneOf(IEnumerable<TimeOnly?> validValues, string because = "",
        params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(validValues.Contains(Subject))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:time} to be one of {0}{reason}, but found {1}.", validValues, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        throw new NotSupportedException("Equals is not part of Fluent Assertions. Did you mean Be() instead?");
}

#endif
