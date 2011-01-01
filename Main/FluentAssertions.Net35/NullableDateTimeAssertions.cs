using System;
using System.Diagnostics;

namespace FluentAssertions
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
            return HaveValue("Expected variable to have a value. Actual: null");
        }

        public AndConstraint<NullableDateTimeAssertions> HaveValue(string reason, params object[] reasonParameters)
        {
            Verification.Verify(Subject.HasValue, "Expected variable to have a value. Actual: null", null, Subject, reason, reasonParameters);
            return new AndConstraint<NullableDateTimeAssertions>(this);
        }

        public AndConstraint<NullableDateTimeAssertions> NotHaveValue()
        {
            return NotHaveValue("Did not expect variable to have a value. Actual: {0}", Subject);
        }

        public AndConstraint<NullableDateTimeAssertions> NotHaveValue(string reason, params object[] reasonParameters)
        {
            Verification.Verify(!Subject.HasValue, "Did not expect variable to have a value, but found {1}", null, Subject, reason, reasonParameters);
            return new AndConstraint<NullableDateTimeAssertions>(this);
        }

        public AndConstraint<DateTimeAssertions> Be(DateTime? expected)
        {
            return Be(expected, "Expected expected {0}. Actual: {1}", expected, Subject);
        }

        public AndConstraint<DateTimeAssertions> Be(DateTime? expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(Subject == expected, "Expected {0}, but found {1}{2}", expected, Subject, reason, reasonParameters);
            return new AndConstraint<DateTimeAssertions>(this);
        }
    }
}