using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="IComparable"/> is in the expected state.
    /// </summary>
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

        /// <summary>
        /// Asserts that the numeric value is greater than or equal to zero.
        /// </summary>
        public AndConstraint<NumericAssertions<T>> BePositive()
        {
            return BePositive(String.Empty);
        }

        /// <summary>
        /// Asserts that the numeric value is greater than or equal to zero.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BePositive(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(default(T)) > 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected positive value{reason}, but found {0}", Subject);
            
            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the numeric value is less than zero.
        /// </summary>
        public AndConstraint<NumericAssertions<T>> BeNegative()
        {
            return BeNegative(String.Empty);
        }

        /// <summary>
        /// Asserts that the numeric value is less than zero.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeNegative(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(default(T)) < 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected negative value{reason}, but found {0}", Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the numeric value is less than the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The value to compare the current numeric value with.</param>
        public AndConstraint<NumericAssertions<T>> BeLessThan(T expected)
        {
            return BeLessThan(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the numeric value is less than the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The value to compare the current numeric value with.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeLessThan(T expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(expected) < 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value less than {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the numeric value is less than or equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The value to compare the current numeric value with.</param>
        public AndConstraint<NumericAssertions<T>> BeLessOrEqualTo(T expected)
        {
            return BeLessOrEqualTo(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the numeric value is less than or equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The value to compare the current numeric value with.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeLessOrEqualTo(T expected, string reason,
            params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(expected) <= 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value less or equal to {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the numeric value is greater than the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The value to compare the current numeric value with.</param>
        public AndConstraint<NumericAssertions<T>> BeGreaterThan(T expected)
        {
            return BeGreaterThan(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the numeric value is greater than the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The value to compare the current numeric value with.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeGreaterThan(T expected, string reason,
            params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(expected) > 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value greater than {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the numeric value is greater than or equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The value to compare the current numeric value with.</param>
        public AndConstraint<NumericAssertions<T>> BeGreaterOrEqualTo(T expected)
        {
            return BeGreaterOrEqualTo(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the numeric value is greater than or equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The value to compare the current numeric value with.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeGreaterOrEqualTo(T expected, string reason,
            params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(expected) >= 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value greater or equal to {0}{reason}, but found {1}.", expected, Subject);
            
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
            return BeInRange(minimumValue, maximumValue, string.Empty);
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
                .FailWith("Expected value {0} to be between {1} and {2}{reason}, but it was not.",
                    Subject, minimumValue, maximumValue);

            return new AndConstraint<NumericAssertions<T>>(this);
        }
    }
}