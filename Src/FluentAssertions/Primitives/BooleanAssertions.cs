using System;
using System.Diagnostics;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Primitives;

/// <summary>
/// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class BooleanAssertions
    : BooleanAssertions<BooleanAssertions>
{
    public BooleanAssertions(bool? value)
        : base(value)
    {
    }
}

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
/// <summary>
/// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class BooleanAssertions<TAssertions>
    where TAssertions : BooleanAssertions<TAssertions>
{
    public BooleanAssertions(bool? value)
    {
        Subject = value;
    }

    /// <summary>
    /// Gets the object whose value is being asserted.
    /// </summary>
    public bool? Subject { get; }

    /// <summary>
    /// Asserts that the value is <see langword="false"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeFalse(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject == false)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:boolean} to be {0}{reason}, but found {1}.", false, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the value is <see langword="true"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeTrue(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject == true)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:boolean} to be {0}{reason}, but found {1}.", true, Subject);

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
    public AndConstraint<TAssertions> Be(bool expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject == expected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:boolean} to be {0}{reason}, but found {1}.", expected, Subject);

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
    public AndConstraint<TAssertions> NotBe(bool unexpected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject != unexpected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:boolean} not to be {0}{reason}, but found {1}.", unexpected, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the value implies the specified <paramref name="consequent"/> value.
    /// </summary>
    /// <param name="consequent">The right hand side of the implication</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> Imply(bool consequent,
        string because = "",
        params object[] becauseArgs)
    {
        bool? antecedent = Subject;

        Execute.Assertion
            .ForCondition(antecedent is not null)
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:antecedent} ({0}) to imply consequent ({1}){reason}, ", antecedent, consequent)
            .FailWith("but found null.")
            .Then
            .ForCondition(!antecedent.Value || consequent)
            .FailWith("but it did not.")
            .Then
            .ClearExpectation();

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        throw new NotSupportedException("Equals is not part of Fluent Assertions. Did you mean Be() instead?");
}
