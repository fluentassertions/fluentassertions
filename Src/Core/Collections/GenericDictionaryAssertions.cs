using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Collections
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="IDictionary{TKey,TValue}"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class GenericDictionaryAssertions<TKey, TValue> : 
        ReferenceTypeAssertions<IDictionary<TKey, TValue>, GenericDictionaryAssertions<TKey, TValue>>
    {
        public GenericDictionaryAssertions(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary != null)
            {
                Subject = dictionary;
            }
        }

        #region HaveCount

        /// <summary>
        /// Asserts that the number of items in the dictionary matches the supplied <paramref name="expected" /> amount.
        /// </summary>
        /// <param name="expected">The expected number of items.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> HaveCount(int expected,
            string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {0} item(s){reason}, but found {1}.", expected, Subject);
            }

            int actualCount = Subject.Count;

            Execute.Assertion
                .ForCondition((actualCount == expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:dictionary} {0} to have {1} item(s){reason}, but found {2}.", Subject, expected, actualCount);

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        /// <summary>
        /// Asserts that the number of items in the dictionary matches a condition stated by a predicate.
        /// </summary>
        /// <param name="countPredicate">The predicate which must be satisfied by the amount of items.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> HaveCount(Expression<Func<int, bool>> countPredicate,
            string because = "", params object[] becauseArgs)
        {
            if (countPredicate == null)
            {
                throw new NullReferenceException("Cannot compare dictionary count against a <null> predicate.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to have {0} items{reason}, but found {1}.", countPredicate.Body, Subject);
            }

            Func<int, bool> compiledPredicate = countPredicate.Compile();

            int actualCount = Subject.Count;

            if (!compiledPredicate(actualCount))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} {0} to have a count {1}{reason}, but count is {2}.",
                        Subject, countPredicate.Body, actualCount);
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        #endregion

        #region BeEmpty

        /// <summary>
        /// Asserts that the dictionary does not contain any items.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> BeEmpty(string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to be empty{reason}, but found {0}.", Subject);
            }

            Execute.Assertion
                .ForCondition(!Subject.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:dictionary} to not have any items{reason}, but found {0}.", Subject.Count);

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        /// <summary>
        /// Asserts that the dictionary contains at least 1 item.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotBeEmpty(string because = "",
            params object [] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} not to be empty{reason}, but found {0}.", Subject);
            }

            Execute.Assertion
                .ForCondition(Subject.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected one or more items{reason}, but found none.");

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        #endregion

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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> Equal(IDictionary<TKey, TValue> expected,
            string because = "", params object [] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to be equal to {0}{reason}, but found {1}.", expected, Subject);
            }

            if (expected == null)
            {
                throw new ArgumentNullException("expected", "Cannot compare dictionary with <null>.");
            }

            IEnumerable<TKey> missingKeys = expected.Keys.Except(Subject.Keys);
            IEnumerable<TKey> additionalKeys = Subject.Keys.Except(expected.Keys);

            if (missingKeys.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to be equal to {0}{reason}, but could not find keys {1}.", expected,
                        missingKeys);
            }

            if (additionalKeys.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to be equal to {0}{reason}, but found additional keys {1}.", expected,
                        additionalKeys);
            }

            foreach (var key in expected.Keys)
            {
                Execute.Assertion
                    .ForCondition(Subject[key].IsSameOrEqualTo(expected[key]))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to be equal to {0}{reason}, but {1} differs at key {2}.", 
                    expected, Subject, key);
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotEqual(IDictionary<TKey, TValue> unexpected,
            string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected dictionaries not to be equal{reason}, but found {0}.", Subject);
            }

            if (unexpected == null)
            {
                throw new ArgumentNullException("unexpected", "Cannot compare dictionary with <null>.");
            }

            IEnumerable<TKey> missingKeys = unexpected.Keys.Except(Subject.Keys);
            IEnumerable<TKey> additionalKeys = Subject.Keys.Except(unexpected.Keys);

            bool foundDifference = missingKeys.Any()
                || additionalKeys.Any()
                    || (Subject.Keys.Any(key => !Subject[key].IsSameOrEqualTo(unexpected[key])));

            if (!foundDifference)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect dictionaries {0} and {1} to be equal{reason}.", unexpected, Subject);
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        #endregion

        #region ContainKey

        /// <summary>
        /// Asserts that the dictionary contains the specified key. Keys are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected key</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public WhichValueConstraint<TKey, TValue> ContainKey(TKey expected,
            string because = "", params object [] becauseArgs)
        {
            AndConstraint<GenericDictionaryAssertions<TKey, TValue>> andConstraint = ContainKeys(new [] { expected }, because, becauseArgs);

            return new WhichValueConstraint<TKey, TValue>(andConstraint.And, Subject[expected]);
        }

        /// <summary>
        /// Asserts that the dictionary contains all of the specified keys. Keys are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected keys</param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> ContainKeys(params TKey[] expected)
        {
            return ContainKeys(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the dictionary contains all of the specified keys. Keys are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected keys</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> ContainKeys(IEnumerable<TKey> expected,
            string because = "", params object[] becauseArgs)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify key containment against a <null> collection of keys");
            }

            TKey [] expectedKeys = expected.ToArray();

            if (!expectedKeys.Any())
            {
                throw new ArgumentException("Cannot verify key containment against an empty sequence");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to contain keys {0}{reason}, but found {1}.", expected, Subject);
            }
            
            var missingKeys = expectedKeys.Where(key => !Subject.ContainsKey(key));

            if (missingKeys.Any())
            {
                if (expectedKeys.Count() > 1)
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to contain key {1}{reason}, but could not find {2}.", Subject,
                            expected, missingKeys);
                }
                else
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to contain key {1}{reason}.", Subject,
                            expected.Cast<object>().First());
                }
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        #endregion

        #region NotContainKey

        /// <summary>
        /// Asserts that the current dictionary does not contain the specified <paramref name="unexpected" /> key.
        /// Keys are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected key</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContainKey(TKey unexpected,
            string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} not to contain key {0}{reason}, but found {1}.", unexpected, Subject);
            }

            if (Subject.ContainsKey(unexpected))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("{context:Dictionary} {0} should not contain key {1}{reason}, but found it anyhow.", Subject, unexpected);
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        /// <summary>
        /// Asserts that the dictionary does not contain any of the specified keys. Keys are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected keys</param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContainKeys(params TKey[] unexpected)
        {
            return NotContainKeys(unexpected, String.Empty);
        }

        /// <summary>
        /// Asserts that the dictionary does not contain any of the specified keys. Keys are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected keys</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContainKeys(IEnumerable<TKey> unexpected,
            string because = "", params object[] becauseArgs)
        {
            if (unexpected == null)
            {
                throw new NullReferenceException("Cannot verify key containment against a <null> collection of keys");
            }

            TKey[] unexpectedKeys = unexpected.ToArray();

            if (!unexpectedKeys.Any())
            {
                throw new ArgumentException("Cannot verify key containment against an empty sequence");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to contain keys {0}{reason}, but found {1}.", unexpected, Subject);
            }

            var foundKeys = unexpectedKeys.Intersect(Subject.Keys);

            if (foundKeys.Any())
            {
                if (unexpectedKeys.Count() > 1)
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to not contain key {1}{reason}, but found {2}.", Subject,
                            unexpected, foundKeys);
                }
                else
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to not contain key {1}{reason}.", Subject,
                            unexpected.Cast<object>().First());
                }
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<GenericDictionaryAssertions<TKey, TValue>, TValue> ContainValue(TValue expected,
            string because = "", params object[] becauseArgs)
        {
            AndWhichConstraint<GenericDictionaryAssertions<TKey, TValue>, IEnumerable<TValue>> innerConstraint =
                    ContainValuesAndWhich(new[] {expected}, because, becauseArgs);

            return
                new AndWhichConstraint
                    <GenericDictionaryAssertions<TKey, TValue>, TValue>(
                    innerConstraint.And, innerConstraint.Which);
        }

        /// <summary>
        /// Asserts that the dictionary contains all of the specified values. Values are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected values</param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> ContainValues(params TValue[] expected)
        {
            return ContainValues(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the dictionary contains all of the specified values. Values are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected values</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> ContainValues(IEnumerable<TValue> expected,
            string because = "", params object[] becauseArgs)
        {
            return ContainValuesAndWhich(expected, because, becauseArgs);
        }

        private AndWhichConstraint<GenericDictionaryAssertions<TKey, TValue>, IEnumerable<TValue>> ContainValuesAndWhich(IEnumerable<TValue> expected, string because = "",
            params object[] becauseArgs)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify value containment against a <null> collection of values");
            }

            TValue [] expectedValues = expected.ToArray();

            if (!expectedValues.Any())
            {
                throw new ArgumentException("Cannot verify value containment with an empty sequence");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to contain value {0}{reason}, but found {1}.", expected, Subject);
            }

            var missingValues = expectedValues.Except(Subject.Values);
            if (missingValues.Any())
            {
                if (expectedValues.Length > 1)
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to contain value {1}{reason}, but could not find {2}.", Subject,
                            expected, missingValues);
                }
                else
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to contain value {1}{reason}.", Subject,
                            expected.Cast<object>().First());
                }
            }

            return
                new AndWhichConstraint
                    <GenericDictionaryAssertions<TKey, TValue>,
                        IEnumerable<TValue>>(this,
                            RepetitionPreservingIntersect(Subject.Values, expectedValues));
        }

        /// <summary>
        /// Returns an enumerable consisting of all items in the first collection also appearing in the second.
        /// </summary>
        /// <remarks>Enumerable.Intersect is not suitable because it drops any repeated elements.</remarks>
        private IEnumerable<TValue> RepetitionPreservingIntersect(
            IEnumerable<TValue> first, IEnumerable<TValue> second)
        {
            var secondSet = new HashSet<TValue>(second);
            return first.Where(secondSet.Contains);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContainValue(TValue unexpected,
            string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} not to contain value {0}{reason}, but found {1}.", unexpected, Subject);
            }

            if (Subject.Values.Contains(unexpected))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("{context:Dictionary} {0} should not contain value {1}{reason}, but found it anyhow.", Subject, unexpected);
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        /// <summary>
        /// Asserts that the dictionary does not contain any of the specified values. Values are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected values</param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContainValues(params TValue[] unexpected)
        {
            return NotContainValues(unexpected, String.Empty);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContainValues(IEnumerable<TValue> unexpected,
            string because = "", params object[] becauseArgs)
        {
            if (unexpected == null)
            {
                throw new NullReferenceException("Cannot verify value containment against a <null> collection of values");
            }

            var unexpectedValues = unexpected.ToArray();

            if (!unexpectedValues.Any())
            {
                throw new ArgumentException("Cannot verify value containment with an empty sequence");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to not contain value {0}{reason}, but found {1}.", unexpected, Subject);
            }

            var foundValues = unexpectedValues.Intersect(Subject.Values);
            if (foundValues.Any())
            {
                if (unexpectedValues.Length > 1)
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to not contain value {1}{reason}, but found {2}.", Subject,
                            unexpected, foundValues);
                }
                else
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to not contain value {1}{reason}.", Subject,
                            unexpected.Cast<object>().First());
                }
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        #endregion

        #region Contain

        /// <summary>
        /// Asserts that the current dictionary contains the specified <paramref name="expected"/>.
        /// Keys and values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected key/value pairs.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> Contain(IEnumerable<KeyValuePair<TKey, TValue>> expected,
            string because = "", params object[] becauseArgs)
        {
            if (expected == null)
            {
                throw new ArgumentNullException("expected", "Cannot compare dictionary with <null>.");
            }

            KeyValuePair<TKey, TValue>[] expectedKeyValuePairs = expected.ToArray();

            if (!expectedKeyValuePairs.Any())
            {
                throw new ArgumentException("Cannot verify key containment against an empty collection of key/value pairs");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to contain key/value pairs {0}{reason}, but dictionary is {1}.", expected, Subject);
            }

            var expectedKeys = expectedKeyValuePairs.Select(keyValuePair => keyValuePair.Key).ToArray();
            var missingKeys = expectedKeys.Where(key => !Subject.ContainsKey(key));

            if (missingKeys.Any())
            {
                if (expectedKeyValuePairs.Count() > 1)
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to contain key(s) {1}{reason}, but could not find keys {2}.", Subject,
                            expectedKeys, missingKeys);
                }
                else
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} {0} to contain key {1}{reason}.", Subject,
                            expectedKeys.Cast<object>().First());
                }
            }

            KeyValuePair<TKey, TValue>[] keyValuePairsNotSameOrEqualInSubject = expectedKeyValuePairs.Where(keyValuePair => !Subject[keyValuePair.Key].IsSameOrEqualTo(keyValuePair.Value)).ToArray();

            if (keyValuePairsNotSameOrEqualInSubject.Any())
            {
                if (keyValuePairsNotSameOrEqualInSubject.Count() > 1)
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} to contain {0}{reason}, but {context:dictionary} differs at keys {1}.",
                            expectedKeyValuePairs, keyValuePairsNotSameOrEqualInSubject.Select(keyValuePair => keyValuePair.Key));
                }
                else
                {
                    var expectedKeyValuePair = keyValuePairsNotSameOrEqualInSubject.First();
                    TValue actual = Subject[expectedKeyValuePair.Key];

                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:dictionary} to contain value {0} at key {1}{reason}, but found {2}.", expectedKeyValuePair.Value, expectedKeyValuePair.Key, actual);
                }
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        /// <summary>
        /// Asserts that the current dictionary contains the specified <paramref name="expected"/>.
        /// Keys and values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected <see cref="KeyValuePair{TKey,TValue}"/></param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> Contain(KeyValuePair<TKey, TValue> expected,
            string because = "", params object[] becauseArgs)
        {
            return Contain(expected.Key, expected.Value, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current dictionary contains the specified <paramref name="value" /> for the supplied <paramref
        /// name="key" />. Values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="key">The key for which to validate the value</param>
        /// <param name="value">The value to validate</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> Contain(TKey key, TValue value,
            string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to contain value {0} at key {1}{reason}, but dictionary is {2}.", value, key,
                        Subject);
            }

            if (Subject.ContainsKey(key))
            {
                TValue actual = Subject[key];

                Execute.Assertion
                    .ForCondition(actual.IsSameOrEqualTo(value))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to contain value {0} at key {1}{reason}, but found {2}.", value, key, actual);
            }
            else
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to contain value {0} at key {1}{reason}, but the key was not found.", value,
                        key);
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        #endregion

        #region NotContain

        /// <summary>
        /// Asserts that the current dictionary does not contain the specified <paramref name="items"/>.
        /// Keys and values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="items">The unexpected key/value pairs</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContain(IEnumerable<KeyValuePair<TKey, TValue>> items,
            string because = "", params object[] becauseArgs)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items", "Cannot compare dictionary with <null>.");
            }

            KeyValuePair<TKey, TValue>[] keyValuePairs = items.ToArray();

            if (!keyValuePairs.Any())
            {
                throw new ArgumentException("Cannot verify key containment against an empty collection of key/value pairs");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} to not contain key/value pairs {0}{reason}, but dictionary is {1}.", items, Subject);
            }

            var keyValuePairsFound = keyValuePairs.Where(keyValuePair => Subject.ContainsKey(keyValuePair.Key)).ToArray();

            if (keyValuePairsFound.Any())
            {
                var keyValuePairsSameOrEqualInSubject = keyValuePairsFound.Where(keyValuePair => Subject[keyValuePair.Key].IsSameOrEqualTo(keyValuePair.Value)).ToArray();

                if (keyValuePairsSameOrEqualInSubject.Any())
                {
                    if (keyValuePairsSameOrEqualInSubject.Count() > 1)
                    {
                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected {context:dictionary} to not contain key/value pairs {0}{reason}, but found them anyhow.", keyValuePairs);
                    }
                    else
                    {
                        var keyValuePair = keyValuePairsSameOrEqualInSubject.First();
                        
                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected {context:dictionary} to not contain value {0} at key {1}{reason}, but found it anyhow.", keyValuePair.Value, keyValuePair.Key);
                    }
                }
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        /// <summary>
        /// Asserts that the current dictionary does not contain the specified <paramref name="item"/>.
        /// Keys and values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="item">The unexpected <see cref="KeyValuePair{TKey,TValue}"/></param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContain(KeyValuePair<TKey, TValue> item,
            string because = "", params object[] becauseArgs)
        {
            return NotContain(item.Key, item.Value, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current dictionary does not contain the specified <paramref name="value" /> for the
        /// supplied <paramref name="key" />. Values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="key">The key for which to validate the value</param>
        /// <param name="value">The value to validate</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContain(TKey key, TValue value,
            string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} not to contain value {0} at key {1}{reason}, but dictionary is {2}.", value,
                        key, Subject);
            }

            if (Subject.ContainsKey(key))
            {
                TValue actual = Subject[key];

                Execute.Assertion
                    .ForCondition(!actual.IsSameOrEqualTo(value))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:dictionary} not to contain value {0} at key {1}{reason}, but found it anyhow.", value, key);
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        #endregion

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Context
        {
            get { return "dictionary"; }
        }
    }
}