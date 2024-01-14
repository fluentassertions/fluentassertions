using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Types;

/// <summary>
/// Contains a number of methods to assert that a <see cref="ICustomAttributeProvider"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public abstract class ReflectionAssertions<TSubject, TAssertions> : ReferenceTypeAssertions<TSubject, TAssertions>
    where TSubject : ICustomAttributeProvider
    where TAssertions : ReflectionAssertions<TSubject, TAssertions>
{
    protected ReflectionAssertions(TSubject subject)
        : base(subject)
    {
    }

    /// <summary>
    /// Asserts that the subject is decorated with the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<TAssertions, TAttribute> BeDecoratedWith<TAttribute>(
        string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        return BeDecoratedWith<TAttribute>(_ => true, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the subject is not decorated with the specified <typeparamref name="TAttribute"/>.
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
    /// Asserts that the subject is decorated with an attribute of type <typeparamref name="TAttribute"/>
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
    public AndWhichConstraint<TAssertions, TAttribute> BeDecoratedWith<TAttribute>(
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

            string message = isMatchingAttributePredicate is LambdaExpression { Body: ConstantExpression { Value: true } }
                ? $"Expected {Identifier} {SubjectDescription} to be decorated with {typeof(TAttribute)}{{reason}}" +
                ", but that attribute was not found."
                : $"Expected {Identifier} {SubjectDescription} to be decorated with {typeof(TAttribute)} that matches {{0}}{{reason}}" +
                ", but no matching attribute was found.";

            Execute.Assertion
                .ForCondition(attributes.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(message, isMatchingAttributePredicate);
        }

        return new AndWhichConstraint<TAssertions, TAttribute>((TAssertions)this, attributes);
    }

    /// <summary>
    /// Asserts that the subject is not decorated with an attribute of type <typeparamref name="TAttribute"/>
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

            string message = isMatchingAttributePredicate is LambdaExpression { Body: ConstantExpression { Value: true } }
                ? $"Expected {Identifier} {SubjectDescription} to not be decorated with {typeof(TAttribute)}{{reason}}" +
                ", but that attribute was found."
                : $"Expected {Identifier} {SubjectDescription} to not be decorated with {typeof(TAttribute)} that matches {{0}}{{reason}}" +
                ", but a matching attribute was found.";

            Execute.Assertion
                .ForCondition(!attributes.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(message, isMatchingAttributePredicate);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the subject is decorated with, or inherits from a parent class, the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<TAssertions, TAttribute> BeDecoratedWithOrInherit<TAttribute>(
        string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        IEnumerable<TAttribute> attributes = Subject.GetMatchingOrInheritedAttributes<TAttribute>();

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(attributes.Any())
            .FailWith(
                $"Expected {Identifier} {SubjectDescription} to be decorated with or inherit {typeof(TAttribute)}{{reason}}" +
                ", but that attribute was not found.");

        return new AndWhichConstraint<TAssertions, TAttribute>((TAssertions)this, attributes);
    }

    /// <summary>
    /// Asserts that the subject is decorated with, or inherits from a parent class, an attribute of type <typeparamref name="TAttribute"/>
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
    public AndWhichConstraint<TAssertions, TAttribute> BeDecoratedWithOrInherit<TAttribute>(
        Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate);

        BeDecoratedWithOrInherit<TAttribute>(because, becauseArgs);

        IEnumerable<TAttribute> attributes = Subject.GetMatchingOrInheritedAttributes(isMatchingAttributePredicate);

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(attributes.Any())
            .FailWith(
                $"Expected {Identifier} {SubjectDescription} to be decorated with or inherit {typeof(TAttribute)} that matches {{0}}{{reason}}" +
                ", but no matching attribute was found.", isMatchingAttributePredicate);

        return new AndWhichConstraint<TAssertions, TAttribute>((TAssertions)this, attributes);
    }

    /// <summary>
    /// Asserts that the subject is not decorated with and does not inherit from a parent class,
    /// the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeDecoratedWithOrInherit<TAttribute>(
        string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(!Subject.IsDecoratedWithOrInherit<TAttribute>())
            .FailWith(
                $"Expected {Identifier} {SubjectDescription} to not be decorated with or inherit {typeof(TAttribute)}{{reason}}" +
                ", but that attribute was found.");

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the subject is not decorated with and does not inherit from a parent class, an
    /// attribute of type <typeparamref name="TAttribute"/> that matches the specified
    /// <paramref name="isMatchingAttributePredicate"/>.
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
    public AndConstraint<TAssertions> NotBeDecoratedWithOrInherit<TAttribute>(
        Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate);

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(!Subject.IsDecoratedWithOrInherit(isMatchingAttributePredicate))
            .FailWith(
                $"Expected {Identifier} {SubjectDescription} to not be decorated with or inherit {typeof(TAttribute)} that matches {{0}}{{reason}}" +
                ", but a matching attribute was found.", isMatchingAttributePredicate);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    internal abstract string SubjectDescription { get; }
}
