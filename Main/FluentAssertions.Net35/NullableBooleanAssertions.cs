using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class NullableBooleanAssertions : BooleanAssertions
    {
        protected internal NullableBooleanAssertions(bool? value)
            : base(value)
        {
        }

        public AndConstraint<NullableBooleanAssertions> HaveValue()
        {
            return HaveValue(String.Empty);
        }

        public AndConstraint<NullableBooleanAssertions> HaveValue(string reason, params object[] reasonParameters)
        {
            Execute.Verify(Subject.HasValue, "Expected a value{2}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<NullableBooleanAssertions>(this);
        }

        public AndConstraint<NullableBooleanAssertions> NotHaveValue()
        {
            return NotHaveValue(String.Empty);
        }

        public AndConstraint<NullableBooleanAssertions> NotHaveValue(string reason, params object[] reasonParameters)
        {
            Execute.Verify(!Subject.HasValue, "Did not expect a value{2}, but found {1}.", null, Subject, reason,
                reasonParameters);

            return new AndConstraint<NullableBooleanAssertions>(this);
        }
    }
}