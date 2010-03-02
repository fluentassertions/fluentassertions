using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions
{
    public static partial class CustomAssertionExtensions
    {
        [DebuggerNonUserCode]
        public class CollectionAssertions : Assertions<ICollection, CollectionAssertions>
        {
            private readonly IEnumerable<object> actualIEnumerable;

            internal CollectionAssertions(IEnumerable collection)
            {
                actualIEnumerable = collection.Cast<object>();
                ActualValue = actualIEnumerable.ToList();
            }

            #region HaveCount

            public AndConstraint<CollectionAssertions> HaveCount(int expected)
            {
                return HaveCount(expected, string.Empty);
            }

            public AndConstraint<CollectionAssertions> HaveCount(int expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => ActualValue.Count == expected, "Expected <{0}> items{2}, but found <{1}>.",
                           expected, ActualValue.Count, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }

            #endregion

            #region (Not)BeEmpty

            public AndConstraint<CollectionAssertions> BeEmpty()
            {
                return BeEmpty(string.Empty);
            }

            public AndConstraint<CollectionAssertions> BeEmpty(string reason, params object[] reasonParameters)
            {
                AssertThat(() => ActualValue.Count == 0, "Expected no items{2}, but found <{1}>.",
                           null, ActualValue.Count, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }

            public AndConstraint<CollectionAssertions> NotBeEmpty()
            {
                return NotBeEmpty(string.Empty);
            }

            public AndConstraint<CollectionAssertions> NotBeEmpty(string reason, params object[] reasonParameters)
            {
                AssertThat(() => ActualValue.Count > 0, "Expected one or more items{2}.",
                           null, ActualValue.Count, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }

            #endregion

            #region HaveElementAt

            public AndConstraint<CollectionAssertions> HaveElementAt(int index, object element)
            {
                return HaveElementAt(index, element, string.Empty);
            }

            public AndConstraint<CollectionAssertions> HaveElementAt(int index, object expected, string reason,
                                                                     params object[] reasonParameters)
            {
                var actual = actualIEnumerable.ElementAt(index);
                AssertThat(() => Assert.AreEqual(expected, actual),
                           "Expected <{0}> at the supplied index{2}, but found <{1}>.",
                           expected, actual, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }

            #endregion

            #region OnlyHaveUniqueItems

            public AndConstraint<CollectionAssertions> OnlyHaveUniqueItems()
            {
                return OnlyHaveUniqueItems(string.Empty);
            }

            public AndConstraint<CollectionAssertions> OnlyHaveUniqueItems(string reason, params object[] reasonParameters)
            {
                AssertThat(() => CollectionAssert.AllItemsAreUnique(ActualValue),
                           "Expected only unique items in current collection{2}.",
                           null, null, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }

            #endregion

            #region OnlyContainItemsOfType

            public AndConstraint<CollectionAssertions> OnlyContainItemsOfType<T>()
            {
                return OnlyContainItemsOfType<T>(string.Empty);
            }

            public AndConstraint<CollectionAssertions> OnlyContainItemsOfType<T>(string reason, params object[] reasonParameters)
            {
                AssertThat(() => CollectionAssert.AllItemsAreInstancesOfType(ActualValue, typeof(T)),
                           "Expected only <{0}> items in current collection{2}.",
                           typeof(T), null, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }

            #endregion

            #region NotContainNulls

            public AndConstraint<CollectionAssertions> NotContainNulls()
            {
                return NotContainNulls(string.Empty);
            }

            public AndConstraint<CollectionAssertions> NotContainNulls(string reason, params object[] reasonParameters)
            {
                AssertThat(() => CollectionAssert.AllItemsAreNotNull(ActualValue, reason, reasonParameters),
                           "Did not expect current collection to contain null values because we want to test the failure message.",
                           null, null, reason, reasonParameters);

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
                return Equal(expected, string.Empty);
            }

            /// <summary>
            /// Expects the current collection to contain all the same elements in the same order as the collection identified by 
            /// <param name="expected"/>. Elements are compared using their <see cref="object.Equals(object)"/>.
            /// </summary>
            public AndConstraint<CollectionAssertions> Equal(params object[] elements)
            {
                return Equal(elements, string.Empty);
            }

            /// <summary>
            /// Expects the current collection to contain all the same elements in the same order as the collection identified by 
            /// <param name="expected"/>. Elements are compared using their <see cref="object.Equals(object)"/>.
            /// </summary>
            public AndConstraint<CollectionAssertions> Equal(IEnumerable expected, string reason, params object[] reasonParameters)
            {
                var collection = expected.Cast<object>().ToList();

                AssertThat(() => CollectionAssert.AreEqual(collection, ActualValue), "Expected collections to be equal{2}.",
                           null, null, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }

            /// <summary>
            /// Expects the current collection not to contain all the same elements in the same order as the collection identified by 
            /// <param name="expected"/>. Elements are compared using their <see cref="object.Equals(object)"/>.
            /// </summary>
            public AndConstraint<CollectionAssertions> NotEqual(IEnumerable expected)
            {
                return NotEqual(expected, string.Empty);
            }

            /// <summary>
            /// Expects the current collection not to contain all the same elements in the same order as the collection identified by 
            /// <param name="expected"/>. Elements are compared using their <see cref="object.Equals(object)"/>.
            /// </summary>
            public AndConstraint<CollectionAssertions> NotEqual(IEnumerable expected, string reason,
                                                                params object[] reasonParameters)
            {
                var collection = expected.Cast<object>().ToList();

                AssertThat(() => CollectionAssert.AreNotEqual(collection, ActualValue),
                           "Did not expect collections to be equal{2}.", null, null, reason, reasonParameters);

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
                return BeEquivalentTo(expected, string.Empty);
            }

            /// <summary>
            /// Expects the current collection to contain all elements of the collection identified by <param name="expected"/>,
            /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)"/>.
            /// </summary>
            public AndConstraint<CollectionAssertions> BeEquivalentTo(params object[] elements)
            {
                return BeEquivalentTo(elements, string.Empty);
            }

            /// <summary>
            /// Expects the current collection to contain all elements of the collection identified by <param name="expected"/>,
            /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)"/>.
            /// </summary>
            public AndConstraint<CollectionAssertions> BeEquivalentTo(IEnumerable expected, string reason,
                                                                      params object[] reasonParameters)
            {
                var collection = expected.Cast<object>().ToList();

                AssertThat(() => CollectionAssert.AreEquivalent(collection, ActualValue),
                           "Expected collections to be equivalent{2}.", null, null, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }

            /// <summary>
            /// Expects the current collection not to contain all elements of the collection identified by <param name="expected"/>,
            /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)"/>.
            /// </summary>
            public AndConstraint<CollectionAssertions> NotBeEquivalentTo(IEnumerable expected)
            {
                return NotBeEquivalentTo(expected, string.Empty);
            }

            /// <summary>
            /// Expects the current collection not to contain all elements of the collection identified by <param name="expected"/>,
            /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)"/>.
            /// </summary>
            public AndConstraint<CollectionAssertions> NotBeEquivalentTo(IEnumerable expected, string reason,
                                                                         params object[] reasonParameters)
            {
                var collection = expected.Cast<object>().ToList();

                AssertThat(() => CollectionAssert.AreNotEquivalent(collection, ActualValue),
                           "Did not expect collections to be equivalent{2}.", null, null, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }

            #endregion

            #region (Not)Contain

            public AndConstraint<CollectionAssertions> Contain(object expected)
            {
                return Contain(new[] { expected }, string.Empty);
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
                return Contain(expected, string.Empty);
            }

            /// <summary>
            /// Expects the current collection to contain the specified elements in any order. Elements are compared
            /// using their <see cref="object.Equals(object)"/> implementation.
            /// </summary>
            public AndConstraint<CollectionAssertions> Contain(IEnumerable expected, string reason, params object[] reasonParameters)
            {
                var expectedCollection = expected.Cast<object>().ToList();

                AssertThat(() => CollectionAssert.IsSubsetOf(expectedCollection, ActualValue),
                           "Expected current collection to contain <{0}>{2}.", expectedCollection, null, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }

            public AndConstraint<CollectionAssertions> NotContain(object expected)
            {
                return NotContain(expected, string.Empty);
            }

            public AndConstraint<CollectionAssertions> NotContain(object unexpected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => CollectionAssert.DoesNotContain(ActualValue, unexpected),
                           "Did not expect current collection to contain <{0}>{2}.", unexpected, null, reason, reasonParameters);

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
                return ContainInOrder(expected, string.Empty);
            }

            /// <summary>
            /// Expects the current collection to contain the specified elements in the exact same order. Elements are compared
            /// using their <see cref="object.Equals(object)"/> implementation.
            /// </summary>
            public AndConstraint<CollectionAssertions> ContainInOrder(IEnumerable expected, string reason, params object[] reasonParameters)
            {
                // Remove anything that is not in the expected collection
                var intersection = ActualValue.Cast<object>().Intersect(expected.Cast<object>()).ToList();

                var expectedCollection = expected.Cast<object>().ToList();

                AssertThat(() => CollectionAssert.AreEqual(expectedCollection, intersection),
                           "Expected current collection to contain <{0}> in that order{2}.", expected, null, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }

            #endregion

            #region (Not)BeSubsetOf

            public AndConstraint<CollectionAssertions> BeSubsetOf(IEnumerable expected)
            {
                return BeSubsetOf(expected, string.Empty);
            }

            public AndConstraint<CollectionAssertions> BeSubsetOf(IEnumerable expected, string reason,
                                                                  params object[] reasonParameters)
            {
                var collection = expected.Cast<object>().ToList();

                AssertThat(() => CollectionAssert.IsSubsetOf(ActualValue, collection),
                           "Expected current collection to be a subset of the supplied collection{2}.",
                           null, null, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }

            public AndConstraint<CollectionAssertions> NotBeSubsetOf(IEnumerable expected)
            {
                return NotBeSubsetOf(expected, string.Empty);
            }

            public AndConstraint<CollectionAssertions> NotBeSubsetOf(IEnumerable expected, string reason,
                                                                     params object[] reasonParameters)
            {
                var collection = expected.Cast<object>().ToList();

                AssertThat(() => CollectionAssert.IsNotSubsetOf(ActualValue, collection),
                           "Did not expect current collection to be a subset of the supplied collection{2}.",
                           null, null, reason, reasonParameters);

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
                int actualCount = ActualValue.Count;
                int expectedCount = otherCollection.Cast<object>().Count();

                AssertThat(() => actualCount == expectedCount,
                           "Expected collection to have <{0}> items{2}, but found <{1}>.",
                           expectedCount, actualCount, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }
        }
    }
}