using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Collections;

[DebuggerNonUserCode]
public class AsyncEnumerableAssertions<T> : AsyncEnumerableAssertions<IAsyncEnumerable<T>, T, AsyncEnumerableAssertions<T>>
{
    public AsyncEnumerableAssertions(IAsyncEnumerable<T> actualValue)
        : base(actualValue)
    {
    }
}

[DebuggerNonUserCode]
public class AsyncEnumerableAssertions<TCollection, T>
    : AsyncEnumerableAssertions<TCollection, T, AsyncEnumerableAssertions<TCollection, T>>
    where TCollection : IAsyncEnumerable<T>
{
    public AsyncEnumerableAssertions(TCollection actualValue)
        : base(actualValue)
    {
    }
}

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
[DebuggerNonUserCode]
public class AsyncEnumerableAssertions<TCollection, T, TAssertions> : ReferenceTypeAssertions<TCollection, TAssertions>
    where TCollection : IAsyncEnumerable<T>
    where TAssertions : AsyncEnumerableAssertions<TCollection, T, TAssertions>
{
    private readonly GenericCollectionAssertions<T> genericCollectionAssertions;

    public AsyncEnumerableAssertions(TCollection actualValue)
        : base(actualValue)
    {
        genericCollectionAssertions = new GenericCollectionAssertions<T>(actualValue.ToBlockingEnumerable());
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
    public AndWhichConstraint<TAssertions, IEnumerable<TExpectation>> AllBeAssignableTo<TExpectation>(string because = "",
        params object[] becauseArgs)
    {
        var matches = genericCollectionAssertions.GetAllBeAssignableToMatches<TExpectation>(because, becauseArgs);

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
    public AndConstraint<TAssertions> AllBeAssignableTo(Type expectedType, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectedType);

        genericCollectionAssertions.AssertAllBeAssignableTo(expectedType, because, becauseArgs);

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
    public AndConstraint<TAssertions> AllBeEquivalentTo<TExpectation>(TExpectation expectation,
        string because = "", params object[] becauseArgs)
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
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
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
    public AndConstraint<TAssertions> AllBeEquivalentTo<TExpectation>(TExpectation expectation,
        Func<EquivalencyOptions<TExpectation>, EquivalencyOptions<TExpectation>> config,
        string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(config);

        IAsyncEnumerable<TExpectation> repeatedExpectation = RepeatAsManyAs(expectation, genericCollectionAssertions.Subject).ToAsyncEnumerable();

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
    public AndWhichConstraint<TAssertions, IEnumerable<TExpectation>> AllBeOfType<TExpectation>(string because = "",
        params object[] becauseArgs)
    {
        var matches = genericCollectionAssertions.GetAllBeOfTypeMatches<TExpectation>(because, becauseArgs);

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
    public AndConstraint<TAssertions> AllBeOfType(Type expectedType, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectedType);

        genericCollectionAssertions.AssertAllBeOfType(expectedType, because, becauseArgs);

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
    public AndConstraint<TAssertions> BeEmpty(string because = "", params object[] becauseArgs)
    {
        genericCollectionAssertions.AssertBeEmpty(because, becauseArgs);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a collection of objects is equivalent to another collection of objects.
    /// </summary>
    /// <remarks>
    /// Objects within the collections are equivalent when both object graphs have equally named properties with the same
    /// value, irrespective of the type of those objects. Two properties are also equal if one type can be converted to another
    /// and the result is equal.
    /// The type of a collection property is ignored as long as the collection implements <see cref="IAsyncEnumerable{T}"/> and all
    /// items in the collection are structurally equal.
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
    /// </remarks>
    /// <param name="expectation">An <see cref="IAsyncEnumerable{T}"/> with the expected elements.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> BeEquivalentTo<TExpectation>(IAsyncEnumerable<TExpectation> expectation,
        string because = "", params object[] becauseArgs)
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
    /// The type of a collection property is ignored as long as the collection implements <see cref="IAsyncEnumerable{T}"/> and all
    /// items in the collection are structurally equal.
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
    /// </remarks>
    /// <param name="expectation">An <see cref="IAsyncEnumerable{T}"/> with the expected elements.</param>
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
    public AndConstraint<TAssertions> BeEquivalentTo<TExpectation>(IAsyncEnumerable<TExpectation> expectation,
        Func<EquivalencyOptions<TExpectation>, EquivalencyOptions<TExpectation>> config, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(config);

        EquivalencyOptions<IEnumerable<TExpectation>> options =
            config(AssertionOptions.CloneDefaults<TExpectation>()).AsCollection();

        var context =
            new EquivalencyValidationContext(Node.From<IEnumerable<TExpectation>>(() => AssertionScope.Current.CallerIdentity),
                options)
            {
                Reason = new Reason(because, becauseArgs),
                TraceWriter = options.TraceWriter,
            };

        var comparands = new Comparands
        {
            Subject = Subject,
            Expectation = expectation,
            CompileTimeType = typeof(IAsyncEnumerable<TExpectation>),
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
        Expression<Func<T, TSelector>> propertyExpression, string because = "", params object[] becauseArgs)
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
        IComparer<T> comparer, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer, nameof(comparer),
            "Cannot assert collection ordering without specifying a comparer.");

        return genericCollectionAssertions.BeInOrder(comparer, SortOrder.Ascending, because, becauseArgs);
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
        Expression<Func<T, TSelector>> propertyExpression, IComparer<TSelector> comparer, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer, nameof(comparer),
            "Cannot assert collection ordering without specifying a comparer.");

        return genericCollectionAssertions.BeOrderedBy(propertyExpression, comparer, SortOrder.Ascending, because, becauseArgs);
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
    public AndConstraint<SubsequentOrderingAssertions<T>> BeInAscendingOrder(string because = "", params object[] becauseArgs)
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
    public AndConstraint<SubsequentOrderingAssertions<T>> BeInAscendingOrder(Func<T, T, int> comparison, string because = "",
        params object[] becauseArgs)
    {
        return genericCollectionAssertions.BeInOrder(Comparer<T>.Create((x, y) => comparison(x, y)), SortOrder.Ascending, because, becauseArgs);
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
        Expression<Func<T, TSelector>> propertyExpression, string because = "", params object[] becauseArgs)
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
        IComparer<T> comparer, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer, nameof(comparer),
            "Cannot assert collection ordering without specifying a comparer.");

        return genericCollectionAssertions.BeInOrder(comparer, SortOrder.Descending, because, becauseArgs);
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
        Expression<Func<T, TSelector>> propertyExpression, IComparer<TSelector> comparer, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(comparer, nameof(comparer),
            "Cannot assert collection ordering without specifying a comparer.");

        return genericCollectionAssertions.BeOrderedBy(propertyExpression, comparer, SortOrder.Descending, because, becauseArgs);
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
    public AndConstraint<SubsequentOrderingAssertions<T>> BeInDescendingOrder(string because = "", params object[] becauseArgs)
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
    public AndConstraint<SubsequentOrderingAssertions<T>> BeInDescendingOrder(Func<T, T, int> comparison, string because = "",
        params object[] becauseArgs)
    {
        return genericCollectionAssertions.BeInOrder(Comparer<T>.Create((x, y) => comparison(x, y)), SortOrder.Descending, because, becauseArgs);
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
    public AndConstraint<TAssertions> BeNullOrEmpty(string because = "", params object[] becauseArgs)
    {
        genericCollectionAssertions.AssertBeNullOrEmpty(because, becauseArgs);

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
    public AndConstraint<TAssertions> BeSubsetOf(IEnumerable<T> expectedSuperset, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectedSuperset, nameof(expectedSuperset),
            "Cannot verify a subset against a <null> collection.");

        genericCollectionAssertions.AssertBeSubsetOf(expectedSuperset, because, becauseArgs);

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
    public AndWhichConstraint<TAssertions, T> Contain(T expected, string because = "", params object[] becauseArgs)
    {
        IEnumerable<T> matches = genericCollectionAssertions.GetContainMatches(expected, because, becauseArgs);

        return new AndWhichConstraint<TAssertions, T>((TAssertions)this, matches);
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
    public AndWhichConstraint<TAssertions, T> Contain(Expression<Func<T, bool>> predicate, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(predicate);

        IEnumerable<T> matches = genericCollectionAssertions.GetContainMatches(predicate, because, becauseArgs);

        return new AndWhichConstraint<TAssertions, T>((TAssertions)this, matches);
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
    public AndConstraint<TAssertions> Contain(IEnumerable<T> expected, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot verify containment against a <null> collection");

        genericCollectionAssertions.AssertContain(expected, because, becauseArgs);

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
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
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
    public AndWhichConstraint<TAssertions, T> ContainEquivalentOf<TExpectation>(TExpectation expectation, string because = "",
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
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
    /// </para>
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
    public AndWhichConstraint<TAssertions, T> ContainEquivalentOf<TExpectation>(TExpectation expectation,
        Func<EquivalencyOptions<TExpectation>,
            EquivalencyOptions<TExpectation>> config, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(config);

        var unmatchedItem = genericCollectionAssertions.GetContainEquivalentOfUnmatchedItem(expectation, config, because, becauseArgs);

        return new AndWhichConstraint<TAssertions, T>((TAssertions)this, unmatchedItem);
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
    public AndConstraint<TAssertions> ContainInOrder(IEnumerable<T> expected, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot verify ordered containment against a <null> collection.");

        genericCollectionAssertions.AssertContainInOrder(expected, because, becauseArgs);

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
    public AndConstraint<TAssertions> ContainInConsecutiveOrder(IEnumerable<T> expected, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot verify ordered containment against a <null> collection.");

        genericCollectionAssertions.AssertContainInConsecutiveOrder(expected, because, becauseArgs);

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
    public AndConstraint<TAssertions> ContainItemsAssignableTo<TExpectation>(string because = "", params object[] becauseArgs)
    {
        genericCollectionAssertions.AssertContainItemsAssignableTo<TExpectation>(because, becauseArgs);

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
    public AndConstraint<TAssertions> NotContainItemsAssignableTo<TExpectation>(string because = "", params object[] becauseArgs) =>
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
    public AndConstraint<TAssertions> NotContainItemsAssignableTo(Type type, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(type);

        genericCollectionAssertions.AssertNotContainItemsAssignableTo(type, because, becauseArgs);

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
        T match = genericCollectionAssertions.GetContainSingleMatch(because, becauseArgs);

        return new AndWhichConstraint<TAssertions, T>((TAssertions)this, match);
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

        T[] matches = genericCollectionAssertions.GetContainSingleMatches(predicate, because, becauseArgs);

        return new AndWhichConstraint<TAssertions, T>((TAssertions)this, matches);
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
    public AndConstraint<TAssertions> EndWith(IEnumerable<T> expectation, string because = "", params object[] becauseArgs)
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
        IEnumerable<TExpectation> expectation, Func<T, TExpectation, bool> equalityComparison, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectation, nameof(expectation), "Cannot compare collection with <null>.");

        genericCollectionAssertions.AssertEndWith(expectation, equalityComparison, because, becauseArgs);

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
    public AndConstraint<TAssertions> EndWith(T element, string because = "", params object[] becauseArgs)
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
        genericCollectionAssertions.AssertSubjectEquality(elements, ObjectExtensions.GetComparer<T>(), string.Empty);

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
        IEnumerable<TExpectation> expectation, Func<T, TExpectation, bool> equalityComparison, string because = "",
        params object[] becauseArgs)
    {
        genericCollectionAssertions.AssertSubjectEquality(expectation, equalityComparison, because, becauseArgs);

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
    public AndConstraint<TAssertions> Equal(IEnumerable<T> expected, string because = "", params object[] becauseArgs)
    {
        genericCollectionAssertions.AssertSubjectEquality(expected, ObjectExtensions.GetComparer<T>(), because, becauseArgs);

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
    public AndConstraint<TAssertions> HaveCount(int expected, string because = "", params object[] becauseArgs)
    {
        genericCollectionAssertions.AssertHaveCount(expected, because, becauseArgs);

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
    public AndConstraint<TAssertions> HaveCount(Expression<Func<int, bool>> countPredicate, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(countPredicate, nameof(countPredicate),
            "Cannot compare collection count against a <null> predicate.");

        genericCollectionAssertions.AssertHaveCount(countPredicate, because, becauseArgs);

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
    public AndConstraint<TAssertions> HaveCountGreaterThanOrEqualTo(int expected, string because = "",
        params object[] becauseArgs)
    {
        genericCollectionAssertions.AssertHaveCountGreaterThanOrEqualTo(expected, because, becauseArgs);

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
    public AndConstraint<TAssertions> HaveCountGreaterThan(int expected, string because = "", params object[] becauseArgs)
    {
        genericCollectionAssertions.AssertHaveCountGreaterThan(expected, because, becauseArgs);

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
    public AndConstraint<TAssertions> HaveCountLessThanOrEqualTo(int expected, string because = "", params object[] becauseArgs)
    {
        genericCollectionAssertions.AssertHaveCountLessThanOrEqualTo(expected, because, becauseArgs);

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
    public AndConstraint<TAssertions> HaveCountLessThan(int expected, string because = "", params object[] becauseArgs)
    {
        genericCollectionAssertions.AssertHaveCountLessThan(expected, because, becauseArgs);

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
    public AndWhichConstraint<TAssertions, T> HaveElementAt(int index, T element, string because = "",
        params object[] becauseArgs)
    {
        T actual = genericCollectionAssertions.GetElementAt(index, element, because, becauseArgs);

        return new AndWhichConstraint<TAssertions, T>((TAssertions)this, actual);
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
    public AndConstraint<TAssertions> HaveElementPreceding(T successor, T expectation, string because = "",
        params object[] becauseArgs)
    {
        genericCollectionAssertions.AssertHaveElementPreceding(successor, expectation, because, becauseArgs);

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
    public AndConstraint<TAssertions> HaveElementSucceeding(T predecessor, T expectation, string because = "",
        params object[] becauseArgs)
    {
        genericCollectionAssertions.AssertHaveElementSucceeding(predecessor, expectation, because, becauseArgs);

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
    public AndConstraint<TAssertions> HaveSameCount<TExpectation>(IEnumerable<TExpectation> otherCollection, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection), "Cannot verify count against a <null> collection.");

        genericCollectionAssertions.AssertHaveSameCount(otherCollection, because, becauseArgs);

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
    public AndConstraint<TAssertions> IntersectWith(IEnumerable<T> otherCollection, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection),
            "Cannot verify intersection against a <null> collection.");

        genericCollectionAssertions.AssertIntersectWith(otherCollection, because, becauseArgs);

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
        genericCollectionAssertions.AssertNotBeEmpty(because, becauseArgs);

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
    public AndConstraint<TAssertions> NotBeEquivalentTo<TExpectation>(IEnumerable<TExpectation> unexpected, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot verify inequivalence against a <null> collection.");

        if (Subject is null)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} not to be equivalent{reason}, but found <null>.");
        }

        if (ReferenceEquals(Subject, unexpected))
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:collection} {0} not to be equivalent with collection {1}{reason}, but they both reference the same object.",
                    Subject,
                    unexpected);
        }

        return NotBeEquivalentTo(unexpected.ConvertOrCastToList().ToAsyncEnumerable(), config => config, because, becauseArgs);
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
    /// <see cref="AssertionOptions"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeEquivalentTo<TExpectation>(IAsyncEnumerable<TExpectation> unexpected,
        Func<EquivalencyOptions<TExpectation>, EquivalencyOptions<TExpectation>> config,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot verify inequivalence against a <null> collection.");

        if (Subject is null)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} not to be equivalent{reason}, but found <null>.");
        }

        string[] failures;

        using (var scope = new AssertionScope())
        {
            BeEquivalentTo(unexpected, config);

            failures = scope.Discard();
        }

        Execute.Assertion
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
        Expression<Func<T, TSelector>> propertyExpression, string because = "", params object[] becauseArgs)
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
        IComparer<T> comparer, string because = "", params object[] becauseArgs)
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
        Expression<Func<T, TSelector>> propertyExpression, IComparer<TSelector> comparer, string because = "",
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
    public AndConstraint<TAssertions> NotBeInAscendingOrder(Func<T, T, int> comparison, string because = "",
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
        Expression<Func<T, TSelector>> propertyExpression, string because = "", params object[] becauseArgs)
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
        IComparer<T> comparer, string because = "", params object[] becauseArgs)
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
        Expression<Func<T, TSelector>> propertyExpression, IComparer<TSelector> comparer, string because = "",
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
    public AndConstraint<TAssertions> NotBeInDescendingOrder(Func<T, T, int> comparison, string because = "",
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
    public AndConstraint<TAssertions> NotBeSubsetOf(IEnumerable<T> unexpectedSuperset, string because = "",
        params object[] becauseArgs)
    {
        genericCollectionAssertions.AssertNotBeSubsetOf(unexpectedSuperset, because, becauseArgs);

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
    public AndWhichConstraint<TAssertions, T> NotContain(T unexpected, string because = "", params object[] becauseArgs)
    {
        IEnumerable<T> matched = genericCollectionAssertions.GetNotContainMatches(unexpected, because, becauseArgs);

        return new AndWhichConstraint<TAssertions, T>((TAssertions)this, matched);
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
    public AndConstraint<TAssertions> NotContain(Expression<Func<T, bool>> predicate, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(predicate);

        genericCollectionAssertions.AssertNotContain(predicate, because, becauseArgs);

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
    public AndConstraint<TAssertions> NotContain(IEnumerable<T> unexpected, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot verify non-containment against a <null> collection");

        genericCollectionAssertions.AssertNotContain(unexpected, because, becauseArgs);

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
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
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
    public AndConstraint<TAssertions> NotContainEquivalentOf<TExpectation>(TExpectation unexpected, string because = "",
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
    /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
    /// </para>
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
    public AndConstraint<TAssertions> NotContainEquivalentOf<TExpectation>(TExpectation unexpected,
        Func<EquivalencyOptions<TExpectation>,
            EquivalencyOptions<TExpectation>> config, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(config);

        genericCollectionAssertions.AssertNotContainEquivalentOf(unexpected, config, because, becauseArgs);

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
    public AndConstraint<TAssertions> NotContainInOrder(IEnumerable<T> unexpected, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected),
            "Cannot verify absence of ordered containment against a <null> collection.");

        genericCollectionAssertions.AssertNotContainInOrder(unexpected, because, becauseArgs);

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
    public AndConstraint<TAssertions> NotContainInConsecutiveOrder(IEnumerable<T> unexpected, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected),
            "Cannot verify absence of ordered containment against a <null> collection.");

        genericCollectionAssertions.AssertNotContainInConsecutiveOrder(unexpected, because, becauseArgs);

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
    public AndConstraint<TAssertions> NotContainNulls<TKey>(Expression<Func<T, TKey>> predicate, string because = "",
        params object[] becauseArgs)
        where TKey : class
    {
        Guard.ThrowIfArgumentIsNull(predicate);

        genericCollectionAssertions.AssertNotContainNulls(predicate, because, becauseArgs);

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
        genericCollectionAssertions.AssertNotContainNulls(because, becauseArgs);

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
    public AndConstraint<TAssertions> NotEqual(IEnumerable<T> unexpected, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot compare collection with <null>.");

        genericCollectionAssertions.AssertNotEqual(unexpected, because, becauseArgs);

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
    public AndConstraint<TAssertions> NotHaveCount(int unexpected, string because = "", params object[] becauseArgs)
    {
        genericCollectionAssertions.AssertNotHaveCount(unexpected, because, becauseArgs);

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

        genericCollectionAssertions.AssertNotHaveSameCount(otherCollection, because, becauseArgs);

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
    public AndConstraint<TAssertions> NotIntersectWith(IEnumerable<T> otherCollection, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection),
            "Cannot verify intersection against a <null> collection.");

        genericCollectionAssertions.AssertNotIntersectWith(otherCollection, because, becauseArgs);

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
        Expression<Func<T, bool>> predicate, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(predicate);

        genericCollectionAssertions.AssertOnlyContain(predicate, because, becauseArgs);

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
    public AndConstraint<TAssertions> OnlyHaveUniqueItems<TKey>(Expression<Func<T, TKey>> predicate, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(predicate);

        genericCollectionAssertions.AssertOnlyHaveUniqueItems(predicate, because, becauseArgs);

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
        genericCollectionAssertions.AssertOnlyHaveUniqueItems(because, becauseArgs);

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
    public AndConstraint<TAssertions> AllSatisfy(Action<T> expected, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot verify against a <null> inspector");

        genericCollectionAssertions.AssertAllSatisfy(expected, because, becauseArgs);

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
    public AndConstraint<TAssertions> SatisfyRespectively(IEnumerable<Action<T>> expected, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot verify against a <null> collection of inspectors");

        genericCollectionAssertions.AssertSatisfyRespectively(expected, because, becauseArgs);

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
    public AndConstraint<TAssertions> Satisfy(IEnumerable<Expression<Func<T, bool>>> predicates, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(predicates, nameof(predicates), "Cannot verify against a <null> collection of predicates");

        genericCollectionAssertions.AssertSatisfy(predicates, because, becauseArgs);

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
    public AndConstraint<TAssertions> StartWith(IEnumerable<T> expectation, string because = "", params object[] becauseArgs)
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
        IEnumerable<TExpectation> expectation, Func<T, TExpectation, bool> equalityComparison, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectation, nameof(expectation), "Cannot compare collection with <null>.");

        genericCollectionAssertions.AssertStartsWith(expectation, equalityComparison, because, becauseArgs);

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
    public AndConstraint<TAssertions> StartWith(T element, string because = "", params object[] becauseArgs)
    {
        return StartWith([element], ObjectExtensions.GetComparer<T>(), because, becauseArgs);
    }

    /// <summary>
    /// Asserts the current collection does not have all elements in ascending order. Elements are compared
    /// using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    private AndConstraint<TAssertions> NotBeInOrder(IComparer<T> comparer, SortOrder order, string because = "",
        params object[] becauseArgs)
    {
        genericCollectionAssertions.AssertNotBeInOrder(comparer, order, because, becauseArgs);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    private AndConstraint<TAssertions> NotBeOrderedBy<TSelector>(
        Expression<Func<T, TSelector>> propertyExpression,
        IComparer<TSelector> comparer,
        SortOrder direction,
        string because,
        object[] becauseArgs)
    {
        genericCollectionAssertions.AssertNotBeOrderedBy(propertyExpression, comparer, direction, because, becauseArgs);

        return new AndConstraint<TAssertions>((TAssertions)this);
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

    private static IEnumerable<TExpectation> RepeatAsManyAs<TExpectation>(TExpectation value, IEnumerable<T> enumerable)
    {
        if (enumerable is null)
        {
            return [];
        }

        return GenericCollectionAssertions<T>.RepeatAsManyAsIterator(value, enumerable);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        throw new NotSupportedException(
            "Equals is not part of Fluent Assertions. Did you mean BeSameAs(), Equal(), or BeEquivalentTo() instead?");

    private protected static IComparer<TItem> GetComparer<TItem>() =>
        typeof(TItem) == typeof(string) ? (IComparer<TItem>)StringComparer.Ordinal : Comparer<TItem>.Default;
}
