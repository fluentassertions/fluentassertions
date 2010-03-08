using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class NumericAssertions<T> : Assertions<T?, NumericAssertions<T>>
        where T : struct, IComparable
    {
        protected NumericAssertions()
        {
        }

        protected NumericAssertions(T? value)
        {
            ActualValue = value;
        }

        internal NumericAssertions(T value)
        {
            ActualValue = value;
        }

        public AndConstraint<NumericAssertions<T>> BePositive()
        {
            return BePositive(String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BePositive(string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.Value.CompareTo(0) > 0,
                "Expected {0}{2}, but found <{1}>", "positive value", ActualValue.Value, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeNegative()
        {
            return BeNegative(String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeNegative(string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.Value.CompareTo(0) < 0,
                "Expected {0}{2}, but found <{1}>", "negative value", ActualValue.Value, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> Equal(T expected)
        {
            return Equal(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> Equal(T expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.Value.CompareTo(expected) == 0,
                "Expected <{0}>{2}, but found <{1}>.", expected, ActualValue.Value, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> NotEqual(T expected)
        {
            return NotEqual(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> NotEqual(T expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.Value.CompareTo(expected) != 0,
                "Did not expect <{0}>{2}.", expected, ActualValue.Value, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeLessThan(T expected)
        {
            return BeLessThan(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeLessThan(T expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.Value.CompareTo(expected) < 0,
                "Expected a value less than <{0}>{2}, but found <{1}>.", expected, ActualValue.Value, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeLessOrEqualTo(T expected)
        {
            return BeLessOrEqualTo(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeLessOrEqualTo(T expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.Value.CompareTo(expected) <= 0,
                "Expected a value less or equal to <{0}>{2}, but found <{1}>.", expected, ActualValue.Value, reason,
                reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeGreaterThan(T expected)
        {
            return BeGreaterThan(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeGreaterThan(T expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.Value.CompareTo(expected) > 0,
                "Expected a value greater than <{0}>{2}, but found <{1}>.", expected, ActualValue.Value, reason,
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
            VerifyThat(() => ActualValue.Value.CompareTo(expected) >= 0,
                "Expected a value greater or equal to <{0}>{2}, but found <{1}>.", expected, ActualValue.Value, reason,
                reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }
    }
}