using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class DateTimeAssertions : Assertions<DateTime?, DateTimeAssertions>
    {
        protected DateTimeAssertions(DateTime? value)
        {
            ActualValue = value;
        }

        internal DateTimeAssertions(DateTime value)
        {
            ActualValue = value;
        }

        public AndConstraint<DateTimeAssertions> Equal(DateTime expected)
        {
            return Equal(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> Equal(DateTime expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Value == expected),
                "Expected {0}{2}, but found {1}.",
                expected, ActualValue.Value, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> BeBefore(DateTime expected)
        {
            return BeBefore(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> BeBefore(DateTime expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Value.CompareTo(expected) < 0),
                "Expected a date/time before {0}{2}, but found {1}.",
                expected, ActualValue.Value, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> BeOnOrBefore(DateTime expected)
        {
            return BeOnOrBefore(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> BeOnOrBefore(DateTime expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Value.CompareTo(expected) <= 0),
                "Expected a date/time on or before {0}{2}, but found {1}.",
                expected, ActualValue.Value, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> BeAfter(DateTime expected)
        {
            return BeAfter(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> BeAfter(DateTime expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Value.CompareTo(expected) > 0),
                "Expected a date/time after {0}{2}, but found {1}.",
                expected, ActualValue.Value, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> BeOnOrAfter(DateTime expected)
        {
            return BeOnOrAfter(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> BeOnOrAfter(DateTime expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Value.CompareTo(expected) >= 0),
                "Expected a date/time on or after {0}{2}, but found {1}.",
                expected, ActualValue.Value, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> HaveYear(int expected)
        {
            return HaveYear(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> HaveYear(int expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Value.Year == expected),
                "Expected year {0}{2}, but found {1}.",
                expected, ActualValue.Value.Year, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> HaveMonth(int expected)
        {
            return HaveMonth(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> HaveMonth(int expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Value.Month == expected),
                "Expected month {0}{2}, but found {1}.",
                expected, ActualValue.Value.Month, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> HaveDay(int expected)
        {
            return HaveDay(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> HaveDay(int expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Value.Day == expected),
                "Expected day {0}{2}, but found {1}.",
                expected, ActualValue.Value.Day, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> HaveHour(int expected)
        {
            return HaveHour(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> HaveHour(int expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Value.Hour == expected),
                "Expected hour {0}{2}, but found {1}.",
                expected, ActualValue.Value.Hour, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> HaveMinute(int expected)
        {
            return HaveMinute(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> HaveMinute(int expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Value.Minute == expected),
                "Expected minute {0}{2}, but found {1}.",
                expected, ActualValue.Value.Minute, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> HaveSecond(int expected)
        {
            return HaveSecond(expected, String.Empty);
        }

        public AndConstraint<DateTimeAssertions> HaveSecond(int expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Value.Second == expected),
                "Expected second {0}{2}, but found {1}.",
                expected, ActualValue.Value.Second, reason, reasonParameters);

            return new AndConstraint<DateTimeAssertions>(this);
        }
    }
}