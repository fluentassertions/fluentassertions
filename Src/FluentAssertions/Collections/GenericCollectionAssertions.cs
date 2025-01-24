using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Collections.MaximumMatching;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;
using FluentAssertions.Primitives;

namespace FluentAssertions.Collections;

[DebuggerNonUserCode]
public class GenericCollectionAssertions<T> : GenericCollectionAssertions<IEnumerable<T>, T, GenericCollectionAssertions<T>>
{
    public GenericCollectionAssertions(IEnumerable<T> actualValue, AssertionChain assertionChain)
        : base(actualValue, assertionChain)
    {
    }
}

[DebuggerNonUserCode]
public class GenericCollectionAssertions<TCollection, T>
    : GenericCollectionAssertions<TCollection, T, GenericCollectionAssertions<TCollection, T>>
    where TCollection : IEnumerable<T>
{
    public GenericCollectionAssertions(TCollection actualValue, AssertionChain assertionChain)
        : base(actualValue, assertionChain)
    {
    }
}

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals

[DebuggerNonUserCode]
public class GenericCollectionAssertions<TCollection, T, TAssertions> : ReferenceTypeAssertions<TCollection, TAssertions>
    where TCollection : IEnumerable<T>
    where TAssertions : GenericCollectionAssertions<TCollection, T, TAssertions>
{
    private readonly AssertionChain assertionChain;

    public GenericCollectionAssertions(TCollection actualValue, AssertionChain assertionChain)
        : base(actualValue, assertionChain)
    {
        this.assertionChain = assertionChain;
    }

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => "collection";

    /// <summary>
    /// Asserts that all items in the collection are of the specified type <typeparamref name="TExpectation" />
    /// </summary>
    /// <typeparam name="TExpectation">The expected type of the objects</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<TAssertions, IEnumerable<TExpectation>> AllBeAssignableTo<TExpectation>(
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected type to be {0}{reason}, but found {context:the collection} is <null>.",
                typeof(TExpectation).FullName);

        IEnumerable<TExpectation> matches = [];

        if (assertionChain.Succeeded)
        {
            assertionChain
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected type to be {0}{reason}, ", typeof(TExpectation).FullName, chain => chain
                    .ForCondition(Subject!.All(x => x is not null))
                    .FailWith("but found a null element.")
                    .Then
                    .ForCondition(Subject.All(x => typeof(TExpectation).IsAssignableFrom(GetType(x))))
                    .FailWith("but found {0}.", () => $"[{string.Join(", ", Subject.Select(x => GetType(x).FullName))}]"));

            matches = Subject!.OfType<TExpectation>();
        }

        return new AndWhichConstraint<TAssertions, IEnumerable<TExpectation>>((TAssertions)this, matches);
    }

    /// <summary>
    /// Asserts that all items in the collection are of the specified type <paramref name="expectedType"/>
    /// </summary>
    /// <param name="expectedType">The expected type of the objects</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expectedType"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> AllBeAssignableTo(Type expectedType,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectedType);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected type to be {0}{reason}, ", expectedType.FullName, chain => chain
                .Given(() => Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("but found {context:collection} is <null>.")
                .Then
                .ForCondition(subject => subject.All(x => x is not null))
                .FailWith("but found a null element.")
                .Then
                .ForCondition(subject => subject.All(x => expectedType.IsAssignableFrom(GetType(x))))
                .FailWith("but found {0}.", subject => $"[{string.Join(", ", subject.Select(x => GetType(x).FullName))}]"));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that all elements in a collection of objects are equivalent to a given object.
    /// </summary>
    /// <remarks>
    /// Objects within the collection are equivalent to given object when both object graphs have equally named properties with the same
    /// value, irrespective of the type of those objects. Two properties are also equal if one type can be converted to another
    /// and the result is equal.
    /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable{T}"/> and all
    /// items in the collection are structurally equal.
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionConfiguration"/>.
    /// </remarks>
    /// <param name="expectation">The expected element.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> AllBeEquivalentTo<TExpectation>(TExpectation expectation,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return AllBeEquivalentTo(expectation, options => options, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that all elements in a collection of objects are equivalent to a given object.
    /// </summary>
    /// <remarks>
    /// Objects within the collection are equivalent to given object when both object graphs have equally named properties with the same
    /// value, irrespective of the type of those objects. Two properties are also equal if one type can be converted to another
    /// and the result is equal.
    /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable{T}"/> and all
    /// items in the collection are structurally equal.
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionConfiguration"/>.
    /// </remarks>
    /// <param name="expectation">The expected element.</param>
    /// <param name="config">
    /// A reference to the <see cref="EquivalencyOptions{TExpectation}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{TExpectation}"/> class. The global defaults are determined by the
    /// <see cref="AssertionConfiguration"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="config"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> AllBeEquivalentTo<TExpectation>(TExpectation expectation,
        Func<EquivalencyOptions<TExpectation>, EquivalencyOptions<TExpectation>> config,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(config);

        TExpectation[] repeatedExpectation = RepeatAsManyAs(expectation, Subject).ToArray();

        // Because we have just manually created the collection based on single element
        // we are sure that we can force strict ordering, because ordering does not matter in terms
        // of correctness. On the other hand we do not want to change ordering rules for nested objects
        // in case user needs to use them. Strict ordering improves algorithmic complexity
        // from O(n^2) to O(n). For bigger tables it is necessary in order to achieve acceptable
        // execution times.
        Func<EquivalencyOptions<TExpectation>, EquivalencyOptions<TExpectation>> forceStrictOrderingConfig =
            x => config(x).WithStrictOrderingFor(s => string.IsNullOrEmpty(s.Path));

        return BeEquivalentTo(repeatedExpectation, forceStrictOrderingConfig, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that all items in the collection are of the exact specified type <typeparamref name="TExpectation" />
    /// </summary>
    /// <typeparam name="TExpectation">The expected type of the objects</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<TAssertions, IEnumerable<TExpectation>> AllBeOfType<TExpectation>(
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected type to be {0}{reason}, but found {context:collection} is <null>.",
                typeof(TExpectation).FullName);

        IEnumerable<TExpectation> matches = [];

        if (assertionChain.Succeeded)
        {
            assertionChain
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected type to be {0}{reason}, ", typeof(TExpectation).FullName, chain => chain
                    .ForCondition(Subject!.All(x => x is not null))
                    .FailWith("but found a null element.")
                    .Then
                    .ForCondition(Subject.All(x => typeof(TExpectation) == GetType(x)))
                    .FailWith("but found {0}.", () => $"[{string.Join(", ", Subject.Select(x => GetType(x).FullName))}]"));

            matches = Subject!.OfType<TExpectation>();
        }

        return new AndWhichConstraint<TAssertions, IEnumerable<TExpectation>>((TAssertions)this, matches);
    }

    /// <summary>
    /// Asserts that all items in the collection are of the exact specified type <paramref name="expectedType"/>
    /// </summary>
    /// <param name="expectedType">The expected type of the objects</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expectedType"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> AllBeOfType(Type expectedType, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectedType);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected type to be {0}{reason}, ", expectedType.FullName, chain => chain
                .Given(() => Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("but found {context:collection} is <null>.")
                .Then
                .ForCondition(subject => subject.All(x => x is not null))
                .FailWith("but found a null element.")
                .Then
                .ForCondition(subject => subject.All(x => expectedType == GetType(x)))
                .FailWith("but found {0}.", subject => $"[{string.Join(", ", subject.Select(x => GetType(x).FullName))}]"));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the collection does not contain any items.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeEmpty([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        var singleItemArray = Subject?.Take(1).ToArray();

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to be empty{reason}, ", chain => chain
                .Given(() => singleItemArray)
                .ForCondition(subject => subject is not null)
                .FailWith("but found <null>.")
                .Then
                .ForCondition(subject => subject.Length == 0)
                .FailWith("but found at least one item {0}.", singleItemArray));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a collection of objects is equivalent to another collection of objects.
    /// </summary>
    /// <remarks>
    /// Objects within the collections are equivalent when both object graphs have equally named properties with the same
    /// value, irrespective of the type of those objects. Two properties are also equal if one type can be converted to another
    /// and the result is equal.
    /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable{T}"/> and all
    /// items in the collection are structurally equal.
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionConfiguration"/>.
    /// </remarks>
    /// <param name="expectation">An <see cref="IEnumerable{T}"/> with the expected elements.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> BeEquivalentTo<TExpectation>(IEnumerable<TExpectation> expectation,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return BeEquivalentTo(expectation, config => config, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that a collection of objects is equivalent to another collection of objects.
    /// </summary>
    /// <remarks>
    /// Objects within the collections are equivalent when both object graphs have equally named properties with the same
    /// value, irrespective of the type of those objects. Two properties are also equal if one type can be converted to another
    /// and the result is equal.
    /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable{T}"/> and all
    /// items in the collection are structurally equal.
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionConfiguration"/>.
    /// </remarks>
    /// <param name="expectation">An <see cref="IEnumerable{T}"/> with the expected elements.</param>
    /// <param name="config">
    /// A reference to the <see cref="EquivalencyOptions{TExpectation}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{TExpectation}"/> class. The global defaults are determined by the
    /// <see cref="AssertionConfiguration"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="config"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> BeEquivalentTo<TExpectation>(IEnumerable<TExpectation> expectation,
        Func<EquivalencyOptions<TExpectation>, EquivalencyOptions<TExpectation>> config,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(config);

        EquivalencyOptions<IEnumerable<TExpectation>> options =
            config(AssertionConfiguration.Current.Equivalency.CloneDefaults<TExpectation>()).AsCollection();

        var context =
            new EquivalencyValidationContext(
                Node.From<IEnumerable<TExpectation>>(() => CallerIdentifier.DetermineCallerIdentity()),
                options)
            {
                Reason = new Reason(because, becauseArgs),
                TraceWriter = options.TraceWriter,
            };

        var comparands = new Comparands
        {
            Subject = Subject,
            Expectation = expectation,
            CompileTimeType = typeof(IEnumerable<TExpectation>),
        };

        new EquivalencyValidator().AssertEquality(comparands, context);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a collection is ordered in ascending order according to the value of the specified
    /// <paramref name="propertyExpression"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// A lambda expression that references the property that should be used to determine the expected ordering.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    public AndConstraint<SubsequentOrderingAssertions<T>> BeInAscendingOrder<TSelector>(
        Expression<Func<T, TSelector>> propertyExpression,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return BeInAscendingOrder(propertyExpression, GetComparer<TSelector>(), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that a collection is ordered in ascending order according to the value of the specified
    /// <see cref="IComparer{T}"/> implementation.
    /// </summary>
    /// <param name="comparer">
    /// The object that should be used to determine the expected ordering.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public AndConstraint<SubsequentOrderingAssertions<T>> BeInAscendingOrder(
        IComparer<T> comparer, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer, nameof(comparer),
            "Cannot assert collection ordering without specifying a comparer.");

        return BeInOrder(comparer, SortOrder.Ascending, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that a collection is ordered in ascending order according to the value of the specified
    /// <paramref name="propertyExpression"/> and <see cref="IComparer{T}"/> implementation.
    /// </summary>
    /// <param name="propertyExpression">
    /// A lambda expression that references the property that should be used to determine the expected ordering.
    /// </param>
    /// <param name="comparer">
    /// The object that should be used to determine the expected ordering.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public AndConstraint<SubsequentOrderingAssertions<T>> BeInAscendingOrder<TSelector>(
        Expression<Func<T, TSelector>> propertyExpression, IComparer<TSelector> comparer,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer, nameof(comparer),
            "Cannot assert collection ordering without specifying a comparer.");

        return BeOrderedBy(propertyExpression, comparer, SortOrder.Ascending, because, becauseArgs);
    }

    /// <summary>
    /// Expects the current collection to have all elements in ascending order. Elements are compared
    /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    public AndConstraint<SubsequentOrderingAssertions<T>> BeInAscendingOrder(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return BeInAscendingOrder(GetComparer<T>(), because, becauseArgs);
    }

    /// <summary>
    /// Expects the current collection to have all elements in ascending order. Elements are compared
    /// using the given lambda expression.
    /// </summary>
    /// <param name="comparison">
    /// A lambda expression that should be used to determine the expected ordering between two objects.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    public AndConstraint<SubsequentOrderingAssertions<T>> BeInAscendingOrder(Func<T, T, int> comparison,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return BeInOrder(Comparer<T>.Create((x, y) => comparison(x, y)), SortOrder.Ascending, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that a collection is ordered in descending order according to the value of the specified
    /// <paramref name="propertyExpression"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// A lambda expression that references the property that should be used to determine the expected ordering.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    public AndConstraint<SubsequentOrderingAssertions<T>> BeInDescendingOrder<TSelector>(
        Expression<Func<T, TSelector>> propertyExpression, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return BeInDescendingOrder(propertyExpression, GetComparer<TSelector>(), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that a collection is ordered in descending order according to the value of the specified
    /// <see cref="IComparer{T}"/> implementation.
    /// </summary>
    /// <param name="comparer">
    /// The object that should be used to determine the expected ordering.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public AndConstraint<SubsequentOrderingAssertions<T>> BeInDescendingOrder(
        IComparer<T> comparer, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer, nameof(comparer),
            "Cannot assert collection ordering without specifying a comparer.");

        return BeInOrder(comparer, SortOrder.Descending, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that a collection is ordered in descending order according to the value of the specified
    /// <paramref name="propertyExpression"/> and <see cref="IComparer{T}"/> implementation.
    /// </summary>
    /// <param name="propertyExpression">
    /// A lambda expression that references the property that should be used to determine the expected ordering.
    /// </param>
    /// <param name="comparer">
    /// The object that should be used to determine the expected ordering.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public AndConstraint<SubsequentOrderingAssertions<T>> BeInDescendingOrder<TSelector>(
        Expression<Func<T, TSelector>> propertyExpression, IComparer<TSelector> comparer,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer, nameof(comparer),
            "Cannot assert collection ordering without specifying a comparer.");

        return BeOrderedBy(propertyExpression, comparer, SortOrder.Descending, because, becauseArgs);
    }

    /// <summary>
    /// Expects the current collection to have all elements in descending order. Elements are compared
    /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    public AndConstraint<SubsequentOrderingAssertions<T>> BeInDescendingOrder(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return BeInDescendingOrder(GetComparer<T>(), because, becauseArgs);
    }

    /// <summary>
    /// Expects the current collection to have all elements in descending order. Elements are compared
    /// using the given lambda expression.
    /// </summary>
    /// <param name="comparison">
    /// A lambda expression that should be used to determine the expected ordering between two objects.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    public AndConstraint<SubsequentOrderingAssertions<T>> BeInDescendingOrder(Func<T, T, int> comparison,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return BeInOrder(Comparer<T>.Create((x, y) => comparison(x, y)), SortOrder.Descending, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the collection is null or does not contain any items.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeNullOrEmpty(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        var singleItemArray = Subject?.Take(1).ToArray();
        var nullOrEmpty = singleItemArray is null || singleItemArray.Length == 0;

        assertionChain.ForCondition(nullOrEmpty)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:collection} to be null or empty{reason}, but found at least one item {0}.",
                singleItemArray);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the collection is a subset of the <paramref name="expectedSuperset" />.
    /// </summary>
    /// <param name="expectedSuperset">An <see cref="IEnumerable{T}"/> with the expected superset.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expectedSuperset"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> BeSubsetOf(IEnumerable<T> expectedSuperset,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectedSuperset, nameof(expectedSuperset),
            "Cannot verify a subset against a <null> collection.");

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to be a subset of {0}{reason}, ", expectedSuperset, chain => chain
                .Given(() => Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("but found <null>.")
                .Then
                .Given(subject => subject.Except(expectedSuperset))
                .ForCondition(excessItems => !excessItems.Any())
                .FailWith("but items {0} are not part of the superset.", excessItems => excessItems));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the collection contains the specified item.
    /// </summary>
    /// <param name="expected">The expectation item.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndWhichConstraint<TAssertions, T> Contain(T expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to contain {0}{reason}, but found <null>.", expected);

        IEnumerable<T> matches = [];

        if (assertionChain.Succeeded)
        {
            ICollection<T> collection = Subject.ConvertOrCastToCollection();

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(collection.Contains(expected))
                .FailWith("Expected {context:collection} {0} to contain {1}{reason}.", collection, expected);

            matches = collection.Where(item => EqualityComparer<T>.Default.Equals(item, expected));
        }

        return new AndWhichConstraint<TAssertions, T>((TAssertions)this, matches, assertionChain);
    }

    /// <summary>
    /// Asserts that the collection contains at least one item that matches the predicate.
    /// </summary>
    /// <param name="predicate">A predicate to match the items in the collection against.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<TAssertions, T> Contain(Expression<Func<T, bool>> predicate,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(predicate);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to contain {0}{reason}, but found <null>.", predicate.Body);

        IEnumerable<T> matches = [];

        int? firstMatchingIndex = null;
        if (assertionChain.Succeeded)
        {
            Func<T, bool> func = predicate.Compile();

            foreach (var (item, index) in Subject!.Select((item, index) => (item, index)))
            {
                if (func(item))
                {
                    firstMatchingIndex = index;
                    break;
                }
            }

            assertionChain
                .ForCondition(firstMatchingIndex.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} {0} to have an item matching {1}{reason}.", Subject, predicate.Body);

            matches = Subject.Where(func);
        }

        return new AndWhichConstraint<TAssertions, T>((TAssertions)this, matches, assertionChain, $"[{firstMatchingIndex}]");
    }

    /// <summary>
    /// Expects the current collection to contain the specified elements in any order. Elements are compared
    /// using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="expected">An <see cref="IEnumerable{T}"/> with the expected elements.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="expected"/> is empty.</exception>
    public AndConstraint<TAssertions> Contain(IEnumerable<T> expected, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot verify containment against a <null> collection");

        ICollection<T> expectedObjects = expected.ConvertOrCastToCollection();
        Guard.ThrowIfArgumentIsEmpty(expectedObjects, nameof(expected), "Cannot verify containment against an empty collection");

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to contain {0}{reason}, but found <null>.", expectedObjects);

        if (assertionChain.Succeeded)
        {
            IEnumerable<T> missingItems = expectedObjects.Except(Subject!);

            if (missingItems.Any())
            {
                if (expectedObjects.Count > 1)
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} {0} to contain {1}{reason}, but could not find {2}.",
                            Subject, expectedObjects, missingItems);
                }
                else
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} {0} to contain {1}{reason}.",
                            Subject, expectedObjects.Single());
                }
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that at least one element in the collection is equivalent to <paramref name="expectation"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Important:</b> You cannot use this method to assert whether a subset of the collection is equivalent to the <paramref name="expectation"/>.
    /// This usually means that the expectation is meant to be a <i>single</i> item.
    /// </para>
    /// <para>
    /// By default, objects within the collection are seen as equivalent to the expected object when both object graphs have equally named properties with the same
    /// value, irrespective of the type of those objects.
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionConfiguration"/>.
    /// </para>
    /// </remarks>
    /// <param name="expectation">The expected element.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndWhichConstraint<TAssertions, T> ContainEquivalentOf<TExpectation>(TExpectation expectation,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return ContainEquivalentOf(expectation, config => config, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that at least one element in the collection is equivalent to <paramref name="expectation"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Important:</b> You cannot use this method to assert whether a subset of the collection is equivalent to the <paramref name="expectation"/>.
    /// This usually means that the expectation is meant to be a <i>single</i> item.
    /// </para>
    /// <para>
    /// By default, objects within the collection are seen as equivalent to the expected object when both object graphs have equally named properties with the same
    /// value, irrespective of the type of those objects.
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionConfiguration"/>.
    /// </para>
    /// </remarks>
    /// <param name="expectation">The expected element.</param>
    /// <param name="config">
    /// A reference to the <see cref="EquivalencyOptions{TExpectation}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{TExpectation}"/> class. The global defaults are determined by the
    /// <see cref="AssertionConfiguration"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="config"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<TAssertions, T> ContainEquivalentOf<TExpectation>(TExpectation expectation,
        Func<EquivalencyOptions<TExpectation>,
            EquivalencyOptions<TExpectation>> config, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(config);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to contain equivalent of {0}{reason}, but found <null>.", expectation);

        if (assertionChain.Succeeded)
        {
            EquivalencyOptions<TExpectation> options = config(AssertionConfiguration.Current.Equivalency.CloneDefaults<TExpectation>());

            using var scope = new AssertionScope();
            assertionChain.AddReportable("configuration", () => options.ToString());

            foreach ((T actualItem, int index) in Subject!.Select((item, index) => (item, index)))
            {
                var context =
                    new EquivalencyValidationContext(Node.From<TExpectation>(() => CurrentAssertionChain.CallerIdentifier),
                        options)
                    {
                        Reason = new Reason(because, becauseArgs),
                        TraceWriter = options.TraceWriter
                    };

                var comparands = new Comparands
                {
                    Subject = actualItem,
                    Expectation = expectation,
                    CompileTimeType = typeof(TExpectation),
                };

                new EquivalencyValidator().AssertEquality(comparands, context);

                string[] failures = scope.Discard();

                if (failures.Length == 0)
                {
                    return new AndWhichConstraint<TAssertions, T>((TAssertions)this, actualItem, assertionChain, $"[{index}]");
                }
            }

            assertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} {0} to contain equivalent of {1}{reason}.", Subject, expectation);
        }

        return new AndWhichConstraint<TAssertions, T>((TAssertions)this, default(T));
    }

    /// <summary>
    /// Expects the current collection to contain the specified elements in the exact same order, not necessarily consecutive.
    /// using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="expected">An <see cref="IEnumerable{T}"/> with the expected elements.</param>
    public AndConstraint<TAssertions> ContainInOrder(params T[] expected)
    {
        return ContainInOrder(expected, string.Empty);
    }

    /// <summary>
    /// Expects the current collection to contain the specified elements in the exact same order, not necessarily consecutive.
    /// </summary>
    /// <remarks>
    /// Elements are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </remarks>
    /// <param name="expected">An <see cref="IEnumerable{T}"/> with the expected elements.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> ContainInOrder(IEnumerable<T> expected,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot verify ordered containment against a <null> collection.");

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to contain {0} in order{reason}, but found <null>.", expected);

        if (assertionChain.Succeeded)
        {
            IList<T> expectedItems = expected.ConvertOrCastToList();
            IList<T> actualItems = Subject.ConvertOrCastToList();

            int subjectIndex = 0;

            for (int index = 0; index < expectedItems.Count; index++)
            {
                T expectedItem = expectedItems[index];
                subjectIndex = IndexOf(actualItems, expectedItem, startIndex: subjectIndex);

                if (subjectIndex == -1)
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith(
                            "Expected {context:collection} {0} to contain items {1} in order{reason}" +
                            ", but {2} (index {3}) did not appear (in the right order).",
                            Subject, expected, expectedItem, index);
                }
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Expects the current collection to contain the specified elements in the exact same order, and to be consecutive.
    /// using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="expected">An <see cref="IEnumerable{T}"/> with the expected elements.</param>
    public AndConstraint<TAssertions> ContainInConsecutiveOrder(params T[] expected)
    {
        return ContainInConsecutiveOrder(expected, string.Empty);
    }

    /// <summary>
    /// Expects the current collection to contain the specified elements in the exact same order, and to be consecutive.
    /// </summary>
    /// <remarks>
    /// Elements are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </remarks>
    /// <param name="expected">An <see cref="IEnumerable{T}"/> with the expected elements.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> ContainInConsecutiveOrder(IEnumerable<T> expected,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot verify ordered containment against a <null> collection.");

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to contain {0} in order{reason}, but found <null>.", expected);

        if (assertionChain.Succeeded)
        {
            IList<T> expectedItems = expected.ConvertOrCastToList();

            if (expectedItems.Count == 0)
            {
                return new AndConstraint<TAssertions>((TAssertions)this);
            }

            IList<T> actualItems = Subject.ConvertOrCastToList();

            int subjectIndex = 0;
            int highestIndex = 0;

            while (subjectIndex != -1)
            {
                subjectIndex = IndexOf(actualItems, expectedItems[0], startIndex: subjectIndex);

                if (subjectIndex != -1)
                {
                    int consecutiveItems = ConsecutiveItemCount(actualItems, expectedItems, startIndex: subjectIndex);

                    if (consecutiveItems == expectedItems.Count)
                    {
                        return new AndConstraint<TAssertions>((TAssertions)this);
                    }

                    highestIndex = Math.Max(highestIndex, consecutiveItems);
                    subjectIndex++;
                }
            }

            assertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:collection} {0} to contain items {1} in order{reason}" +
                    ", but {2} (index {3}) did not appear (in the right consecutive order).",
                    Subject, expected, expectedItems[highestIndex], highestIndex);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current collection contains at least one element that is assignable to the type <typeparamref name="TExpectation" />.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> ContainItemsAssignableTo<TExpectation>(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to contain at least one element assignable to type {0}{reason}, ",
                typeof(TExpectation).FullName, chain => chain
                    .ForCondition(Subject is not null)
                    .FailWith("but found <null>.")
                    .Then
                    .Given(() => Subject.ConvertOrCastToCollection())
                    .ForCondition(subject => subject.Any(x => typeof(TExpectation).IsAssignableFrom(GetType(x))))
                    .FailWith("but found {0}.", subject => subject.Select(x => GetType(x))));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current collection does not contain any elements that are assignable to the type <typeparamref name="TExpectation" />.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions>
        NotContainItemsAssignableTo<TExpectation>(string because = "", params object[] becauseArgs) =>
        NotContainItemsAssignableTo(typeof(TExpectation), because, becauseArgs);

    /// <summary>
    /// Asserts that the current collection does not contain any elements that are assignable to the given type.
    /// </summary>
    /// <param name="type">
    /// Object type that should not be in collection
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotContainItemsAssignableTo(Type type,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(type);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to not contain any elements assignable to type {0}{reason}, ",
                type.FullName, chain => chain
                    .ForCondition(Subject is not null)
                    .FailWith("but found <null>.")
                    .Then
                    .Given(() => Subject.ConvertOrCastToCollection())
                    .ForCondition(subject => subject.All(x => !type.IsAssignableFrom(GetType(x))))
                    .FailWith("but found {0}.", subject => subject.Select(x => GetType(x))));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Expects the current collection to contain only a single item.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<TAssertions, T> ContainSingle(string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to contain a single item{reason}, but found <null>.");

        T match = default;

        if (assertionChain.Succeeded)
        {
            ICollection<T> actualItems = Subject.ConvertOrCastToCollection();

            switch (actualItems.Count)
            {
                case 0: // Fail, Collection is empty
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} to contain a single item{reason}, but the collection is empty.");

                    break;
                case 1: // Success Condition
                    match = actualItems.Single();
                    break;
                default: // Fail, Collection contains more than a single item
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} to contain a single item{reason}, but found {0}.", Subject);

                    break;
            }
        }

        return new AndWhichConstraint<TAssertions, T>((TAssertions)this, match, assertionChain, "[0]");
    }

    /// <summary>
    /// Expects the current collection to contain only a single item matching the specified <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">The predicate that will be used to find the matching items.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<TAssertions, T> ContainSingle(Expression<Func<T, bool>> predicate,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(predicate);

        const string expectationPrefix =
            "Expected {context:collection} to contain a single item matching {0}{reason}, ";

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(expectationPrefix + "but found <null>.", predicate);

        T[] matches = [];

        if (assertionChain.Succeeded)
        {
            ICollection<T> actualItems = Subject.ConvertOrCastToCollection();

            assertionChain
                .ForCondition(actualItems.Count > 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(expectationPrefix + "but the collection is empty.", predicate);

            matches = actualItems.Where(predicate.Compile()).ToArray();
            int count = matches.Length;

            if (count == 0)
            {
                assertionChain
                    .BecauseOf(because, becauseArgs)
                    .FailWith(expectationPrefix + "but no such item was found.", predicate);
            }
            else if (count > 1)
            {
                assertionChain
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        expectationPrefix + "but " + count.ToString(CultureInfo.InvariantCulture) + " such items were found.",
                        predicate);
            }
            else
            {
                // Can never happen
            }
        }

        return new AndWhichConstraint<TAssertions, T>((TAssertions)this, matches, assertionChain, "[0]");
    }

    /// <summary>
    /// Asserts that the current collection ends with same elements in the same order as the collection identified by
    /// <paramref name="expectation" />. Elements are compared using their <see cref="object.Equals(object)" />.
    /// </summary>
    /// <param name="expectation">
    /// A collection of expected elements.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> EndWith(IEnumerable<T> expectation, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return EndWith(expectation, (a, b) => EqualityComparer<T>.Default.Equals(a, b), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current collection ends with same elements in the same order as the collection identified by
    /// <paramref name="expectation" />. Elements are compared using <paramref name="equalityComparison"/>.
    /// </summary>
    /// <param name="expectation">
    /// A collection of expected elements.
    /// </param>
    /// <param name="equalityComparison">
    /// A equality comparison the is used to determine whether two objects should be treated as equal.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expectation"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> EndWith<TExpectation>(
        IEnumerable<TExpectation> expectation, Func<T, TExpectation, bool> equalityComparison,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectation, nameof(expectation), "Cannot compare collection with <null>.");

        AssertCollectionEndsWith(Subject, expectation.ConvertOrCastToCollection(), equalityComparison, because, becauseArgs);
        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the collection ends with the specified <paramref name="element"/>.
    /// </summary>
    /// <param name="element">
    /// The element that is expected to appear at the end of the collection. The object's <see cref="object.Equals(object)"/>
    /// is used to compare the element.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> EndWith(T element, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return EndWith([element], ObjectExtensions.GetComparer<T>(), because, becauseArgs);
    }

    /// <summary>
    /// Expects the current collection to contain all the same elements in the same order as the collection identified by
    /// <paramref name="elements" />. Elements are compared using their <see cref="object.Equals(object)" /> method.
    /// </summary>
    /// <param name="elements">A params array with the expected elements.</param>
    public AndConstraint<TAssertions> Equal(params T[] elements)
    {
        AssertSubjectEquality(elements, ObjectExtensions.GetComparer<T>(), string.Empty);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that two collections contain the same items in the same order, where equality is determined using a
    /// <paramref name="equalityComparison"/>.
    /// </summary>
    /// <param name="expectation">
    /// The collection to compare the subject with.
    /// </param>
    /// <param name="equalityComparison">
    /// A equality comparison the is used to determine whether two objects should be treated as equal.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> Equal<TExpectation>(
        IEnumerable<TExpectation> expectation, Func<T, TExpectation, bool> equalityComparison,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        AssertSubjectEquality(expectation, equalityComparison, because, becauseArgs);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Expects the current collection to contain all the same elements in the same order as the collection identified by
    /// <paramref name="expected" />. Elements are compared using their <see cref="object.Equals(object)" />.
    /// </summary>
    /// <param name="expected">An <see cref="IEnumerable{T}"/> with the expected elements.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> Equal(IEnumerable<T> expected, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        AssertSubjectEquality(expected, ObjectExtensions.GetComparer<T>(), because, becauseArgs);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the number of items in the collection matches the supplied <paramref name="expected" /> amount.
    /// </summary>
    /// <param name="expected">The expected number of items in the collection.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveCount(int expected, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to contain {0} item(s){reason}, but found <null>.", expected);

        if (assertionChain.Succeeded)
        {
            int actualCount = Subject!.Count();

            assertionChain
                .ForCondition(actualCount == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:collection} to contain {0} item(s){reason}, but found {1}: {2}.",
                    expected, actualCount, Subject);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the number of items in the collection matches a condition stated by the <paramref name="countPredicate"/>.
    /// </summary>
    /// <param name="countPredicate">A predicate that yields the number of items that is expected to be in the collection.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="countPredicate"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> HaveCount(Expression<Func<int, bool>> countPredicate,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(countPredicate, nameof(countPredicate),
            "Cannot compare collection count against a <null> predicate.");

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to contain {0} items{reason}, but found <null>.", countPredicate.Body);

        if (assertionChain.Succeeded)
        {
            Func<int, bool> compiledPredicate = countPredicate.Compile();

            int actualCount = Subject!.Count();

            if (!compiledPredicate(actualCount))
            {
                assertionChain
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to have a count {0}{reason}, but count is {1}: {2}.",
                        countPredicate.Body, actualCount, Subject);
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the number of items in the collection is greater than or equal to the supplied <paramref name="expected" /> amount.
    /// </summary>
    /// <param name="expected">The number to which the actual number items in the collection will be compared.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveCountGreaterThanOrEqualTo(int expected,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to contain at least {0} item(s){reason}, ", expected, chain => chain
                .Given(() => Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("but found <null>.")
                .Then
                .Given(subject => subject.Count())
                .ForCondition(actualCount => actualCount >= expected)
                .FailWith("but found {0}: {1}.", actualCount => actualCount, _ => Subject));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the number of items in the collection is greater than the supplied <paramref name="expected" /> amount.
    /// </summary>
    /// <param name="expected">The number to which the actual number items in the collection will be compared.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveCountGreaterThan(int expected, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to contain more than {0} item(s){reason}, ", expected, chain => chain
                .Given(() => Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("but found <null>.")
                .Then
                .Given(subject => subject.Count())
                .ForCondition(actualCount => actualCount > expected)
                .FailWith("but found {0}: {1}.", actualCount => actualCount, _ => Subject));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the number of items in the collection is less than or equal to the supplied <paramref name="expected" /> amount.
    /// </summary>
    /// <param name="expected">The number to which the actual number items in the collection will be compared.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveCountLessThanOrEqualTo(int expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to contain at most {0} item(s){reason}, ", expected, chain => chain
                .Given(() => Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("but found <null>.")
                .Then
                .Given(subject => subject.Count())
                .ForCondition(actualCount => actualCount <= expected)
                .FailWith("but found {0}: {1}.", actualCount => actualCount, _ => Subject));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the number of items in the collection is less than the supplied <paramref name="expected" /> amount.
    /// </summary>
    /// <param name="expected">The number to which the actual number items in the collection will be compared.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveCountLessThan(int expected, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to contain fewer than {0} item(s){reason}, ", expected, chain => chain
                .Given(() => Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("but found <null>.")
                .Then
                .Given(subject => subject.Count())
                .ForCondition(actualCount => actualCount < expected)
                .FailWith("but found {0}: {1}.", actualCount => actualCount, _ => Subject));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current collection has the supplied <paramref name="element" /> at the
    /// supplied <paramref name="index" />.
    /// </summary>
    /// <param name="index">The index where the element is expected</param>
    /// <param name="element">The expected element</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<TAssertions, T> HaveElementAt(int index, T element,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to have element at index {0}{reason}, but found <null>.", index);

        T actual = default;

        if (assertionChain.Succeeded)
        {
            if (index < Subject!.Count())
            {
                actual = Subject.ElementAt(index);

                assertionChain
                    .ForCondition(ObjectExtensions.GetComparer<T>()(actual, element))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {0} at index {1}{reason}, but found {2}.", element, index, actual);
            }
            else
            {
                assertionChain
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {0} at index {1}{reason}, but found no element.", element, index);
            }
        }

        return new AndWhichConstraint<TAssertions, T>((TAssertions)this, actual, assertionChain, $"[{index}]");
    }

    /// <summary>
    /// Asserts that the <paramref name="expectation"/> element directly precedes the <paramref name="successor"/>.
    /// </summary>
    /// <param name="successor">The element that should succeed <paramref name="expectation"/>.</param>
    /// <param name="expectation">The expected element that should precede <paramref name="successor"/>.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveElementPreceding(T successor, T expectation,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to have {0} precede {1}{reason}, ", expectation, successor, chain =>
                chain
                    .Given(() => Subject)
                    .ForCondition(subject => subject is not null)
                    .FailWith("but the collection is <null>.")
                    .Then
                    .ForCondition(subject => subject.Any())
                    .FailWith("but the collection is empty.")
                    .Then
                    .ForCondition(subject => HasPredecessor(successor, subject))
                    .FailWith("but found nothing.")
                    .Then
                    .Given(subject => PredecessorOf(successor, subject))
                    .ForCondition(predecessor => ObjectExtensions.GetComparer<T>()(predecessor, expectation))
                    .FailWith("but found {0}.", predecessor => predecessor));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the <paramref name="expectation"/> element directly succeeds the <paramref name="predecessor"/>.
    /// </summary>
    /// <param name="predecessor">The element that should precede <paramref name="expectation"/>.</param>
    /// <param name="expectation">The element that should succeed <paramref name="predecessor"/>.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveElementSucceeding(T predecessor, T expectation,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to have {0} succeed {1}{reason}, ", expectation, predecessor,
                chain => chain
                    .Given(() => Subject)
                    .ForCondition(subject => subject is not null)
                    .FailWith("but the collection is <null>.")
                    .Then
                    .ForCondition(subject => subject.Any())
                    .FailWith("but the collection is empty.")
                    .Then
                    .ForCondition(subject => HasSuccessor(predecessor, subject))
                    .FailWith("but found nothing.")
                    .Then
                    .Given(subject => SuccessorOf(predecessor, subject))
                    .ForCondition(successor => ObjectExtensions.GetComparer<T>()(successor, expectation))
                    .FailWith("but found {0}.", successor => successor));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Assert that the current collection has the same number of elements as <paramref name="otherCollection" />.
    /// </summary>
    /// <param name="otherCollection">The other collection with the same expected number of elements</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="otherCollection"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> HaveSameCount<TExpectation>(IEnumerable<TExpectation> otherCollection,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection), "Cannot verify count against a <null> collection.");

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to have ", chain => chain
                .Given(() => Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("the same count as {0}{reason}, but found <null>.", otherCollection)
                .Then
                .Given(subject => (actual: subject.Count(), expected: otherCollection.Count()))
                .ForCondition(count => count.actual == count.expected)
                .FailWith("{0} item(s){reason}, but found {1}.", count => count.expected, count => count.actual));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the collection shares one or more items with the specified <paramref name="otherCollection"/>.
    /// </summary>
    /// <param name="otherCollection">The <see cref="IEnumerable{T}"/> with the expected shared items.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="otherCollection"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> IntersectWith(IEnumerable<T> otherCollection,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection),
            "Cannot verify intersection against a <null> collection.");

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to intersect with {0}{reason}, but found <null>.", otherCollection);

        if (assertionChain.Succeeded)
        {
            IEnumerable<T> sharedItems = Subject!.Intersect(otherCollection);

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(sharedItems.Any())
                .FailWith(
                    "Expected {context:collection} to intersect with {0}{reason}, but {1} does not contain any shared items.",
                    otherCollection, Subject);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the collection contains at least 1 item.
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
        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} not to be empty{reason}", chain => chain
                .Given(() => Subject)
                .ForCondition(subject => subject is not null)
                .FailWith(", but found <null>.")
                .Then
                .ForCondition(subject => subject.Any())
                .FailWith("."));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Expects the current collection not to contain all elements of the collection identified by <paramref name="unexpected" />,
    /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)" />.
    /// </summary>
    /// <param name="unexpected">An <see cref="IEnumerable{T}"/> with the unexpected elements.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotBeEquivalentTo<TExpectation>(IEnumerable<TExpectation> unexpected,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot verify inequivalence against a <null> collection.");

        if (Subject is null)
        {
            assertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} not to be equivalent{reason}, but found <null>.");
        }

        if (ReferenceEquals(Subject, unexpected))
        {
            assertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:collection} {0} not to be equivalent with collection {1}{reason}, but they both reference the same object.",
                    Subject,
                    unexpected);
        }

        return NotBeEquivalentTo(unexpected.ConvertOrCastToList(), config => config, because, becauseArgs);
    }

    /// <summary>
    /// Expects the current collection not to contain all elements of the collection identified by <paramref name="unexpected" />,
    /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)" />.
    /// </summary>
    /// <param name="unexpected">An <see cref="IEnumerable{T}"/> with the unexpected elements.</param>
    /// /// <param name="config">
    /// A reference to the <see cref="EquivalencyOptions{TExpectation}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{TExpectation}"/> class. The global defaults are determined by the
    /// <see cref="AssertionConfiguration"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeEquivalentTo<TExpectation>(IEnumerable<TExpectation> unexpected,
        Func<EquivalencyOptions<TExpectation>, EquivalencyOptions<TExpectation>> config,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot verify inequivalence against a <null> collection.");

        if (Subject is null)
        {
            assertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} not to be equivalent{reason}, but found <null>.");
        }

        string[] failures;

        using (var scope = new AssertionScope())
        {
            BeEquivalentTo(unexpected, config);

            failures = scope.Discard();
        }

        assertionChain
            .ForCondition(failures.Length > 0)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:collection} {0} not to be equivalent to collection {1}{reason}.", Subject,
                unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a collection is not ordered in ascending order according to the value of the specified
    /// <paramref name="propertyExpression"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// A lambda expression that references the property that should be used to determine the expected ordering.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    public AndConstraint<TAssertions> NotBeInAscendingOrder<TSelector>(
        Expression<Func<T, TSelector>> propertyExpression, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return NotBeInAscendingOrder(propertyExpression, GetComparer<TSelector>(), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that a collection is not ordered in ascending order according to the value of the specified
    /// <see cref="IComparer{T}"/> implementation.
    /// </summary>
    /// <param name="comparer">
    /// The object that should be used to determine the expected ordering.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotBeInAscendingOrder(
        IComparer<T> comparer, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer, nameof(comparer),
            "Cannot assert collection ordering without specifying a comparer.");

        return NotBeInOrder(comparer, SortOrder.Ascending, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that a collection is not ordered in ascending order according to the value of the specified
    /// <paramref name="propertyExpression"/> and <see cref="IComparer{T}"/> implementation.
    /// </summary>
    /// <param name="propertyExpression">
    /// A lambda expression that references the property that should be used to determine the expected ordering.
    /// </param>
    /// <param name="comparer">
    /// The object that should be used to determine the expected ordering.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotBeInAscendingOrder<TSelector>(
        Expression<Func<T, TSelector>> propertyExpression, IComparer<TSelector> comparer,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer, nameof(comparer),
            "Cannot assert collection ordering without specifying a comparer.");

        return NotBeOrderedBy(propertyExpression, comparer, SortOrder.Ascending, because, becauseArgs);
    }

    /// <summary>
    /// Asserts the current collection does not have all elements in ascending order. Elements are compared
    /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    public AndConstraint<TAssertions> NotBeInAscendingOrder(string because = "", params object[] becauseArgs)
    {
        return NotBeInAscendingOrder(GetComparer<T>(), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that a collection is not ordered in ascending order according to the provided lambda expression.
    /// </summary>
    /// <param name="comparison">
    /// A lambda expression that should be used to determine the expected ordering between two objects.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    public AndConstraint<TAssertions> NotBeInAscendingOrder(Func<T, T, int> comparison,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return NotBeInOrder(Comparer<T>.Create((x, y) => comparison(x, y)), SortOrder.Ascending, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that a collection is not ordered in descending order according to the value of the specified
    /// <paramref name="propertyExpression"/>.
    /// </summary>
    /// <param name="propertyExpression">
    /// A lambda expression that references the property that should be used to determine the expected ordering.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    public AndConstraint<TAssertions> NotBeInDescendingOrder<TSelector>(
        Expression<Func<T, TSelector>> propertyExpression, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return NotBeInDescendingOrder(propertyExpression, GetComparer<TSelector>(), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that a collection is not ordered in descending order according to the value of the specified
    /// <see cref="IComparer{T}"/> implementation.
    /// </summary>
    /// <param name="comparer">
    /// The object that should be used to determine the expected ordering.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotBeInDescendingOrder(
        IComparer<T> comparer, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer, nameof(comparer),
            "Cannot assert collection ordering without specifying a comparer.");

        return NotBeInOrder(comparer, SortOrder.Descending, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that a collection not is ordered in descending order according to the value of the specified
    /// <paramref name="propertyExpression"/> and <see cref="IComparer{T}"/> implementation.
    /// </summary>
    /// <param name="propertyExpression">
    /// A lambda expression that references the property that should be used to determine the expected ordering.
    /// </param>
    /// <param name="comparer">
    /// The object that should be used to determine the expected ordering.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotBeInDescendingOrder<TSelector>(
        Expression<Func<T, TSelector>> propertyExpression, IComparer<TSelector> comparer,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer, nameof(comparer),
            "Cannot assert collection ordering without specifying a comparer.");

        return NotBeOrderedBy(propertyExpression, comparer, SortOrder.Descending, because, becauseArgs);
    }

    /// <summary>
    /// Asserts the current collection does not have all elements in descending order. Elements are compared
    /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    public AndConstraint<TAssertions> NotBeInDescendingOrder(string because = "", params object[] becauseArgs)
    {
        return NotBeInDescendingOrder(GetComparer<T>(), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that a collection is not ordered in descending order according to the provided lambda expression.
    /// </summary>
    /// <param name="comparison">
    /// A lambda expression that should be used to determine the expected ordering between two objects.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
    /// </remarks>
    public AndConstraint<TAssertions> NotBeInDescendingOrder(Func<T, T, int> comparison,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return NotBeInOrder(Comparer<T>.Create((x, y) => comparison(x, y)), SortOrder.Descending, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the collection is not null and contains at least 1 item.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeNullOrEmpty(string because = "", params object[] becauseArgs)
    {
        return NotBeNull(because, becauseArgs)
            .And.NotBeEmpty(because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the collection is not a subset of the <paramref name="unexpectedSuperset" />.
    /// </summary>
    /// <param name="unexpectedSuperset">An <see cref="IEnumerable{T}"/> with the unexpected superset.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeSubsetOf(IEnumerable<T> unexpectedSuperset,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Cannot assert a <null> collection against a subset.");

        if (assertionChain.Succeeded)
        {
            if (ReferenceEquals(Subject, unexpectedSuperset))
            {
                assertionChain
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Did not expect {context:collection} {0} to be a subset of {1}{reason}, but they both reference the same object.",
                        Subject,
                        unexpectedSuperset);
            }

            ICollection<T> actualItems = Subject.ConvertOrCastToCollection();

            if (actualItems.Intersect(unexpectedSuperset).Count() == actualItems.Count)
            {
                assertionChain
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {context:collection} {0} to be a subset of {1}{reason}.", actualItems,
                        unexpectedSuperset);
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current collection does not contain the supplied <paramref name="unexpected" /> item.
    /// </summary>
    /// <param name="unexpected">The element that is not expected to be in the collection</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> NotContain(T unexpected, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to not contain {0}{reason}, but found <null>.", unexpected);

        if (assertionChain.Succeeded)
        {
            ICollection<T> collection = Subject.ConvertOrCastToCollection();

            if (collection.Contains(unexpected))
            {
                assertionChain
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} {0} to not contain {1}{reason}.", collection, unexpected);
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the collection does not contain any items that match the predicate.
    /// </summary>
    /// <param name="predicate">A predicate to match the items in the collection against.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotContain(Expression<Func<T, bool>> predicate,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(predicate);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} not to contain {0}{reason}, but found <null>.", predicate.Body);

        if (assertionChain.Succeeded)
        {
            Func<T, bool> compiledPredicate = predicate.Compile();
            IEnumerable<T> unexpectedItems = Subject!.Where(item => compiledPredicate(item));

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(!unexpectedItems.Any())
                .FailWith("Expected {context:collection} {0} to not have any items matching {1}{reason}, but found {2}.",
                    Subject, predicate, unexpectedItems);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current collection does not contain the supplied items. Elements are compared
    /// using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="unexpected">An <see cref="IEnumerable{T}"/> with the unexpected elements.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="unexpected"/> is empty.</exception>
    public AndConstraint<TAssertions> NotContain(IEnumerable<T> unexpected, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot verify non-containment against a <null> collection");

        ICollection<T> unexpectedObjects = unexpected.ConvertOrCastToCollection();

        Guard.ThrowIfArgumentIsEmpty(unexpectedObjects, nameof(unexpected),
            "Cannot verify non-containment against an empty collection");

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to not contain {0}{reason}, but found <null>.", unexpected);

        if (assertionChain.Succeeded)
        {
            IEnumerable<T> foundItems = unexpectedObjects.Intersect(Subject!);

            if (foundItems.Any())
            {
                if (unexpectedObjects.Count > 1)
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} {0} to not contain {1}{reason}, but found {2}.", Subject,
                            unexpected, foundItems);
                }
                else
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} {0} to not contain {1}{reason}.",
                            Subject, unexpectedObjects.First());
                }
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that no element in the collection is equivalent to <paramref name="unexpected"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Important:</b> You cannot use this method to assert whether a subset of the collection is not equivalent to the <paramref name="unexpected"/>.
    /// This usually means that the expectation is meant to be a <i>single</i> item.
    /// </para>
    /// <para>
    /// By default, objects within the collection are seen as not equivalent to the expected object when both object graphs have unequally named properties with the same
    /// value, irrespective of the type of those objects.
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionConfiguration"/>.
    /// </para>
    /// </remarks>
    /// <param name="unexpected">The unexpected element.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> NotContainEquivalentOf<TExpectation>(TExpectation unexpected,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return NotContainEquivalentOf(unexpected, config => config, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that no element in the collection is equivalent to <paramref name="unexpected"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Important:</b> You cannot use this method to assert whether a subset of the collection is not equivalent to the <paramref name="unexpected"/>.
    /// This usually means that the expectation is meant to be a <i>single</i> item.
    /// </para>
    /// <para>
    /// By default, objects within the collection are seen as not equivalent to the expected object when both object graphs have unequally named properties with the same
    /// value, irrespective of the type of those objects.
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionConfiguration"/>.
    /// </para>
    /// </remarks>
    /// <param name="unexpected">The unexpected element.</param>
    /// <param name="config">
    /// A reference to the <see cref="EquivalencyOptions{TExpectation}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{TExpectation}"/> class. The global defaults are determined by the
    /// <see cref="AssertionConfiguration"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="config"/> is <see langword="null"/>.</exception>
    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "Needs refactoring")]
    public AndConstraint<TAssertions> NotContainEquivalentOf<TExpectation>(TExpectation unexpected,
        Func<EquivalencyOptions<TExpectation>,
            EquivalencyOptions<TExpectation>> config, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(config);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} not to contain equivalent of {0}{reason}, but collection is <null>.",
                unexpected);

        if (assertionChain.Succeeded)
        {
            EquivalencyOptions<TExpectation> options = config(AssertionConfiguration.Current.Equivalency.CloneDefaults<TExpectation>());

            var foundIndices = new List<int>();

            using (var scope = new AssertionScope())
            {
                int index = 0;

                foreach (T actualItem in Subject!)
                {
                    var context =
                        new EquivalencyValidationContext(Node.From<TExpectation>(() => CurrentAssertionChain.CallerIdentifier),
                            options)
                        {
                            Reason = new Reason(because, becauseArgs),
                            TraceWriter = options.TraceWriter
                        };

                    var comparands = new Comparands
                    {
                        Subject = actualItem,
                        Expectation = unexpected,
                        CompileTimeType = typeof(TExpectation),
                    };

                    new EquivalencyValidator().AssertEquality(comparands, context);

                    string[] failures = scope.Discard();

                    if (failures.Length == 0)
                    {
                        foundIndices.Add(index);
                    }

                    index++;
                }
            }

            if (foundIndices.Count > 0)
            {
                using (new AssertionScope())
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .WithReportable("configuration", () => options.ToString())
                        .WithExpectation("Expected {context:collection} {0} not to contain equivalent of {1}{reason}, ", Subject,
                            unexpected, chain =>
                            {
                                if (foundIndices.Count == 1)
                                {
                                    chain.FailWith("but found one at index {0}.", foundIndices[0]);
                                }
                                else
                                {
                                    chain.FailWith("but found several at indices {0}.", foundIndices);
                                }
                            });
                }
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts the current collection does not contain the specified elements in the exact same order, not necessarily consecutive.
    /// </summary>
    /// <remarks>
    /// Elements are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </remarks>
    /// <param name="unexpected">A <see cref="Array"/> with the unexpected elements.</param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotContainInOrder(params T[] unexpected)
    {
        return NotContainInOrder(unexpected, string.Empty);
    }

    /// <summary>
    /// Asserts the current collection does not contain the specified elements in the exact same order, not necessarily consecutive.
    /// </summary>
    /// <remarks>
    /// Elements are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </remarks>
    /// <param name="unexpected">An <see cref="IEnumerable{T}"/> with the unexpected elements.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotContainInOrder(IEnumerable<T> unexpected,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected),
            "Cannot verify absence of ordered containment against a <null> collection.");

        if (Subject is null)
        {
            assertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith("Cannot verify absence of ordered containment in a <null> collection.");

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        IList<T> unexpectedItems = unexpected.ConvertOrCastToList();

        if (unexpectedItems.Any())
        {
            IList<T> actualItems = Subject.ConvertOrCastToList();
            int subjectIndex = 0;

            foreach (var unexpectedItem in unexpectedItems)
            {
                subjectIndex = IndexOf(actualItems, unexpectedItem, startIndex: subjectIndex);

                if (subjectIndex == -1)
                {
                    return new AndConstraint<TAssertions>((TAssertions)this);
                }
            }

            assertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:collection} {0} to not contain items {1} in order{reason}, " +
                    "but items appeared in order ending at index {2}.",
                    Subject, unexpected, subjectIndex - 1);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts the current collection does not contain the specified elements in the exact same order and are consecutive.
    /// </summary>
    /// <remarks>
    /// Elements are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </remarks>
    /// <param name="unexpected">A <see cref="Array"/> with the unexpected elements.</param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotContainInConsecutiveOrder(params T[] unexpected)
    {
        return NotContainInConsecutiveOrder(unexpected, string.Empty);
    }

    /// <summary>
    /// Asserts the current collection does not contain the specified elements in the exact same order and consecutively.
    /// </summary>
    /// <remarks>
    /// Elements are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </remarks>
    /// <param name="unexpected">An <see cref="IEnumerable{T}"/> with the unexpected elements.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotContainInConsecutiveOrder(IEnumerable<T> unexpected,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected),
            "Cannot verify absence of ordered containment against a <null> collection.");

        if (Subject is null)
        {
            assertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith("Cannot verify absence of ordered containment in a <null> collection.");

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        IList<T> unexpectedItems = unexpected.ConvertOrCastToList();

        if (unexpectedItems.Any())
        {
            IList<T> actualItems = Subject.ConvertOrCastToList();

            if (unexpectedItems.Count > actualItems.Count)
            {
                return new AndConstraint<TAssertions>((TAssertions)this);
            }

            int subjectIndex = 0;

            while (subjectIndex != -1)
            {
                subjectIndex = IndexOf(actualItems, unexpectedItems[0], startIndex: subjectIndex);

                if (subjectIndex != -1)
                {
                    int consecutiveItems = ConsecutiveItemCount(actualItems, unexpectedItems, startIndex: subjectIndex);

                    if (consecutiveItems == unexpectedItems.Count)
                    {
                        assertionChain
                            .BecauseOf(because, becauseArgs)
                            .FailWith(
                                "Expected {context:collection} {0} to not contain items {1} in consecutive order{reason}, " +
                                "but items appeared in order ending at index {2}.",
                                Subject, unexpectedItems, (subjectIndex + consecutiveItems) - 2);
                    }

                    subjectIndex++;
                }
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the collection does not contain any <see langword="null"/> items.
    /// </summary>
    /// <param name="predicate">The predicate when evaluated should not be null.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotContainNulls<TKey>(Expression<Func<T, TKey>> predicate,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
        where TKey : class
    {
        Guard.ThrowIfArgumentIsNull(predicate);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} not to contain <null>s{reason}, but collection is <null>.");

        if (assertionChain.Succeeded)
        {
            Func<T, TKey> compiledPredicate = predicate.Compile();

            T[] values = Subject!
                .Where(e => compiledPredicate(e) is null)
                .ToArray();

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(values.Length == 0)
                .FailWith("Expected {context:collection} not to contain <null>s on {0}{reason}, but found {1}.",
                    predicate.Body, values);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the collection does not contain any <see langword="null"/> items.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotContainNulls(string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} not to contain <null>s{reason}, but collection is <null>.");

        if (assertionChain.Succeeded)
        {
            int[] indices = Subject!
                .Select((item, index) => (Item: item, Index: index))
                .Where(e => e.Item is null)
                .Select(e => e.Index)
                .ToArray();

            if (indices.Length > 0)
            {
                if (indices.Length > 1)
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith(
                            "Expected {context:collection} not to contain <null>s{reason}, but found several at indices {0}.",
                            indices);
                }
                else
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} not to contain <null>s{reason}, but found one at index {0}.",
                            indices[0]);
                }
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Expects the current collection not to contain all the same elements in the same order as the collection identified by
    /// <paramref name="unexpected" />. Elements are compared using their <see cref="object.Equals(object)" />.
    /// </summary>
    /// <param name="unexpected">An <see cref="IEnumerable{T}"/> with the elements that are not expected.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotEqual(IEnumerable<T> unexpected, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot compare collection with <null>.");

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected collections not to be equal{reason}, ", chain => chain
                .Given(() => Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("but found <null>.")
                .Then
                .ForCondition(subject => !ReferenceEquals(subject, unexpected))
                .FailWith("but they both reference the same object."))
            .Then
            .Given(() => Subject.ConvertOrCastToCollection())
            .ForCondition(actualItems => !actualItems.SequenceEqual(unexpected))
            .FailWith("Did not expect collections {0} and {1} to be equal{reason}.", _ => unexpected,
                actualItems => actualItems);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the number of items in the collection does not match the supplied <paramref name="unexpected" /> amount.
    /// </summary>
    /// <param name="unexpected">The unexpected number of items in the collection.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHaveCount(int unexpected, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to not contain {0} item(s){reason}, ", unexpected, chain => chain
                .Given(() => Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("but found <null>.")
                .Then
                .Given(subject => subject.Count())
                .ForCondition(actualCount => actualCount != unexpected)
                .FailWith("but found {0}.", actualCount => actualCount));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Assert that the current collection does not have the same number of elements as <paramref name="otherCollection" />.
    /// </summary>
    /// <param name="otherCollection">The other collection with the unexpected number of elements</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="otherCollection"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotHaveSameCount<TExpectation>(IEnumerable<TExpectation> otherCollection,
        string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection), "Cannot verify count against a <null> collection.");

        assertionChain
            .BecauseOf(because, becauseArgs)
            .Given(() => Subject)
            .ForCondition(subject => subject is not null)
            .FailWith(
                "Expected {context:collection} to not have the same count as {0}{reason}, but found <null>.",
                otherCollection)
            .Then
            .ForCondition(subject => !ReferenceEquals(subject, otherCollection))
            .FailWith(
                "Expected {context:collection} {0} to not have the same count as {1}{reason}, but they both reference the same object.",
                subject => subject, _ => otherCollection)
            .Then
            .Given(subject => (actual: subject.Count(), expected: otherCollection.Count()))
            .ForCondition(count => count.actual != count.expected)
            .FailWith(
                "Expected {context:collection} to not have {0} item(s){reason}, but found {1}.",
                count => count.expected, count => count.actual);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the collection does not share any items with the specified <paramref name="otherCollection"/>.
    /// </summary>
    /// <param name="otherCollection">The <see cref="IEnumerable{T}"/> to compare to.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="otherCollection"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotIntersectWith(IEnumerable<T> otherCollection,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection),
            "Cannot verify intersection against a <null> collection.");

        assertionChain
            .BecauseOf(because, becauseArgs)
            .Given(() => Subject)
            .ForCondition(subject => subject is not null)
            .FailWith(
                "Did not expect {context:collection} to intersect with {0}{reason}, but found <null>.",
                otherCollection)
            .Then
            .ForCondition(subject => !ReferenceEquals(subject, otherCollection))
            .FailWith(
                "Did not expect {context:collection} {0} to intersect with {1}{reason}, but they both reference the same object.",
                subject => subject, _ => otherCollection)
            .Then
            .Given(subject => subject.Intersect(otherCollection))
            .ForCondition(sharedItems => !sharedItems.Any())
            .FailWith(
                "Did not expect {context:collection} to intersect with {0}{reason}, but found the following shared items {1}.",
                _ => otherCollection, sharedItems => sharedItems);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the collection only contains items that match a predicate.
    /// </summary>
    /// <param name="predicate">A predicate to match the items in the collection against.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> OnlyContain(
        Expression<Func<T, bool>> predicate, [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(predicate);

        Func<T, bool> compiledPredicate = predicate.Compile();

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to contain only items matching {0}{reason}, ", predicate.Body,
                chain => chain
                    .Given(() => Subject)
                    .ForCondition(subject => subject is not null)
                    .FailWith("but the collection is <null>.")
                    .Then
                    .Given(subject => subject.ConvertOrCastToCollection().Where(item => !compiledPredicate(item)))
                    .ForCondition(mismatchingItems => !mismatchingItems.Any())
                    .FailWith("but {0} do(es) not match.", mismatchingItems => mismatchingItems));

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the collection does not contain any duplicate items.
    /// </summary>
    /// <param name="predicate">The predicate to group the items by.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> OnlyHaveUniqueItems<TKey>(Expression<Func<T, TKey>> predicate,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(predicate);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to only have unique items{reason}, but found <null>.");

        if (assertionChain.Succeeded)
        {
            Func<T, TKey> compiledPredicate = predicate.Compile();

            IGrouping<TKey, T>[] groupWithMultipleItems = Subject!
                .GroupBy(compiledPredicate)
                .Where(g => g.Count() > 1)
                .ToArray();

            if (groupWithMultipleItems.Length > 0)
            {
                if (groupWithMultipleItems.Length > 1)
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith(
                            "Expected {context:collection} to only have unique items on {0}{reason}, but items {1} are not unique.",
                            predicate.Body,
                            groupWithMultipleItems.SelectMany(g => g));
                }
                else
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith(
                            "Expected {context:collection} to only have unique items on {0}{reason}, but item {1} is not unique.",
                            predicate.Body,
                            groupWithMultipleItems[0].First());
                }
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the collection does not contain any duplicate items.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> OnlyHaveUniqueItems(string because = "", params object[] becauseArgs)
    {
        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to only have unique items{reason}, but found <null>.");

        if (assertionChain.Succeeded)
        {
            T[] groupWithMultipleItems = Subject!
                .GroupBy(o => o)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToArray();

            if (groupWithMultipleItems.Length > 0)
            {
                if (groupWithMultipleItems.Length > 1)
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith(
                            "Expected {context:collection} to only have unique items{reason}, but items {0} are not unique.",
                            groupWithMultipleItems);
                }
                else
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} to only have unique items{reason}, but item {0} is not unique.",
                            groupWithMultipleItems[0]);
                }
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a collection contains only items which meet
    /// the criteria provided by the inspector.
    /// </summary>
    /// <param name="expected">
    /// The element inspector, which inspects each element in turn.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> AllSatisfy(Action<T> expected, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot verify against a <null> inspector");

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to contain only items satisfying the inspector{reason}, ",
                chain => chain
                    .Given(() => Subject)
                    .ForCondition(subject => subject is not null)
                    .FailWith("but collection is <null>."));

        if (assertionChain.Succeeded)
        {
            string[] failuresFromInspectors;

            using (CallerIdentifier.OverrideStackSearchUsingCurrentScope())
            {
                var elementInspectors = Subject.Select(_ => expected);
                failuresFromInspectors = CollectFailuresFromInspectors(elementInspectors);
            }

            if (failuresFromInspectors.Length > 0)
            {
                string failureMessage = Environment.NewLine
                    + string.Join(Environment.NewLine, failuresFromInspectors.Select(x => x.IndentLines()));

                assertionChain
                    .BecauseOf(because, becauseArgs)
                    .WithExpectation("Expected {context:collection} to contain only items satisfying the inspector{reason}:",
                        chain => chain
                            .FailWithPreFormatted(failureMessage));
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a collection contains exactly a given number of elements, which meet
    /// the criteria provided by the element inspectors.
    /// </summary>
    /// <param name="elementInspectors">
    /// The element inspectors, which inspect each element in turn. The
    /// total number of element inspectors must exactly match the number of elements in the collection.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="elementInspectors"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="elementInspectors"/> is empty.</exception>
    public AndConstraint<TAssertions> SatisfyRespectively(params Action<T>[] elementInspectors)
    {
        return SatisfyRespectively(elementInspectors, string.Empty);
    }

    /// <summary>
    /// Asserts that a collection contains exactly a given number of elements, which meet
    /// the criteria provided by the element inspectors.
    /// </summary>
    /// <param name="expected">
    /// The element inspectors, which inspect each element in turn. The
    /// total number of element inspectors must exactly match the number of elements in the collection.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="expected"/> is empty.</exception>
    public AndConstraint<TAssertions> SatisfyRespectively(IEnumerable<Action<T>> expected,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot verify against a <null> collection of inspectors");

        ICollection<Action<T>> elementInspectors = expected.ConvertOrCastToCollection();

        Guard.ThrowIfArgumentIsEmpty(elementInspectors, nameof(expected),
            "Cannot verify against an empty collection of inspectors");

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to satisfy all inspectors{reason}, ", chain => chain
                .Given(() => Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("but collection is <null>.")
                .Then
                .ForCondition(subject => subject.Any())
                .FailWith("but collection is empty."))
            .Then
            .Given(() => (elements: Subject.Count(), inspectors: elementInspectors.Count))
            .ForCondition(count => count.elements == count.inspectors)
            .FailWith(
                "Expected {context:collection} to contain exactly {0} items{reason}, but it contains {1} items",
                count => count.inspectors, count => count.elements);

        if (assertionChain.Succeeded)
        {
            string[] failuresFromInspectors;

            using (CallerIdentifier.OverrideStackSearchUsingCurrentScope())
            {
                failuresFromInspectors = CollectFailuresFromInspectors(elementInspectors);
            }

            if (failuresFromInspectors.Length > 0)
            {
                string failureMessage = Environment.NewLine
                    + string.Join(Environment.NewLine, failuresFromInspectors.Select(x => x.IndentLines()));

                assertionChain
                    .BecauseOf(because, becauseArgs)
                    .WithExpectation(
                        "Expected {context:collection} to satisfy all inspectors{reason}, but some inspectors are not satisfied:",
                        chain => chain
                            .FailWithPreFormatted(failureMessage));
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a collection contains exactly a given number of elements which meet
    /// the criteria provided by the element predicates. Assertion fails if it is not possible
    /// to find a one-to-one mapping between the elements of the collection and the predicates.
    /// The order of the predicates does not need to match the order of the elements.
    /// </summary>
    /// <param name="predicates">
    /// The predicates that the elements of the collection must match.
    /// The total number of predicates must exactly match the number of elements in the collection.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="predicates"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="predicates"/> is empty.</exception>
    public AndConstraint<TAssertions> Satisfy(params Expression<Func<T, bool>>[] predicates)
    {
        return Satisfy(predicates, because: string.Empty);
    }

    /// <summary>
    /// Asserts that a collection contains exactly a given number of elements which meet
    /// the criteria provided by the element predicates. Assertion fails if it is not possible
    /// to find a one-to-one mapping between the elements of the collection and the predicates.
    /// </summary>
    /// <param name="predicates">
    /// The predicates that the elements of the collection must match.
    /// The total number of predicates must exactly match the number of elements in the collection.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="predicates"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="predicates"/> is empty.</exception>
    public AndConstraint<TAssertions> Satisfy(IEnumerable<Expression<Func<T, bool>>> predicates,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(predicates, nameof(predicates), "Cannot verify against a <null> collection of predicates");

        IList<Expression<Func<T, bool>>> predicatesList = predicates.ConvertOrCastToList();

        Guard.ThrowIfArgumentIsEmpty(predicatesList, nameof(predicates),
            "Cannot verify against an empty collection of predicates");

        assertionChain
            .BecauseOf(because, becauseArgs)
            .Given(() => Subject)
            .ForCondition(subject => subject is not null)
            .FailWith("Expected {context:collection} to satisfy all predicates{reason}, but collection is <null>.")
            .Then
            .ForCondition(subject => subject.Any())
            .FailWith("Expected {context:collection} to satisfy all predicates{reason}, but collection is empty.");

        if (assertionChain.Succeeded)
        {
            MaximumMatchingSolution<T> maximumMatchingSolution = new MaximumMatchingProblem<T>(predicatesList, Subject).Solve();

            if (maximumMatchingSolution.UnmatchedPredicatesExist || maximumMatchingSolution.UnmatchedElementsExist)
            {
                string message = string.Empty;
                var doubleNewLine = Environment.NewLine + Environment.NewLine;

                List<MaximumMatching.Predicate<T>> unmatchedPredicates = maximumMatchingSolution.GetUnmatchedPredicates();

                if (unmatchedPredicates.Count > 0)
                {
                    message += doubleNewLine + "The following predicates did not have matching elements:";

                    message += doubleNewLine +
                        string.Join(Environment.NewLine,
                            unmatchedPredicates.Select(predicate => Formatter.ToString(predicate.Expression)));
                }

                List<Element<T>> unmatchedElements = maximumMatchingSolution.GetUnmatchedElements();

                if (unmatchedElements.Count > 0)
                {
                    message += doubleNewLine + "The following elements did not match any predicate:";

                    IEnumerable<string> elementDescriptions = unmatchedElements
                        .Select(element => $"Index: {element.Index}, Element: {Formatter.ToString(element.Value)}");

                    message += doubleNewLine + string.Join(doubleNewLine, elementDescriptions);
                }

                assertionChain
                    .BecauseOf(because, becauseArgs)
                    .WithExpectation("Expected {context:collection} to satisfy all predicates{reason}, but:", chain => chain
                        .FailWithPreFormatted(message));
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current collection starts with same elements in the same order as the collection identified by
    /// <paramref name="expectation" />. Elements are compared using their <see cref="object.Equals(object)" />.
    /// </summary>
    /// <param name="expectation">
    /// A collection of expected elements.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expectation"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> StartWith(IEnumerable<T> expectation, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return StartWith(expectation, (a, b) => EqualityComparer<T>.Default.Equals(a, b), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current collection starts with same elements in the same order as the collection identified by
    /// <paramref name="expectation" />. Elements are compared using <paramref name="equalityComparison"/>.
    /// </summary>
    /// <param name="expectation">
    /// A collection of expected elements.
    /// </param>
    /// <param name="equalityComparison">
    /// A equality comparison the is used to determine whether two objects should be treated as equal.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expectation"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> StartWith<TExpectation>(
        IEnumerable<TExpectation> expectation, Func<T, TExpectation, bool> equalityComparison,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectation, nameof(expectation), "Cannot compare collection with <null>.");

        AssertCollectionStartsWith(Subject, expectation.ConvertOrCastToCollection(), equalityComparison, because, becauseArgs);
        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the collection starts with the specified <paramref name="element"/>.
    /// </summary>
    /// <param name="element">
    /// The element that is expected to appear at the start of the collection. The object's <see cref="object.Equals(object)"/>
    /// is used to compare the element.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> StartWith(T element, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        return StartWith([element], ObjectExtensions.GetComparer<T>(), because, becauseArgs);
    }

    internal AndConstraint<SubsequentOrderingAssertions<T>> BeOrderedBy<TSelector>(
        Expression<Func<T, TSelector>> propertyExpression,
        IComparer<TSelector> comparer,
        SortOrder direction,
        string because,
        object[] becauseArgs)
    {
        if (IsValidProperty(propertyExpression, because, becauseArgs))
        {
            ICollection<T> unordered = Subject.ConvertOrCastToCollection();

            IOrderedEnumerable<T> expectation = GetOrderedEnumerable(
                propertyExpression,
                comparer,
                direction,
                unordered);

            assertionChain
                .ForCondition(unordered.SequenceEqual(expectation))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} {0} to be ordered {1}{reason} and result in {2}.",
                    () => Subject, () => GetExpressionOrderString(propertyExpression), () => expectation);

            return new AndConstraint<SubsequentOrderingAssertions<T>>(
                new SubsequentOrderingAssertions<T>(Subject, expectation, assertionChain));
        }

        return new AndConstraint<SubsequentOrderingAssertions<T>>(
            new SubsequentOrderingAssertions<T>(Subject, Enumerable.Empty<T>().OrderBy(x => x), assertionChain));
    }

    internal virtual IOrderedEnumerable<T> GetOrderedEnumerable<TSelector>(
        Expression<Func<T, TSelector>> propertyExpression,
        IComparer<TSelector> comparer,
        SortOrder direction,
        ICollection<T> unordered)
    {
        Func<T, TSelector> keySelector = propertyExpression.Compile();

        IOrderedEnumerable<T> expectation = direction == SortOrder.Ascending
            ? unordered.OrderBy(keySelector, comparer)
            : unordered.OrderByDescending(keySelector, comparer);

        return expectation;
    }

    protected static IEnumerable<TExpectation> RepeatAsManyAs<TExpectation>(TExpectation value, IEnumerable<T> enumerable)
    {
        if (enumerable is null)
        {
            return [];
        }

        return RepeatAsManyAsIterator(value, enumerable);
    }

    protected void AssertCollectionEndsWith<TActual, TExpectation>(IEnumerable<TActual> actual,
        ICollection<TExpectation> expected, Func<TActual, TExpectation, bool> equalityComparison,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(equalityComparison);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to end with {0}{reason}, ", expected, chain => chain
                .Given(() => actual)
                .AssertCollectionIsNotNull()
                .Then
                .AssertCollectionHasEnoughItems(expected.Count)
                .Then
                .AssertCollectionsHaveSameItems(expected, (a, e) =>
                {
                    int firstIndexToCompare = a.Count - e.Count;
                    int index = a.Skip(firstIndexToCompare).IndexOfFirstDifferenceWith(e, equalityComparison);
                    return index >= 0 ? index + firstIndexToCompare : index;
                }));
    }

    protected void AssertCollectionStartsWith<TActual, TExpectation>(IEnumerable<TActual> actualItems,
        ICollection<TExpectation> expected, Func<TActual, TExpectation, bool> equalityComparison,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(equalityComparison);

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected {context:collection} to start with {0}{reason}, ", expected, chain => chain
                .Given(() => actualItems)
                .AssertCollectionIsNotNull()
                .Then
                .AssertCollectionHasEnoughItems(expected.Count)
                .Then
                .AssertCollectionsHaveSameItems(expected,
                    (a, e) => a.Take(e.Count).IndexOfFirstDifferenceWith(e, equalityComparison)));
    }

    protected void AssertSubjectEquality<TExpectation>(IEnumerable<TExpectation> expectation,
        Func<T, TExpectation, bool> equalityComparison, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(equalityComparison);

        bool subjectIsNull = Subject is null;
        bool expectationIsNull = expectation is null;

        if (subjectIsNull && expectationIsNull)
        {
            return;
        }

        Guard.ThrowIfArgumentIsNull(expectation, nameof(expectation), "Cannot compare collection with <null>.");

        ICollection<TExpectation> expectedItems = expectation.ConvertOrCastToCollection();

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(!subjectIsNull)
            .FailWith("Expected {context:collection} to be equal to {0}{reason}, but found <null>.", expectedItems)
            .Then
            .WithExpectation("Expected {context:collection} to be equal to {0}{reason}, ", expectedItems, chain => chain
                .Given(() => Subject.ConvertOrCastToCollection())
                .AssertCollectionsHaveSameCount(expectedItems.Count)
                .Then
                .AssertCollectionsHaveSameItems(expectedItems, (a, e) => a.IndexOfFirstDifferenceWith(e, equalityComparison)));
    }

    private static string GetExpressionOrderString<TSelector>(Expression<Func<T, TSelector>> propertyExpression)
    {
        string orderString = propertyExpression.GetMemberPath().ToString();

        return orderString is "\"\"" ? string.Empty : "by " + orderString;
    }

    private static Type GetType<TType>(TType o)
    {
        return o is Type t ? t : o.GetType();
    }

    private static bool HasPredecessor(T successor, TCollection subject)
    {
        return !ReferenceEquals(subject.First(), successor);
    }

    private static bool HasSuccessor(T predecessor, TCollection subject)
    {
        return !ReferenceEquals(subject.Last(), predecessor);
    }

    private static T PredecessorOf(T successor, TCollection subject)
    {
        IList<T> collection = subject.ConvertOrCastToList();
        int index = collection.IndexOf(successor);
        return index > 0 ? collection[index - 1] : default;
    }

    private static IEnumerable<TExpectation> RepeatAsManyAsIterator<TExpectation>(TExpectation value, IEnumerable<T> enumerable)
    {
        using IEnumerator<T> enumerator = enumerable.GetEnumerator();

        while (enumerator.MoveNext())
        {
            yield return value;
        }
    }

    private static T SuccessorOf(T predecessor, TCollection subject)
    {
        IList<T> collection = subject.ConvertOrCastToList();
        int index = collection.IndexOf(predecessor);
        return index < (collection.Count - 1) ? collection[index + 1] : default;
    }

    private string[] CollectFailuresFromInspectors(IEnumerable<Action<T>> elementInspectors)
    {
        string[] collectionFailures;

        using (var collectionScope = new AssertionScope())
        {
            int index = 0;

            foreach ((T element, Action<T> inspector) in Subject.Zip(elementInspectors,
                         (element, inspector) => (element, inspector)))
            {
                string[] inspectorFailures;

                using (var itemScope = new AssertionScope())
                {
                    inspector(element);
                    inspectorFailures = itemScope.Discard();
                }

                if (inspectorFailures.Length > 0)
                {
                    // Adding one tab and removing trailing dot to allow nested SatisfyRespectively
                    string failures = string.Join(Environment.NewLine,
                        inspectorFailures.Select(x => x.IndentLines().TrimEnd('.')));

                    collectionScope.AddPreFormattedFailure($"At index {index}:{Environment.NewLine}{failures}");
                }

                index++;
            }

            collectionFailures = collectionScope.Discard();
        }

        return collectionFailures;
    }

    private bool IsValidProperty<TSelector>(Expression<Func<T, TSelector>> propertyExpression,
        [StringSyntax("CompositeFormat")] string because,
        object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(propertyExpression, nameof(propertyExpression),
            "Cannot assert collection ordering without specifying a property.");

        propertyExpression.ValidateMemberPath();

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to be ordered by {0}{reason} but found <null>.",
                () => propertyExpression.GetMemberPath());

        return assertionChain.Succeeded;
    }

    private AndConstraint<TAssertions> NotBeOrderedBy<TSelector>(
        Expression<Func<T, TSelector>> propertyExpression,
        IComparer<TSelector> comparer,
        SortOrder direction,
        string because,
        object[] becauseArgs)
    {
        if (IsValidProperty(propertyExpression, because, becauseArgs))
        {
            ICollection<T> unordered = Subject.ConvertOrCastToCollection();

            IOrderedEnumerable<T> expectation = GetOrderedEnumerable(
                propertyExpression,
                comparer,
                direction,
                unordered);

            assertionChain
                .ForCondition(!unordered.SequenceEqual(expectation))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} {0} to not be ordered {1}{reason} and not result in {2}.",
                    () => Subject, () => GetExpressionOrderString(propertyExpression), () => expectation);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Expects the current collection to have all elements in the specified <paramref name="expectedOrder"/>.
    /// Elements are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    private AndConstraint<SubsequentOrderingAssertions<T>> BeInOrder(
        IComparer<T> comparer, SortOrder expectedOrder, [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        string sortOrder = expectedOrder == SortOrder.Ascending ? "ascending" : "descending";

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith($"Expected {{context:collection}} to be in {sortOrder} order{{reason}}, but found <null>.");

        IOrderedEnumerable<T> ordering = Array.Empty<T>().OrderBy(x => x);

        if (assertionChain.Succeeded)
        {
            IList<T> actualItems = Subject.ConvertOrCastToList();

            ordering = expectedOrder == SortOrder.Ascending
                ? actualItems.OrderBy(item => item, comparer)
                : actualItems.OrderByDescending(item => item, comparer);

            T[] orderedItems = ordering.ToArray();
            Func<T, T, bool> areSameOrEqual = ObjectExtensions.GetComparer<T>();

            for (int index = 0; index < orderedItems.Length; index++)
            {
                if (!areSameOrEqual(actualItems[index], orderedItems[index]))
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} to be in " + sortOrder +
                            " order{reason}, but found {0} where item at index {1} is in wrong order.",
                            actualItems, index);

                    return new AndConstraint<SubsequentOrderingAssertions<T>>(
                        new SubsequentOrderingAssertions<T>(Subject, Enumerable.Empty<T>().OrderBy(x => x), assertionChain));
                }
            }
        }

        return new AndConstraint<SubsequentOrderingAssertions<T>>(
            new SubsequentOrderingAssertions<T>(Subject, ordering, assertionChain));
    }

    /// <summary>
    /// Asserts the current collection does not have all elements in ascending order. Elements are compared
    /// using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    private AndConstraint<TAssertions> NotBeInOrder(IComparer<T> comparer, SortOrder order,
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        string sortOrder = order == SortOrder.Ascending ? "ascending" : "descending";

        assertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith($"Did not expect {{context:collection}} to be in {sortOrder} order{{reason}}, but found <null>.");

        if (assertionChain.Succeeded)
        {
            IList<T> actualItems = Subject.ConvertOrCastToList();

            T[] orderedItems = order == SortOrder.Ascending
                ? actualItems.OrderBy(item => item, comparer).ToArray()
                : actualItems.OrderByDescending(item => item, comparer).ToArray();

            Func<T, T, bool> areSameOrEqual = ObjectExtensions.GetComparer<T>();

            bool itemsAreUnordered = actualItems
                .Where((actualItem, index) => !areSameOrEqual(actualItem, orderedItems[index]))
                .Any();

            assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(itemsAreUnordered)
                .FailWith(
                    "Did not expect {context:collection} to be in " + sortOrder + " order{reason}, but found {0}.",
                    actualItems);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        throw new NotSupportedException(
            "Equals is not part of Fluent Assertions. Did you mean BeSameAs(), Equal(), or BeEquivalentTo() instead?");

    private static int IndexOf(IList<T> items, T item, int startIndex)
    {
        Func<T, T, bool> comparer = ObjectExtensions.GetComparer<T>();

        for (; startIndex < items.Count; startIndex++)
        {
            if (comparer(items[startIndex], item))
            {
                startIndex++;
                return startIndex;
            }
        }

        return -1;
    }

    private static int ConsecutiveItemCount(IList<T> actualItems, IList<T> expectedItems, int startIndex)
    {
        for (var index = 1; index < expectedItems.Count; index++)
        {
            T unexpectedItem = expectedItems[index];

            int previousSubjectIndex = startIndex;
            startIndex = IndexOf(actualItems, unexpectedItem, startIndex: startIndex);

            if (startIndex == -1 || !previousSubjectIndex.IsConsecutiveTo(startIndex))
            {
                return index;
            }
        }

        return expectedItems.Count;
    }

    private protected static IComparer<TItem> GetComparer<TItem>() =>
        typeof(TItem) == typeof(string) ? (IComparer<TItem>)StringComparer.Ordinal : Comparer<TItem>.Default;
}
