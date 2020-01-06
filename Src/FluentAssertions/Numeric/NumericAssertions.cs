using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

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
                    throw new InvalidOperationException("This class only supports types implementing IComparable.");
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
                .FailWith("Expected {context:value} to be {0}{reason}, but found {1}.", expected, Subject);

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
                .FailWith("Expected {context:value} to be {0}{reason}, but found {1}.", expected, Subject);

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
                .FailWith("Did not expect {context:value} to be {0}{reason}.", unexpected);

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
                .FailWith("Did not expect {context:value} to be {0}{reason}.", unexpected);

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
                .FailWith("Expected {context:value} to be positive{reason}, but found {0}.", Subject);

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
                .FailWith("Expected {context:value} to be negative{reason}, but found {0}.", Subject);

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
                .FailWith("Expected {context:value} to be less than {0}{reason}, but found {1}.", expected, Subject);

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
                .FailWith("Expected {context:value} to be less or equal to {0}{reason}, but found {1}.", expected, Subject);

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
                .FailWith("Expected {context:value} to be greater than {0}{reason}, but found {1}.", expected, Subject);

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
                .FailWith("Expected {context:value} to be greater or equal to {0}{reason}, but found {1}.", expected, Subject);

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
                .FailWith("Expected {context:value} to be between {0} and {1}{reason}, but found {2}.",
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
                .FailWith("Expected {context:value} to not be between {0} and {1}{reason}, but found {2}.",
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
                .FailWith("Expected {context:value} to be one of {0}{reason}, but found {1}.", validValues, Subject);

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

        /// <summary>
        /// Asserts that the <paramref name="predicate" /> is satisfied.
        /// </summary>
        /// <param name="predicate">
        /// The predicate which must be satisfied
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<NumericAssertions<T>> Match(Expression<Func<T, bool>> predicate,
            string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate));

            Execute.Assertion
                .ForCondition(predicate.Compile()((T)Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to match {0}{reason}, but found {1}.", predicate.Body, Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }
    }
}
