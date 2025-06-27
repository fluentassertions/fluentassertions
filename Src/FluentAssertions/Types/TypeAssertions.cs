using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Types;

/// <summary>
/// Contains a number of methods to assert that a <see cref="Type"/> meets certain expectations.
/// </summary>
[DebuggerNonUserCode]
[SuppressMessage("Class Design", "AV1010:Member hides inherited member")]
public class TypeAssertions : ReferenceTypeAssertions<Type, TypeAssertions>
{
    private readonly AssertionChain assertionChain;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeAssertions"/> class.
    /// </summary>
    public TypeAssertions(Type type, AssertionChain assertionChain)
        : base(type, assertionChain)
    {
        this.assertionChain = assertionChain;
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is equal to the specified <typeparamref name="TExpected"/> type.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeAssertions> Be<TExpected>([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return Be(typeof(TExpected), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is equal to the specified <paramref name="expected"/> type.
    /// </summary>
    /// <param name="expected">The expected type</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeAssertions> Be(Type expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject == expected)
            .FailWith(GetFailureMessageIfTypesAreDifferent(Subject, expected));

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts than an instance of the subject type is assignable variable of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to which instances of the type should be assignable.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain assertions.</returns>
    public new AndConstraint<TypeAssertions> BeAssignableTo<T>([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return BeAssignableTo(typeof(T), because, becauseArgs);
    }

    /// <summary>
    /// Asserts than an instance of the subject type is assignable variable of given <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type to which instances of the type should be assignable.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain assertions.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
    public new AndConstraint<TypeAssertions> BeAssignableTo(Type type,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(type);

        bool isAssignable = type.IsGenericTypeDefinition
            ? Subject.IsAssignableToOpenGeneric(type)
            : type.IsAssignableFrom(Subject);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(isAssignable)
            .FailWith("Expected {context:type} {0} to be assignable to {1}{reason}, but it is not.", Subject, type);

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts than an instance of the subject type is not assignable variable of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to which instances of the type should not be assignable.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain assertions.</returns>
    public new AndConstraint<TypeAssertions> NotBeAssignableTo<T>([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return NotBeAssignableTo(typeof(T), because, becauseArgs);
    }

    /// <summary>
    /// Asserts than an instance of the subject type is not assignable variable of given <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type to which instances of the type should not be assignable.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain assertions.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
    public new AndConstraint<TypeAssertions> NotBeAssignableTo(Type type,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(type);

        bool isAssignable = type.IsGenericTypeDefinition
            ? Subject.IsAssignableToOpenGeneric(type)
            : type.IsAssignableFrom(Subject);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(!isAssignable)
            .FailWith("Expected {context:type} {0} to not be assignable to {1}{reason}, but it is.", Subject, type);

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Creates an error message in case the specified <paramref name="actual"/> type differs from the
    /// <paramref name="expected"/> type.
    /// </summary>
    /// <returns>
    /// An empty <see cref="string"/> if the two specified types are the same, or an error message that describes that
    /// the two specified types are not the same.
    /// </returns>
    private static string GetFailureMessageIfTypesAreDifferent(Type actual, Type expected)
    {
        if (actual == expected)
        {
            return string.Empty;
        }

        string expectedType = expected?.FullName ?? "<null>";
        string actualType = actual?.FullName ?? "<null>";

        if (expectedType == actualType)
        {
            expectedType = "[" + expected.AssemblyQualifiedName + "]";
            actualType = "[" + actual.AssemblyQualifiedName + "]";
        }

        return $"Expected type to be {expectedType}{{reason}}, but found {actualType}.";
    }

    /// <summary>
    /// Asserts that the current type is not equal to the specified <typeparamref name="TUnexpected"/> type.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeAssertions> NotBe<TUnexpected>([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return NotBe(typeof(TUnexpected), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current type is not equal to the specified <paramref name="unexpected"/> type.
    /// </summary>
    /// <param name="unexpected">The unexpected type</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeAssertions> NotBe(Type unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        string nameOfUnexpectedType = unexpected is not null ? $"[{unexpected.AssemblyQualifiedName}]" : "<null>";

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject != unexpected)
            .FailWith("Expected type not to be " + nameOfUnexpectedType + "{reason}, but it is.");

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is decorated with the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<TypeAssertions, TAttribute> BeDecoratedWith<TAttribute>(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        IEnumerable<TAttribute> attributes = Subject.GetMatchingAttributes<TAttribute>();

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(attributes.Any())
            .FailWith("Expected type {0} to be decorated with {1}{reason}, but the attribute was not found.",
                Subject, typeof(TAttribute));

        return new AndWhichConstraint<TypeAssertions, TAttribute>(this, attributes, assertionChain);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is decorated with an attribute of type <typeparamref name="TAttribute"/>
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
    public AndWhichConstraint<TypeAssertions, TAttribute> BeDecoratedWith<TAttribute>(
        Expression<Func<TAttribute, bool>> isMatchingAttributePredicate,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate);

        BeDecoratedWith<TAttribute>(because, becauseArgs);

        IEnumerable<TAttribute> attributes = Subject.GetMatchingAttributes(isMatchingAttributePredicate);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(attributes.Any())
            .FailWith(
                "Expected type {0} to be decorated with {1} that matches {2}{reason}, but no matching attribute was found.",
                Subject, typeof(TAttribute), isMatchingAttributePredicate);

        return new AndWhichConstraint<TypeAssertions, TAttribute>(this, attributes, assertionChain);
    }

    /// <summary>
    /// Asserts that the current <see cref="System.Type"/> is decorated with, or inherits from a parent class, the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<TypeAssertions, TAttribute> BeDecoratedWithOrInherit<TAttribute>(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        IEnumerable<TAttribute> attributes = Subject.GetMatchingOrInheritedAttributes<TAttribute>();

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(attributes.Any())
            .FailWith("Expected type {0} to be decorated with or inherit {1}{reason}, but the attribute was not found.",
                Subject, typeof(TAttribute));

        return new AndWhichConstraint<TypeAssertions, TAttribute>(this, attributes, assertionChain);
    }

    /// <summary>
    /// Asserts that the current <see cref="System.Type"/> is decorated with, or inherits from a parent class, an attribute of type <typeparamref name="TAttribute"/>
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
    public AndWhichConstraint<TypeAssertions, TAttribute> BeDecoratedWithOrInherit<TAttribute>(
        Expression<Func<TAttribute, bool>> isMatchingAttributePredicate,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate);

        BeDecoratedWithOrInherit<TAttribute>(because, becauseArgs);

        IEnumerable<TAttribute> attributes = Subject.GetMatchingOrInheritedAttributes(isMatchingAttributePredicate);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(attributes.Any())
            .FailWith(
                "Expected type {0} to be decorated with or inherit {1} that matches {2}{reason}" +
                ", but no matching attribute was found.", Subject, typeof(TAttribute), isMatchingAttributePredicate);

        return new AndWhichConstraint<TypeAssertions, TAttribute>(this, attributes, assertionChain);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is not decorated with the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeAssertions> NotBeDecoratedWith<TAttribute>([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
        where TAttribute : Attribute
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(!Subject.IsDecoratedWith<TAttribute>())
            .FailWith("Expected type {0} to not be decorated with {1}{reason}, but the attribute was found.",
                Subject, typeof(TAttribute));

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is not decorated with an attribute of type
    /// <typeparamref name="TAttribute"/> that matches the specified <paramref name="isMatchingAttributePredicate"/>.
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
    public AndConstraint<TypeAssertions> NotBeDecoratedWith<TAttribute>(
        Expression<Func<TAttribute, bool>> isMatchingAttributePredicate,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(!Subject.IsDecoratedWith(isMatchingAttributePredicate))
            .FailWith(
                "Expected type {0} to not be decorated with {1} that matches {2}{reason}, but a matching attribute was found.",
                Subject, typeof(TAttribute), isMatchingAttributePredicate);

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is not decorated with and does not inherit from a parent class,
    /// the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TypeAssertions> NotBeDecoratedWithOrInherit<TAttribute>(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(!Subject.IsDecoratedWithOrInherit<TAttribute>())
            .FailWith("Expected type {0} to not be decorated with or inherit {1}{reason}, but the attribute was found.",
                Subject, typeof(TAttribute));

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is not decorated with and does not inherit from a parent class, an
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
    public AndConstraint<TypeAssertions> NotBeDecoratedWithOrInherit<TAttribute>(
        Expression<Func<TAttribute, bool>> isMatchingAttributePredicate,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(!Subject.IsDecoratedWithOrInherit(isMatchingAttributePredicate))
            .FailWith(
                "Expected type {0} to not be decorated with or inherit {1} that matches {2}{reason}" +
                ", but a matching attribute was found.", Subject, typeof(TAttribute), isMatchingAttributePredicate);

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> implements <paramref name="interfaceType"/>.
    /// </summary>
    /// <param name="interfaceType">The interface that should be implemented.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="interfaceType"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeAssertions> Implement(Type interfaceType,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(interfaceType);

        AssertSubjectImplements(interfaceType, because, becauseArgs);

        return new AndConstraint<TypeAssertions>(this);
    }

    private bool AssertSubjectImplements(Type interfaceType,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        bool containsInterface = interfaceType.IsAssignableFrom(Subject) && interfaceType != Subject;

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected type {0} to implement interface {1}{reason}", Subject, interfaceType, chain => chain
                .ForCondition(interfaceType.IsInterface)
                .FailWith(", but {0} is not an interface.", interfaceType)
                .Then
                .ForCondition(containsInterface)
                .FailWith(", but it does not."));

        return assertionChain.Succeeded;
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> implements interface <typeparamref name="TInterface"/>.
    /// </summary>
    /// <typeparam name="TInterface">The interface that should be implemented.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TypeAssertions> Implement<TInterface>([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
        where TInterface : class
    {
        return Implement(typeof(TInterface), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not implement <paramref name="interfaceType"/>.
    /// </summary>
    /// <param name="interfaceType">The interface that should be not implemented.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="interfaceType"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeAssertions> NotImplement(Type interfaceType,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(interfaceType);

        bool containsInterface = interfaceType.IsAssignableFrom(Subject) && interfaceType != Subject;

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected type {0} to not implement interface {1}{reason}", Subject, interfaceType, chain => chain
                .ForCondition(interfaceType.IsInterface)
                .FailWith(", but {0} is not an interface.", interfaceType)
                .Then
                .ForCondition(!containsInterface)
                .FailWith(", but it does.", interfaceType));

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not implement interface <typeparamref name="TInterface"/>.
    /// </summary>
    /// <typeparam name="TInterface">The interface that should not be implemented.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TypeAssertions> NotImplement<TInterface>([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
        where TInterface : class
    {
        return NotImplement(typeof(TInterface), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is derived from <paramref name="baseType"/>.
    /// </summary>
    /// <param name="baseType">The type that should be derived from.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="baseType"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeAssertions> BeDerivedFrom(Type baseType,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(baseType);

        bool isDerivedFrom = baseType.IsGenericTypeDefinition
            ? Subject.IsDerivedFromOpenGeneric(baseType)
            : Subject.IsSubclassOf(baseType);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected type {0} to be derived from {1}{reason}", Subject, baseType, chain => chain
                .ForCondition(!baseType.IsInterface)
                .FailWith(", but {0} is an interface.", baseType)
                .Then
                .ForCondition(isDerivedFrom)
                .FailWith(", but it is not."));

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is derived from <typeparamref name="TBaseClass"/>.
    /// </summary>
    /// <typeparam name="TBaseClass">The type that should be derived from.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TypeAssertions> BeDerivedFrom<TBaseClass>([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
        where TBaseClass : class
    {
        return BeDerivedFrom(typeof(TBaseClass), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is not derived from <paramref name="baseType"/>.
    /// </summary>
    /// <param name="baseType">The type that should not be derived from.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="baseType"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeAssertions> NotBeDerivedFrom(Type baseType,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(baseType);

        bool isDerivedFrom = baseType.IsGenericTypeDefinition
            ? Subject.IsDerivedFromOpenGeneric(baseType)
            : Subject.IsSubclassOf(baseType);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected type {0} not to be derived from {1}{reason}", Subject, baseType, chain => chain
                .ForCondition(!baseType.IsInterface)
                .FailWith(", but {0} is an interface.", baseType)
                .Then
                .ForCondition(!isDerivedFrom)
                .FailWith(", but it is."));

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is not derived from <typeparamref name="TBaseClass"/>.
    /// </summary>
    /// <typeparam name="TBaseClass">The type that should not be derived from.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TypeAssertions> NotBeDerivedFrom<TBaseClass>([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
        where TBaseClass : class
    {
        return NotBeDerivedFrom(typeof(TBaseClass), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is sealed.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="InvalidOperationException"><see cref="ReferenceTypeAssertions{Type, TypeAssertions}.Subject"/>
    /// is not a class.</exception>
    public AndConstraint<TypeAssertions> BeSealed([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected type to be sealed{reason}, but {context:type} is <null>.");

        if (assertionChain.Succeeded)
        {
            AssertThatSubjectIsClass();

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.IsCSharpSealed())
                .FailWith("Expected type {0} to be sealed{reason}.", Subject);
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is not sealed.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="InvalidOperationException"><see cref="ReferenceTypeAssertions{Type, TypeAssertions}.Subject"/>
    /// is not a class.</exception>
    public AndConstraint<TypeAssertions> NotBeSealed([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected type not to be sealed{reason}, but {context:type} is <null>.");

        if (assertionChain.Succeeded)
        {
            AssertThatSubjectIsClass();

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(!Subject.IsCSharpSealed())
                .FailWith("Expected type {0} not to be sealed{reason}.", Subject);
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is abstract.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="InvalidOperationException"><see cref="ReferenceTypeAssertions{Type, TypeAssertions}.Subject"/>
    /// is not a class.</exception>
    public AndConstraint<TypeAssertions> BeAbstract([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected type to be abstract{reason}, but {context:type} is <null>.");

        if (assertionChain.Succeeded)
        {
            AssertThatSubjectIsClass();

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.IsCSharpAbstract())
                .FailWith("Expected {context:type} {0} to be abstract{reason}.", Subject);
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is not abstract.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="InvalidOperationException"><see cref="ReferenceTypeAssertions{Type, TypeAssertions}.Subject"/>
    /// is not a class.</exception>
    public AndConstraint<TypeAssertions> NotBeAbstract([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected type not to be abstract{reason}, but {context:type} is <null>.");

        if (assertionChain.Succeeded)
        {
            AssertThatSubjectIsClass();

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(!Subject.IsCSharpAbstract())
                .FailWith("Expected type {0} not to be abstract{reason}.", Subject);
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is static.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="InvalidOperationException"><see cref="ReferenceTypeAssertions{Type, TypeAssertions}.Subject"/>
    /// is not a class.</exception>
    public AndConstraint<TypeAssertions> BeStatic([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected type to be static{reason}, but {context:type} is <null>.");

        if (assertionChain.Succeeded)
        {
            AssertThatSubjectIsClass();

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.IsCSharpStatic())
                .FailWith("Expected type {0} to be static{reason}.", Subject);
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> is not static.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="InvalidOperationException"><see cref="ReferenceTypeAssertions{Type, TypeAssertions}.Subject"/>
    /// is not a class.</exception>
    public AndConstraint<TypeAssertions> NotBeStatic([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected type not to be static{reason}, but {context:type} is <null>.");

        if (assertionChain.Succeeded)
        {
            AssertThatSubjectIsClass();

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(!Subject.IsCSharpStatic())
                .FailWith("Expected type {0} not to be static{reason}.", Subject);
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> has a property of type <paramref name="propertyType"/> named
    /// <paramref name="name"/>.
    /// </summary>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="name">The name of the property.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="propertyType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    public AndWhichConstraint<TypeAssertions, PropertyInfo> HaveProperty(
        Type propertyType, string name,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(propertyType);
        Guard.ThrowIfArgumentIsNullOrEmpty(name);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                $"Cannot determine if a type has a property named {name} if the type is <null>.");

        PropertyInfo propertyInfo = null;

        if (assertionChain.Succeeded)
        {
            propertyInfo = Subject.FindPropertyByName(name);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(propertyInfo is not null)
                .FailWith(() =>
                {
                    var subjectDescription = assertionChain.HasOverriddenCallerIdentifier
                        ? assertionChain.CallerIdentifier
                        : Subject!.Name;

                    return new FailReason(
                        $"Expected {subjectDescription} to have a property {name} of type {propertyType.Name}{{reason}}, but it does not.");
                })
                .Then
                .ForCondition(propertyInfo.PropertyType == propertyType)
                .FailWith($"Expected property {propertyInfo.Name} to be of type {propertyType}{{reason}}, but it is not.",
                    propertyInfo);
        }

        return new AndWhichConstraint<TypeAssertions, PropertyInfo>(this, propertyInfo);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> has a property of type <typeparamref name="TProperty"/> named
    /// <paramref name="name"/>.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="name">The name of the property.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    public AndWhichConstraint<TypeAssertions, PropertyInfo> HaveProperty<TProperty>(
        string name, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return HaveProperty(typeof(TProperty), name, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not have a property named <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    public AndConstraint<TypeAssertions> NotHaveProperty(string name,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNullOrEmpty(name);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith($"Cannot determine if a type has an unexpected property named {name} if the type is <null>.");

        if (assertionChain.Succeeded)
        {
            PropertyInfo propertyInfo = Subject.FindPropertyByName(name);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(propertyInfo is null)
                .FailWith(() =>
                {
                    var subjectDescription =
                        assertionChain.HasOverriddenCallerIdentifier ? assertionChain.CallerIdentifier : Subject!.Name;

                    return new FailReason(
                        $"Did not expect {subjectDescription} to have a property {propertyInfo?.Name}{{reason}}, but it does.");
                });
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> explicitly implements a property named
    /// <paramref name="name"/> from interface <paramref name="interfaceType" />.
    /// </summary>
    /// <param name="interfaceType">The type of the interface.</param>
    /// <param name="name">The name of the property.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="interfaceType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    public AndConstraint<TypeAssertions> HaveExplicitProperty(
        Type interfaceType, string name,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(interfaceType);
        Guard.ThrowIfArgumentIsNullOrEmpty(name);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                $"Expected {{context:type}} to explicitly implement {interfaceType}.{name}{{reason}}" +
                ", but {context:type} is <null>.");

        if (assertionChain.Succeeded && AssertSubjectImplements(interfaceType, because, becauseArgs))
        {
            var explicitlyImplementsProperty = Subject.HasExplicitlyImplementedProperty(interfaceType, name);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(explicitlyImplementsProperty)
                .FailWith(
                    $"Expected {Subject} to explicitly implement {interfaceType}.{name}{{reason}}, but it does not.");
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> explicitly implements a property named
    /// <paramref name="name"/> from interface <typeparamref name="TInterface"/>.
    /// </summary>
    /// <typeparam name="TInterface">The interface whose member is being explicitly implemented.</typeparam>
    /// <param name="name">The name of the property.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    public AndConstraint<TypeAssertions> HaveExplicitProperty<TInterface>(
        string name,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TInterface : class
    {
        return HaveExplicitProperty(typeof(TInterface), name, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not explicitly implement a property named
    /// <paramref name="name"/> from interface <paramref name="interfaceType" />.
    /// </summary>
    /// <param name="interfaceType">The type of the interface.</param>
    /// <param name="name">The name of the property.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="interfaceType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    public AndConstraint<TypeAssertions> NotHaveExplicitProperty(
        Type interfaceType, string name,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(interfaceType);
        Guard.ThrowIfArgumentIsNullOrEmpty(name);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                $"Expected {{context:type}} to not explicitly implement {interfaceType}.{name}{{reason}}" +
                ", but {context:type} is <null>.");

        if (assertionChain.Succeeded && AssertSubjectImplements(interfaceType, because, becauseArgs))
        {
            var explicitlyImplementsProperty = Subject.HasExplicitlyImplementedProperty(interfaceType, name);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(!explicitlyImplementsProperty)
                .FailWith(
                    $"Expected {Subject} to not explicitly implement {interfaceType}.{name}{{reason}}" +
                    ", but it does.");
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not explicitly implement a property named
    /// <paramref name="name"/> from interface <typeparamref name="TInterface"/>.
    /// </summary>
    /// <typeparam name="TInterface">The interface whose member is not being explicitly implemented.</typeparam>
    /// <param name="name">The name of the property.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    public AndConstraint<TypeAssertions> NotHaveExplicitProperty<TInterface>(
        string name,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TInterface : class
    {
        return NotHaveExplicitProperty(typeof(TInterface), name, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> explicitly implements a method named <paramref name="name"/>
    /// from interface <paramref name="interfaceType" />.
    /// </summary>
    /// <param name="interfaceType">The type of the interface.</param>
    /// <param name="name">The name of the method.</param>
    /// <param name="parameterTypes">The expected types of the method parameters.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="interfaceType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="parameterTypes"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeAssertions> HaveExplicitMethod(
        Type interfaceType, string name, IEnumerable<Type> parameterTypes,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(interfaceType);
        Guard.ThrowIfArgumentIsNullOrEmpty(name);
        Guard.ThrowIfArgumentIsNull(parameterTypes);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                $"Expected {{context:type}} to explicitly implement {interfaceType}.{name}" +
                $"({GetParameterString(parameterTypes)}){{reason}}, but {{context:type}} is <null>.");

        if (assertionChain.Succeeded && AssertSubjectImplements(interfaceType, because, becauseArgs))
        {
            var explicitlyImplementsMethod = Subject.HasMethod($"{interfaceType}.{name}", parameterTypes);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(explicitlyImplementsMethod)
                .FailWith(
                    $"Expected {Subject} to explicitly implement {interfaceType}.{name}" +
                    $"({GetParameterString(parameterTypes)}){{reason}}, but it does not.");
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> explicitly implements a method named <paramref name="name"/>
    /// from interface  <typeparamref name="TInterface"/>.
    /// </summary>
    /// <typeparam name="TInterface">The interface whose member is being explicitly implemented.</typeparam>
    /// <param name="name">The name of the method.</param>
    /// <param name="parameterTypes">The expected types of the method parameters.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="parameterTypes"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeAssertions> HaveExplicitMethod<TInterface>(
        string name, IEnumerable<Type> parameterTypes,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TInterface : class
    {
        return HaveExplicitMethod(typeof(TInterface), name, parameterTypes, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not explicitly implement a method named <paramref name="name"/>
    /// from interface <paramref name="interfaceType" />.
    /// </summary>
    /// <param name="interfaceType">The type of the interface.</param>
    /// <param name="name">The name of the method.</param>
    /// <param name="parameterTypes">The expected types of the method parameters.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="interfaceType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="parameterTypes"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeAssertions> NotHaveExplicitMethod(
        Type interfaceType, string name, IEnumerable<Type> parameterTypes,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(interfaceType);
        Guard.ThrowIfArgumentIsNullOrEmpty(name);
        Guard.ThrowIfArgumentIsNull(parameterTypes);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                $"Expected {{context:type}} to not explicitly implement {interfaceType}.{name}" +
                $"({GetParameterString(parameterTypes)}){{reason}}, but {{context:type}} is <null>.");

        if (assertionChain.Succeeded && AssertSubjectImplements(interfaceType, because, becauseArgs))
        {
            var explicitlyImplementsMethod = Subject.HasMethod($"{interfaceType}.{name}", parameterTypes);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(!explicitlyImplementsMethod)
                .FailWith(
                    $"Expected {Subject} to not explicitly implement {interfaceType}.{name}" +
                    $"({GetParameterString(parameterTypes)}){{reason}}, but it does.");
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not explicitly implement a method named <paramref name="name"/>
    /// from interface <typeparamref name="TInterface"/>.
    /// </summary>
    /// <typeparam name="TInterface">The interface whose member is not being explicitly implemented.</typeparam>
    /// <param name="name">The name of the method.</param>
    /// <param name="parameterTypes">The expected types of the method parameters.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="parameterTypes"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeAssertions> NotHaveExplicitMethod<TInterface>(
        string name, IEnumerable<Type> parameterTypes,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TInterface : class
    {
        return NotHaveExplicitMethod(typeof(TInterface), name, parameterTypes, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> has an indexer of type <paramref name="indexerType"/>.
    /// with parameter types <paramref name="parameterTypes"/>.
    /// </summary>
    /// <param name="indexerType">The type of the indexer.</param>
    /// <param name="parameterTypes">The parameter types for the indexer.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="indexerType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="parameterTypes"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<TypeAssertions, PropertyInfo> HaveIndexer(
        Type indexerType, IEnumerable<Type> parameterTypes,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(indexerType);
        Guard.ThrowIfArgumentIsNull(parameterTypes);

        string parameterString = GetParameterString(parameterTypes);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                $"Expected {indexerType.Name} {{context:type}}[{parameterString}] to exist{{reason}}" +
                ", but {context:type} is <null>.");

        PropertyInfo propertyInfo = null;

        if (assertionChain.Succeeded)
        {
            propertyInfo = Subject.GetIndexerByParameterTypes(parameterTypes);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(propertyInfo is not null)
                .FailWith(
                    $"Expected {indexerType.Name} {Subject}[{parameterString}] to exist{{reason}}" +
                    ", but it does not.")
                .Then
                .ForCondition(propertyInfo.PropertyType == indexerType)
                .FailWith("Expected {0} to be of type {1}{reason}, but it is not.", propertyInfo, indexerType);
        }

        return new AndWhichConstraint<TypeAssertions, PropertyInfo>(this, propertyInfo, assertionChain,
            $"[{parameterString}]");
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not have an indexer that takes parameter types
    /// <paramref name="parameterTypes"/>.
    /// </summary>
    /// <param name="parameterTypes">The expected indexer's parameter types.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="parameterTypes"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeAssertions> NotHaveIndexer(
        IEnumerable<Type> parameterTypes,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(parameterTypes);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                $"Expected indexer {{context:type}}[{GetParameterString(parameterTypes)}] to not exist{{reason}}" +
                ", but {context:type} is <null>.");

        if (assertionChain.Succeeded)
        {
            PropertyInfo propertyInfo = Subject.GetIndexerByParameterTypes(parameterTypes);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(propertyInfo is null)
                .FailWith(
                    $"Expected indexer {Subject}[{GetParameterString(parameterTypes)}] to not exist{{reason}}" +
                    ", but it does.");
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> has a method named <paramref name="name"/> with parameter types
    /// <paramref name="parameterTypes"/>.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="parameterTypes">The parameter types for the indexer.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="parameterTypes"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<TypeAssertions, MethodInfo> HaveMethod(
        string name, IEnumerable<Type> parameterTypes,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNullOrEmpty(name);
        Guard.ThrowIfArgumentIsNull(parameterTypes);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                $"Expected method {{context:type}}.{name}({GetParameterString(parameterTypes)}) to exist{{reason}}" +
                ", but {context:type} is <null>.");

        MethodInfo methodInfo = null;

        if (assertionChain.Succeeded)
        {
            methodInfo = Subject.GetMethod(name, parameterTypes);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(methodInfo is not null)
                .FailWith(
                    $"Expected method {Subject}.{name}({GetParameterString(parameterTypes)}) to exist{{reason}}" +
                    ", but it does not.");
        }

        return new AndWhichConstraint<TypeAssertions, MethodInfo>(this, methodInfo);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not expose a method named <paramref name="name"/>
    /// with parameter types <paramref name="parameterTypes"/>.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="parameterTypes">The method parameter types.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="parameterTypes"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeAssertions> NotHaveMethod(
        string name, IEnumerable<Type> parameterTypes,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNullOrEmpty(name);
        Guard.ThrowIfArgumentIsNull(parameterTypes);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                $"Expected method {{context:type}}.{name}({GetParameterString(parameterTypes)}) to not exist{{reason}}" +
                ", but {context:type} is <null>.");

        if (assertionChain.Succeeded)
        {
            MethodInfo methodInfo = Subject.GetMethod(name, parameterTypes);
            var methodInfoDescription = MethodInfoAssertions.GetDescriptionFor(methodInfo);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(methodInfo is null)
                .FailWith(
                    $"Expected method {methodInfoDescription}({GetParameterString(parameterTypes)}) to not exist{{reason}}" +
                    ", but it does.");
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> has a constructor with <paramref name="parameterTypes"/>.
    /// </summary>
    /// <param name="parameterTypes">The parameter types for the indexer.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="parameterTypes"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<TypeAssertions, ConstructorInfo> HaveConstructor(
        IEnumerable<Type> parameterTypes,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(parameterTypes);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                $"Expected constructor {{context:type}}({GetParameterString(parameterTypes)}) to exist{{reason}}" +
                ", but {context:type} is <null>.");

        ConstructorInfo constructorInfo = null;

        if (assertionChain.Succeeded)
        {
            constructorInfo = Subject.GetConstructor(parameterTypes);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(constructorInfo is not null)
                .FailWith(
                    $"Expected constructor {Subject}({GetParameterString(parameterTypes)}) to exist{{reason}}" +
                    ", but it does not.");
        }

        return new AndWhichConstraint<TypeAssertions, ConstructorInfo>(this, constructorInfo);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> has a default constructor.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndWhichConstraint<TypeAssertions, ConstructorInfo> HaveDefaultConstructor(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return HaveConstructor([], because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not have a constructor with <paramref name="parameterTypes"/>.
    /// </summary>
    /// <param name="parameterTypes">The parameter types for the indexer.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="parameterTypes"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<TypeAssertions, ConstructorInfo> NotHaveConstructor(
        IEnumerable<Type> parameterTypes,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(parameterTypes);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                $"Expected constructor {{context:type}}({GetParameterString(parameterTypes)}) not to exist{{reason}}" +
                ", but {context:type} is <null>.");

        ConstructorInfo constructorInfo = null;

        if (assertionChain.Succeeded)
        {
            constructorInfo = Subject.GetConstructor(parameterTypes);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(constructorInfo is null)
                .FailWith(
                    $"Expected constructor {Subject}({GetParameterString(parameterTypes)}) not to exist{{reason}}" +
                    ", but it does.");
        }

        return new AndWhichConstraint<TypeAssertions, ConstructorInfo>(this, constructorInfo);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not have a default constructor.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndWhichConstraint<TypeAssertions, ConstructorInfo> NotHaveDefaultConstructor(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return NotHaveConstructor([], because, becauseArgs);
    }

    private static string GetParameterString(IEnumerable<Type> parameterTypes)
    {
        return string.Join(", ", parameterTypes.Select(p => p.FullName));
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> has the specified C# <paramref name="accessModifier"/>.
    /// </summary>
    /// <param name="accessModifier">The expected C# access modifier.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="accessModifier"/>
    /// is not a <see cref="CSharpAccessModifier"/> value.</exception>
    public AndConstraint<TypeAssertions> HaveAccessModifier(
        CSharpAccessModifier accessModifier,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsOutOfRange(accessModifier);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith($"Expected {{context:type}} to be {accessModifier}{{reason}}, but {{context:type}} is <null>.");

        if (assertionChain.Succeeded)
        {
            CSharpAccessModifier subjectAccessModifier = Subject.GetCSharpAccessModifier();

            assertionChain.ForCondition(accessModifier == subjectAccessModifier)
                .BecauseOf(because, becauseArgs)
                .ForCondition(accessModifier == subjectAccessModifier)
                .FailWith(
                    $"Expected {{context:type}} {Subject.Name} to be {accessModifier}{{reason}}" +
                    $", but it is {subjectAccessModifier}.");
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not have the specified C# <paramref name="accessModifier"/>.
    /// </summary>
    /// <param name="accessModifier">The unexpected C# access modifier.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="accessModifier"/>
    /// is not a <see cref="CSharpAccessModifier"/> value.</exception>
    public AndConstraint<TypeAssertions> NotHaveAccessModifier(
        CSharpAccessModifier accessModifier,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsOutOfRange(accessModifier);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith($"Expected {{context:type}} not to be {accessModifier}{{reason}}, but {{context:type}} is <null>.");

        if (assertionChain.Succeeded)
        {
            CSharpAccessModifier subjectAccessModifier = Subject.GetCSharpAccessModifier();

            assertionChain
                .ForCondition(accessModifier != subjectAccessModifier)
                .BecauseOf(because, becauseArgs)
                .ForCondition(accessModifier != subjectAccessModifier)
                .FailWith($"Expected {{context:type}} {Subject.Name} not to be {accessModifier}{{reason}}, but it is.");
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> has an implicit conversion operator that converts
    /// <typeparamref name="TSource"/> into <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TSource">The type to convert from.</typeparam>
    /// <typeparam name="TTarget">The type to convert to.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndWhichConstraint<TypeAssertions, MethodInfo> HaveImplicitConversionOperator<TSource, TTarget>(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return HaveImplicitConversionOperator(typeof(TSource), typeof(TTarget), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> has an implicit conversion operator that converts
    /// <paramref name="sourceType"/> into <paramref name="targetType"/>.
    /// </summary>
    /// <param name="sourceType">The type to convert from.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="sourceType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="targetType"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<TypeAssertions, MethodInfo> HaveImplicitConversionOperator(
        Type sourceType, Type targetType,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(sourceType);
        Guard.ThrowIfArgumentIsNull(targetType);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected public static implicit {0}({1}) to exist{reason}, but {context:type} is <null>.",
                targetType, sourceType);

        MethodInfo methodInfo = null;

        if (assertionChain.Succeeded)
        {
            methodInfo = Subject.GetImplicitConversionOperator(sourceType, targetType);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(methodInfo is not null)
                .FailWith("Expected public static implicit {0}({1}) to exist{reason}, but it does not.",
                    targetType, sourceType);
        }

        return new AndWhichConstraint<TypeAssertions, MethodInfo>(this, methodInfo, assertionChain);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not have an implicit conversion operator that converts
    /// <typeparamref name="TSource"/> into <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TSource">The type to convert from.</typeparam>
    /// <typeparam name="TTarget">The type to convert to.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TypeAssertions> NotHaveImplicitConversionOperator<TSource, TTarget>(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return NotHaveImplicitConversionOperator(typeof(TSource), typeof(TTarget), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not have an implicit conversion operator that converts
    /// <paramref name="sourceType"/> into <paramref name="targetType"/>.
    /// </summary>
    /// <param name="sourceType">The type to convert from.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="sourceType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="targetType"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeAssertions> NotHaveImplicitConversionOperator(
        Type sourceType, Type targetType,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(sourceType);
        Guard.ThrowIfArgumentIsNull(targetType);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected public static implicit {0}({1}) to not exist{reason}, but {context:type} is <null>.",
                targetType, sourceType);

        if (assertionChain.Succeeded)
        {
            MethodInfo methodInfo = Subject.GetImplicitConversionOperator(sourceType, targetType);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(methodInfo is null)
                .FailWith("Expected public static implicit {0}({1}) to not exist{reason}, but it does.",
                    targetType, sourceType);
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> has an explicit conversion operator that converts
    /// <typeparamref name="TSource"/> into <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TSource">The type to convert from.</typeparam>
    /// <typeparam name="TTarget">The type to convert to.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndWhichConstraint<TypeAssertions, MethodInfo> HaveExplicitConversionOperator<TSource, TTarget>(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return HaveExplicitConversionOperator(typeof(TSource), typeof(TTarget), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> has an explicit conversion operator that converts
    /// <paramref name="sourceType"/> into <paramref name="targetType"/>.
    /// </summary>
    /// <param name="sourceType">The type to convert from.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="sourceType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="targetType"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<TypeAssertions, MethodInfo> HaveExplicitConversionOperator(
        Type sourceType, Type targetType,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(sourceType);
        Guard.ThrowIfArgumentIsNull(targetType);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected public static explicit {0}({1}) to exist{reason}, but {context:type} is <null>.",
                targetType, sourceType);

        MethodInfo methodInfo = null;

        if (assertionChain.Succeeded)
        {
            methodInfo = Subject.GetExplicitConversionOperator(sourceType, targetType);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(methodInfo is not null)
                .FailWith("Expected public static explicit {0}({1}) to exist{reason}, but it does not.",
                    targetType, sourceType);
        }

        return new AndWhichConstraint<TypeAssertions, MethodInfo>(this, methodInfo);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not have an explicit conversion operator that converts
    /// <typeparamref name="TSource"/> into <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TSource">The type to convert from.</typeparam>
    /// <typeparam name="TTarget">The type to convert to.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TypeAssertions> NotHaveExplicitConversionOperator<TSource, TTarget>(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return NotHaveExplicitConversionOperator(typeof(TSource), typeof(TTarget), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current <see cref="Type"/> does not have an explicit conversion operator that converts
    /// <paramref name="sourceType"/> into <paramref name="targetType"/>.
    /// </summary>
    /// <param name="sourceType">The type to convert from.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="sourceType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="targetType"/> is <see langword="null"/>.</exception>
    public AndConstraint<TypeAssertions> NotHaveExplicitConversionOperator(
        Type sourceType, Type targetType,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(sourceType);
        Guard.ThrowIfArgumentIsNull(targetType);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected public static explicit {0}({1}) to not exist{reason}, but {context:type} is <null>.",
                targetType, sourceType);

        if (assertionChain.Succeeded)
        {
            MethodInfo methodInfo = Subject.GetExplicitConversionOperator(sourceType, targetType);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(methodInfo is null)
                .FailWith("Expected public static explicit {0}({1}) to not exist{reason}, but it does.",
                    targetType, sourceType);
        }

        return new AndConstraint<TypeAssertions>(this);
    }

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => "type";

    private void AssertThatSubjectIsClass()
    {
        if (Subject.IsInterface || Subject.IsValueType || typeof(Delegate).IsAssignableFrom(Subject.BaseType))
        {
            throw new InvalidOperationException($"{Subject} must be a class.");
        }
    }
}
