using System;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Primitives;

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
/// <summary>
/// Contains a number of methods to assert that a reference type object is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public abstract class ReferenceTypeAssertions<TSubject, TAssertions>
    where TAssertions : ReferenceTypeAssertions<TSubject, TAssertions>
{
    protected ReferenceTypeAssertions(TSubject subject)
    {
        Subject = subject;
    }

    /// <summary>
    /// Gets the object whose value is being asserted.
    /// </summary>
    public TSubject Subject { get; }

    /// <summary>
    /// Asserts that the current object has not been initialized yet.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeNull(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject is null)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Expected {context} to be <null>{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current object has been initialized.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeNull(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Expected {context} not to be <null>{reason}.");

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that an object reference refers to the exact same object as another object reference.
    /// </summary>
    /// <param name="expected">The expected object</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> BeSameAs(TSubject expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .UsingLineBreaks
            .ForCondition(ReferenceEquals(Subject, expected))
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Expected {context} to refer to {0}{reason}, but found {1}.", expected, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that an object reference refers to a different object than another object reference refers to.
    /// </summary>
    /// <param name="unexpected">The unexpected object</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> NotBeSameAs(TSubject unexpected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .UsingLineBreaks
            .ForCondition(!ReferenceEquals(Subject, unexpected))
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Did not expect {context} to refer to {0}{reason}.", unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the object is of the specified type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The expected type of the object.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<TAssertions, T> BeOfType<T>(string because = "", params object[] becauseArgs)
    {
        BeOfType(typeof(T), because, becauseArgs);

        T typedSubject = Subject is T type
            ? type
            : default;

        return new AndWhichConstraint<TAssertions, T>((TAssertions)this, typedSubject);
    }

    /// <summary>
    /// Asserts that the object is of the <paramref name="expectedType"/>.
    /// </summary>
    /// <param name="expectedType">
    /// The type that the subject is supposed to be.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expectedType"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> BeOfType(Type expectedType, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectedType);

        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier("type")
            .FailWith("Expected {context} to be {0}{reason}, but found <null>.", expectedType);

        if (success)
        {
            Type subjectType = Subject.GetType();

            if (expectedType.IsGenericTypeDefinition && subjectType.IsGenericType)
            {
                subjectType.GetGenericTypeDefinition().Should().Be(expectedType, because, becauseArgs);
            }
            else
            {
                subjectType.Should().Be(expectedType, because, becauseArgs);
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the object is not of the specified type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type that the subject is not supposed to be.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeOfType<T>(string because = "", params object[] becauseArgs)
    {
        NotBeOfType(typeof(T), because, becauseArgs);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the object is not the <paramref name="unexpectedType"/>.
    /// </summary>
    /// <param name="unexpectedType">
    /// The type that the subject is not supposed to be.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpectedType"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotBeOfType(Type unexpectedType, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpectedType);

        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier("type")
            .FailWith("Expected {context} not to be {0}{reason}, but found <null>.", unexpectedType);

        if (success)
        {
            Type subjectType = Subject.GetType();

            if (unexpectedType.IsGenericTypeDefinition && subjectType.IsGenericType)
            {
                subjectType.GetGenericTypeDefinition().Should().NotBe(unexpectedType, because, becauseArgs);
            }
            else
            {
                subjectType.Should().NotBe(unexpectedType, because, becauseArgs);
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the object is assignable to a variable of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to which the object should be assignable to.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <returns>An <see cref="AndWhichConstraint{TAssertions, T}"/> which can be used to chain assertions.</returns>
    public AndWhichConstraint<TAssertions, T> BeAssignableTo<T>(string because = "", params object[] becauseArgs)
    {
        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier("type")
            .FailWith("Expected {context} to be assignable to {0}{reason}, but found <null>.", typeof(T));

        if (success)
        {
            Execute.Assertion
                .ForCondition(Subject is T)
                .BecauseOf(because, becauseArgs)
                .WithDefaultIdentifier(Identifier)
                .FailWith("Expected {context} to be assignable to {0}{reason}, but {1} is not.", typeof(T), Subject.GetType());
        }

        T typedSubject = Subject is T type
            ? type
            : default;

        return new AndWhichConstraint<TAssertions, T>((TAssertions)this, typedSubject);
    }

    /// <summary>
    /// Asserts that the object is assignable to a variable of given <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type to which the object should be assignable to.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <returns>An <see cref="AndConstraint{TAssertions}"/> which can be used to chain assertions.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> BeAssignableTo(Type type, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(type);

        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier("type")
            .FailWith("Expected {context} to be assignable to {0}{reason}, but found <null>.", type);

        if (success)
        {
            bool isAssignable = type.IsGenericTypeDefinition
                ? Subject.GetType().IsAssignableToOpenGeneric(type)
                : type.IsAssignableFrom(Subject.GetType());

            Execute.Assertion
                .ForCondition(isAssignable)
                .BecauseOf(because, becauseArgs)
                .WithDefaultIdentifier(Identifier)
                .FailWith("Expected {context} to be assignable to {0}{reason}, but {1} is not.",
                    type,
                    Subject.GetType());
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the object is not assignable to a variable of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to which the object should not be assignable to.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <returns>An <see cref="AndConstraint{TAssertions}"/> which can be used to chain assertions.</returns>
    public AndConstraint<TAssertions> NotBeAssignableTo<T>(string because = "", params object[] becauseArgs)
    {
        return NotBeAssignableTo(typeof(T), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the object is not assignable to a variable of given <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type to which the object should not be assignable to.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <returns>An <see cref="AndConstraint{TAssertions}"/> which can be used to chain assertions.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotBeAssignableTo(Type type, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(type);

        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier("type")
            .FailWith("Expected {context} to not be assignable to {0}{reason}, but found <null>.", type);

        if (success)
        {
            bool isAssignable = type.IsGenericTypeDefinition
                ? Subject.GetType().IsAssignableToOpenGeneric(type)
                : type.IsAssignableFrom(Subject.GetType());

            Execute.Assertion
                .ForCondition(!isAssignable)
                .BecauseOf(because, becauseArgs)
                .WithDefaultIdentifier(Identifier)
                .FailWith("Expected {context} to not be assignable to {0}{reason}, but {1} is.", type, Subject.GetType());
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the <paramref name="predicate" /> is satisfied.
    /// </summary>
    /// <param name="predicate">The predicate which must be satisfied by the <typeparamref name="TSubject" />.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <returns>An <see cref="AndConstraint{T}" /> which can be used to chain assertions.</returns>
    public AndConstraint<TAssertions> Match(Expression<Func<TSubject, bool>> predicate,
        string because = "",
        params object[] becauseArgs)
    {
        return Match<TSubject>(predicate, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the <paramref name="predicate" /> is satisfied.
    /// </summary>
    /// <param name="predicate">The predicate which must be satisfied by the <typeparamref name="TSubject" />.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <returns>An <see cref="AndConstraint{T}" /> which can be used to chain assertions.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> Match<T>(Expression<Func<T, bool>> predicate,
        string because = "",
        params object[] becauseArgs)
        where T : TSubject
    {
        Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate), "Cannot match an object against a <null> predicate.");

        Execute.Assertion
            .ForCondition(predicate.Compile()((T)Subject))
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Expected {context:object} to match {1}{reason}, but found {0}.", Subject, predicate);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// It should be a user-friendly name as it is included in the failure message.
    /// </summary>
    protected abstract string Identifier { get; }

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        throw new NotSupportedException("Equals is not part of Fluent Assertions. Did you mean BeSameAs() instead?");
}
