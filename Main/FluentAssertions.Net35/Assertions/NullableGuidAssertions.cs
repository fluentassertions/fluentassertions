using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    [DebuggerNonUserCode]
    public class NullableGuidAssertions : GuidAssertions
    {
        protected internal NullableGuidAssertions(Guid? value)
            : base(value)
        {
        }

        public AndConstraint<NullableGuidAssertions> HaveValue()
        {
            return HaveValue(String.Empty);
        }

        public AndConstraint<NullableGuidAssertions> HaveValue(string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value{reason}.");

            return new AndConstraint<NullableGuidAssertions>(this);
        }

        public AndConstraint<NullableGuidAssertions> NotHaveValue()
        {
            return NotHaveValue(String.Empty);
        }

        public AndConstraint<NullableGuidAssertions> NotHaveValue(string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!Subject.HasValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect a value{reason}, but found {0}.", Subject);

            return new AndConstraint<NullableGuidAssertions>(this);
        }
    }
}