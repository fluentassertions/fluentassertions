using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions
{
    public class CollectionAssertions : Assertions<IEnumerable, CollectionAssertions>
    {
        internal CollectionAssertions(IEnumerable collection)
        {
            if (collection != null)
            {
                ActualValue = collection;
            }
        }

        #region HaveCount

        public AndConstraint<CollectionAssertions> HaveCount(int expected)
        {
            return HaveCount(expected, String.Empty);
        }

        public AndConstraint<CollectionAssertions> HaveCount(int expected, string reason, params object[] reasonParameters)
        {
            IEnumerable<object> enumerable = ActualValue.Cast<object>();
            VerifyThat(() => enumerable.Count() == expected, "Expected {0} items{2}, but found {1}.",
                expected, enumerable.Count(), reason, reasonParameters);

            return new AndConstraint<CollectionAssertions>(this);
        }

        #endregion

        #region (Not)BeEmpty

        public AndConstraint<CollectionAssertions> BeEmpty()
        {
            return BeEmpty(String.Empty);
        }

        public AndConstraint<CollectionAssertions> BeEmpty(string reason, params object[] reasonParameters)
        {
            IEnumerable<object> enumerable = ActualValue.Cast<object>();
            
            VerifyThat(() => enumerable.Count() == 0, "Expected no items{2}, but found {1}.",
                null, enumerable.Count(), reason, reasonParameters);

            return new AndConstraint<CollectionAssertions>(this);
        }

        public AndConstraint<CollectionAssertions> NotBeEmpty()
        {
            return NotBeEmpty(String.Empty);
        }

        public AndConstraint<CollectionAssertions> NotBeEmpty(string reason, params object[] reasonParameters)
        {
            IEnumerable<object> enumerable = ActualValue.Cast<object>();

            VerifyThat(() => enumerable.Count() > 0, "Expected one or more items{2}.",
                null, enumerable.Count(), reason, reasonParameters);

            return new AndConstraint<CollectionAssertions>(this);
        }

        #endregion

        #region HaveElementAt

        public AndConstraint<CollectionAssertions> HaveElementAt(int index, object element)
        {
            return HaveElementAt(index, element, String.Empty);
        }

        public AndConstraint<CollectionAssertions> HaveElementAt(int index, object expected, string reason,
            params object[] reasonParameters)
        {
            IEnumerable<object> enumerable = ActualValue.Cast<object>();

            if (index < enumerable.Count())
            {
                var actual = ActualValue.Cast<object>().ElementAt(index);

                VerifyThat(actual.Equals(expected),
                    "Expected {0} at index " + index + "{2}, but found {1}.",
                    expected, actual, reason, reasonParameters);
            } 
            else
            {
                FailWith("Expected {0} at index " + index + "{2}, but found no element.",
                    expected, null, reason, reasonParameters);    
            }

            return new AndConstraint<CollectionAssertions>(this);
        }

        #endregion

        #region OnlyHaveUniqueItems

        public AndConstraint<CollectionAssertions> OnlyHaveUniqueItems()
        {
            return OnlyHaveUniqueItems(String.Empty);
        }

        public AndConstraint<CollectionAssertions> OnlyHaveUniqueItems(string reason, params object[] reasonParameters)
        {
            var groupWithMultipleItems = ActualValue.Cast<object>().GroupBy(o => o).FirstOrDefault(g => g.Count() > 1);
            if (groupWithMultipleItems != null)
            {
                FailWith("Expected only unique items{2}, but item {1} was found multiple times.", 
                    null, groupWithMultipleItems.Key, reason, reasonParameters);
            }

            return new AndConstraint<CollectionAssertions>(this);
        }

        #endregion

        #region ContainItemsAssignableTo

        public AndConstraint<CollectionAssertions> ContainItemsAssignableTo<T>()
        {
            return ContainItemsAssignableTo<T>(String.Empty);
        }

        public AndConstraint<CollectionAssertions> ContainItemsAssignableTo<T>(string reason, params object[] reasonParameters)
        {
            int index = 0;
            foreach (var item in ActualValue)
            {
                if (!typeof(T).IsAssignableFrom(item.GetType()))
                {
                    FailWith("Expected only {0} items in collection{2}, but item <" + item + "> at index " + index + " is of type {1}.",
                        typeof(T), item.GetType(), reason, reasonParameters);    
                }
                    
                ++index;
            }

            return new AndConstraint<CollectionAssertions>(this);
        }

        #endregion

        #region NotContainNulls

        public AndConstraint<CollectionAssertions> NotContainNulls()
        {
            return NotContainNulls(String.Empty);
        }

        public AndConstraint<CollectionAssertions> NotContainNulls(string reason, params object[] reasonParameters)
        {
            var values = ActualValue.Cast<object>().ToArray();
            for (int index = 0; index < values.Length; index++)
            {
                if (ReferenceEquals(values[index], null))
                {
                    FailWith("Expected no <null> in collection{2}, but found one at index {1}.", null, index, reason, reasonParameters);
                }
            }

            return new AndConstraint<CollectionAssertions>(this);
        }

        #endregion

        #region (Not)Equal

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by 
        /// <param name="expected"/>. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<CollectionAssertions> Equal(IEnumerable expected)
        {
            return Equal(expected, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by 
        /// <param name="expected"/>. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<CollectionAssertions> Equal(params object[] elements)
        {
            return Equal(elements, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by 
        /// <param name="expected"/>. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<CollectionAssertions> Equal(IEnumerable expected, string reason, params object[] reasonParameters)
        {
            var expectedItems = expected.Cast<object>().ToArray();
            var actualItems = ActualValue.Cast<object>().ToArray();

            for (int index = 0; index < expectedItems.Length; index++)
            {
                if (!actualItems[index].Equals(expectedItems[index]))
                {
                    FailWith("Expected collection {1} to be equal to {0}{2}, but it differs at index " + index, 
                        expected, ActualValue, reason, reasonParameters);
                }
            }

            return new AndConstraint<CollectionAssertions>(this);
        }

        /// <summary>
        /// Expects the current collection not to contain all the same elements in the same order as the collection identified by 
        /// <param name="expected"/>. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<CollectionAssertions> NotEqual(IEnumerable expected)
        {
            return NotEqual(expected, String.Empty);
        }

        /// <summary>
        /// Expects the current collection not to contain all the same elements in the same order as the collection identified by 
        /// <param name="expected"/>. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<CollectionAssertions> NotEqual(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            var actualitems = ActualValue.Cast<object>().ToList();

            if (actualitems.SequenceEqual(expected.Cast<object>()))
            {
                FailWith("Did not expect collections {0} and {1} to be equal{2}.", expected, ActualValue, reason, reasonParameters);
            }

            return new AndConstraint<CollectionAssertions>(this);
        }

        #endregion

        #region (Not)BeEquivalentTo

        /// <summary>
        /// Expects the current collection to contain all elements of the collection identified by <param name="expected"/>,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<CollectionAssertions> BeEquivalentTo(IEnumerable expected)
        {
            return BeEquivalentTo(expected, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain all elements of the collection identified by <param name="expected"/>,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<CollectionAssertions> BeEquivalentTo(params object[] elements)
        {
            return BeEquivalentTo(elements, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain all elements of the collection identified by <param name="expected"/>,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<CollectionAssertions> BeEquivalentTo(IEnumerable expected, string reason,
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

            var expectedItems = expected.Cast<object>().ToList();
            var actualItems = ActualValue.Cast<object>().ToList();
                
            if (!AreEquivalent(expectedItems, actualItems))
            {
                FailWith(
                    "Expected collection {1} to contain the same items as {0} in any order{2}.", 
                    expectedItems, actualItems, reason, reasonParameters);
            }

            return new AndConstraint<CollectionAssertions>(this);
        }

        /// <summary>
        /// Expects the current collection not to contain all elements of the collection identified by <param name="expected"/>,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<CollectionAssertions> NotBeEquivalentTo(IEnumerable expected)
        {
            return NotBeEquivalentTo(expected, String.Empty);
        }

        /// <summary>
        /// Expects the current collection not to contain all elements of the collection identified by <param name="expected"/>,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)"/>.
        /// </summary>
        public AndConstraint<CollectionAssertions> NotBeEquivalentTo(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            if (AreEquivalent(expected.Cast<object>(), ActualValue.Cast<object>()))
            {
                FailWith("Expected collection {1} not be equivalent with collection {0}.", expected, ActualValue, reason, reasonParameters);
            }

            return new AndConstraint<CollectionAssertions>(this);
        }

        private static bool AreEquivalent(IEnumerable<object> expectedItems, IEnumerable<object> actualItems)
        {
            return (expectedItems.Intersect(actualItems).Count() == expectedItems.Count()) && 
                (expectedItems.Count() == actualItems.Count());
        }

        #endregion

        #region (Not)Contain

        public AndConstraint<CollectionAssertions> Contain(object expected)
        {
            return Contain(new[] { expected }, String.Empty);
        }

        public AndConstraint<CollectionAssertions> Contain(object expected, string reason, params object[] reasonParameters)
        {
            return Contain(new[] { expected }, reason, reasonParameters);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in any order. Elements are compared
        /// using their <see cref="object.Equals(object)"/> implementation.
        /// </summary>
        public AndConstraint<CollectionAssertions> Contain(IEnumerable expected)
        {
            return Contain(expected, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in any order. Elements are compared
        /// using their <see cref="object.Equals(object)"/> implementation.
        /// </summary>
        public AndConstraint<CollectionAssertions> Contain(IEnumerable expected, string reason, params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Connect verify containment against a <null> collection");
            }

            if (expected.Cast<object>().Count() == 0)
            {
                throw new ArgumentException("Connect verify containment against an empty collection");
            }
                
            var missingItems = expected.Cast<object>().Except(ActualValue.Cast<object>());
            if (missingItems.Count() > 0)
            {
                FailWith("Expected collection {1} to contain {0}{2}, but could not find " + Format(missingItems) + ".",
                    expected, ActualValue, reason, reasonParameters);
            }

            return new AndConstraint<CollectionAssertions>(this);
        }

        public AndConstraint<CollectionAssertions> NotContain(object unexpected)
        {
            return NotContain(unexpected, String.Empty);
        }

        public AndConstraint<CollectionAssertions> NotContain(object unexpected, string reason, params object[] reasonParameters)
        {
            if (ActualValue.Cast<object>().Contains(unexpected))
            {
                FailWith("Collection {1} should not contain {0}{2}, but found it anyhow.",
                    unexpected, ActualValue, reason, reasonParameters);
            }

            return new AndConstraint<CollectionAssertions>(this);
        }

        #endregion

        #region ContainInOrder

        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order. Elements are compared
        /// using their <see cref="object.Equals(object)"/> implementation.
        /// </summary>
        public AndConstraint<CollectionAssertions> ContainInOrder(IEnumerable expected)
        {
            return ContainInOrder(expected, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order. Elements are compared
        /// using their <see cref="object.Equals(object)"/> implementation.
        /// </summary>
        public AndConstraint<CollectionAssertions> ContainInOrder(IEnumerable expected, string reason, params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify ordered containment against a <null> collection.");
            }
                
            var expectedItems = expected.Cast<object>().ToList();
            var missingItems = expectedItems.Except(ActualValue.Cast<object>());
            if (missingItems.Count() > 0)
            {
                FailWith(
                    "Expected items {0} in ordered collection {1}{2}, but " + Format(missingItems) +
                        " did not appear.",
                    expected, ActualValue, reason, reasonParameters);
            }

            // Remove anything that is not in the expected collection
            var actualMatchingItems = ActualValue.Cast<object>().Intersect(expectedItems).ToList();

            if (!expectedItems.SequenceEqual(actualMatchingItems))
            {
                FailWith("Expected items {0} in ordered collection {1}{2}, but the order did not match.",
                    expected, ActualValue, reason, reasonParameters);
            }

            return new AndConstraint<CollectionAssertions>(this);
        }

        #endregion

        #region (Not)BeSubsetOf

        public AndConstraint<CollectionAssertions> BeSubsetOf(IEnumerable expected)
        {
            return BeSubsetOf(expected, String.Empty);
        }

        public AndConstraint<CollectionAssertions> BeSubsetOf(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify a subset against a <null> collection.");
            }

            IEnumerable<object> enumerable = ActualValue.Cast<object>();
                
            if (enumerable.Count() == 0)
            {
                FailWith("Expected collection to be a subset of {0}{2}, but the subset is empty.",
                    expected, null, reason, reasonParameters);
            }
            else
            {
                var expectedItems = expected.Cast<object>();
                var actualItems = ActualValue.Cast<object>();

                var excessItems = actualItems.Except(expectedItems);

                if (excessItems.Count() > 0)
                {
                    FailWith(
                        "Expected collection to be a subset of {0}{2}, but items {1} are not part of the superset.",
                        expected, excessItems, reason, reasonParameters);
                }
            }

            return new AndConstraint<CollectionAssertions>(this);
        }

        public AndConstraint<CollectionAssertions> NotBeSubsetOf(IEnumerable expected)
        {
            return NotBeSubsetOf(expected, String.Empty);
        }

        public AndConstraint<CollectionAssertions> NotBeSubsetOf(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            var expectedItems = expected.Cast<object>();
            var actualItems = ActualValue.Cast<object>();

            if (actualItems.Intersect(expectedItems).Count() == actualItems.Count())
            {
                FailWith("Expected collection {1} not to be a subset of {0}{2}, but it is anyhow.",
                    expectedItems, actualItems, reason, reasonParameters);
            }

            return new AndConstraint<CollectionAssertions>(this);
        }

        #endregion

        /// <summary>
        /// Assert that the current collection has the same number of elements as <paramref name="otherCollection"/>.
        /// </summary>
        public AndConstraint<CollectionAssertions> HaveSameCount(IEnumerable otherCollection)
        {
            return HaveSameCount(otherCollection, "");
        }

        /// <summary>
        /// Assert that the current collection has the same number of elements as <paramref name="otherCollection"/>.
        /// </summary>
        public AndConstraint<CollectionAssertions> HaveSameCount(IEnumerable otherCollection, string reason, params object[] reasonParameters)
        {
            IEnumerable<object> enumerable = ActualValue.Cast<object>();

            int actualCount = enumerable.Count();
            int expectedCount = otherCollection.Cast<object>().Count();

            VerifyThat(() => actualCount == expectedCount,
                "Expected collection to have {0} items{2}, but found {1}.",
                expectedCount, actualCount, reason, reasonParameters);

            return new AndConstraint<CollectionAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current collection has not been initialized yet with an actual collection.
        /// </summary>
        public AndConstraint<CollectionAssertions> BeNull()
        {
            return BeNull("");
        }

        /// <summary>
        /// Asserts that the current collection has not been initialized yet with an actual collection.
        /// </summary>
        public AndConstraint<CollectionAssertions> BeNull(string reason, params object[] reasonParameters)
        {
            if (!ReferenceEquals(ActualValue, null))
            {
                FailWith("Expected collection to be <null>{2}, but found {1}.", null, ActualValue, reason, reasonParameters);  
            }

            return new AndConstraint<CollectionAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current collection has been initialized with an actual collection.
        /// </summary>
        public AndConstraint<CollectionAssertions> NotBeNull()
        {
            return NotBeNull("");
        }

        /// <summary>
        /// Asserts that the current collection has been initialized with an actual collection.
        /// </summary>
        public AndConstraint<CollectionAssertions> NotBeNull(string reason, params object[] reasonParameters)
        {
            if (ReferenceEquals(ActualValue, null))
            {
                FailWith("Expected collection not to be <null>{2}.", null, ActualValue, reason, reasonParameters);
            }

            return new AndConstraint<CollectionAssertions>(this);
        }
    }
}