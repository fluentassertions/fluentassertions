using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class NullableNumericAssertions<T> : NumericAssertions<T>
        where T : struct, IComparable
    {
        internal NullableNumericAssertions(T? expected)
            : base(expected)
        {
        }

        public AndConstraint<NullableNumericAssertions<T>> HaveValue()
        {
            return HaveValue(String.Empty);
        }

        public AndConstraint<NullableNumericAssertions<T>> HaveValue(string reason, params object[] reasonParameters)
        {
            VerifyThat(ActualValue.HasValue, "Expected a value{2}.", null, ActualValue, reason, reasonParameters);

            return new AndConstraint<NullableNumericAssertions<T>>(this);
        }

        public AndConstraint<NullableNumericAssertions<T>> NotHaveValue()
        {
            return NotHaveValue(String.Empty);
        }

        public AndConstraint<NullableNumericAssertions<T>> NotHaveValue(string reason, params object[] reasonParameters)
        {
            VerifyThat(!ActualValue.HasValue, "Did not expect a value{2}.", null, ActualValue, reason, reasonParameters);

            return new AndConstraint<NullableNumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> Equal(T? expected)
        {
            return Equal(expected, "Expected expected: {0}. Actual: {1}", expected, ActualValue);
        }

        public AndConstraint<NumericAssertions<T>> Equal(T? expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.Equals(expected), "Expected value {0}{2}, but found {1}.",
                expected, ActualValue, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }
    }
}