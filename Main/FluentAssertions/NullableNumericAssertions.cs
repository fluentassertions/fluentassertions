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
            VerifyThat(Subject.HasValue, "Expected a value{2}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<NullableNumericAssertions<T>>(this);
        }

        public AndConstraint<NullableNumericAssertions<T>> NotHaveValue()
        {
            return NotHaveValue(String.Empty);
        }

        public AndConstraint<NullableNumericAssertions<T>> NotHaveValue(string reason, params object[] reasonParameters)
        {
            VerifyThat(!Subject.HasValue, "Did not expect a value{2}, but found {1}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<NullableNumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> Be(T? expected)
        {
            return Be(expected, "Expected expected: {0}. Actual: {1}", expected, Subject);
        }

        public AndConstraint<NumericAssertions<T>> Be(T? expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => Subject.Equals(expected), "Expected value {0}{2}, but found {1}.",
                expected, Subject, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }
    }
}