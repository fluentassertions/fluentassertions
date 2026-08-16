using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
/// <summary>
/// Contains the members shared by <see cref="ValueTypeAssertions{TSubject, TAssertions}"/> and
/// <see cref="NullableValueTypeAssertions{TSubject, TAssertions}"/>.
/// </summary>
/// <remarks>
/// Don't derive from this class directly. Use <see cref="ValueTypeAssertions{TSubject,TAssertions}"/> and
/// <see cref="NullableValueTypeAssertions{TSubject,TAssertions}"/> instead.
/// </remarks>
[DebuggerNonUserCode]
public abstract class ValueTypeAssertionsBase<TSubject, TSubjectResult, TAssertions>(AssertionChain assertionChain)
    where TSubject : struct
    where TAssertions : ValueTypeAssertionsBase<TSubject, TSubjectResult, TAssertions>
{
    /// <summary>
    /// Gets the object whose value is being asserted.
    /// </summary>
    public abstract TSubjectResult Subject { get; }

    /// <summary>
    /// Asserts that the value is equal to the specified <paramref name="expected"/> value.
    /// </summary>
    /// <param name="expected">The expected value</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> Be(TSubject expected, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        bool areEqual = Subject is TSubject subject && EqualityComparer<TSubject>.Default.Equals(subject, expected);

        CurrentAssertionChain
            .ForCondition(areEqual)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Expected {context} to be {0}{reason}, but found {1}.", expected, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the value is equal to the specified <paramref name="expected"/> value.
    /// </summary>
    /// <param name="expected">The expected value</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> Be(TSubject? expected, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        bool areEqual = expected is { } value
            ? Subject is TSubject subject && EqualityComparer<TSubject>.Default.Equals(subject, value)
            : Subject is not TSubject;

        CurrentAssertionChain
            .ForCondition(areEqual)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Expected {context} to be {0}{reason}, but found {1}.", expected, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the value is not equal to the specified <paramref name="unexpected"/> value.
    /// </summary>
    /// <param name="unexpected">The unexpected value</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> NotBe(TSubject unexpected, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        bool areEqual = Subject is TSubject subject && EqualityComparer<TSubject>.Default.Equals(subject, unexpected);

        CurrentAssertionChain
            .ForCondition(!areEqual)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Did not expect {context} to be {0}{reason}.", unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the value is not equal to the specified <paramref name="unexpected"/> value.
    /// </summary>
    /// <param name="unexpected">The unexpected value</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> NotBe(TSubject? unexpected, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        bool areEqual = unexpected is { } value
            ? Subject is TSubject subject && EqualityComparer<TSubject>.Default.Equals(subject, value)
            : Subject is not TSubject;

        CurrentAssertionChain
            .ForCondition(!areEqual)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Did not expect {context} to be {0}{reason}.", unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the <paramref name="predicate" /> is satisfied.
    /// </summary>
    /// <param name="predicate">The predicate which must be satisfied by the <typeparamref name="TSubject" />.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <returns>An <see cref="AndConstraint{T}" /> which can be used to chain assertions.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> Match(Expression<Func<TSubject, bool>> predicate,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate), "Cannot match a value against a <null> predicate.");

        CurrentAssertionChain
            .ForCondition(Subject is TSubject)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Expected {context} to match {1}{reason}, but found {0}.", Subject, predicate);

        if (CurrentAssertionChain.Succeeded)
        {
            CurrentAssertionChain
                .ForCondition(Subject is TSubject subject && predicate.Compile()(subject))
                .BecauseOf(because, becauseArgs)
                .WithDefaultIdentifier(Identifier)
                .FailWith("Expected {context} to match {1}{reason}, but found {0}.", Subject, predicate);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// It should be a user-friendly name as it is included in the failure message.
    /// </summary>
    protected abstract string Identifier { get; }

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        throw new NotSupportedException("Equals is not part of Fluent Assertions. Did you mean Be() instead?");

    /// <summary>
    /// Provides access to the <see cref="AssertionChain"/> that this assertion class was initialized with.
    /// </summary>
    public AssertionChain CurrentAssertionChain { get; } = assertionChain;
}
