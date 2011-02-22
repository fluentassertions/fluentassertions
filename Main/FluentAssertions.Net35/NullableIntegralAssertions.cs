using System;

namespace FluentAssertions
{
    public class NullableIntegralAssertions<T> : NullableNumericAssertions<T> where T : struct, IComparable
    {
        public NullableIntegralAssertions(T? expected) : base(expected)
        {
        }
        
        public AndConstraint<NumericAssertions<T>> Be(T? expected)
        {
            return Be(expected, "Expected expected: {0}. Actual: {1}", expected, Subject);
        }

        public AndConstraint<NumericAssertions<T>> Be(T? expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Equals(expected), "Expected value {0}{2}, but found {1}.",
                expected, Subject, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }
    }
}