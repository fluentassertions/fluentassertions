using System;

namespace FluentAssertions
{
    public class IntegralAssertions<T> : NumericAssertions<T> where T : struct, IComparable
    {
        protected internal IntegralAssertions(T? value) : base(value)
        {
        }
        
        public AndConstraint<NumericAssertions<T>> Be(T expected)
        {
            return Be(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> Be(T expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(expected) == 0,
                "Expected {0}{2}, but found {1}.", expected, Subject.Value, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> NotBe(T expected)
        {
            return NotBe(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> NotBe(T expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(expected) != 0,
                "Did not expect {0}{2}.", expected, Subject.Value, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }
    }
}