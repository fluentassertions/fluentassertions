using System;
using System.Collections.Generic;
using System.Diagnostics;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that two <see cref="DateTime"/> objects differ in the expected way.
    /// </summary>
    /// <remarks>
    /// You can use the <see cref="FluentDateTimeExtensions"/> and <see cref="TimeSpanConversionExtensions"/> for a more fluent
    /// way of specifying a <see cref="DateTime"/> or a <see cref="TimeSpan"/>.
    /// </remarks>
    [DebuggerNonUserCode]
    public class TimeSpanAssertions
    {
        #region Private Definitions

        private readonly DateTimeAssertions parentAssertions;
        private readonly TimeSpanPredicate predicate;

        private readonly Dictionary<TimeSpanCondition, TimeSpanPredicate> predicates = new Dictionary
            <TimeSpanCondition, TimeSpanPredicate>
        {
            { TimeSpanCondition.MoreThan, new TimeSpanPredicate((ts1, ts2) => ts1 > ts2, "more than") },
            { TimeSpanCondition.AtLeast, new TimeSpanPredicate((ts1, ts2) => ts1 >= ts2, "at least") },
            { TimeSpanCondition.Exactly, new TimeSpanPredicate((ts1, ts2) => ts1 == ts2, "exactly") },
            { TimeSpanCondition.Within, new TimeSpanPredicate((ts1, ts2) => ts1 <= ts2, "within") },
            { TimeSpanCondition.LessThan, new TimeSpanPredicate((ts1, ts2) => ts1 < ts2, "less than") }
        };

        private readonly DateTime? subject;
        private readonly TimeSpan timeSpan;

        #endregion

        protected internal TimeSpanAssertions(DateTimeAssertions parentAssertions, DateTime? subject, TimeSpanCondition condition,
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
        public AndConstraint<DateTimeAssertions> Before(DateTime target)
        {
            return Before(target, string.Empty);
        }

        /// <summary>
        /// Asserts that a <see cref="DateTime"/> occurs a specified amount of time before another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="target">
        /// The <see cref="DateTime"/> to compare the subject with.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<DateTimeAssertions> Before(DateTime target, string reason, params object [] reasonArgs)
        {
            var actual = target.Subtract(subject.Value);

            if (!predicate.IsMatchedBy(actual, timeSpan))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith(
                        "Expected date and/or time {0} to be " + predicate.DisplayText +
                            " {1} before {2}{reason}, but it differs {3}.",
                        subject, timeSpan, target, actual);
            }

            return new AndConstraint<DateTimeAssertions>(parentAssertions);
        }

        /// <summary>
        /// Asserts that a <see cref="DateTime"/> occurs a specified amount of time after another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="target">
        /// The <see cref="DateTime"/> to compare the subject with.
        /// </param>
        public AndConstraint<DateTimeAssertions> After(DateTime target)
        {
            return After(target, string.Empty);
        }

        /// <summary>
        /// Asserts that a <see cref="DateTime"/> occurs a specified amount of time after another <see cref="DateTime"/>.
        /// </summary>
        /// <param name="target">
        /// The <see cref="DateTime"/> to compare the subject with.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<DateTimeAssertions> After(DateTime target, string reason, params object [] reasonArgs)
        {
            var actual = subject.Value.Subtract(target);

            if (!predicate.IsMatchedBy(actual, timeSpan))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith(
                        "Expected date and/or time {0} to be " + predicate.DisplayText +
                            " {1} after {2}{reason}, but it differs {3}.",
                        subject, timeSpan, target, actual);
            }

            return new AndConstraint<DateTimeAssertions>(parentAssertions);
        }

        /// <summary>
        /// Provides the logic and the display text for a <see cref="TimeSpanCondition"/>.
        /// </summary>
        private class TimeSpanPredicate
        {
            private readonly string displayText;
            private readonly Func<TimeSpan, TimeSpan, bool> lambda;

            public TimeSpanPredicate(Func<TimeSpan, TimeSpan, bool> lambda, string displayText)
            {
                this.lambda = lambda;
                this.displayText = displayText;
            }

            public string DisplayText
            {
                get { return displayText; }
            }

            public bool IsMatchedBy(TimeSpan actual, TimeSpan expected)
            {
                return lambda(actual, expected);
            }
        }
    }
}