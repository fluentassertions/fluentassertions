using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    [DebuggerNonUserCode]
    public class NullableSimpleTimeSpanAssertions : SimpleTimeSpanAssertions
    {
        protected internal NullableSimpleTimeSpanAssertions(TimeSpan? value)
            : base(value)
        {
        }

        public AndConstraint<NullableSimpleTimeSpanAssertions> HaveValue()
        {
            return HaveValue(String.Empty);
        }

        public AndConstraint<NullableSimpleTimeSpanAssertions> HaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value{0}.");

            return new AndConstraint<NullableSimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<NullableSimpleTimeSpanAssertions> NotHaveValue()
        {
            return NotHaveValue(String.Empty);
        }

        public AndConstraint<NullableSimpleTimeSpanAssertions> NotHaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect a value{0}, but found {1}.", Subject);

            return new AndConstraint<NullableSimpleTimeSpanAssertions>(this);
        }
    }
}