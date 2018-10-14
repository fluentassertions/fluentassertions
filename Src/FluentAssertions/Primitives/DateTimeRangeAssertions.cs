using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that two <see cref="DateTime"/> objects differ in the expected way.
    /// </summary>
    /// <remarks>
    /// You can use the <see cref="FluentAssertions.Extensions.FluentDateTimeExtensions"/> and
    /// <see cref="FluentAssertions.Extensions.FluentTimeSpanExtensions"/> for a more fluent
    /// way of specifying a <see cref="DateTime"/> or a <see cref="TimeSpan"/>.
    /// </remarks>
    [DebuggerNonUserCode]
    public class DateTimeRangeAssertions
    {
        #region Private Definitions

        private readonly DateTimeAssertions parentAssertions;
        private readonly TimeSpanPredicate predicate;

        private readonly Dictionary<TimeSpanCondition, TimeSpanPredicate> predicates = new Dictionary
            <TimeSpanCondition, TimeSpanPredicate>
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

        protected internal DateTimeRangeAssertions(DateTimeAssertions parentAssertions, DateTime? subject,
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
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<DateTimeAssertions> Before(DateTime target, string because = "",
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

                if (!predicate.IsMatchedBy(actual, timeSpan))
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith(
                            "Expected date and/or time {0} to be " + predicate.DisplayText +
                            " {1} before {2}{reason}, but it differs {3}.",
                            subject, timeSpan, target, actual);
                }
            }

            return new AndConstraint<DateTimeAssertions>(parentAssertions);
        }

        /// <summary>
        /// Asserts that a <see cref="DateTime"/> occurs a specified amount of time after another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="target">
        /// The <see cref="DateTime"/> to compare the subject with.
        /// </param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<DateTimeAssertions> After(DateTime target, string because = "",
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

                if (!predicate.IsMatchedBy(actual, timeSpan))
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith(
                            "Expected date and/or time {0} to be " + predicate.DisplayText +
                            " {1} after {2}{reason}, but it differs {3}.",
                            subject, timeSpan, target, actual);
                }
            }

            return new AndConstraint<DateTimeAssertions>(parentAssertions);
        }
    }
}
