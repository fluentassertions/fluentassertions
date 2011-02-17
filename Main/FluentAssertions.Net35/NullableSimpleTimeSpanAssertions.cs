using System;
using System.Diagnostics;

namespace FluentAssertions
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

        public AndConstraint<NullableSimpleTimeSpanAssertions> HaveValue(string reason, params object[] reasonParameters)
        {
            Verification.Verify(Subject.HasValue, "Expected a value{2}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<NullableSimpleTimeSpanAssertions>(this);
        }

        public AndConstraint<NullableSimpleTimeSpanAssertions> NotHaveValue()
        {
            return NotHaveValue(String.Empty);
        }

        public AndConstraint<NullableSimpleTimeSpanAssertions> NotHaveValue(string reason, params object[] reasonParameters)
        {
            Verification.Verify(!Subject.HasValue, "Did not expect a value{2}, but found {1}.", null, Subject, reason,
                reasonParameters);

            return new AndConstraint<NullableSimpleTimeSpanAssertions>(this);
        }
    }
}