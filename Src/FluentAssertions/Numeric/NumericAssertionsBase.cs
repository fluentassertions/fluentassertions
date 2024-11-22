using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Numeric;

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
public abstract class NumericAssertionsBase<T, TSubject, TAssertions>
    where T : struct, IComparable<T>
    where TAssertions : NumericAssertionsBase<T, TSubject, TAssertions>
{
    public abstract TSubject Subject { get; }

    protected NumericAssertionsBase(AssertionChain assertionChain)
    {
        CurrentAssertionChain = assertionChain;
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
    public AndConstraint<TAssertions> Be(T expected, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is T subject && subject.CompareTo(expected) == 0)
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
    public AndConstraint<TAssertions> Be(T? expected, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(expected is { } value ? Subject is T subject && subject.CompareTo(value) == 0 : Subject is not T)
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
    public AndConstraint<TAssertions> NotBe(T unexpected, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is not T subject || subject.CompareTo(unexpected) != 0)
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
    public AndConstraint<TAssertions> NotBe(T? unexpected, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(unexpected is { } value ? Subject is not T subject || subject.CompareTo(value) != 0 : Subject is T)
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
    public AndConstraint<TAssertions> BePositive([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is T subject && subject.CompareTo(default) > 0)
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
    public AndConstraint<TAssertions> BeNegative([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is T value && !IsNaN(value) && value.CompareTo(default) < 0)
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
    public AndConstraint<TAssertions> BeLessThan(T expected, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (IsNaN(expected))
        {
            throw new ArgumentException("A value can never be less than NaN", nameof(expected));
        }

        CurrentAssertionChain
            .ForCondition(Subject is T value && !IsNaN(value) && value.CompareTo(expected) < 0)
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
    public AndConstraint<TAssertions> BeLessThanOrEqualTo(T expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (IsNaN(expected))
        {
            throw new ArgumentException("A value can never be less than or equal to NaN", nameof(expected));
        }

        CurrentAssertionChain
            .ForCondition(Subject is T value && !IsNaN(value) && value.CompareTo(expected) <= 0)
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
    public AndConstraint<TAssertions> BeGreaterThan(T expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (IsNaN(expected))
        {
            throw new ArgumentException("A value can never be greater than NaN", nameof(expected));
        }

        CurrentAssertionChain
            .ForCondition(Subject is T subject && subject.CompareTo(expected) > 0)
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
    public AndConstraint<TAssertions> BeGreaterThanOrEqualTo(T expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (IsNaN(expected))
        {
            throw new ArgumentException("A value can never be greater than or equal to a NaN", nameof(expected));
        }

        CurrentAssertionChain
            .ForCondition(Subject is T subject && subject.CompareTo(expected) >= 0)
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
    public AndConstraint<TAssertions> BeInRange(T minimumValue, T maximumValue,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (IsNaN(minimumValue) || IsNaN(maximumValue))
        {
            throw new ArgumentException("A range cannot begin or end with NaN");
        }

        CurrentAssertionChain
            .ForCondition(Subject is T value && value.CompareTo(minimumValue) >= 0 && value.CompareTo(maximumValue) <= 0)
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
    public AndConstraint<TAssertions> NotBeInRange(T minimumValue, T maximumValue,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (IsNaN(minimumValue) || IsNaN(maximumValue))
        {
            throw new ArgumentException("A range cannot begin or end with NaN");
        }

        CurrentAssertionChain
            .ForCondition(Subject is T value && !(value.CompareTo(minimumValue) >= 0 && value.CompareTo(maximumValue) <= 0))
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
    public AndConstraint<TAssertions> BeOneOf(IEnumerable<T> validValues,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is T value && validValues.Contains(value))
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
    public AndConstraint<TAssertions> BeOfType(Type expectedType, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
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
    public AndConstraint<TAssertions> NotBeOfType(Type unexpectedType, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpectedType);

        CurrentAssertionChain
            .ForCondition(Subject is T)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected type not to be " + unexpectedType + "{reason}, but found <null>.");

        if (CurrentAssertionChain.Succeeded)
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
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(predicate);

        CurrentAssertionChain
            .ForCondition(Subject is T expression && predicate.Compile()(expression))
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

        if (Subject is not T subject || expected is not { } expectedValue)
        {
            return noDifferenceMessage;
        }

        var difference = CalculateDifferenceForFailureMessage(subject, expectedValue);
        return difference is null ? noDifferenceMessage : $" (difference of {difference}).";
    }

    /// <summary>
    /// Provides access to the <see cref="AssertionChain"/> that this assertion class was initialized with.
    /// </summary>
    public AssertionChain CurrentAssertionChain { get; }
}
