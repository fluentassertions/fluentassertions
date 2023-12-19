using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Primitives;

namespace FluentAssertionsAsync.Types;

/// <summary>
/// Contains a number of methods to assert that a <see cref="MemberInfo"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public abstract class MemberInfoAssertions<TSubject, TAssertions> : ReferenceTypeAssertions<TSubject, TAssertions>
    where TSubject : MemberInfo
    where TAssertions : MemberInfoAssertions<TSubject, TAssertions>
{
    protected MemberInfoAssertions(TSubject subject)
        : base(subject)
    {
    }

    /// <summary>
    /// Asserts that the selected member is decorated with the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<MemberInfoAssertions<TSubject, TAssertions>, TAttribute> BeDecoratedWith<TAttribute>(
        string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        return BeDecoratedWith<TAttribute>(_ => true, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the selected member is not decorated with the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeDecoratedWith<TAttribute>(
        string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        return NotBeDecoratedWith<TAttribute>(_ => true, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the selected member is decorated with an attribute of type <typeparamref name="TAttribute"/>
    /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
    /// </summary>
    /// <param name="isMatchingAttributePredicate">
    /// The predicate that the attribute must match.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="isMatchingAttributePredicate"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<MemberInfoAssertions<TSubject, TAssertions>, TAttribute> BeDecoratedWith<TAttribute>(
        Expression<Func<TAttribute, bool>> isMatchingAttributePredicate,
        string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate);

        bool success = Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                $"Expected {Identifier} to be decorated with {typeof(TAttribute)}{{reason}}" +
                ", but {context:member} is <null>.");

        IEnumerable<TAttribute> attributes = Array.Empty<TAttribute>();

        if (success)
        {
            attributes = Subject.GetMatchingAttributes(isMatchingAttributePredicate);

            Execute.Assertion
                .ForCondition(attributes.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    $"Expected {Identifier} {SubjectDescription} to be decorated with {typeof(TAttribute)}{{reason}}" +
                    ", but that attribute was not found.");
        }

        return new AndWhichConstraint<MemberInfoAssertions<TSubject, TAssertions>, TAttribute>(this, attributes);
    }

    /// <summary>
    /// Asserts that the selected member is not decorated with an attribute of type <typeparamref name="TAttribute"/>
    /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
    /// </summary>
    /// <param name="isMatchingAttributePredicate">
    /// The predicate that the attribute must match.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="isMatchingAttributePredicate"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotBeDecoratedWith<TAttribute>(
        Expression<Func<TAttribute, bool>> isMatchingAttributePredicate,
        string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate);

        bool success = Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                $"Expected {Identifier} to not be decorated with {typeof(TAttribute)}{{reason}}" +
                ", but {context:member} is <null>.");

        if (success)
        {
            IEnumerable<TAttribute> attributes = Subject.GetMatchingAttributes(isMatchingAttributePredicate);

            Execute.Assertion
                .ForCondition(!attributes.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    $"Expected {Identifier} {SubjectDescription} to not be decorated with {typeof(TAttribute)}{{reason}}" +
                    ", but that attribute was found.");
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    protected override string Identifier => "member";

    internal virtual string SubjectDescription => $"{Subject.DeclaringType}.{Subject.Name}";
}
