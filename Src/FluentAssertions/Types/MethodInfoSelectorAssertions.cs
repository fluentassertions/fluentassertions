using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Types;

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
/// <summary>
/// Contains assertions for the <see cref="MethodInfo"/> objects returned by the parent <see cref="MethodInfoSelector"/>.
/// </summary>
[DebuggerNonUserCode]
public class MethodInfoSelectorAssertions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodInfoSelectorAssertions"/> class.
    /// </summary>
    /// <param name="methods">The methods to assert.</param>
    /// <exception cref="ArgumentNullException"><paramref name="methods"/> is <see langword="null"/>.</exception>
    public MethodInfoSelectorAssertions(params MethodInfo[] methods)
    {
        Guard.ThrowIfArgumentIsNull(methods);

        SubjectMethods = methods;
    }

    /// <summary>
    /// Gets the object whose value is being asserted.
    /// </summary>
    public IEnumerable<MethodInfo> SubjectMethods { get; }

    /// <summary>
    /// Asserts that the selected methods are virtual.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<MethodInfoSelectorAssertions> BeVirtual(string because = "", params object[] becauseArgs)
    {
        MethodInfo[] nonVirtualMethods = GetAllNonVirtualMethodsFromSelection();

        string failureMessage =
            "Expected all selected methods to be virtual{reason}, but the following methods are not virtual:" +
            Environment.NewLine +
            GetDescriptionsFor(nonVirtualMethods);

        Execute.Assertion
            .ForCondition(nonVirtualMethods.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(failureMessage);

        return new AndConstraint<MethodInfoSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected methods are not virtual.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<MethodInfoSelectorAssertions> NotBeVirtual(string because = "", params object[] becauseArgs)
    {
        MethodInfo[] virtualMethods = GetAllVirtualMethodsFromSelection();

        string failureMessage =
            "Expected all selected methods not to be virtual{reason}, but the following methods are virtual:" +
            Environment.NewLine +
            GetDescriptionsFor(virtualMethods);

        Execute.Assertion
            .ForCondition(virtualMethods.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(failureMessage);

        return new AndConstraint<MethodInfoSelectorAssertions>(this);
    }

    private MethodInfo[] GetAllNonVirtualMethodsFromSelection()
    {
        return SubjectMethods.Where(method => method.IsNonVirtual()).ToArray();
    }

    private MethodInfo[] GetAllVirtualMethodsFromSelection()
    {
        return SubjectMethods.Where(method => !method.IsNonVirtual()).ToArray();
    }

    /// <summary>
    /// Asserts that the selected methods are async.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<MethodInfoSelectorAssertions> BeAsync(string because = "", params object[] becauseArgs)
    {
        MethodInfo[] nonAsyncMethods = SubjectMethods.Where(method => !method.IsAsync()).ToArray();

        string failureMessage =
            "Expected all selected methods to be async{reason}, but the following methods are not:" +
            Environment.NewLine +
            GetDescriptionsFor(nonAsyncMethods);

        Execute.Assertion
            .ForCondition(nonAsyncMethods.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(failureMessage);

        return new AndConstraint<MethodInfoSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected methods are not async.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<MethodInfoSelectorAssertions> NotBeAsync(string because = "", params object[] becauseArgs)
    {
        MethodInfo[] asyncMethods = SubjectMethods.Where(method => method.IsAsync()).ToArray();

        string failureMessage =
            "Expected all selected methods not to be async{reason}, but the following methods are:" +
            Environment.NewLine +
            GetDescriptionsFor(asyncMethods);

        Execute.Assertion
            .ForCondition(asyncMethods.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(failureMessage);

        return new AndConstraint<MethodInfoSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected methods are decorated with the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<MethodInfoSelectorAssertions> BeDecoratedWith<TAttribute>(string because = "",
        params object[] becauseArgs)
        where TAttribute : Attribute
    {
        return BeDecoratedWith<TAttribute>(_ => true, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the selected methods are decorated with an attribute of type <typeparamref name="TAttribute"/>
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
    public AndConstraint<MethodInfoSelectorAssertions> BeDecoratedWith<TAttribute>(
        Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate);

        MethodInfo[] methodsWithoutAttribute = GetMethodsWithout(isMatchingAttributePredicate);

        string failureMessage =
            "Expected all selected methods to be decorated with {0}{reason}, but the following methods are not:" +
            Environment.NewLine +
            GetDescriptionsFor(methodsWithoutAttribute);

        Execute.Assertion
            .ForCondition(methodsWithoutAttribute.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(failureMessage, typeof(TAttribute));

        return new AndConstraint<MethodInfoSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected methods are not decorated with the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<MethodInfoSelectorAssertions> NotBeDecoratedWith<TAttribute>(string because = "",
        params object[] becauseArgs)
        where TAttribute : Attribute
    {
        return NotBeDecoratedWith<TAttribute>(_ => true, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the selected methods are not decorated with an attribute of type <typeparamref name="TAttribute"/>
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
    public AndConstraint<MethodInfoSelectorAssertions> NotBeDecoratedWith<TAttribute>(
        Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string because = "", params object[] becauseArgs)
        where TAttribute : Attribute
    {
        Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate);

        MethodInfo[] methodsWithAttribute = GetMethodsWith(isMatchingAttributePredicate);

        string failureMessage =
            "Expected all selected methods to not be decorated with {0}{reason}, but the following methods are:" +
            Environment.NewLine +
            GetDescriptionsFor(methodsWithAttribute);

        Execute.Assertion
            .ForCondition(methodsWithAttribute.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(failureMessage, typeof(TAttribute));

        return new AndConstraint<MethodInfoSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected methods have specified <paramref name="accessModifier"/>.
    /// </summary>
    /// <param name="accessModifier">The expected access modifier.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<MethodInfoSelectorAssertions> Be(CSharpAccessModifier accessModifier, string because = "",
        params object[] becauseArgs)
    {
        var methods = SubjectMethods.Where(pi => pi.GetCSharpAccessModifier() != accessModifier).ToArray();

        var message = $"Expected all selected methods to be {accessModifier}{{reason}}, but the following methods are not:" +
            Environment.NewLine + GetDescriptionsFor(methods);

        Execute.Assertion
            .ForCondition(methods.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(message);

        return new AndConstraint<MethodInfoSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected methods don't have specified <paramref name="accessModifier"/>
    /// </summary>
    /// <param name="accessModifier">The expected access modifier.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<MethodInfoSelectorAssertions> NotBe(CSharpAccessModifier accessModifier, string because = "",
        params object[] becauseArgs)
    {
        var methods = SubjectMethods.Where(pi => pi.GetCSharpAccessModifier() == accessModifier).ToArray();

        var message = $"Expected all selected methods to not be {accessModifier}{{reason}}, but the following methods are:" +
            Environment.NewLine + GetDescriptionsFor(methods);

        Execute.Assertion
            .ForCondition(methods.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(message);

        return new AndConstraint<MethodInfoSelectorAssertions>(this);
    }

    private MethodInfo[] GetMethodsWithout<TAttribute>(Expression<Func<TAttribute, bool>> isMatchingPredicate)
        where TAttribute : Attribute
    {
        return SubjectMethods.Where(method => !method.IsDecoratedWith(isMatchingPredicate)).ToArray();
    }

    private MethodInfo[] GetMethodsWith<TAttribute>(Expression<Func<TAttribute, bool>> isMatchingPredicate)
        where TAttribute : Attribute
    {
        return SubjectMethods.Where(method => method.IsDecoratedWith(isMatchingPredicate)).ToArray();
    }

    private static string GetDescriptionsFor(IEnumerable<MethodInfo> methods)
    {
        IEnumerable<string> descriptions = methods.Select(method => MethodInfoAssertions.GetDescriptionFor(method));

        return string.Join(Environment.NewLine, descriptions);
    }

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
#pragma warning disable CA1822 // Do not change signature of a public member
    protected string Context => "method";
#pragma warning restore CA1822

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        throw new NotSupportedException("Equals is not part of Fluent Assertions. Did you mean Be() instead?");
}
