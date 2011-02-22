using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public abstract class NumericAssertions<T> where T : struct, IComparable
    {
        protected internal NumericAssertions(T? value)
        {
            Subject = value;
        }

        public T? Subject { get; private set; }

        public AndConstraint<NumericAssertions<T>> BePositive()
        {
            return BePositive(String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BePositive(string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(0) > 0,
                "Expected positive value{2}, but found {1}", null, Subject.Value, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeNegative()
        {
            return BeNegative(String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeNegative(string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(0) < 0,
                "Expected negative value{2}, but found {1}", null, Subject.Value, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeLessThan(T expected)
        {
            return BeLessThan(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeLessThan(T expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(expected) < 0,
                "Expected a value less than {0}{2}, but found {1}.", expected, Subject.Value, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeLessOrEqualTo(T expected)
        {
            return BeLessOrEqualTo(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeLessOrEqualTo(T expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(expected) <= 0,
                "Expected a value less or equal to {0}{2}, but found {1}.", expected, Subject.Value, reason,
                reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeGreaterThan(T expected)
        {
            return BeGreaterThan(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeGreaterThan(T expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(expected) > 0,
                "Expected a value greater than {0}{2}, but found {1}.", expected, Subject.Value, reason,
                reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeGreaterOrEqualTo(T expected)
        {
            return BeGreaterOrEqualTo(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeGreaterOrEqualTo(T expected, string reason,
            params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.Value.CompareTo(expected) >= 0,
                "Expected a value greater or equal to {0}{2}, but found {1}.", expected, Subject.Value, reason,
                reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }
    }
}