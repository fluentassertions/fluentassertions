using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

/// <summary>
/// Contains a number of methods to assert that a nullable value type is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public abstract class NullableValueTypeAssertions<TSubject, TAssertions>(TSubject? subject, AssertionChain assertionChain)
    : ValueTypeAssertionsBase<TSubject, TSubject?, TAssertions>(assertionChain)
    where TSubject : struct
    where TAssertions : NullableValueTypeAssertions<TSubject, TAssertions>
{
    /// <inheritdoc/>
    public override TSubject? Subject { get; } = subject;

    /// <summary>
    /// Asserts that the current value has not been initialized yet.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeNull([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is null)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Expected {context} to be <null>{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current value has been initialized.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeNull([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Expected {context} not to be <null>{reason}.");

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current value has been initialized. Equivalent to <see cref="NotBeNull"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveValue([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return NotBeNull(because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current value has not been initialized yet. Equivalent to <see cref="BeNull"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHaveValue([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return BeNull(because, becauseArgs);
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
    public AndConstraint<TAssertions> Match(Expression<Func<TSubject?, bool>> predicate,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate), "Cannot match a value against a <null> predicate.");

        CurrentAssertionChain
            .ForCondition(predicate.Compile()(Subject))
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Expected {context} to match {1}{reason}, but found {0}.", Subject, predicate);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }
}
