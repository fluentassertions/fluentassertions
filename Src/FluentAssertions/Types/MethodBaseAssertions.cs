using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Types;

/// <summary>
/// Contains a number of methods to assert that a <see cref="MethodBase"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public abstract class MethodBaseAssertions<TSubject, TAssertions> : MemberInfoAssertions<TSubject, TAssertions>
    where TSubject : MethodBase
    where TAssertions : MethodBaseAssertions<TSubject, TAssertions>
{
    private readonly AssertionChain assertionChain;

    protected MethodBaseAssertions(TSubject subject, AssertionChain assertionChain)
        : base(subject, assertionChain)
    {
        this.assertionChain = assertionChain;
    }

    /// <summary>
    /// Asserts that the selected member has the specified C# <paramref name="accessModifier"/>.
    /// </summary>
    /// <param name="accessModifier">The expected C# access modifier.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="accessModifier"/>
    /// is not a <see cref="CSharpAccessModifier"/> value.</exception>
    public AndConstraint<TAssertions> HaveAccessModifier(
        CSharpAccessModifier accessModifier,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsOutOfRange(accessModifier);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith($"Expected method to be {accessModifier}{{reason}}, but {{context:method}} is <null>.");

        if (assertionChain.Succeeded)
        {
            CSharpAccessModifier subjectAccessModifier = Subject.GetCSharpAccessModifier();

            assertionChain
                .ForCondition(accessModifier == subjectAccessModifier)
                .BecauseOf(because, becauseArgs)
                .FailWith(() =>
                {
                    var subject = assertionChain.HasOverriddenCallerIdentifier
                        ? assertionChain.CallerIdentifier
                        : "method " + Subject.ToFormattedString();

                    return new FailReason(
                        $"Expected {subject} to be {accessModifier}{{reason}}, but it is {subjectAccessModifier}.");
                });
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the selected member does not have the specified C# <paramref name="accessModifier"/>.
    /// </summary>
    /// <param name="accessModifier">The unexpected C# access modifier.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="accessModifier"/>
    /// is not a <see cref="CSharpAccessModifier"/> value.</exception>
    public AndConstraint<TAssertions> NotHaveAccessModifier(CSharpAccessModifier accessModifier,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsOutOfRange(accessModifier);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith($"Expected method not to be {accessModifier}{{reason}}, but {{context:member}} is <null>.");

        if (assertionChain.Succeeded)
        {
            CSharpAccessModifier subjectAccessModifier = Subject.GetCSharpAccessModifier();

            assertionChain
                .ForCondition(accessModifier != subjectAccessModifier)
                .BecauseOf(because, becauseArgs)
                .FailWith(() =>
                {
                    var subject = assertionChain.HasOverriddenCallerIdentifier
                        ? assertionChain.CallerIdentifier
                        : "method " + Subject.ToFormattedString();

                    return new FailReason($"Expected {subject} not to be {accessModifier}{{reason}}, but it is.");
                });
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    internal static string GetParameterString(MethodBase methodBase)
    {
        IEnumerable<Type> parameterTypes = methodBase.GetParameters().Select(p => p.ParameterType);

        return string.Join(", ", parameterTypes.Select(p => p.FullName));
    }
}
