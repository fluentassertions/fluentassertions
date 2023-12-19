using System;
using System.Diagnostics;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Primitives;

/// <summary>
/// Contains a number of methods to assert that a <see cref="Guid"/> is in the correct state.
/// </summary>
[DebuggerNonUserCode]
public class GuidAssertions : GuidAssertions<GuidAssertions>
{
    public GuidAssertions(Guid? value)
        : base(value)
    {
    }
}

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
/// <summary>
/// Contains a number of methods to assert that a <see cref="Guid"/> is in the correct state.
/// </summary>
[DebuggerNonUserCode]
public class GuidAssertions<TAssertions>
    where TAssertions : GuidAssertions<TAssertions>
{
    public GuidAssertions(Guid? value)
    {
        Subject = value;
    }

    /// <summary>
    /// Gets the object whose value is being asserted.
    /// </summary>
    public Guid? Subject { get; }

    #region BeEmpty / NotBeEmpty

    /// <summary>
    /// Asserts that the <see cref="Guid"/> is <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeEmpty(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject == Guid.Empty)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Guid} to be empty{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the <see cref="Guid"/> is not <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeEmpty(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject is { } value && value != Guid.Empty)
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:Guid} to be empty{reason}.");

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    #endregion

    #region Be / NotBe

    /// <summary>
    /// Asserts that the <see cref="Guid"/> is equal to the <paramref name="expected"/> GUID.
    /// </summary>
    /// <param name="expected">The expected <see cref="string"/> value to compare the actual value with.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentException">The format of <paramref name="expected"/> is invalid.</exception>
    public AndConstraint<TAssertions> Be(string expected, string because = "", params object[] becauseArgs)
    {
        if (!Guid.TryParse(expected, out Guid expectedGuid))
        {
            throw new ArgumentException($"Unable to parse \"{expected}\" as a valid GUID", nameof(expected));
        }

        return Be(expectedGuid, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the <see cref="Guid"/> is equal to the <paramref name="expected"/> GUID.
    /// </summary>
    /// <param name="expected">The expected value to compare the actual value with.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> Be(Guid expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject == expected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Guid} to be {0}{reason}, but found {1}.", expected, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the <see cref="Guid"/> is not equal to the <paramref name="unexpected"/> GUID.
    /// </summary>
    /// <param name="unexpected">The unexpected value to compare the actual value with.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentException">The format of <paramref name="unexpected"/> is invalid.</exception>
    public AndConstraint<TAssertions> NotBe(string unexpected, string because = "", params object[] becauseArgs)
    {
        if (!Guid.TryParse(unexpected, out Guid unexpectedGuid))
        {
            throw new ArgumentException($"Unable to parse \"{unexpected}\" as a valid GUID", nameof(unexpected));
        }

        return NotBe(unexpectedGuid, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the <see cref="Guid"/> is not equal to the <paramref name="unexpected"/> GUID.
    /// </summary>
    /// <param name="unexpected">The unexpected value to compare the actual value with.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBe(Guid unexpected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject != unexpected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:Guid} to be {0}{reason}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    #endregion

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        throw new NotSupportedException("Equals is not part of Fluent Assertions. Did you mean Be() or BeOneOf() instead?");
}
