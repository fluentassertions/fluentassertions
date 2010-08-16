using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAssertions
{
    public abstract class CollectionAssertions<TSubject, TAssertions> : Assertions<TSubject, TAssertions>
        where TAssertions : CollectionAssertions<TSubject, TAssertions>
        where TSubject : IEnumerable
    {
        public AndConstraint<TAssertions> HaveCount(int expected)
        {
            return HaveCount(expected, String.Empty);
        }

        public AndConstraint<TAssertions> HaveCount(int expected, string reason, params object[] reasonParameters)
        {
            VerifySubjectCollectionAgainstNull("Expected {0} items{2}, but found {1}", expected, reason,
                                               reasonParameters);

            IEnumerable<object> enumerable = Subject.Cast<object>();
            VerifyThat(() => enumerable.Count() == expected, "Expected {0} items{2}, but found {1}.",
                expected, enumerable.Count(), reason, reasonParameters);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the number of items in the collection matches a condition stated by a predicate.
        /// </summary>
        public AndConstraint<TAssertions> HaveCount(Expression<Func<int, bool>> countPredicate)
        {
            return HaveCount(countPredicate, String.Empty);
        }

        /// <summary>
        /// Asserts that the number of items in the collection matches a condition stated by a predicate.
        /// </summary>
        public AndConstraint<TAssertions> HaveCount(Expression<Func<int, bool>> countPredicate, string reason, params object[] reasonParameters)
        {
            if (countPredicate == null)
            {
                throw new NullReferenceException("Cannot compare collection count against a <null> predicate.");
            }

            VerifySubjectCollectionAgainstNull("Expected {0} items{2}, but found {1}", countPredicate.Body, reason,
                                               reasonParameters);

            Func<int, bool> compiledPredicate = countPredicate.Compile();

            int actualCount = Subject.Cast<object>().Count();

            if (!compiledPredicate(actualCount))
            {
                FailWith("Expected collection {0} to have a count " + countPredicate.Body + "{2}, but count is {1}.", Subject, actualCount, reason, reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeEmpty()
        {
            return BeEmpty(String.Empty);
        }

        public AndConstraint<TAssertions> BeEmpty(string reason, params object[] reasonParameters)
        {
            VerifySubjectCollectionAgainstNull("Expected collection to be empty{2}, but found {1}", "", reason,
                                               reasonParameters);

            IEnumerable<object> enumerable = Subject.Cast<object>();

            VerifyThat(() => enumerable.Count() == 0, "Expected no items{2}, but found {1}.",
                null, enumerable.Count(), reason, reasonParameters);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> NotBeEmpty()
        {
            return NotBeEmpty(String.Empty);
        }

        public AndConstraint<TAssertions> NotBeEmpty(string reason, params object[] reasonParameters)
        {
            VerifySubjectCollectionAgainstNull("Expected collection not to be empty{2}, but found {1}", "", reason,
                                               reasonParameters);

            IEnumerable<object> enumerable = Subject.Cast<object>();

            VerifyThat(() => enumerable.Count() > 0, "Expected one or more items{2}.",
                null, enumerable.Count(), reason, reasonParameters);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> OnlyHaveUniqueItems()
        {
            return OnlyHaveUniqueItems(String.Empty);
        }

        public AndConstraint<TAssertions> OnlyHaveUniqueItems(string reason, params object[] reasonParameters)
        {
            VerifySubjectCollectionAgainstNull("Expected collection to only have unique items{2}, but found {1}", "", reason,
                                               reasonParameters);

            var groupWithMultipleItems = Subject.Cast<object>().GroupBy(o => o).FirstOrDefault(g => g.Count() > 1);
            if (groupWithMultipleItems != null)
            {
                FailWith("Expected only unique items{2}, but item {1} was found multiple times.",
                    null, groupWithMultipleItems.Key, reason, reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> NotContainNulls()
        {
            return NotContainNulls(String.Empty);
        }

        public AndConstraint<TAssertions> NotContainNulls(string reason, params object[] reasonParameters)
        {
            VerifySubjectCollectionAgainstNull("Expected collection to not contain nulls{2}, but found {1}", "", reason,
                                               reasonParameters);

            var values = Subject.Cast<object>().ToArray();
            for (int index = 0; index < values.Length; index++)
            {
                if (ReferenceEquals(values[index], null))
                {
                    FailWith("Expected no <null> in collection{2}, but found one at index {1}.", null, index, reason,
                        reasonParameters);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by 
        /// <param name="expected"/>. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<TAssertions> Equal(IEnumerable expected)
        {
            return Equal(expected, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by 
        /// <param name="expected"/>. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<TAssertions> Equal(params object[] elements)
        {
            return Equal(elements, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by 
        /// <param name="expected"/>. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<TAssertions> Equal(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            VerifySubjectCollectionAgainstNull("Expected collections to be equal{2}, but found {1}", expected, reason,
                                               reasonParameters);

            if (expected == null)
            {
                throw new ArgumentNullException("expected", "Cannot compare collection with <null>.");
            }

            var expectedItems = expected.Cast<object>().ToArray();
            var actualItems = Subject.Cast<object>().ToArray();

            for (int index = 0; index < expectedItems.Length; index++)
            {
                if ((actualItems.Length <= index ) || !actualItems[index].Equals(expectedItems[index]))
                {
                    FailWith("Expected collection {1} to be equal to {0}{2}, but it differs at index " + index,
                        expected, Subject, reason, reasonParameters);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection not to contain all the same elements in the same order as the collection identified by 
        /// <param name="expected"/>. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<TAssertions> NotEqual(IEnumerable expected)
        {
            return NotEqual(expected, String.Empty);
        }

        /// <summary>
        /// Expects the current collection not to contain all the same elements in the same order as the collection identified by 
        /// <param name="expected"/>. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<TAssertions> NotEqual(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            VerifySubjectCollectionAgainstNull("Expected collections not to be equal{2}, but found {1}", expected, reason,
                                               reasonParameters);

            if (expected == null)
            {
                throw new ArgumentNullException("expected", "Cannot compare collection with <null>.");
            }

            var actualitems = Subject.Cast<object>().ToList();

            if (actualitems.SequenceEqual(expected.Cast<object>()))
            {
                FailWith("Did not expect collections {0} and {1} to be equal{2}.", expected, Subject, reason,
                    reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to contain all elements of the collection identified by <param name="expected"/>,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<TAssertions> BeEquivalentTo(IEnumerable expected)
        {
            return BeEquivalentTo(expected, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain all elements of the collection identified by <param name="expected"/>,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<TAssertions> BeEquivalentTo(params object[] elements)
        {
            return BeEquivalentTo(elements, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain all elements of the collection identified by <param name="expected"/>,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<TAssertions> BeEquivalentTo(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify equivalence against a <null> collection.");
            }

            if (expected.Cast<object>().Count() == 0)
            {
                throw new ArgumentException("Cannot verify equivalence against an empty collection.");
            }

            VerifySubjectCollectionAgainstNull("Expected collections to be equivalent{2}, but found {1}", expected, reason,
                                               reasonParameters);

            var expectedItems = expected.Cast<object>().ToList();
            var actualItems = Subject.Cast<object>().ToList();

            if (!AreEquivalent(expectedItems, actualItems))
            {
                FailWith(
                    "Expected collection {1} to contain the same items as {0} in any order{2}.",
                    expectedItems, actualItems, reason, reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection not to contain all elements of the collection identified by <param name="expected"/>,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<TAssertions> NotBeEquivalentTo(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify inequivalence against a <null> collection.");
            }

            if (expected.Cast<object>().Count() == 0)
            {
                throw new ArgumentException("Cannot verify inequivalence against an empty collection.");
            }

            VerifySubjectCollectionAgainstNull("Expected collections not to be equivalent{2}, but found {1}", expected, reason,
                                               reasonParameters);

            if (AreEquivalent(expected.Cast<object>(), Subject.Cast<object>()))
            {
                FailWith("Expected collection {1} not be equivalent with collection {0}.", expected, Subject, reason,
                    reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private static bool AreEquivalent(IEnumerable<object> expectedItems, IEnumerable<object> actualItems)
        {
            return (expectedItems.Intersect(actualItems).Count() == expectedItems.Count()) &&
                (expectedItems.Count() == actualItems.Count());
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in any order. Elements are compared
        /// using their <see cref="object.Equals(object)"/> implementation.
        /// </summary>
        public AndConstraint<TAssertions> Contain(IEnumerable expected)
        {
            return Contain(expected, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in any order. Elements are compared
        /// using their <see cref="object.Equals(object)"/> implementation.
        /// </summary>
        public AndConstraint<TAssertions> Contain(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Connect verify containment against a <null> collection");
            }

            IEnumerable<object> expectedObjects = expected.Cast<object>();
            if (expectedObjects.Count() == 0)
            {
                throw new ArgumentException("Connect verify containment against an empty collection");
            }

            VerifySubjectCollectionAgainstNull("Expected collection to contain {0}, but found {1}", expected, reason,
                                               reasonParameters);

            if (expected is string)
            {
                if (!Subject.Cast<object>().Contains(expected))
                {
                    FailWith("Expected collection {1} to contain {0}{2}.", expected, Subject, reason, reasonParameters);
                }
            }
            else
            {
                var missingItems = expectedObjects.Except(Subject.Cast<object>());
                if (missingItems.Count() > 0)
                {
                    if (expectedObjects.Count() > 1)
                    {
                        FailWith(
                            "Expected collection {1} to contain {0}{2}, but could not find " + Format(missingItems) + ".",
                            expected, Subject, reason, reasonParameters);
                    }
                    else
                    {
                        FailWith(
                            "Expected collection {1} to contain {0}{2}.", expectedObjects.Single(), Subject, reason, reasonParameters);
                    }
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order. Elements are compared
        /// using their <see cref="object.Equals(object)"/> implementation.
        /// </summary>
        public AndConstraint<TAssertions> ContainInOrder(IEnumerable expected)
        {
            return ContainInOrder(expected, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order. Elements are compared
        /// using their <see cref="object.Equals(object)"/> implementation.
        /// </summary>
        public AndConstraint<TAssertions> ContainInOrder(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify ordered containment against a <null> collection.");
            }

            VerifySubjectCollectionAgainstNull("Expected collection to contain {0} in order, but found {1}", expected, reason,
                                               reasonParameters);

            var expectedItems = expected.Cast<object>().ToList();
            var actualItems = Subject.Cast<object>();
            var missingItems = expectedItems.Except(actualItems);
            if (missingItems.Count() > 0)
            {
                FailWith(
                    "Expected items {0} in ordered collection {1}{2}, but " + Format(missingItems) +
                        " did not appear.",
                    expected, Subject, reason, reasonParameters);
            }

            var actualMatchingItems = RemoveItemsThatWereNotExpected(actualItems, expectedItems);

            if (!expectedItems.SequenceEqual(actualMatchingItems))
            {
                FailWith("Expected items {0} in ordered collection {1}{2}, but the order did not match.",
                    expected, Subject, reason, reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private static IEnumerable<object> RemoveItemsThatWereNotExpected(IEnumerable<object> actualItems, IEnumerable<object> expectedItems)
        {
            return actualItems.Where(item => expectedItems.Any(expected => expected.Equals(item))).ToArray();
        }

        public AndConstraint<TAssertions> BeSubsetOf(IEnumerable expected)
        {
            return BeSubsetOf(expected, String.Empty);
        }

        public AndConstraint<TAssertions> BeSubsetOf(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify a subset against a <null> collection.");
            }

            VerifySubjectCollectionAgainstNull("Expected collection to be a subset of {0}{2}, but found {1}", expected, reason,
                                               reasonParameters);

            IEnumerable<object> enumerable = Subject.Cast<object>();

            if (enumerable.Count() == 0)
            {
                FailWith("Expected collection to be a subset of {0}{2}, but the subset is empty.",
                    expected, null, reason, reasonParameters);
            }
            else
            {
                var expectedItems = expected.Cast<object>();
                var actualItems = Subject.Cast<object>();

                var excessItems = actualItems.Except(expectedItems);

                if (excessItems.Count() > 0)
                {
                    FailWith(
                        "Expected collection to be a subset of {0}{2}, but items {1} are not part of the superset.",
                        expected, excessItems, reason, reasonParameters);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> NotBeSubsetOf(IEnumerable expected)
        {
            return NotBeSubsetOf(expected, String.Empty);
        }

        public AndConstraint<TAssertions> NotBeSubsetOf(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            VerifySubjectCollectionAgainstNull("Expected collection not to be a subset of {0}{2}, but found {1}",
                                               expected, reason, reasonParameters);

            var expectedItems = expected.Cast<object>();
            var actualItems = Subject.Cast<object>();

            if (actualItems.Intersect(expectedItems).Count() == actualItems.Count())
            {
                FailWith("Expected collection {1} not to be a subset of {0}{2}, but it is anyhow.",
                    expectedItems, actualItems, reason, reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Assert that the current collection has the same number of elements as <paramref name="otherCollection"/>.
        /// </summary>
        public AndConstraint<TAssertions> HaveSameCount(IEnumerable otherCollection)
        {
            return HaveSameCount(otherCollection, "");
        }

        /// <summary>
        /// Assert that the current collection has the same number of elements as <paramref name="otherCollection"/>.
        /// </summary>
        public AndConstraint<TAssertions> HaveSameCount(IEnumerable otherCollection, string reason,
            params object[] reasonParameters)
        {
            if (otherCollection == null)
            {
                throw new NullReferenceException("Cannot verify count against a <null> collection.");
            }

            VerifySubjectCollectionAgainstNull("Expected collection to have the same count as {0}{2}, but found {1}", otherCollection, reason,
                                               reasonParameters);

            IEnumerable<object> enumerable = Subject.Cast<object>();

            int actualCount = enumerable.Count();
            int expectedCount = otherCollection.Cast<object>().Count();

            VerifyThat(() => actualCount == expectedCount,
                "Expected collection to have {0} items{2}, but found {1}.",
                expectedCount, actualCount, reason, reasonParameters);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current collection has not been initialized yet with an actual collection.
        /// </summary>
        public AndConstraint<TAssertions> BeNull()
        {
            return BeNull("");
        }

        /// <summary>
        /// Asserts that the current collection has not been initialized yet with an actual collection.
        /// </summary>
        public AndConstraint<TAssertions> BeNull(string reason, params object[] reasonParameters)
        {
            if (!ReferenceEquals(Subject, null))
            {
                FailWith("Expected collection to be <null>{2}, but found {1}.", null, Subject, reason,
                    reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current collection has been initialized with an actual collection.
        /// </summary>
        public AndConstraint<TAssertions> NotBeNull()
        {
            return NotBeNull("");
        }

        /// <summary>
        /// Asserts that the current collection has been initialized with an actual collection.
        /// </summary>
        public AndConstraint<TAssertions> NotBeNull(string reason, params object[] reasonParameters)
        {
            if (ReferenceEquals(Subject, null))
            {
                FailWith("Expected collection not to be <null>{2}.", null, Subject, reason, reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> HaveElementAt(int index, object element)
        {
            return HaveElementAt(index, element, String.Empty);
        }

        public AndConstraint<TAssertions> HaveElementAt(int index, object expected, string reason,
            params object[] reasonParameters)
        {
            VerifySubjectCollectionAgainstNull("Expected collection to have element at index {0}{2}, but found {1}",
                                               index, reason, reasonParameters);

            IEnumerable<object> enumerable = Subject.Cast<object>();

            if (index < enumerable.Count())
            {
                var actual = Subject.Cast<object>().ElementAt(index);

                VerifyThat(actual.Equals(expected),
                    "Expected {0} at index " + index + "{2}, but found {1}.",
                    expected, actual, reason, reasonParameters);
            }
            else
            {
                FailWith("Expected {0} at index " + index + "{2}, but found no element.",
                    expected, null, reason, reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> ContainItemsAssignableTo<T>()
        {
            return ContainItemsAssignableTo<T>(String.Empty);
        }

        public AndConstraint<TAssertions> ContainItemsAssignableTo<T>(string reason,
            params object[] reasonParameters)
        {
            VerifySubjectCollectionAgainstNull("Expected collection to contain element assignable to type {0}{2}, but found {1}",
                                               typeof(T), reason, reasonParameters);

            int index = 0;
            foreach (var item in Subject)
            {
                if (!typeof(T).IsAssignableFrom(item.GetType()))
                {
                    FailWith(
                        "Expected only {0} items in collection{2}, but item <" + item + "> at index " + index +
                            " is of type {1}.",
                        typeof(T), item.GetType(), reason, reasonParameters);
                }

                ++index;
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection not to contain all elements of the collection identified by <param name="expected"/>,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<TAssertions> NotBeEquivalentTo(IEnumerable expected)
        {
            return NotBeEquivalentTo(expected, String.Empty);
        }

        public AndConstraint<TAssertions> NotContain(object unexpected)
        {
            return NotContain(unexpected, String.Empty);
        }

        public AndConstraint<TAssertions> NotContain(object unexpected, string reason,
            params object[] reasonParameters)
        {
            VerifySubjectCollectionAgainstNull("Expected collection not to contain element {0}{2}, but found {1}",
                                               unexpected, reason, reasonParameters);

            if (Subject.Cast<object>().Contains(unexpected))
            {
                FailWith("Collection {1} should not contain {0}{2}, but found it anyhow.",
                    unexpected, Subject, reason, reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        protected void VerifySubjectCollectionAgainstNull(string formattedMessage, object expected, string reason, object[] reasonParameters)
        {
            if (Subject == null)
            {
                FailWith(formattedMessage, expected, Subject, reason, reasonParameters);
            }
        }
    }
}