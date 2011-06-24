using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    [DebuggerNonUserCode]
    public class NullableDateTimeAssertions : DateTimeAssertions
    {
        protected internal NullableDateTimeAssertions(DateTime? expected)
            : base(expected)
        {
        }

        public AndConstraint<NullableDateTimeAssertions> HaveValue()
        {
            return HaveValue(string.Empty);
        }

        public AndConstraint<NullableDateTimeAssertions> HaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected variable to have a value{reason}, but found {0}", Subject);

            return new AndConstraint<NullableDateTimeAssertions>(this);
        }

        public AndConstraint<NullableDateTimeAssertions> NotHaveValue()
        {
            return NotHaveValue(string.Empty);
        }

        public AndConstraint<NullableDateTimeAssertions> NotHaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect variable to have a value{reason}, but found {0}", Subject);
            
            return new AndConstraint<NullableDateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> Be(DateTime? expected)
        {
            return Be(expected, string.Empty);
        }

        public AndConstraint<DateTimeAssertions> Be(DateTime? expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}", expected, Subject);

            return new AndConstraint<DateTimeAssertions>(this);
        }
    }
}