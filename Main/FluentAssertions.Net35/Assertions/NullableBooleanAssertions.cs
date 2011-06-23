using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
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

        public AndConstraint<NullableBooleanAssertions> HaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value{0}.");

            return new AndConstraint<NullableBooleanAssertions>(this);
        }

        public AndConstraint<NullableBooleanAssertions> NotHaveValue()
        {
            return NotHaveValue(String.Empty);
        }

        public AndConstraint<NullableBooleanAssertions> NotHaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect a value{0}, but found {1}.", Subject);

            return new AndConstraint<NullableBooleanAssertions>(this);
        }
    }
}