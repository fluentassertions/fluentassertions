using System;
using System.Diagnostics;

namespace FluentAssertions
{
    public static partial class FluentAssertionExtensions
    {
        #region Nested type: NullableNumericAssertions

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
                return HaveValue(string.Empty);
            }

            public AndConstraint<NullableNumericAssertions<T>> HaveValue(string reason, params object[] reasonParameters)
            {
                VerifyThat(ActualValue.HasValue, "Expected a value{2}.", null, ActualValue, reason, reasonParameters);

                return new AndConstraint<NullableNumericAssertions<T>>(this);
            }

            public AndConstraint<NullableNumericAssertions<T>> NotHaveValue()
            {
                return NotHaveValue(string.Empty);
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
                VerifyThat(() => ActualValue.Equals(expected), "Expected value <{0}>{2}, but found <{1}>.",
                           expected, ActualValue, reason, reasonParameters);

                return new AndConstraint<NumericAssertions<T>>(this);
            }
        }

        #endregion

        #region Nested type: NumericAssertions

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
                return BePositive(string.Empty);
            }

            public AndConstraint<NumericAssertions<T>> BePositive(string reason, params object[] reasonParameters)
            {
                VerifyThat(() => ActualValue.Value.CompareTo(0) > 0,
                           "Expected {0}{2}, but found <{1}>", "positive value", ActualValue.Value, reason, reasonParameters);

                return new AndConstraint<NumericAssertions<T>>(this);
            }

            public AndConstraint<NumericAssertions<T>> BeNegative()
            {
                return BeNegative(string.Empty);
            }

            public AndConstraint<NumericAssertions<T>> BeNegative(string reason, params object[] reasonParameters)
            {
                VerifyThat(() => ActualValue.Value.CompareTo(0) < 0,
                           "Expected {0}{2}, but found <{1}>", "negative value", ActualValue.Value, reason, reasonParameters);

                return new AndConstraint<NumericAssertions<T>>(this);
            }

            public AndConstraint<NumericAssertions<T>> Equal(T expected)
            {
                return Equal(expected, string.Empty);
            }

            public AndConstraint<NumericAssertions<T>> Equal(T expected, string reason, params object[] reasonParameters)
            {
                VerifyThat(() => ActualValue.Value.CompareTo(expected) == 0,
                           "Expected <{0}>{2}, but found <{1}>.", expected, ActualValue.Value, reason, reasonParameters);

                return new AndConstraint<NumericAssertions<T>>(this);
            }

            public AndConstraint<NumericAssertions<T>> NotEqual(T expected)
            {
                return NotEqual(expected, string.Empty);
            }

            public AndConstraint<NumericAssertions<T>> NotEqual(T expected, string reason, params object[] reasonParameters)
            {
                VerifyThat(() => ActualValue.Value.CompareTo(expected) != 0,
                           "Did not expect <{0}>{2}.", expected, ActualValue.Value, reason, reasonParameters);

                return new AndConstraint<NumericAssertions<T>>(this);
            }

            public AndConstraint<NumericAssertions<T>> BeLessThan(T expected)
            {
                return BeLessThan(expected, string.Empty);
            }

            public AndConstraint<NumericAssertions<T>> BeLessThan(T expected, string reason, params object[] reasonParameters)
            {
                VerifyThat(() => ActualValue.Value.CompareTo(expected) < 0,
                           "Expected a value less than <{0}>{2}, but found <{1}>.", expected, ActualValue.Value, reason, reasonParameters);

                return new AndConstraint<NumericAssertions<T>>(this);
            }

            public AndConstraint<NumericAssertions<T>> BeLessOrEqualTo(T expected)
            {
                return BeLessOrEqualTo(expected, string.Empty);
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
                return BeGreaterThan(expected, string.Empty);
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
                return BeGreaterOrEqualTo(expected, string.Empty);
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

        #endregion
    }
}