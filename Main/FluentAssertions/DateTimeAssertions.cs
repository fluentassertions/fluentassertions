using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class DateTimeAssertions : Assertions<DateTime?, DateTimeAssertions>
    {
        protected DateTimeAssertions(DateTime? value)
        {
            Subject = value;
        }

        internal DateTimeAssertions(DateTime value)
        {
            Subject = value;
        }

        public AndConstraint<DateTimeAssertions> Be(DateTime expected)
        {
            return Be(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> Be(DateTime expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject.Value == expected),
                "Expected {0}{2}, but found {1}.",
                expected, Subject.Value, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> BeBefore(DateTime expected)
        {
            return BeBefore(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> BeBefore(DateTime expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject.Value.CompareTo(expected) < 0),
                "Expected a date/time before {0}{2}, but found {1}.",
                expected, Subject.Value, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> BeOnOrBefore(DateTime expected)
        {
            return BeOnOrBefore(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> BeOnOrBefore(DateTime expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject.Value.CompareTo(expected) <= 0),
                "Expected a date/time on or before {0}{2}, but found {1}.",
                expected, Subject.Value, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> BeAfter(DateTime expected)
        {
            return BeAfter(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> BeAfter(DateTime expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject.Value.CompareTo(expected) > 0),
                "Expected a date/time after {0}{2}, but found {1}.",
                expected, Subject.Value, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> BeOnOrAfter(DateTime expected)
        {
            return BeOnOrAfter(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> BeOnOrAfter(DateTime expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject.Value.CompareTo(expected) >= 0),
                "Expected a date/time on or after {0}{2}, but found {1}.",
                expected, Subject.Value, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> HaveYear(int expected)
        {
            return HaveYear(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> HaveYear(int expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject.Value.Year == expected),
                "Expected year {0}{2}, but found {1}.",
                expected, Subject.Value.Year, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> HaveMonth(int expected)
        {
            return HaveMonth(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> HaveMonth(int expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject.Value.Month == expected),
                "Expected month {0}{2}, but found {1}.",
                expected, Subject.Value.Month, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> HaveDay(int expected)
        {
            return HaveDay(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> HaveDay(int expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject.Value.Day == expected),
                "Expected day {0}{2}, but found {1}.",
                expected, Subject.Value.Day, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> HaveHour(int expected)
        {
            return HaveHour(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> HaveHour(int expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject.Value.Hour == expected),
                "Expected hour {0}{2}, but found {1}.",
                expected, Subject.Value.Hour, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> HaveMinute(int expected)
        {
            return HaveMinute(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> HaveMinute(int expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject.Value.Minute == expected),
                "Expected minute {0}{2}, but found {1}.",
                expected, Subject.Value.Minute, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> HaveSecond(int expected)
        {
            return HaveSecond(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> HaveSecond(int expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject.Value.Second == expected),
                "Expected second {0}{2}, but found {1}.",
                expected, Subject.Value.Second, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public TimeSpanAssertions BeMoreThan(TimeSpan timeSpan)
        {
            return new TimeSpanAssertions(this, Subject, DateTimeComparison.MoreThan, timeSpan);
        }
    }

    public class TimeSpanAssertions : AssertionsBase<TimeSpan>
    {
        private readonly DateTimeAssertions parentAssertions;
        private readonly DateTime? subject;
        private readonly DateTimeComparison @operator;
        private readonly TimeSpan timeSpan;

        public TimeSpanAssertions(DateTimeAssertions parentAssertions, DateTime? subject, DateTimeComparison @operator, TimeSpan timeSpan)
        {
            this.parentAssertions = parentAssertions;
            this.subject = subject;
            this.@operator = @operator;
            this.timeSpan = timeSpan;
        }

        public AndConstraint<DateTimeAssertions> Before(DateTime target, string reason, params object[] reasonParameters)
        {
            if (@operator == DateTimeComparison.MoreThan)
            {
                if (target.Subtract(subject.Value) <= timeSpan)
                {
                    FailWith("Expected {1} to be more than {3} before {0}{2}.", target, subject, reason, reasonParameters, timeSpan);
                }
            }

            return new AndConstraint<DateTimeAssertions>(parentAssertions);
        }
    }

    public enum DateTimeComparison
    {
        MoreThan
    }
}