using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Equivalency;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Primitives;

/// <summary>
/// Contains a number of methods to assert that an <see cref="object"/> is in the expected state.
/// </summary>
public class ObjectAssertions : ObjectAssertions<object, ObjectAssertions>
{
    public ObjectAssertions(object value)
        : base(value)
    {
    }

    /// <summary>
    /// Asserts that a value equals <paramref name="expected"/> using the provided <paramref name="comparer"/>.
    /// </summary>
    /// <param name="expected">The expected value</param>
    /// <param name="comparer">
    /// An equality comparer to compare values.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public AndConstraint<ObjectAssertions> Be<TExpectation>(TExpectation expected, IEqualityComparer<TExpectation> comparer, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer);

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is TExpectation subject && comparer.Equals(subject, expected))
            .WithDefaultIdentifier(Identifier)
            .FailWith("Expected {context} to be {0}{reason}, but found {1}.", expected,
                Subject);

        return new AndConstraint<ObjectAssertions>(this);
    }

    /// <summary>
    /// Asserts that a value does not equal <paramref name="unexpected"/> using the provided <paramref name="comparer"/>.
    /// </summary>
    /// <param name="unexpected">The unexpected value</param>
    /// <param name="comparer">
    /// An equality comparer to compare values.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public AndConstraint<ObjectAssertions> NotBe<TExpectation>(TExpectation unexpected, IEqualityComparer<TExpectation> comparer, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer);

        Execute.Assertion
            .ForCondition(Subject is not TExpectation subject || !comparer.Equals(subject, unexpected))
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Did not expect {context} to be equal to {0}{reason}.", unexpected);

        return new AndConstraint<ObjectAssertions>(this);
    }

    /// <summary>
    /// Asserts that a value is one of the specified <paramref name="validValues"/> using the provided <paramref name="comparer"/>.
    /// </summary>
    /// <param name="validValues">
    /// The values that are valid.
    /// </param>
    /// <param name="comparer">
    /// An equality comparer to compare values.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="validValues"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public AndConstraint<ObjectAssertions> BeOneOf<TExpectation>(IEnumerable<TExpectation> validValues,
        IEqualityComparer<TExpectation> comparer,
        string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(validValues);
        Guard.ThrowIfArgumentIsNull(comparer);

        Execute.Assertion
            .ForCondition(Subject is TExpectation subject && validValues.Contains(subject, comparer))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:object} to be one of {0}{reason}, but found {1}.", validValues, Subject);

        return new AndConstraint<ObjectAssertions>(this);
    }
}

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
/// <summary>
/// Contains a number of methods to assert that a <typeparamref name="TSubject"/> is in the expected state.
/// </summary>
public class ObjectAssertions<TSubject, TAssertions> : ReferenceTypeAssertions<TSubject, TAssertions>
    where TAssertions : ObjectAssertions<TSubject, TAssertions>
{
    public ObjectAssertions(TSubject value)
        : base(value)
    {
    }

    /// <summary>
    /// Asserts that a value equals <paramref name="expected"/> using its <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="expected">The expected value</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> Be(TSubject expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(ObjectExtensions.GetComparer<TSubject>()(Subject, expected))
            .WithDefaultIdentifier(Identifier)
            .FailWith("Expected {context} to be {0}{reason}, but found {1}.", expected,
                Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a value equals <paramref name="expected"/> using the provided <paramref name="comparer"/>.
    /// </summary>
    /// <param name="expected">The expected value</param>
    /// <param name="comparer">
    /// An equality comparer to compare values.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> Be(TSubject expected, IEqualityComparer<TSubject> comparer, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer);

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(comparer.Equals(Subject, expected))
            .WithDefaultIdentifier(Identifier)
            .FailWith("Expected {context} to be {0}{reason}, but found {1}.", expected,
                Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a value does not equal <paramref name="unexpected"/> using its <see cref="object.Equals(object)" /> method.
    /// </summary>
    /// <param name="unexpected">The unexpected value</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> NotBe(TSubject unexpected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(!ObjectExtensions.GetComparer<TSubject>()(Subject, unexpected))
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Did not expect {context} to be equal to {0}{reason}.", unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a value does not equal <paramref name="unexpected"/> using the provided <paramref name="comparer"/>.
    /// </summary>
    /// <param name="unexpected">The unexpected value</param>
    /// <param name="comparer">
    /// An equality comparer to compare values.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotBe(TSubject unexpected, IEqualityComparer<TSubject> comparer, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer);

        Execute.Assertion
            .ForCondition(!comparer.Equals(Subject, unexpected))
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Did not expect {context} to be equal to {0}{reason}.", unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that an object is equivalent to another object.
    /// </summary>
    /// <remarks>
    /// Objects are equivalent when both object graphs have equally named properties with the same value,
    /// irrespective of the type of those objects. Two properties are also equal if one type can be converted to another and the result is equal.
    /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable{T}"/> and all
    /// items in the collection are structurally equal.
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
    /// </remarks>
    /// <param name="expectation">The expected element.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public async Task<AndConstraint<TAssertions>> BeEquivalentToAsync<TExpectation>(TExpectation expectation, string because = "",
        params object[] becauseArgs)
    {
        return await BeEquivalentToAsync(expectation, config => config, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that an object is equivalent to another object.
    /// </summary>
    /// <remarks>
    /// Objects are equivalent when both object graphs have equally named properties with the same value,
    /// irrespective of the type of those objects. Two properties are also equal if one type can be converted to another and the result is equal.
    /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable{T}"/> and all
    /// items in the collection are structurally equal.
    /// </remarks>
    /// <param name="expectation">The expected element.</param>
    /// <param name="config">
    /// A reference to the <see cref="EquivalencyOptions{TExpectation}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{TExpectation}"/> class. The global defaults are determined by the
    /// <see cref="AssertionOptions"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="config"/> is <see langword="null"/>.</exception>
    public async Task<AndConstraint<TAssertions>> BeEquivalentToAsync<TExpectation>(TExpectation expectation,
        Func<EquivalencyOptions<TExpectation>, EquivalencyOptions<TExpectation>> config, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(config);

        EquivalencyOptions<TExpectation> options = config(AssertionOptions.CloneDefaults<TExpectation>());

        var context = new EquivalencyValidationContext(Node.From<TExpectation>(() =>
            AssertionScope.Current.CallerIdentity), options)
        {
            Reason = new Reason(because, becauseArgs),
            TraceWriter = options.TraceWriter
        };

        var comparands = new Comparands
        {
            Subject = Subject,
            Expectation = expectation,
            CompileTimeType = typeof(TExpectation),
        };

        await new EquivalencyValidator().AssertEqualityAsync(comparands, context);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that an object is not equivalent to another object.
    /// </summary>
    /// <remarks>
    /// Objects are equivalent when both object graphs have equally named properties with the same value,
    /// irrespective of the type of those objects. Two properties are also equal if one type can be converted to another and the result is equal.
    /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable{T}"/> and all
    /// items in the collection are structurally equal.
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
    /// </remarks>
    /// <param name="unexpected">The unexpected element.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public async Task<AndConstraint<TAssertions>> NotBeEquivalentToAsync<TExpectation>(
        TExpectation unexpected,
        string because = "",
        params object[] becauseArgs)
    {
        return await NotBeEquivalentToAsync(unexpected, config => config, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that an object is not equivalent to another object.
    /// </summary>
    /// <remarks>
    /// Objects are equivalent when both object graphs have equally named properties with the same value,
    /// irrespective of the type of those objects. Two properties are also equal if one type can be converted to another and the result is equal.
    /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable{T}"/> and all
    /// items in the collection are structurally equal.
    /// </remarks>
    /// <param name="unexpected">The unexpected element.</param>
    /// <param name="config">
    /// A reference to the <see cref="EquivalencyOptions{TExpectation}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{TExpectation}"/> class. The global defaults are determined by the
    /// <see cref="AssertionOptions"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="config"/> is <see langword="null"/>.</exception>
    public async Task<AndConstraint<TAssertions>> NotBeEquivalentToAsync<TExpectation>(
        TExpectation unexpected,
        Func<EquivalencyOptions<TExpectation>, EquivalencyOptions<TExpectation>> config,
        string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(config);

        bool hasMismatches;

        using (var scope = new AssertionScope())
        {
            await BeEquivalentToAsync(unexpected, config);
            hasMismatches = scope.Discard().Length > 0;
        }

        Execute.Assertion
            .ForCondition(hasMismatches)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Expected {context} not to be equivalent to {0}{reason}, but they are.", unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a value is one of the specified <paramref name="validValues"/>.
    /// </summary>
    /// <param name="validValues">
    /// The values that are valid.
    /// </param>
    public AndConstraint<TAssertions> BeOneOf(params TSubject[] validValues)
    {
        return BeOneOf(validValues, string.Empty);
    }

    /// <summary>
    /// Asserts that a value is one of the specified <paramref name="validValues"/>.
    /// </summary>
    /// <param name="validValues">
    /// The values that are valid.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeOneOf(IEnumerable<TSubject> validValues, string because = "",
        params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(validValues.Contains(Subject))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:object} to be one of {0}{reason}, but found {1}.", validValues, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a value is one of the specified <paramref name="validValues"/>.
    /// </summary>
    /// <param name="validValues">
    /// The values that are valid.
    /// </param>
    /// <param name="comparer">
    /// An equality comparer to compare values.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="validValues"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> BeOneOf(IEnumerable<TSubject> validValues,
        IEqualityComparer<TSubject> comparer,
        string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(validValues);
        Guard.ThrowIfArgumentIsNull(comparer);

        Execute.Assertion
            .ForCondition(validValues.Contains(Subject, comparer))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:object} to be one of {0}{reason}, but found {1}.", validValues, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        throw new NotSupportedException("Equals is not part of Fluent Assertions. Did you mean Be() or BeSameAs() instead?");

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => "object";
}
