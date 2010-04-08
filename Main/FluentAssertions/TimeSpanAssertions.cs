using System;
using System.Collections.Generic;

namespace FluentAssertions
{
    public class TimeSpanAssertions : AssertionsBase<TimeSpan>
    {
        #region Private Definitions

        private readonly DateTimeAssertions parentAssertions;
        private readonly DateTime? subject;
        private readonly TimeSpan timeSpan;
        private readonly TimeSpanPredicate predicate;

        private readonly Dictionary<TimeSpanCondition, TimeSpanPredicate> predicates = new Dictionary<TimeSpanCondition, TimeSpanPredicate>
        {
            { TimeSpanCondition.MoreThan, new TimeSpanPredicate((ts1, ts2) => ts1 > ts2, "more than") },
            { TimeSpanCondition.AtLeast, new TimeSpanPredicate((ts1, ts2) => ts1 >= ts2, "at least") },
            { TimeSpanCondition.Exactly, new TimeSpanPredicate((ts1, ts2) => ts1 == ts2, "exactly") }    
        };

        #endregion

        public TimeSpanAssertions(DateTimeAssertions parentAssertions, DateTime? subject, TimeSpanCondition condition, TimeSpan timeSpan)
        {
            this.parentAssertions = parentAssertions;
            this.subject = subject;
            this.timeSpan = timeSpan;

            predicate = predicates[condition];
        }

        /// <summary>
        /// Asserts that a <see cref="DateTime"/> occurs the specified amount of time before the <param name="target"/> <see cref="DateTime"/>.
        /// </summary>
        public AndConstraint<DateTimeAssertions> Before(DateTime target)
        {
            return Before(target, string.Empty);
        }

        /// <summary>
        /// Asserts that a <see cref="DateTime"/> occurs the specified amount of time before the <param name="target"/> <see cref="DateTime"/>.
        /// </summary>
        public AndConstraint<DateTimeAssertions> Before(DateTime target, string reason, params object[] reasonParameters)
        {
            var actual = target.Subtract(subject.Value);

            if (!predicate.IsMatchedBy(actual, timeSpan))
            {
                FailWith("Expected date and/or time {1} to be " + predicate.DisplayText + " {3} before {0}{2}, but it differs {4}.", target, subject,
                    reason, reasonParameters, timeSpan, actual);
            }
            
            return new AndConstraint<DateTimeAssertions>(parentAssertions);
        }

        private class TimeSpanPredicate
        {
            private readonly Func<TimeSpan, TimeSpan, bool> lambda;
            private readonly string displayText;

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