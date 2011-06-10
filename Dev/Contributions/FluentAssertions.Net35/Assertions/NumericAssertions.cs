using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
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
            return type.IsGenericType &&
                (type.GetGenericTypeDefinition() == typeof (Nullable<>).GetGenericTypeDefinition());
        }

        private static T GetValueOrDefault(T value)
        {
            return (T) typeof (T).GetMethod("GetValueOrDefault", new Type[0]).Invoke(value, null);
        }

        public IComparable Subject { get; private set; }

        public AndConstraint<NumericAssertions<T>> BePositive()
        {
            return BePositive(String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BePositive(string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => Subject.CompareTo(0) > 0,
                "Expected positive value{2}, but found {1}", null, Subject, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeNegative()
        {
            return BeNegative(String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeNegative(string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => Subject.CompareTo(0) < 0,
                "Expected negative value{2}, but found {1}", null, Subject, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeLessThan(T expected)
        {
            return BeLessThan(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeLessThan(T expected, string reason,
            params object[] reasonParameters)
        {
            Execute.Verify(() => Subject.CompareTo(expected) < 0,
                "Expected a value less than {0}{2}, but found {1}.", expected, Subject, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeLessOrEqualTo(T expected)
        {
            return BeLessOrEqualTo(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeLessOrEqualTo(T expected, string reason,
            params object[] reasonParameters)
        {
            Execute.Verify(() => Subject.CompareTo(expected) <= 0,
                "Expected a value less or equal to {0}{2}, but found {1}.", expected, Subject, reason,
                reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> BeGreaterThan(T expected)
        {
            return BeGreaterThan(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> BeGreaterThan(T expected, string reason,
            params object[] reasonParameters)
        {
            Execute.Verify(() => Subject.CompareTo(expected) > 0,
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
            Execute.Verify(() => Subject.CompareTo(expected) >= 0,
                "Expected a value greater or equal to {0}{2}, but found {1}.", expected, Subject, reason,
                reasonParameters);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that a value is within a range.
        /// </summary>
        /// <remarks>
        /// Where the range is continuous or incremental depends on the actual type of the value. 
        /// </remarks>
        /// <param name="minimumValue">
        /// The minimum valid value of the range.
        /// </param>
        /// <param name="maximumValue">
        /// The maximum valid value of the range.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeInRange(T minimumValue, T maximumValue)
        {
            return BeInRange(minimumValue, maximumValue, "");
        }

        /// <summary>
        /// Asserts that a value is within a range.
        /// </summary>
        /// <remarks>
        /// Where the range is continuous or incremental depends on the actual type of the value. 
        /// </remarks>
        /// <param name="minimumValue">
        /// The minimum valid value of the range.
        /// </param>
        /// <param name="maximumValue">
        /// The maximum valid value of the range.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeInRange(T minimumValue, T maximumValue, string reason,
            params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition((Subject.CompareTo(minimumValue) >= 0) && (Subject.CompareTo(maximumValue) <= 0))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected value {1} to be between {2} and {3}{0}, but it was not.",
                    Subject, minimumValue, maximumValue);

            return new AndConstraint<NumericAssertions<T>>(this);
        }
    }
}