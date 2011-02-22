using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public abstract class NullableNumericAssertions<T> : NumericAssertions<T>
        where T : struct, IComparable
    {
        protected internal NullableNumericAssertions(T? expected)
            : base(expected)
        {
        }

        public AndConstraint<NullableNumericAssertions<T>> HaveValue()
        {
            return HaveValue(String.Empty);
        }

        public AndConstraint<NullableNumericAssertions<T>> HaveValue(string reason, params object[] reasonParameters)
        {
            Verification.Verify(Subject.HasValue, "Expected a value{2}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<NullableNumericAssertions<T>>(this);
        }

        public AndConstraint<NullableNumericAssertions<T>> NotHaveValue()
        {
            return NotHaveValue(String.Empty);
        }

        public AndConstraint<NullableNumericAssertions<T>> NotHaveValue(string reason, params object[] reasonParameters)
        {
            Verification.Verify(!Subject.HasValue, "Did not expect a value{2}, but found {1}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<NullableNumericAssertions<T>>(this);
        }

    }
}