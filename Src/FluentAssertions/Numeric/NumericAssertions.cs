using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using FluentAssertions.Execution;
using FluentAssertions.Localization;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="IComparable"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NumericAssertions<T>
        where T : struct
    {
        public NumericAssertions(object value)
        {
            if (!(value is null))
            {
                Subject = value as IComparable;
                if (Subject is null)
                {
                    throw new InvalidOperationException(Resources.Numeric_ThisClassOnlySupportsTypeImplementingIComparable);
                }
            }
        }

        public IComparable Subject { get; private set; }

        /// <summary>
        /// Asserts that the integral number value is exactly the same as the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> Be(T expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!(Subject is null) && Subject.CompareTo(expected) == 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Numeric_ExpectedValueToBeXFormat + Resources.Common_CommaButFoundYFormat,
                    expected, Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the integral number value is exactly the same as the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> Be(T? expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition((Subject is null && !expected.HasValue) || (!(Subject is null) && Subject.CompareTo(expected) == 0))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Numeric_ExpectedValueToBeXFormat + Resources.Common_CommaButFoundYFormat,
                    expected, Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the integral number value is not the same as the <paramref name="unexpected"/> value.
        /// </summary>
        /// <param name="unexpected">The unexpected value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> NotBe(T unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject is null || Subject.CompareTo(unexpected) != 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Numeric_DidNotExpectValueToBeXFormat, unexpected);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the integral number value is not the same as the <paramref name="unexpected"/> value.
        /// </summary>
        /// <param name="unexpected">The unexpected value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> NotBe(T? unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition((Subject is null == unexpected.HasValue) || (!(Subject is null) && Subject.CompareTo(unexpected) != 0))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Numeric_DidNotExpectValueToBeXFormat, unexpected);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the numeric value is greater than zero.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BePositive(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!(Subject is null) && Subject.CompareTo(default(T)) > 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Numeric_ExpectedValueToBePositive + Resources.Common_CommaButFoundXFormat,
                    Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the numeric value is less than zero.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeNegative(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!(Subject is null) && Subject.CompareTo(default(T)) < 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Numeric_ExpectedValueToBeNegative + Resources.Common_CommaButFoundXFormat,
                    Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the numeric value is less than the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The value to compare the current numeric value with.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeLessThan(T expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!(Subject is null) && Subject.CompareTo(expected) < 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Numeric_ExpectedValueToBeLessThanXFormat + Resources.Common_CommaButFoundYFormat,
                    expected, Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the numeric value is less than or equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The value to compare the current numeric value with.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeLessOrEqualTo(T expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!(Subject is null) && Subject.CompareTo(expected) <= 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Numeric_ExpectedValueToBeLessOrEqualToXFormat + Resources.Common_CommaButFoundYFormat,
                    expected, Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the numeric value is greater than the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The value to compare the current numeric value with.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeGreaterThan(T expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!(Subject is null) && Subject.CompareTo(expected) > 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Numeric_ExpectedValueToBeGreaterThanXFormat + Resources.Common_CommaButFoundYFormat,
                    expected, Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the numeric value is greater than or equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The value to compare the current numeric value with.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeGreaterOrEqualTo(T expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!(Subject is null) && Subject.CompareTo(expected) >= 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Numeric_ExpectedValueToBeGreaterOrEqualToXFormat + Resources.Common_CommaButFoundYFormat,
                    expected, Subject);

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
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeInRange(T minimumValue, T maximumValue, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!(Subject is null) && (Subject.CompareTo(minimumValue) >= 0) && (Subject.CompareTo(maximumValue) <= 0))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Numeric_ExpectedValueToBeBetweenXAndYFormat + Resources.Common_CommaButFoundZFormat,
                    minimumValue, maximumValue, Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that a value is not within a range.
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
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<NumericAssertions<T>> NotBeInRange(T minimumValue, T maximumValue, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!(Subject is null) && !((Subject.CompareTo(minimumValue) >= 0) && (Subject.CompareTo(maximumValue) <= 0)))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Numeric_ExpectedValueToNotBeBetweenXAndYFormat + Resources.Common_CommaButFoundZFormat,
                    minimumValue, maximumValue, Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that a value is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeOneOf(params T[] validValues)
        {
            return BeOneOf(validValues, string.Empty);
        }

        /// <summary>
        /// Asserts that a value is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeOneOf(IEnumerable<T> validValues, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!(Subject is null) && validValues.Contains((T)Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Numeric_ExpectecValueToBeOneOfXFormat + Resources.Common_CommaButFoundYFormat,
                    validValues, Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the object is of the specified type <paramref name="expectedType"/>.
        /// </summary>
        /// <param name="expectedType">
        /// The type that the subject is supposed to be of.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> BeOfType(Type expectedType, string because = "", params object[] becauseArgs)
        {
            Type subjectType = Subject.GetType();
            if (expectedType.GetTypeInfo().IsGenericTypeDefinition && subjectType.GetTypeInfo().IsGenericType)
            {
                subjectType.GetGenericTypeDefinition().Should().Be(expectedType, because, becauseArgs);
            }
            else
            {
                subjectType.Should().Be(expectedType, because, becauseArgs);
            }

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the object is not of the specified type <paramref name="unexpectedType"/>.
        /// </summary>
        /// <param name="unexpectedType">
        /// The type that the subject is not supposed to be of.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NumericAssertions<T>> NotBeOfType(Type unexpectedType, string because = "", params object[] becauseArgs)
        {
            Subject.GetType().Should().NotBe(unexpectedType, because, becauseArgs);

            return new AndConstraint<NumericAssertions<T>>(this);
        }
    }
}
