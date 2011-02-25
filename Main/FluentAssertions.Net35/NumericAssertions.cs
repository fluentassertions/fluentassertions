using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class NumericAssertions<T>
    {
        protected internal NumericAssertions(T value)
        {
            if (!ReferenceEquals(value, null))
            {
                Type type = typeof (T);
                if (IsNullable(type))
                {
                    value = GetValueOrDefault(value);
                }

                if (!ReferenceEquals(value, null))
                {
                    Subject = value as IComparable;
                    if (Subject == null)
                    {
                        throw new InvalidOperationException("This class only supports types implementing IComparable.");
                    }
                }
            }
        }

        private static bool IsNullable(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>).GetGenericTypeDefinition());
        }

        private static T GetValueOrDefault(T value)
        {
            return (T) typeof(T).GetMethod("GetValueOrDefault", new Type[0]).Invoke(value, null);
        }

        public IComparable Subject { get; private set; }

        public AndConstraint<NumericAssertions<T>> BePositive()
        {
            return BePositive(String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BePositive(string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.CompareTo(0) > 0,
                "Expected positive value{2}, but found {1}", null, Subject, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeNegative()
        {
            return BeNegative(String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeNegative(string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.CompareTo(0) < 0,
                "Expected negative value{2}, but found {1}", null, Subject, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeLessThan(T expected)
        {
            return BeLessThan(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeLessThan(T expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.CompareTo(expected) < 0,
                "Expected a value less than {0}{2}, but found {1}.", expected, Subject, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeLessOrEqualTo(T expected)
        {
            return BeLessOrEqualTo(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeLessOrEqualTo(T expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.CompareTo(expected) <= 0,
                "Expected a value less or equal to {0}{2}, but found {1}.", expected, Subject, reason,
                reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeGreaterThan(T expected)
        {
            return BeGreaterThan(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeGreaterThan(T expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => Subject.CompareTo(expected) > 0,
                "Expected a value greater than {0}{2}, but found {1}.", expected, Subject, reason,
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
            Verification.Verify(() => Subject.CompareTo(expected) >= 0,
                "Expected a value greater or equal to {0}{2}, but found {1}.", expected, Subject, reason,
                reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }
    }
}