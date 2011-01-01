using System;
using System.Diagnostics;

namespace FluentAssertions
{
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

        public AndConstraint<DateTimeAssertions> Be(DateTime expected)
        {
            return Be(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> Be(DateTime expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => (Subject.Value == expected),
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
            Verification.Verify(() => (Subject.Value.CompareTo(expected) < 0),
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
            Verification.Verify(() => (Subject.Value.CompareTo(expected) <= 0),
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
            Verification.Verify(() => (Subject.Value.CompareTo(expected) > 0),
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
            Verification.Verify(() => (Subject.Value.CompareTo(expected) >= 0),
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
            Verification.Verify(() => (Subject.Value.Year == expected),
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
            Verification.Verify(() => (Subject.Value.Month == expected),
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
            Verification.Verify(() => (Subject.Value.Day == expected),
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
            Verification.Verify(() => (Subject.Value.Hour == expected),
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
            Verification.Verify(() => (Subject.Value.Minute == expected),
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
            Verification.Verify(() => (Subject.Value.Second == expected),
                "Expected second {0}{2}, but found {1}.",
                expected, Subject.Value.Second, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }
        
        /// <summary>
        /// The amount of time that a <see cref="DateTime"/> should exceed compared to another <see cref="DateTime"/>.
        /// </summary>
        public TimeSpanAssertions BeMoreThan(TimeSpan timeSpan)
        {
            return new TimeSpanAssertions(this, Subject, TimeSpanCondition.MoreThan, timeSpan);
        }

        /// <summary>
        /// The amount of time that a <see cref="DateTime"/> should be equal or exceed compared to another <see cref="DateTime"/>.
        /// </summary>
        public TimeSpanAssertions BeAtLeast(TimeSpan timeSpan)
        {
            return new TimeSpanAssertions(this, Subject, TimeSpanCondition.AtLeast, timeSpan);
        }

        /// <summary>
        /// The amount of time that a <see cref="DateTime"/> should differ exactly compared to another <see cref="DateTime"/>.
        /// </summary>
        public TimeSpanAssertions BeExactly(TimeSpan timeSpan)
        {
            return new TimeSpanAssertions(this, Subject, TimeSpanCondition.Exactly, timeSpan);
        }

        /// <summary>
        /// The maximum amount of time that a <see cref="DateTime"/> should differ compared to another <see cref="DateTime"/>.
        /// </summary>
        public TimeSpanAssertions BeWithin(TimeSpan timeSpan)
        {
            return new TimeSpanAssertions(this, Subject, TimeSpanCondition.Within, timeSpan);
        }

        /// <summary>
        /// The amount of time that a <see cref="DateTime"/> should be within another <see cref="DateTime"/>.
        /// </summary>
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