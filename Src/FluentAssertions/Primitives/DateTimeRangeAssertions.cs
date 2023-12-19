using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Extensions;

namespace FluentAssertionsAsync.Primitives;

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
/// <summary>
/// Contains a number of methods to assert that two <see cref="DateTime"/> objects differ in the expected way.
/// </summary>
/// <remarks>
/// You can use the <see cref="FluentDateTimeExtensions"/> and
/// <see cref="FluentTimeSpanExtensions"/> for a more fluent
/// way of specifying a <see cref="DateTime"/> or a <see cref="TimeSpan"/>.
/// </remarks>
[DebuggerNonUserCode]
public class DateTimeRangeAssertions<TAssertions>
    where TAssertions : DateTimeAssertions<TAssertions>
{
    #region Private Definitions

    private readonly TAssertions parentAssertions;
    private readonly TimeSpanPredicate predicate;

    private readonly Dictionary<TimeSpanCondition, TimeSpanPredicate> predicates = new()
    {
        [TimeSpanCondition.MoreThan] = new TimeSpanPredicate((ts1, ts2) => ts1 > ts2, "more than"),
        [TimeSpanCondition.AtLeast] = new TimeSpanPredicate((ts1, ts2) => ts1 >= ts2, "at least"),
        [TimeSpanCondition.Exactly] = new TimeSpanPredicate((ts1, ts2) => ts1 == ts2, "exactly"),
        [TimeSpanCondition.Within] = new TimeSpanPredicate((ts1, ts2) => ts1 <= ts2, "within"),
        [TimeSpanCondition.LessThan] = new TimeSpanPredicate((ts1, ts2) => ts1 < ts2, "less than")
    };

    private readonly DateTime? subject;
    private readonly TimeSpan timeSpan;

    #endregion

    protected internal DateTimeRangeAssertions(TAssertions parentAssertions, DateTime? subject,
        TimeSpanCondition condition,
        TimeSpan timeSpan)
    {
        this.parentAssertions = parentAssertions;
        this.subject = subject;
        this.timeSpan = timeSpan;

        predicate = predicates[condition];
    }

    /// <summary>
    /// Asserts that a <see cref="DateTime"/> occurs a specified amount of time before another <see cref="DateTime"/>.
    /// </summary>
    /// <param name="target">
    /// The <see cref="DateTime"/> to compare the subject with.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> Before(DateTime target, string because = "",
        params object[] becauseArgs)
    {
        bool success = Execute.Assertion
            .ForCondition(subject.HasValue)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected date and/or time {0} to be " + predicate.DisplayText +
                " {1} before {2}{reason}, but found a <null> DateTime.",
                subject, timeSpan, target);

        if (success)
        {
            TimeSpan actual = target - subject.Value;

            Execute.Assertion
                .ForCondition(predicate.IsMatchedBy(actual, timeSpan))
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:the date and time} {0} to be " + predicate.DisplayText +
                    " {1} before {2}{reason}, but it is " + PositionRelativeToTarget(subject.Value, target) + " by {3}.",
                    subject, timeSpan, target, actual.Duration());
        }

        return new AndConstraint<TAssertions>(parentAssertions);
    }

    /// <summary>
    /// Asserts that a <see cref="DateTime"/> occurs a specified amount of time after another <see cref="DateTime"/>.
    /// </summary>
    /// <param name="target">
    /// The <see cref="DateTime"/> to compare the subject with.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> After(DateTime target, string because = "",
        params object[] becauseArgs)
    {
        bool success = Execute.Assertion
            .ForCondition(subject.HasValue)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected date and/or time {0} to be " + predicate.DisplayText +
                " {1} after {2}{reason}, but found a <null> DateTime.",
                subject, timeSpan, target);

        if (success)
        {
            TimeSpan actual = subject.Value - target;

            Execute.Assertion
                .ForCondition(predicate.IsMatchedBy(actual, timeSpan))
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:the date and time} {0} to be " + predicate.DisplayText +
                    " {1} after {2}{reason}, but it is " + PositionRelativeToTarget(subject.Value, target) + " by {3}.",
                    subject, timeSpan, target, actual.Duration());
        }

        return new AndConstraint<TAssertions>(parentAssertions);
    }

    private static string PositionRelativeToTarget(DateTime actual, DateTime target)
    {
        return (actual - target) >= TimeSpan.Zero ? "ahead" : "behind";
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        throw new NotSupportedException("Equals is not part of Fluent Assertions. Did you mean Before() or After() instead?");
}
