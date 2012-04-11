using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Collections
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="IDictionary{TKey,TValue}"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class GenericDictionaryAssertions<TKey, TValue>
    {
        protected internal GenericDictionaryAssertions(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary != null)
            {
                Subject = dictionary;
            }
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public IDictionary<TKey, TValue> Subject { get; private set; }

        #region BeNull

        /// <summary>
        /// Asserts that the current dictionary has not been initialized yet with an actual dictionary.
        /// </summary>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> BeNull()
        {
            return BeNull(string.Empty);
        }

        /// <summary>
        /// Asserts that the current dictionary has not been initialized yet with an actual dictionary.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> BeNull(string reason, params object [] reasonArgs)
        {
            if (!ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary to be <null>{reason}, but found {0}.", Subject);
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        /// <summary>
        /// Asserts that the current dictionary has been initialized with an actual dictionary.
        /// </summary>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotBeNull()
        {
            return NotBeNull(string.Empty);
        }

        /// <summary>
        /// Asserts that the current dictionary has been initialized with an actual dictionary.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotBeNull(string reason, params object [] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary not to be <null>{reason}.");
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        #endregion

        #region HaveCount

        /// <summary>
        /// Asserts that the number of items in the dictionary matches the supplied <paramref name="expected" /> amount.
        /// </summary>
        /// <param name="expected">The expected number of items.</param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> HaveCount(int expected)
        {
            return HaveCount(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the number of items in the dictionary matches the supplied <paramref name="expected" /> amount.
        /// </summary>
        /// <param name="expected">The expected number of items.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> HaveCount(int expected, string reason,
            params object [] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {0} item(s){reason}, but found {1}.", expected, Subject);
            }

            int actualCount = Subject.Count;

            Execute.Verification
                .ForCondition((actualCount == expected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected dictionary {0} to have {1} item(s){reason}, but found {2}.", Subject, expected, actualCount);

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        /// <summary>
        /// Asserts that the number of items in the dictionary matches a condition stated by a predicate.
        /// </summary>
        /// <param name="countPredicate">The predicate which must be statisfied by the amount of items.</param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> HaveCount(Expression<Func<int, bool>> countPredicate)
        {
            return HaveCount(countPredicate, String.Empty);
        }

        /// <summary>
        /// Asserts that the number of items in the dictionary matches a condition stated by a predicate.
        /// </summary>
        /// <param name="countPredicate">The predicate which must be statisfied by the amount of items.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> HaveCount(Expression<Func<int, bool>> countPredicate,
            string reason, params object [] reasonArgs)
        {
            if (countPredicate == null)
            {
                throw new NullReferenceException("Cannot compare dictionary count against a <null> predicate.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {0} items{reason}, but found {1}.", countPredicate.Body, Subject);
            }

            Func<int, bool> compiledPredicate = countPredicate.Compile();

            int actualCount = Subject.Count;

            if (!compiledPredicate(actualCount))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary {0} to have a count {1}{reason}, but count is {2}.",
                        Subject, countPredicate.Body, actualCount);
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        #endregion

        #region BeEmpty

        /// <summary>
        /// Asserts that the dictionary does not contain any items.
        /// </summary>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> BeEmpty()
        {
            return BeEmpty(String.Empty);
        }

        /// <summary>
        /// Asserts that the dictionary does not contain any items.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> BeEmpty(string reason, params object [] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary to be empty{reason}, but found {0}.", Subject);
            }

            Execute.Verification
                .ForCondition(!Subject.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected no items{reason}, but found {0}.", Subject.Count);

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        /// <summary>
        /// Asserts that the dictionary contains at least 1 item.
        /// </summary>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotBeEmpty()
        {
            return NotBeEmpty(String.Empty);
        }

        /// <summary>
        /// Asserts that the dictionary contains at least 1 item.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotBeEmpty(string reason,
            params object [] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary not to be empty{reason}, but found {0}.", Subject);
            }

            Execute.Verification
                .ForCondition(Subject.Any())
                .BecauseOf(reason, reasonArgs)
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
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> Equal(IDictionary<TKey, TValue> expected)
        {
            return Equal(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the current dictionary contains all the same key-value pairs as the
        /// specified <paramref name="expected"/> dictionary. Keys and values are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected dictionary</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> Equal(IDictionary<TKey, TValue> expected, string reason,
            params object [] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary to be equal to {0}{reason}, but found {1}.", expected, Subject);
            }

            if (expected == null)
            {
                throw new ArgumentNullException("expected", "Cannot compare dictionary with <null>.");
            }

            IEnumerable<TKey> missingKeys = expected.Keys.Except(Subject.Keys);
            IEnumerable<TKey> additionalKeys = Subject.Keys.Except(expected.Keys);

            if (missingKeys.Any())
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary to be equal to {0}{reason}, but could not find keys {1}.", expected,
                        missingKeys);
            }

            if (additionalKeys.Any())
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary to be equal to {0}{reason}, but found additional keys {1}.", expected,
                        additionalKeys);
            }

            foreach (var key in expected.Keys)
            {
                Execute.Verification
                    .ForCondition(Subject[key].IsSameOrEqualTo(expected[key]))
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected " + Verification.SubjectNameOr("dictionary") +
                        " to be equal to {0}{reason}, but {1} differs at key {2}.", expected, Subject, key);
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        /// <summary>
        /// Asserts the current dictionary not to contain all the same key-value pairs as the
        /// specified <paramref name="unexpected"/> dictionary. Keys and values are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected dictionary</param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotEqual(IDictionary<TKey, TValue> unexpected)
        {
            return NotEqual(unexpected, String.Empty);
        }

        /// <summary>
        /// Asserts the current dictionary not to contain all the same key-value pairs as the
        /// specified <paramref name="unexpected"/> dictionary. Keys and values are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected dictionary</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotEqual(IDictionary<TKey, TValue> unexpected,
            string reason,
            params object [] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
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
                    || (Subject.Keys.Any(key => !Subject[key].Equals(unexpected[key])));

            if (!foundDifference)
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
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
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> ContainKey(TKey expected)
        {
            return ContainKey(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the dictionary contains the specified key. Keys are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected key</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> ContainKey(TKey expected, string reason,
            params object [] reasonArgs)
        {
            return ContainKeys(new [] { expected }, reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the dictionary contains all of the specified keys. Keys are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected keys</param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> ContainKeys(params TKey [] expected)
        {
            return ContainKeys(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the dictionary contains all of the specified keys. Keys are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected keys</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> ContainKeys(IEnumerable<TKey> expected, string reason,
            params object [] reasonArgs)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify key containment against a <null> collection of keys");
            }

            TKey [] expectedKeys = expected.ToArray();

            if (!expectedKeys.Any())
            {
                throw new ArgumentException("Cannot verify key containment against an empty dictionary");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary to contain keys {0}{reason}, but found {1}.", expected, Subject);
            }

            var missingKeys = expectedKeys.Except(Subject.Keys);
            if (missingKeys.Any())
            {
                if (expectedKeys.Count() > 1)
                {
                    Execute.Verification
                        .BecauseOf(reason, reasonArgs)
                        .FailWith("Expected dictionary {0} to contain key {1}{reason}, but could not find {2}.", Subject,
                            expected, missingKeys);
                }
                else
                {
                    Execute.Verification
                        .BecauseOf(reason, reasonArgs)
                        .FailWith("Expected dictionary {0} to contain key {1}{reason}.", Subject,
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
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContainKey(TKey unexpected)
        {
            return NotContainKey(unexpected, String.Empty);
        }

        /// <summary>
        /// Asserts that the current dictionary does not contain the specified <paramref name="unexpected" /> key.
        /// Keys are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected key</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContainKey(TKey unexpected, string reason,
            params object [] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary not to contain key {0}{reason}, but found {1}.", unexpected, Subject);
            }

            if (Subject.ContainsKey(unexpected))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Dictionary {0} should not contain key {1}{reason}, but found it anyhow.", Subject, unexpected);
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
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> ContainValue(TValue expected)
        {
            return ContainValue(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the dictionary contains the specified value. Values are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> ContainValue(TValue expected, string reason,
            params object [] reasonArgs)
        {
            return ContainValues(new [] { expected }, reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the dictionary contains all of the specified values. Values are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected values</param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> ContainValues(params TValue [] expected)
        {
            return ContainValues(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the dictionary contains all of the specified values. Values are compared using
        /// their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected values</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> ContainValues(IEnumerable<TValue> expected, string reason,
            params object [] reasonArgs)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify value containment against a <null> collection of values");
            }

            TValue [] expectedValues = expected.ToArray();

            if (!expectedValues.Any())
            {
                throw new ArgumentException("Cannot verify value containment against an empty dictionary");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary to contain value {0}{reason}, but found {1}.", expected, Subject);
            }

            var missingValues = expectedValues.Except(Subject.Values);
            if (missingValues.Any())
            {
                if (expectedValues.Count() > 1)
                {
                    Execute.Verification
                        .BecauseOf(reason, reasonArgs)
                        .FailWith("Expected dictionary {0} to contain value {1}{reason}, but could not find {2}.", Subject,
                            expected, missingValues);
                }
                else
                {
                    Execute.Verification
                        .BecauseOf(reason, reasonArgs)
                        .FailWith("Expected dictionary {0} to contain value {1}{reason}.", Subject,
                            expected.Cast<object>().First());
                }
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        #endregion

        #region NotContainValue

        /// <summary>
        /// Asserts that the current dictionary does not contain the specified <paramref name="unexpected" /> value.
        /// Values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContainValue(TValue unexpected)
        {
            return NotContainValue(unexpected, String.Empty);
        }

        /// <summary>
        /// Asserts that the current dictionary does not contain the specified <paramref name="unexpected" /> value.
        /// Values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContainValue(TValue unexpected, string reason,
            params object [] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary not to contain value {0}{reason}, but found {1}.", unexpected, Subject);
            }

            if (Subject.Values.Contains(unexpected))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Dictionary {0} should not contain value {1}{reason}, but found it anyhow.", Subject, unexpected);
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        #endregion

        #region Contain

        /// <summary>
        /// Asserts that the current dictionary contains the specified <paramref name="expected"/>.
        /// Keys and values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected <see cref="KeyValuePair{TKey,TValue}"/></param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> Contain(KeyValuePair<TKey, TValue> expected)
        {
            return Contain(expected.Key, expected.Value);
        }

        /// <summary>
        /// Asserts that the current dictionary contains the specified <paramref name="expected"/>.
        /// Keys and values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected <see cref="KeyValuePair{TKey,TValue}"/></param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> Contain(KeyValuePair<TKey, TValue> expected, string reason,
            params object [] reasonArgs)
        {
            return Contain(expected.Key, expected.Value, reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the current dictionary contains the specified <paramref name="value" /> for the supplied <paramref
        /// name="key" />. Values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="key">The key for which to validate the value</param>
        /// <param name="value">The value to validate</param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> Contain(TKey key, TValue value)
        {
            return Contain(key, value, String.Empty);
        }

        /// <summary>
        /// Asserts that the current dictionary contains the specified <paramref name="value" /> for the supplied <paramref
        /// name="key" />. Values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="key">The key for which to validate the value</param>
        /// <param name="value">The value to validate</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> Contain(TKey key, TValue value, string reason,
            params object [] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary to contain value {0} at key {1}{reason}, but dictionary is {2}.", value, key,
                        Subject);
            }

            if (Subject.ContainsKey(key))
            {
                TValue actual = Subject[key];

                Execute.Verification
                    .ForCondition(actual.Equals(value))
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary to contain value {0} at key {1}{reason}, but found {2}.", value, key, actual);
            }
            else
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary to contain value {0} at key {1}{reason}, but the key was not found.", value,
                        key);
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        #endregion

        #region NotContain

        /// <summary>
        /// Asserts that the current dictionary does not contain the specified <paramref name="item"/>.
        /// Keys and values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="item">The unexpected <see cref="KeyValuePair{TKey,TValue}"/></param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContain(KeyValuePair<TKey, TValue> item)
        {
            return NotContain(item.Key, item.Value);
        }

        /// <summary>
        /// Asserts that the current dictionary does not contain the specified <paramref name="item"/>.
        /// Keys and values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="item">The unexpected <see cref="KeyValuePair{TKey,TValue}"/></param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContain(KeyValuePair<TKey, TValue> item, string reason,
            params object [] reasonArgs)
        {
            return NotContain(item.Key, item.Value, reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the current dictionary does not contain the specified <paramref name="value" /> for the
        /// supplied <paramref name="key" />. Values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="key">The key for which to validate the value</param>
        /// <param name="value">The value to validate</param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContain(TKey key, TValue value)
        {
            return NotContain(key, value, String.Empty);
        }

        /// <summary>
        /// Asserts that the current dictionary does not contain the specified <paramref name="value" /> for the
        /// supplied <paramref name="key" />. Values are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="key">The key for which to validate the value</param>
        /// <param name="value">The value to validate</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GenericDictionaryAssertions<TKey, TValue>> NotContain(TKey key, TValue value, string reason,
            params object [] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary not to contain value {0} at key {1}{reason}, but dictionary is {2}.", value,
                        key, Subject);
            }

            if (Subject.ContainsKey(key))
            {
                TValue actual = Subject[key];

                Execute.Verification
                    .ForCondition(!actual.Equals(value))
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected dictionary not to contain value {0} at key {1}{reason}, but found it anyhow.", value, key);
            }

            return new AndConstraint<GenericDictionaryAssertions<TKey, TValue>>(this);
        }

        #endregion
    }
}