#if NET6_0_OR_GREATER

using System;
using System.Diagnostics;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Primitives;

/// <summary>
/// Contains a number of methods to assert that a nullable <see cref="DateOnly"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class NullableDateOnlyAssertions : NullableDateOnlyAssertions<NullableDateOnlyAssertions>
{
    public NullableDateOnlyAssertions(DateOnly? value)
        : base(value)
    {
    }
}

/// <summary>
/// Contains a number of methods to assert that a nullable <see cref="DateOnly"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class NullableDateOnlyAssertions<TAssertions> : DateOnlyAssertions<TAssertions>
    where TAssertions : NullableDateOnlyAssertions<TAssertions>
{
    public NullableDateOnlyAssertions(DateOnly? value)
        : base(value)
    {
    }

    /// <summary>
    /// Asserts that a nullable <see cref="DateOnly"/> value is not <see langword="null"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveValue(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject.HasValue)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:nullable date} to have a value{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a nullable <see cref="DateOnly"/> value is not <see langword="null"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeNull(string because = "", params object[] becauseArgs)
    {
        return HaveValue(because, becauseArgs);
    }

    /// <summary>
    /// Asserts that a nullable <see cref="DateOnly"/> value is <see langword="null"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHaveValue(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(!Subject.HasValue)
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:nullable date} to have a value{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a nullable <see cref="DateOnly"/> value is <see langword="null"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeNull(string because = "", params object[] becauseArgs)
    {
        return NotHaveValue(because, becauseArgs);
    }
}

#endif
