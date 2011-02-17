using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class SimpleTimeSpanAssertions
    {
        protected internal SimpleTimeSpanAssertions(TimeSpan? value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public TimeSpan? Subject
        {
            get;
            private set;
        }

        public AndConstraint<SimpleTimeSpanAssertions> BePositive()
        {
            return BePositive(String.Empty);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BePositive(string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(new TimeSpan()) > 0,
               "Expected positive value{2}, but found {1}", null, Subject.Value, reason, reasonParameters);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeNegative()
        {
            return BeNegative(String.Empty);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeNegative(string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(new TimeSpan()) < 0,
                "Expected negative value{2}, but found {1}", null, Subject.Value, reason, reasonParameters);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<SimpleTimeSpanAssertions> Be(TimeSpan expected)
        {
            return Be(expected, String.Empty);
        }

        public AndConstraint<SimpleTimeSpanAssertions> Be(TimeSpan expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(expected) == 0,
                "Expected {0}{2}, but found {1}.", expected, Subject.Value, reason, reasonParameters);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<SimpleTimeSpanAssertions> NotBe(TimeSpan expected)
        {
            return NotBe(expected, String.Empty);
        }

        public AndConstraint<SimpleTimeSpanAssertions> NotBe(TimeSpan expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(expected) != 0,
                "Did not expect {0}{2}.", expected, Subject.Value, reason, reasonParameters);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeLessThan(TimeSpan expected)
        {
            return BeLessThan(expected, String.Empty);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeLessThan(TimeSpan expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(expected) < 0,
                "Expected a value less than {0}{2}, but found {1}.", expected, Subject.Value, reason, reasonParameters);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeLessOrEqualTo(TimeSpan expected)
        {
            return BeLessOrEqualTo(expected, String.Empty);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeLessOrEqualTo(TimeSpan expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(expected) <= 0,
                "Expected a value less or equal to {0}{2}, but found {1}.", expected, Subject.Value, reason,
                reasonParameters);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterThan(TimeSpan expected)
        {
            return BeGreaterThan(expected, String.Empty);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterThan(TimeSpan expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(expected) > 0,
                "Expected a value greater than {0}{2}, but found {1}.", expected, Subject.Value, reason,
                reasonParameters);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterOrEqualTo(TimeSpan expected)
        {
            return BeGreaterOrEqualTo(expected, String.Empty);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterOrEqualTo(TimeSpan expected, string reason,
            params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(expected) >= 0,
                "Expected a value greater or equal to {0}{2}, but found {1}.", expected, Subject.Value, reason,
                reasonParameters);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }


    }
}
