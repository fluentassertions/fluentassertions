using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Types;

/// <summary>
/// Contains a number of methods to assert that a <see cref="MethodInfo"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class MethodInfoAssertions : MethodBaseAssertions<MethodInfo, MethodInfoAssertions>
{
    private readonly AssertionChain assertionChain;

    public MethodInfoAssertions(MethodInfo methodInfo, AssertionChain assertionChain)
        : base(methodInfo, assertionChain)
    {
        this.assertionChain = assertionChain;
    }

    /// <summary>
    /// Asserts that the selected method is virtual.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<MethodInfoAssertions> BeVirtual([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected method to be virtual{reason}, but {context:member} is <null>.");

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(!Subject.IsNonVirtual())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected method " + SubjectDescription + " to be virtual{reason}, but it is not virtual.");
        }

        return new AndConstraint<MethodInfoAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected method is not virtual.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<MethodInfoAssertions> NotBeVirtual([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected method not to be virtual{reason}, but {context:member} is <null>.");

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(Subject.IsNonVirtual())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected method " + SubjectDescription + " not to be virtual{reason}, but it is.");
        }

        return new AndConstraint<MethodInfoAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected method is async.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<MethodInfoAssertions> BeAsync([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected method to be async{reason}, but {context:member} is <null>.");

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(Subject.IsAsync())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected method " + SubjectDescription + " to be async{reason}, but it is not.");
        }

        return new AndConstraint<MethodInfoAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected method is not async.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<MethodInfoAssertions> NotBeAsync([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected method not to be async{reason}, but {context:member} is <null>.");

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(!Subject.IsAsync())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected method " + SubjectDescription + " not to be async{reason}, but it is.");
        }

        return new AndConstraint<MethodInfoAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected method returns void.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>> ReturnVoid(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected the return type of method to be void{reason}, but {context:member} is <null>.");

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(typeof(void) == Subject!.ReturnType)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected the return type of method " + Subject.Name + " to be void{reason}, but it is {0}.",
                    Subject.ReturnType.FullName);
        }

        return new AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>>(this);
    }

    /// <summary>
    /// Asserts that the selected method returns <paramref name="returnType"/>.
    /// </summary>
    /// <param name="returnType">The expected return type.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="returnType"/> is <see langword="null"/>.</exception>
    public AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>> Return(Type returnType,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(returnType);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected the return type of method to be {0}{reason}, but {context:member} is <null>.", returnType);

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(returnType == Subject!.ReturnType)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected the return type of method " + Subject.Name + " to be {0}{reason}, but it is {1}.",
                    returnType, Subject.ReturnType.FullName);
        }

        return new AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>>(this);
    }

    /// <summary>
    /// Asserts that the selected method returns <typeparamref name="TReturn"/>.
    /// </summary>
    /// <typeparam name="TReturn">The expected return type.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>> Return<TReturn>(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return Return(typeof(TReturn), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the selected method does not return void.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>> NotReturnVoid(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected the return type of method not to be void{reason}, but {context:member} is <null>.");

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(typeof(void) != Subject!.ReturnType)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected the return type of method " + Subject.Name + " not to be void{reason}, but it is.");
        }

        return new AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>>(this);
    }

    /// <summary>
    /// Asserts that the selected method does not return <paramref name="returnType"/>.
    /// </summary>
    /// <param name="returnType">The unexpected return type.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="returnType"/> is <see langword="null"/>.</exception>
    public AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>> NotReturn(Type returnType,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(returnType);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                "Expected the return type of method not to be {0}{reason}, but {context:member} is <null>.", returnType);

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(returnType != Subject!.ReturnType)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected the return type of method " + Subject.Name + " not to be {0}{reason}, but it is.", returnType);
        }

        return new AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>>(this);
    }

    /// <summary>
    /// Asserts that the selected method does not return <typeparamref name="TReturn"/>.
    /// </summary>
    /// <typeparam name="TReturn">The unexpected return type.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>> NotReturn<TReturn>(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return NotReturn(typeof(TReturn), because, becauseArgs);
    }

    internal static string GetDescriptionFor(MethodInfo method)
    {
        if (method is null)
        {
            return string.Empty;
        }

        var returnTypeName = method.ReturnType.Name;

        return $"{returnTypeName} {method.DeclaringType}.{method.Name}";
    }

    private protected override string SubjectDescription => GetDescriptionFor(Subject);

    protected override string Identifier => "method";
}
