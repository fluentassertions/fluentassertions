using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="TimeSpan"/> is in the expected state.
    /// </summary>
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

        public AndConstraint<SimpleTimeSpanAssertions> BePositive(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(new TimeSpan()) > 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected positive value{reason}, but found {0}", Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeNegative()
        {
            return BeNegative(String.Empty);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeNegative(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(new TimeSpan()) < 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected negative value{reason}, but found {0}", Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<SimpleTimeSpanAssertions> Be(TimeSpan expected)
        {
            return Be(expected, String.Empty);
        }

        public AndConstraint<SimpleTimeSpanAssertions> Be(TimeSpan expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(expected) == 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", expected, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<SimpleTimeSpanAssertions> NotBe(TimeSpan unexpected)
        {
            return NotBe(unexpected, String.Empty);
        }

        public AndConstraint<SimpleTimeSpanAssertions> NotBe(TimeSpan unexpected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(unexpected) != 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect {0}{reason}.", unexpected);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeLessThan(TimeSpan expected)
        {
            return BeLessThan(expected, String.Empty);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeLessThan(TimeSpan expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(expected) < 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value less than {0}{reason}, but found {1}.", expected, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeLessOrEqualTo(TimeSpan expected)
        {
            return BeLessOrEqualTo(expected, String.Empty);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeLessOrEqualTo(TimeSpan expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(expected) <= 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value less or equal to {0}{reason}, but found {1}.", expected, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterThan(TimeSpan expected)
        {
            return BeGreaterThan(expected, String.Empty);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterThan(TimeSpan expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(expected) > 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value greater than {0}{reason}, but found {1}.", expected, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterOrEqualTo(TimeSpan expected)
        {
            return BeGreaterOrEqualTo(expected, String.Empty);
        }

        public AndConstraint<SimpleTimeSpanAssertions> BeGreaterOrEqualTo(TimeSpan expected, string reason,
            params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.CompareTo(expected) >= 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value greater or equal to {0}{reason}, but found {1}.", expected, Subject.Value);

            return new AndConstraint<SimpleTimeSpanAssertions>(this);
        }


    }
}
