using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions.Types;

/// <summary>
/// Contains a number of methods to assert that a <see cref="PropertyInfo"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class PropertyInfoAssertions : MemberInfoAssertions<PropertyInfo, PropertyInfoAssertions>
{
    private readonly AssertionChain assertionChain;

    public PropertyInfoAssertions(PropertyInfo propertyInfo, AssertionChain assertionChain)
        : base(propertyInfo, assertionChain)
    {
        this.assertionChain = assertionChain;
    }

    /// <summary>
    /// Asserts that the selected property is virtual.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<PropertyInfoAssertions> BeVirtual(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected property to be virtual{reason}, but {context:property} is <null>.")
            .Then
            .ForCondition(Subject.IsVirtual())
            .BecauseOf(because, becauseArgs)
            .FailWith(() =>
            {
                var subjectDescription = assertionChain.HasOverriddenCallerIdentifier
                    ? assertionChain.CallerIdentifier
                    : "property " + Subject.ToFormattedString();

                return new FailReason($"Expected {subjectDescription} to be virtual{{reason}}, but it is not.");
            });

        return new AndConstraint<PropertyInfoAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected property is not virtual.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<PropertyInfoAssertions> NotBeVirtual([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected property not to be virtual{reason}, but {context:property} is <null>.")
            .Then
            .ForCondition(!Subject.IsVirtual())
            .BecauseOf(because, becauseArgs)
            .FailWith(() =>
            {
                var subjectDescription = assertionChain.HasOverriddenCallerIdentifier
                    ? assertionChain.CallerIdentifier
                    : "property " + Subject.ToFormattedString();

                return new FailReason($"Expected property {subjectDescription} not to be virtual{{reason}}, but it is.");
            });

        return new AndConstraint<PropertyInfoAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected property has a setter.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<PropertyInfoAssertions> BeWritable(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected property to have a setter{reason}, but {context:property} is <null>.")
            .Then
            .ForCondition(Subject!.CanWrite)
            .BecauseOf(because, becauseArgs)
            .FailWith(() =>
            {
                var subjectDescription = assertionChain.HasOverriddenCallerIdentifier
                    ? assertionChain.CallerIdentifier
                    : "property " + Subject.ToFormattedString();

                return new FailReason($"Expected {subjectDescription} to have a setter{{reason}}.");
            });

        return new AndConstraint<PropertyInfoAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected property has a setter with the specified C# access modifier.
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
    public AndConstraint<PropertyInfoAssertions> BeWritable(CSharpAccessModifier accessModifier,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsOutOfRange(accessModifier);

        var subjectDescription = assertionChain.HasOverriddenCallerIdentifier
            ? assertionChain.CallerIdentifier
            : "property " + Subject.ToFormattedString();

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith($"Expected {{context:project}} to be {accessModifier}{{reason}}, but it is <null>.")
            .Then
            .ForCondition(Subject!.CanWrite)
            .BecauseOf(because, becauseArgs)
            .FailWith($"Expected {subjectDescription} to have a setter{{reason}}.");

        if (assertionChain.Succeeded)
        {
            assertionChain.OverrideCallerIdentifier(() => "setter of " + subjectDescription);
            assertionChain.ReuseOnce();

            Subject!.GetSetMethod(nonPublic: true).Should().HaveAccessModifier(accessModifier, because, becauseArgs);
        }

        return new AndConstraint<PropertyInfoAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected property does not have a setter.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<PropertyInfoAssertions> NotBeWritable(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:property} not to have a setter{reason}, but it is <null>.")
            .Then
            .ForCondition(!Subject!.CanWrite)
            .BecauseOf(because, becauseArgs)
            .FailWith(() =>
            {
                var subjectDescription = assertionChain.HasOverriddenCallerIdentifier
                    ? assertionChain.CallerIdentifier
                    : "property " + Subject.ToFormattedString();

                return new FailReason($"Did not expect {subjectDescription} to have a setter{{reason}}.");
            });

        return new AndConstraint<PropertyInfoAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected property has a getter.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<PropertyInfoAssertions> BeReadable([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected property to have a getter{reason}, but {context:property} is <null>.")
            .Then
            .ForCondition(Subject!.CanRead)
            .BecauseOf(because, becauseArgs)
            .FailWith(() =>
            {
                var subjectDescription = assertionChain.HasOverriddenCallerIdentifier
                    ? assertionChain.CallerIdentifier
                    : "property " + Subject.ToFormattedString();

                return new FailReason($"Expected property {subjectDescription} to have a getter{{reason}}, but it does not.");
            });

        return new AndConstraint<PropertyInfoAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected property has a getter with the specified C# access modifier.
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
    public AndConstraint<PropertyInfoAssertions> BeReadable(CSharpAccessModifier accessModifier,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsOutOfRange(accessModifier);

        var subjectDescription = assertionChain.HasOverriddenCallerIdentifier
            ? assertionChain.CallerIdentifier
            : "property " + Subject.ToFormattedString();

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith($"Expected {{context:property}} to be {accessModifier}{{reason}}, but it is <null>.")
            .Then
            .ForCondition(Subject!.CanRead)
            .BecauseOf(because, becauseArgs)
            .FailWith($"Expected {subjectDescription} to have a getter{{reason}}, but it does not.");

        if (assertionChain.Succeeded)
        {
            assertionChain.OverrideCallerIdentifier(() => "getter of " + subjectDescription);
            assertionChain.ReuseOnce();

            Subject!.GetGetMethod(nonPublic: true).Should().HaveAccessModifier(accessModifier, because, becauseArgs);
        }

        return new AndConstraint<PropertyInfoAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected property does not have a getter.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<PropertyInfoAssertions> NotBeReadable(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected property not to have a getter{reason}, but {context:property} is <null>.")
            .Then
            .ForCondition(!Subject!.CanRead)
            .BecauseOf(because, becauseArgs)
            .FailWith(() =>
            {
                var subjectDescription = assertionChain.HasOverriddenCallerIdentifier
                    ? assertionChain.CallerIdentifier
                    : "property " + Subject.ToFormattedString();

                return new FailReason($"Did not expect {subjectDescription} to have a getter{{reason}}.");
            });

        return new AndConstraint<PropertyInfoAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected property returns a specified type.
    /// </summary>
    /// <param name="propertyType">The expected type of the property.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="propertyType"/> is <see langword="null"/>.</exception>
    public AndConstraint<PropertyInfoAssertions> Return(Type propertyType,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(propertyType);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected type of property to be {0}{reason}, but {context:property} is <null>.", propertyType)
            .Then.ForCondition(Subject!.PropertyType == propertyType)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected type of property {2} to be {0}{reason}, but it is {1}.",
                propertyType, Subject.PropertyType, Subject);

        return new AndConstraint<PropertyInfoAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected property returns <typeparamref name="TReturn"/>.
    /// </summary>
    /// <typeparam name="TReturn">The expected return type.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<PropertyInfoAssertions> Return<TReturn>([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return Return(typeof(TReturn), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the selected property does not return a specified type.
    /// </summary>
    /// <param name="propertyType">The unexpected type of the property.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="propertyType"/> is <see langword="null"/>.</exception>
    public AndConstraint<PropertyInfoAssertions> NotReturn(Type propertyType,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(propertyType);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected type of property not to be {0}{reason}, but {context:property} is <null>.", propertyType)
            .Then
            .ForCondition(Subject!.PropertyType != propertyType)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected type of property {1} not to be {0}{reason}, but it is.", propertyType, Subject);

        return new AndConstraint<PropertyInfoAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected property does not return <typeparamref name="TReturn"/>.
    /// </summary>
    /// <typeparam name="TReturn">The unexpected return type.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<PropertyInfoAssertions> NotReturn<TReturn>([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return NotReturn(typeof(TReturn), because, becauseArgs);
    }

    private protected override string SubjectDescription => Formatter.ToString(Subject);

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => "property";
}

