using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Numeric;

/// <summary>
/// Contains a number of methods to assert that an <see cref="IComparable{T}"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class NumericAssertions<T> : NumericAssertions<T, NumericAssertions<T>>
    where T : struct, IComparable<T>
{
    public NumericAssertions(T value)
        : base(value)
    {
    }
}

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
/// <summary>
/// Contains a number of methods to assert that an <see cref="IComparable{T}"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class NumericAssertions<T, TAssertions>
    where T : struct, IComparable<T>
    where TAssertions : NumericAssertions<T, TAssertions>
{
    public NumericAssertions(T value)
        : this((T?)value)
    {
    }

    private protected NumericAssertions(T? value)
    {
        Subject = value;
    }

    public T? Subject { get; }

    /// <summary>
    /// Asserts that the integral number value is exactly the same as the <paramref name="expected"/> value.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> Be(T expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject?.CompareTo(expected) == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:value} to be {0}{reason}, but found {1}" + GenerateDifferenceMessage(expected), expected,
                Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> Be(T? expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(expected is { } value ? Subject?.CompareTo(value) == 0 : !Subject.HasValue)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:value} to be {0}{reason}, but found {1}" + GenerateDifferenceMessage(expected), expected,
                Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBe(T unexpected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject?.CompareTo(unexpected) != 0)
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:value} to be {0}{reason}.", unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBe(T? unexpected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(unexpected is { } value ? Subject?.CompareTo(value) != 0 : Subject.HasValue)
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:value} to be {0}{reason}.", unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the numeric value is greater than zero.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BePositive(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject?.CompareTo(default) > 0)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:value} to be positive{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the numeric value is less than zero.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeNegative(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject is { } value && !IsNaN(value) && value.CompareTo(default) < 0)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:value} to be negative{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeLessThan(T expected, string because = "", params object[] becauseArgs)
    {
        if (IsNaN(expected))
        {
            throw new ArgumentException("A value can never be less than NaN", nameof(expected));
        }

        Execute.Assertion
            .ForCondition(Subject is { } value && !IsNaN(value) && value.CompareTo(expected) < 0)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:value} to be less than {0}{reason}, but found {1}" + GenerateDifferenceMessage(expected),
                expected, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeLessThanOrEqualTo(T expected, string because = "",
        params object[] becauseArgs)
    {
        if (IsNaN(expected))
        {
            throw new ArgumentException("A value can never be less than or equal to NaN", nameof(expected));
        }

        Execute.Assertion
            .ForCondition(Subject is { } value && !IsNaN(value) && value.CompareTo(expected) <= 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:value} to be less than or equal to {0}{reason}, but found {1}" +
                GenerateDifferenceMessage(expected), expected, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeGreaterThan(T expected, string because = "",
        params object[] becauseArgs)
    {
        if (IsNaN(expected))
        {
            throw new ArgumentException("A value can never be greater than NaN", nameof(expected));
        }

        Execute.Assertion
            .ForCondition(Subject?.CompareTo(expected) > 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:value} to be greater than {0}{reason}, but found {1}" + GenerateDifferenceMessage(expected),
                expected, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeGreaterThanOrEqualTo(T expected, string because = "",
        params object[] becauseArgs)
    {
        if (IsNaN(expected))
        {
            throw new ArgumentException("A value can never be greater than or equal to a NaN", nameof(expected));
        }

        Execute.Assertion
            .ForCondition(Subject?.CompareTo(expected) >= 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:value} to be greater than or equal to {0}{reason}, but found {1}" +
                GenerateDifferenceMessage(expected), expected, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a value is within a range.
    /// </summary>
    /// <remarks>
    /// Where the range is continuous or incremental depends on the actual type of the value.
    /// </remarks>
    /// <param name="minimumValue">
    /// The minimum valid value of the range (inclusive).
    /// </param>
    /// <param name="maximumValue">
    /// The maximum valid value of the range (inclusive).
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeInRange(T minimumValue, T maximumValue, string because = "",
        params object[] becauseArgs)
    {
        if (IsNaN(minimumValue) || IsNaN(maximumValue))
        {
            throw new ArgumentException("A range cannot begin or end with NaN");
        }

        Execute.Assertion
            .ForCondition(Subject is { } value && value.CompareTo(minimumValue) >= 0 && value.CompareTo(maximumValue) <= 0)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:value} to be between {0} and {1}{reason}, but found {2}.",
                minimumValue, maximumValue, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeInRange(T minimumValue, T maximumValue, string because = "",
        params object[] becauseArgs)
    {
        if (IsNaN(minimumValue) || IsNaN(maximumValue))
        {
            throw new ArgumentException("A range cannot begin or end with NaN");
        }

        Execute.Assertion
            .ForCondition(Subject is { } value && !(value.CompareTo(minimumValue) >= 0 && value.CompareTo(maximumValue) <= 0))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:value} to not be between {0} and {1}{reason}, but found {2}.",
                minimumValue, maximumValue, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a value is one of the specified <paramref name="validValues"/>.
    /// </summary>
    /// <param name="validValues">
    /// The values that are valid.
    /// </param>
    public AndConstraint<TAssertions> BeOneOf(params T[] validValues)
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeOneOf(IEnumerable<T> validValues, string because = "",
        params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject is { } value && validValues.Contains(value))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:value} to be one of {0}{reason}, but found {1}.", validValues, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expectedType"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> BeOfType(Type expectedType, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectedType);

        Type subjectType = Subject?.GetType();

        if (expectedType.IsGenericTypeDefinition && subjectType?.IsGenericType == true)
        {
            subjectType.GetGenericTypeDefinition().Should().Be(expectedType, because, becauseArgs);
        }
        else
        {
            subjectType.Should().Be(expectedType, because, becauseArgs);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpectedType"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotBeOfType(Type unexpectedType, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpectedType);

        bool success = Execute.Assertion
            .ForCondition(Subject.HasValue)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected type not to be " + unexpectedType + "{reason}, but found <null>.");

        if (success)
        {
            Subject.GetType().Should().NotBe(unexpectedType, because, becauseArgs);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> Match(Expression<Func<T, bool>> predicate,
        string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(predicate);

        Execute.Assertion
            .ForCondition(predicate.Compile()((T)Subject))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:value} to match {0}{reason}, but found {1}.", predicate.Body, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        throw new NotSupportedException("Equals is not part of Fluent Assertions. Did you mean Be() instead?");

    private protected virtual bool IsNaN(T value) => false;

    /// <summary>
    /// A method to generate additional information upon comparison failures.
    /// </summary>
    /// <param name="subject">The current numeric value verified to be non-null.</param>
    /// <param name="expected">The value to compare the current numeric value with.</param>
    /// <returns>
    /// Returns the difference between a number value and the <paramref name="expected" /> value.
    /// Returns `null` if the compared numbers are small enough that a difference message is irrelevant.
    /// </returns>
    private protected virtual string CalculateDifferenceForFailureMessage(T subject, T expected) => null;

    private string GenerateDifferenceMessage(T? expected)
    {
        const string noDifferenceMessage = ".";

        if (Subject is not { } subject || expected is not T expectedValue)
        {
            return noDifferenceMessage;
        }

        var difference = CalculateDifferenceForFailureMessage(subject, expectedValue);
        return difference is null ? noDifferenceMessage : $" (difference of {difference}).";
    }
}
