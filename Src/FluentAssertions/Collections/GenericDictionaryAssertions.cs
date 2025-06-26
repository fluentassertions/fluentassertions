using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;

namespace FluentAssertions.Collections;

/// <summary>
/// Contains a number of methods to assert that a <typeparamref name="TCollection"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class GenericDictionaryAssertions<TCollection, TKey, TValue>
    : GenericDictionaryAssertions<TCollection, TKey, TValue, GenericDictionaryAssertions<TCollection, TKey, TValue>>
    where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
{
    public GenericDictionaryAssertions(TCollection keyValuePairs, AssertionChain assertionChain)
        : base(keyValuePairs, assertionChain)
    {
    }
}

/// <summary>
/// Contains a number of methods to assert that a <typeparamref name="TCollection"/> is in the expected state.
/// </summary>
public class GenericDictionaryAssertions<TCollection, TKey, TValue, TAssertions>
    : GenericCollectionAssertions<TCollection, KeyValuePair<TKey, TValue>, TAssertions>
    where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
    where TAssertions : GenericDictionaryAssertions<TCollection, TKey, TValue, TAssertions>
{
    private readonly AssertionChain assertionChain;

    public GenericDictionaryAssertions(TCollection keyValuePairs, AssertionChain assertionChain)
        : base(keyValuePairs, assertionChain)
    {
        this.assertionChain = assertionChain;
    }

    #region Equal

    /// <summary>
    /// Asserts that the current dictionary contains all the same key-value pairs as the
    /// specified <paramref name="expected"/> dictionary. Keys and values are compared using
    /// their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="expected">The expected dictionary</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> Equal<T>(T expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where T : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot compare dictionary with <null>.");

        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:dictionary} to be equal to {0}{reason}, but found {1}.", expected, Subject);

        if (assertionChain.Succeeded)
        {
            ICollection<TKey> subjectKeys = GetKeys(Subject).ConvertOrCastToCollection();
            ICollection<TKey> expectedKeys = GetKeys(expected).ConvertOrCastToCollection();
            IEnumerable<TKey> missingKeys = expectedKeys.Except(subjectKeys);
            IEnumerable<TKey> additionalKeys = subjectKeys.Except(expectedKeys);

            if (missingKeys.Any())
            {
                assertionChain
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to be equal to {0}{reason}, but could not find keys {1}.", expected,
                        missingKeys);
            }

            if (additionalKeys.Any())
            {
                assertionChain
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to be equal to {0}{reason}, but found additional keys {1}.",
                        expected,
                        additionalKeys);
            }

            Func<TValue, TValue, bool> areSameOrEqual = ObjectExtensions.GetComparer<TValue>();

            foreach (var key in expectedKeys)
            {
                assertionChain
                    .ForCondition(areSameOrEqual(GetValue(Subject, key), GetValue(expected, key)))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to be equal to {0}{reason}, but {1} differs at key {2}.",
                        expected, Subject, key);
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts the current dictionary not to contain all the same key-value pairs as the
    /// specified <paramref name="unexpected"/> dictionary. Keys and values are compared using
    /// their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="unexpected">The unexpected dictionary</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotEqual<T>(T unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where T : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot compare dictionary with <null>.");

        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected dictionaries not to be equal{reason}, but found {0}.", Subject);

        if (assertionChain.Succeeded)
        {
            if (ReferenceEquals(Subject, unexpected))
            {
                assertionChain
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected dictionaries not to be equal{reason}, but they both reference the same object.");
            }

            IEnumerable<TKey> subjectKeys = GetKeys(Subject).ConvertOrCastToCollection();
            IEnumerable<TKey> unexpectedKeys = GetKeys(unexpected).ConvertOrCastToCollection();
            IEnumerable<TKey> missingKeys = unexpectedKeys.Except(subjectKeys);
            IEnumerable<TKey> additionalKeys = subjectKeys.Except(unexpectedKeys);

            Func<TValue, TValue, bool> areSameOrEqual = ObjectExtensions.GetComparer<TValue>();

            bool foundDifference = missingKeys.Any()
                || additionalKeys.Any()
                || subjectKeys.Any(key => !areSameOrEqual(GetValue(Subject, key), GetValue(unexpected, key)));

            if (!foundDifference)
            {
                assertionChain
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect dictionaries {0} and {1} to be equal{reason}.", unexpected, Subject);
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    #endregion

    /// <summary>
    /// Asserts that two dictionaries are equivalent.
    /// </summary>
    /// <remarks>
    /// The values within the dictionaries are equivalent when both object graphs have equally named properties with the same
    /// value, irrespective of the type of those objects. Two properties are also equal if one type can be converted to another
    /// and the result is equal.
    /// The type of the values in the dictionaries are ignored as long as both dictionaries contain the same keys and
    /// the values for each key are structurally equivalent. Notice that actual behavior is determined by the global
    /// defaults managed by the <see cref="AssertionConfiguration"/> class.
    /// </remarks>
    /// <param name="expectation">The expected element.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> BeEquivalentTo<TExpectation>(TExpectation expectation,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return BeEquivalentTo(expectation, options => options, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that two dictionaries are equivalent.
    /// </summary>
    /// <remarks>
    /// The values within the dictionaries are equivalent when both object graphs have equally named properties with the same
    /// value, irrespective of the type of those objects. Two properties are also equal if one type can be converted to another
    /// and the result is equal.
    /// The type of the values in the dictionaries are ignored as long as both dictionaries contain the same keys and
    /// the values for each key are structurally equivalent. Notice that actual behavior is determined by the global
    /// defaults managed by the <see cref="AssertionConfiguration"/> class.
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
    public AndConstraint<TAssertions> BeEquivalentTo<TExpectation>(TExpectation expectation,
        Func<EquivalencyOptions<TExpectation>, EquivalencyOptions<TExpectation>> config,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(config);

        EquivalencyOptions<TExpectation> options = config(AssertionConfiguration.Current.Equivalency.CloneDefaults<TExpectation>());

        var context =
            new EquivalencyValidationContext(Node.From<TExpectation>(() => CurrentAssertionChain.CallerIdentifier), options)
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

        new EquivalencyValidator().AssertEquality(comparands, context);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    #region ContainKey

    /// <summary>
    /// Asserts that the dictionary contains the specified key.
    /// Key comparison will honor the equality comparer of the dictionary when applicable.
    /// </summary>
    /// <param name="expected">The expected key</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public WhoseValueConstraint<TCollection, TKey, TValue, TAssertions> ContainKey(TKey expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        AndConstraint<TAssertions> andConstraint = ContainKeys([expected], because, becauseArgs);

        _ = TryGetValue(Subject, expected, out TValue value);

        return new WhoseValueConstraint<TCollection, TKey, TValue, TAssertions>(andConstraint.And, value);
    }

    /// <summary>
    /// Asserts that the dictionary contains all of the specified keys.
    /// Key comparison will honor the equality comparer of the dictionary when applicable.
    /// </summary>
    /// <param name="expected">The expected keys</param>
    public AndConstraint<TAssertions> ContainKeys(params TKey[] expected)
    {
        return ContainKeys(expected, string.Empty);
    }

    /// <summary>
    /// Asserts that the dictionary contains all of the specified keys.
    /// Key comparison will honor the equality comparer of the dictionary when applicable.
    /// </summary>
    /// <param name="expected">The expected keys</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="expected"/> is empty.</exception>
    public AndConstraint<TAssertions> ContainKeys(IEnumerable<TKey> expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected),
            "Cannot verify key containment against a <null> collection of keys");

        ICollection<TKey> expectedKeys = expected.ConvertOrCastToCollection();
        Guard.ThrowIfArgumentIsEmpty(expectedKeys, nameof(expected), "Cannot verify key containment against an empty sequence");

        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:dictionary} to contain keys {0}{reason}, but found <null>.", expected);

        if (assertionChain.Succeeded)
        {
            IEnumerable<TKey> missingKeys = expectedKeys.Where(key => !ContainsKey(Subject, key));

            if (missingKeys.Any())
            {
                if (expectedKeys.Count > 1)
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to contain keys {1}{reason}, but could not find {2}.",
                            Subject,
                            expected, missingKeys);
                }
                else
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to contain key {1}{reason}.", Subject,
                            expected.First());
                }
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    #endregion

    #region NotContainKey

    /// <summary>
    /// Asserts that the current dictionary does not contain the specified <paramref name="unexpected" /> key.
    /// Key comparison will honor the equality comparer of the dictionary when applicable.
    /// </summary>
    /// <param name="unexpected">The unexpected key</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotContainKey(TKey unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:dictionary} not to contain key {0}{reason}, but found <null>.", unexpected);

        if (assertionChain.Succeeded && ContainsKey(Subject, unexpected))
        {
            assertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:dictionary} {0} not to contain key {1}{reason}, but found it anyhow.", Subject,
                    unexpected);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the dictionary does not contain any of the specified keys.
    /// Key comparison will honor the equality comparer of the dictionary when applicable.
    /// </summary>
    /// <param name="unexpected">The unexpected keys</param>
    public AndConstraint<TAssertions> NotContainKeys(params TKey[] unexpected)
    {
        return NotContainKeys(unexpected, string.Empty);
    }

    /// <summary>
    /// Asserts that the dictionary does not contain any of the specified keys.
    /// Key comparison will honor the equality comparer of the dictionary when applicable.
    /// </summary>
    /// <param name="unexpected">The unexpected keys</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="unexpected"/> is empty.</exception>
    public AndConstraint<TAssertions> NotContainKeys(IEnumerable<TKey> unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected),
            "Cannot verify key containment against a <null> collection of keys");

        ICollection<TKey> unexpectedKeys = unexpected.ConvertOrCastToCollection();

        Guard.ThrowIfArgumentIsEmpty(unexpectedKeys, nameof(unexpected),
            "Cannot verify key containment against an empty sequence");

        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:dictionary} to not contain keys {0}{reason}, but found <null>.", unexpectedKeys);

        if (assertionChain.Succeeded)
        {
            IEnumerable<TKey> foundKeys = unexpectedKeys.Where(key => ContainsKey(Subject, key));

            if (foundKeys.Any())
            {
                if (unexpectedKeys.Count > 1)
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to not contain keys {1}{reason}, but found {2}.", Subject,
                            unexpectedKeys, foundKeys);
                }
                else
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to not contain key {1}{reason}.", Subject,
                            unexpectedKeys.First());
                }
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    #endregion

    #region ContainValue

    /// <summary>
    /// Asserts that the dictionary contains the specified value. Values are compared using
    /// their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="expected">The expected value</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<TAssertions, TValue> ContainValue(TValue expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        AndWhichConstraint<TAssertions, IEnumerable<TValue>> result =
            ContainValues([expected], because, becauseArgs);

        return new AndWhichConstraint<TAssertions, TValue>(result.And, result.Subject);
    }

    /// <summary>
    /// Asserts that the dictionary contains all of the specified values. Values are compared using
    /// their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="expected">The expected values</param>
    public AndWhichConstraint<TAssertions, IEnumerable<TValue>> ContainValues(params TValue[] expected)
    {
        return ContainValues(expected, string.Empty);
    }

    /// <summary>
    /// Asserts that the dictionary contains all the specified values. Values are compared using
    /// their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="expected">The expected values</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="expected"/> is empty.</exception>
    public AndWhichConstraint<TAssertions, IEnumerable<TValue>> ContainValues(IEnumerable<TValue> expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected),
            "Cannot verify value containment against a <null> collection of values");

        ICollection<TValue> expectedValues = expected.ConvertOrCastToCollection();

        Guard.ThrowIfArgumentIsEmpty(expectedValues, nameof(expected),
            "Cannot verify value containment against an empty sequence");

        var missingValues = new List<TValue>(expectedValues);

        Dictionary<TKey, TValue> matches = new();

        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:dictionary} to contain values {0}{reason}, but found <null>.", expected);

        if (assertionChain.Succeeded)
        {
            foreach (var pair in Subject!)
            {
                if (missingValues.Contains(pair.Value))
                {
                    matches.Add(pair.Key, pair.Value);
                    missingValues.Remove(pair.Value);
                }
            }

            if (missingValues.Count > 0)
            {
                if (expectedValues.Count == 1)
                {
                    assertionChain.FailWith(
                        "Expected {context:dictionary} {0} to contain value {1}{reason}.",
                        Subject, expectedValues.Single());
                }
                else
                {
                    assertionChain.FailWith(
                        "Expected {context:dictionary} {0} to contain values {1}{reason}, but could not find {2}.",
                        Subject, expectedValues, missingValues.Count == 1 ? missingValues.Single() : missingValues);
                }
            }
        }

        string postfix = matches.Count > 0 ? "[" + string.Join(" and ", matches.Keys) + "]" : "";

        return new AndWhichConstraint<TAssertions, IEnumerable<TValue>>((TAssertions)this, matches.Values, assertionChain, postfix);
    }

    #endregion

    #region NotContainValue

    /// <summary>
    /// Asserts that the current dictionary does not contain the specified <paramref name="unexpected" /> value.
    /// Values are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="unexpected">The unexpected value</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotContainValue(TValue unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:dictionary} not to contain value {0}{reason}, but found <null>.", unexpected);

        if (assertionChain.Succeeded && GetValues(Subject).Contains(unexpected))
        {
            assertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:dictionary} {0} not to contain value {1}{reason}, but found it anyhow.", Subject,
                    unexpected);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the dictionary does not contain any of the specified values. Values are compared using
    /// their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="unexpected">The unexpected values</param>
    public AndConstraint<TAssertions> NotContainValues(params TValue[] unexpected)
    {
        return NotContainValues(unexpected, string.Empty);
    }

    /// <summary>
    /// Asserts that the dictionary does not contain any of the specified values. Values are compared using
    /// their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="unexpected">The unexpected values</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="unexpected"/> is empty.</exception>
    public AndConstraint<TAssertions> NotContainValues(IEnumerable<TValue> unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected),
            "Cannot verify value containment against a <null> collection of values");

        ICollection<TValue> unexpectedValues = unexpected.ConvertOrCastToCollection();

        Guard.ThrowIfArgumentIsEmpty(unexpectedValues, nameof(unexpected),
            "Cannot verify value containment with an empty sequence");

        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:dictionary} to not contain values {0}{reason}, but found <null>.", unexpected);

        if (assertionChain.Succeeded)
        {
            IEnumerable<TValue> foundValues = unexpectedValues.Intersect(GetValues(Subject));

            if (foundValues.Any())
            {
                if (unexpectedValues.Count > 1)
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to not contain value {1}{reason}, but found {2}.",
                            Subject,
                            unexpected, foundValues);
                }
                else
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to not contain value {1}{reason}.", Subject,
                            unexpected.First());
                }
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    #endregion

    #region Contain

    /// <summary>
    /// Asserts that the current dictionary contains the specified <paramref name="expected"/>.
    /// Key comparison will honor the equality comparer of the dictionary when applicable.
    /// Values are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="expected">The expected key/value pairs.</param>
    public AndConstraint<TAssertions> Contain(params KeyValuePair<TKey, TValue>[] expected)
    {
        return Contain(expected, string.Empty);
    }

    /// <summary>
    /// Asserts that the current dictionary contains the specified <paramref name="expected"/>.
    /// Key comparison will honor the equality comparer of the dictionary when applicable.
    /// Values are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="expected">The expected key/value pairs.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="expected"/> is empty.</exception>
    [SuppressMessage("Class Design", "AV1010:Member hides inherited member")]
    public new AndConstraint<TAssertions> Contain(IEnumerable<KeyValuePair<TKey, TValue>> expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot compare dictionary with <null>.");

        ICollection<KeyValuePair<TKey, TValue>> expectedKeyValuePairs = expected.ConvertOrCastToCollection();

        Guard.ThrowIfArgumentIsEmpty(expectedKeyValuePairs, nameof(expected),
            "Cannot verify key containment against an empty collection of key/value pairs");

        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:dictionary} to contain key/value pairs {0}{reason}, but dictionary is <null>.",
                expected);

        if (assertionChain.Succeeded)
        {
            TKey[] expectedKeys = expectedKeyValuePairs.Select(keyValuePair => keyValuePair.Key).ToArray();
            IEnumerable<TKey> missingKeys = expectedKeys.Where(key => !ContainsKey(Subject, key));

            if (missingKeys.Any())
            {
                if (expectedKeyValuePairs.Count > 1)
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to contain key(s) {1}{reason}, but could not find keys {2}.",
                            Subject,
                            expectedKeys, missingKeys);
                }
                else
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to contain key {1}{reason}.", Subject,
                            expectedKeys[0]);
                }
            }

            Func<TValue, TValue, bool> areSameOrEqual = ObjectExtensions.GetComparer<TValue>();

            KeyValuePair<TKey, TValue>[] keyValuePairsNotSameOrEqualInSubject = expectedKeyValuePairs
                .Where(keyValuePair => !areSameOrEqual(GetValue(Subject, keyValuePair.Key), keyValuePair.Value)).ToArray();

            if (keyValuePairsNotSameOrEqualInSubject.Length > 0)
            {
                if (keyValuePairsNotSameOrEqualInSubject.Length > 1)
                {
                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith(
                            "Expected {context:dictionary} to contain {0}{reason}, but {context:dictionary} differs at keys {1}.",
                            expectedKeyValuePairs, keyValuePairsNotSameOrEqualInSubject.Select(keyValuePair => keyValuePair.Key));
                }
                else
                {
                    KeyValuePair<TKey, TValue> expectedKeyValuePair = keyValuePairsNotSameOrEqualInSubject[0];
                    TValue actual = GetValue(Subject, expectedKeyValuePair.Key);

                    assertionChain
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} to contain value {0} at key {1}{reason}, but found {2}.",
                            expectedKeyValuePair.Value, expectedKeyValuePair.Key, actual);
                }
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current dictionary contains the specified <paramref name="expected"/>.
    /// Key comparison will honor the equality comparer of the dictionary when applicable.
    /// Values are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="expected">The expected <see cref="KeyValuePair{TKey,TValue}"/></param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    [SuppressMessage("Class Design", "AV1010:Member hides inherited member")]
    public new AndConstraint<TAssertions> Contain(KeyValuePair<TKey, TValue> expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return Contain(expected.Key, expected.Value, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current dictionary contains the specified <paramref name="value" /> for the supplied
    /// <paramref name="key" />.
    /// Key comparison will honor the equality comparer of the dictionary when applicable.
    /// Values are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="key">The key for which to validate the value</param>
    /// <param name="value">The value to validate</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> Contain(TKey key, TValue value,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:dictionary} to contain value {0} at key {1}{reason}, but dictionary is <null>.",
                value, key);

        if (assertionChain.Succeeded)
        {
            if (TryGetValue(Subject, key, out TValue actual))
            {
                Func<TValue, TValue, bool> areSameOrEqual = ObjectExtensions.GetComparer<TValue>();

                assertionChain
                    .ForCondition(areSameOrEqual(actual, value))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to contain value {0} at key {1}{reason}, but found {2}.", value, key,
                        actual);
            }
            else
            {
                assertionChain
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to contain value {0} at key {1}{reason}, but the key was not found.",
                        value,
                        key);
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    #endregion

    #region NotContain

    /// <summary>
    /// Asserts that the current dictionary does not contain the specified <paramref name="items"/>.
    /// Key comparison will honor the equality comparer of the dictionary when applicable.
    /// Values are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="items">The unexpected key/value pairs</param>
    public AndConstraint<TAssertions> NotContain(params KeyValuePair<TKey, TValue>[] items)
    {
        return NotContain(items, string.Empty);
    }

    /// <summary>
    /// Asserts that the current dictionary does not contain the specified <paramref name="items"/>.
    /// Key comparison will honor the equality comparer of the dictionary when applicable.
    /// Values are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="items">The unexpected key/value pairs</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="items"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="items"/> is empty.</exception>
    [SuppressMessage("Class Design", "AV1010:Member hides inherited member")]
    public new AndConstraint<TAssertions> NotContain(IEnumerable<KeyValuePair<TKey, TValue>> items,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(items, nameof(items), "Cannot compare dictionary with <null>.");

        ICollection<KeyValuePair<TKey, TValue>> keyValuePairs = items.ConvertOrCastToCollection();

        Guard.ThrowIfArgumentIsEmpty(keyValuePairs, nameof(items),
            "Cannot verify key containment against an empty collection of key/value pairs");

        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:dictionary} to not contain key/value pairs {0}{reason}, but dictionary is <null>.",
                items);

        if (assertionChain.Succeeded)
        {
            KeyValuePair<TKey, TValue>[] keyValuePairsFound =
                keyValuePairs.Where(keyValuePair => ContainsKey(Subject, keyValuePair.Key)).ToArray();

            if (keyValuePairsFound.Length > 0)
            {
                Func<TValue, TValue, bool> areSameOrEqual = ObjectExtensions.GetComparer<TValue>();

                KeyValuePair<TKey, TValue>[] keyValuePairsSameOrEqualInSubject = keyValuePairsFound
                    .Where(keyValuePair => areSameOrEqual(GetValue(Subject, keyValuePair.Key), keyValuePair.Value)).ToArray();

                if (keyValuePairsSameOrEqualInSubject.Length > 0)
                {
                    if (keyValuePairsSameOrEqualInSubject.Length > 1)
                    {
                        assertionChain
                            .BecauseOf(because, becauseArgs)
                            .FailWith(
                                "Expected {context:dictionary} to not contain key/value pairs {0}{reason}, but found them anyhow.",
                                keyValuePairs);
                    }
                    else
                    {
                        KeyValuePair<TKey, TValue> keyValuePair = keyValuePairsSameOrEqualInSubject[0];

                        assertionChain
                            .BecauseOf(because, becauseArgs)
                            .FailWith(
                                "Expected {context:dictionary} to not contain value {0} at key {1}{reason}, but found it anyhow.",
                                keyValuePair.Value, keyValuePair.Key);
                    }
                }
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current dictionary does not contain the specified <paramref name="item"/>.
    /// Key comparison will honor the equality comparer of the dictionary when applicable.
    /// Values are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="item">The unexpected <see cref="KeyValuePair{TKey,TValue}"/></param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    [SuppressMessage("Class Design", "AV1010:Member hides inherited member")]
    public new AndConstraint<TAssertions> NotContain(KeyValuePair<TKey, TValue> item,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return NotContain(item.Key, item.Value, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current dictionary does not contain the specified <paramref name="value" /> for the
    /// supplied <paramref name="key" />.
    /// Key comparison will honor the equality comparer of the dictionary when applicable.
    /// Values are compared using their <see cref="object.Equals(object)" /> implementation.
    /// </summary>
    /// <param name="key">The key for which to validate the value</param>
    /// <param name="value">The value to validate</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotContain(TKey key, TValue value,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:dictionary} not to contain value {0} at key {1}{reason}, but dictionary is <null>.",
                value, key);

        if (assertionChain.Succeeded && TryGetValue(Subject, key, out TValue actual))
        {
            assertionChain
                .ForCondition(!ObjectExtensions.GetComparer<TValue>()(actual, value))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:dictionary} not to contain value {0} at key {1}{reason}, but found it anyhow.",
                    value, key);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    #endregion

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => "dictionary";

    private static IEnumerable<TKey> GetKeys(TCollection collection) =>
        collection.GetKeys<TCollection, TKey, TValue>();

    private static IEnumerable<TKey> GetKeys<T>(T collection)
        where T : IEnumerable<KeyValuePair<TKey, TValue>> =>
        collection.GetKeys<T, TKey, TValue>();

    private static IEnumerable<TValue> GetValues(TCollection collection) =>
        collection.GetValues<TCollection, TKey, TValue>();

    private static bool ContainsKey(TCollection collection, TKey key) =>
        collection.ContainsKey<TCollection, TKey, TValue>(key);

    private static bool TryGetValue(TCollection collection, TKey key, out TValue value) =>
        collection.TryGetValue(key, out value);

    private static TValue GetValue(TCollection collection, TKey key) =>
        collection.GetValue<TCollection, TKey, TValue>(key);

    private static TValue GetValue<T>(T collection, TKey key)
        where T : IEnumerable<KeyValuePair<TKey, TValue>> =>
        collection.GetValue<T, TKey, TValue>(key);
}
