using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions
{
    public static partial class FluentAssertionExtensions
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
                VerifyThat(() => ActualValue.Count == expected, "Expected <{0}> items{2}, but found <{1}>.",
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
                VerifyThat(() => ActualValue.Count == 0, "Expected no items{2}, but found <{1}>.",
                           null, ActualValue.Count, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }

            public AndConstraint<CollectionAssertions> NotBeEmpty()
            {
                return NotBeEmpty(string.Empty);
            }

            public AndConstraint<CollectionAssertions> NotBeEmpty(string reason, params object[] reasonParameters)
            {
                VerifyThat(() => ActualValue.Count > 0, "Expected one or more items{2}.",
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
                VerifyThat(() => Assert.AreEqual(expected, actual),
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
                VerifyThat(() => CollectionAssert.AllItemsAreUnique(ActualValue),
                           "Expected only unique items in current collection{2}.",
                           null, null, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }

            #endregion

            #region ContainItemsAssignableTo

            public AndConstraint<CollectionAssertions> ContainItemsAssignableTo<T>()
            {
                return ContainItemsAssignableTo<T>(string.Empty);
            }

            public AndConstraint<CollectionAssertions> ContainItemsAssignableTo<T>(string reason, params object[] reasonParameters)
            {
                int index = 0;
                foreach (var item in ActualValue)
                {
                    if (!typeof(T).IsAssignableFrom(item.GetType()))
                    {
                        FailWith("Expected only <{0}> items in collection{2}, but item <" + item + "> at index " + index + " is of type <{1}>.",
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
                return NotContainNulls(string.Empty);
            }

            public AndConstraint<CollectionAssertions> NotContainNulls(string reason, params object[] reasonParameters)
            {
                VerifyThat(() => CollectionAssert.AllItemsAreNotNull(ActualValue, reason, reasonParameters),
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
                var expectedItems = expected.Cast<object>().ToArray();
                var actualItems = ActualValue.Cast<object>().ToArray();

                for (int index = 0; index < expectedItems.Length; index++)
                {
                    if (!actualItems[index].Equals(expectedItems[index]))
                    {
                        FailWith("Expected collection <{1}> to be equal to <{0}>{2}, but it differs at index " + index, 
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
                return NotEqual(expected, string.Empty);
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
                    FailWith("Did not expect collections <{0}> and <{1}> to be equal{2}.", expected, ActualValue, reason, reasonParameters);
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
                if (expected == null)
                {
                    throw new NullReferenceException("Cannot verify equivalence against a <null> collection.");
                }

                if (expected.Cast<object>().Count() == 0)
                {
                    throw new ArgumentException("Cannot verify equivalence against an empty collection.");
                }

                var collection = expected.Cast<object>().ToList();

                VerifyThat(() => CollectionAssert.AreEquivalent(collection, ActualValue),
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

                VerifyThat(() => CollectionAssert.AreNotEquivalent(collection, ActualValue),
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
                    FailWith("Expected collection <{1}> to contain <{0}>{2}, but could not find <" + Expand(missingItems) + ">.",
                        expected, ActualValue, reason, reasonParameters);
                }

                return new AndConstraint<CollectionAssertions>(this);
            }

            public AndConstraint<CollectionAssertions> NotContain(object unexpected)
            {
                return NotContain(unexpected, string.Empty);
            }

            public AndConstraint<CollectionAssertions> NotContain(object unexpected, string reason, params object[] reasonParameters)
            {
                if (ActualValue.Cast<object>().Contains(unexpected))
                {
                    FailWith("Collection <{1}> should not contain <{0}>{2}, but found it anyhow.",
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
                return ContainInOrder(expected, string.Empty);
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
                        "Expected items <{0}> in ordered collection <{1}>{2}, but <" + Expand(missingItems) +
                            "> did not appear.",
                        expected, ActualValue, reason, reasonParameters);
                }

                // Remove anything that is not in the expected collection
                var actualMatchingItems = ActualValue.Cast<object>().Intersect(expectedItems).ToList();

                if (!expectedItems.SequenceEqual(actualMatchingItems))
                {
                    FailWith("Expected items <{0}> in ordered collection <{1}>{2}, but the order did not match.",
                        expected, ActualValue, reason, reasonParameters);
                }

                return new AndConstraint<CollectionAssertions>(this);
            }

            private static object Expand(object val)
            {
                var enumerable = val as IEnumerable;
                if ((enumerable != null) && !(val is string))
                {
                    return string.Join(", ", enumerable.Cast<object>().Select(o => o.ToString()).ToArray());
                }

                return val;
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
                if (expected == null)
                {
                    throw new NullReferenceException("Cannot verify a subset against a <null> collection.");
                }
                
                if (ActualValue.Count == 0)
                {
                    FailWith("Expected collection to be a subset of <{0}>{2}, but the subset is empty.",
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
                            "Expected collection to be a subset of <{0}>{2}, but items <{1}> are not part of the superset.",
                            expected, excessItems, reason, reasonParameters);
                    }
                }

                return new AndConstraint<CollectionAssertions>(this);
            }

            public AndConstraint<CollectionAssertions> NotBeSubsetOf(IEnumerable expected)
            {
                return NotBeSubsetOf(expected, string.Empty);
            }

            public AndConstraint<CollectionAssertions> NotBeSubsetOf(IEnumerable expected, string reason,
                                                                     params object[] reasonParameters)
            {
                var expectedItems = expected.Cast<object>();
                var actualItems = ActualValue.Cast<object>();

                if (expectedItems.Contains(actualItems))
                {
                    FailWith("Expected collection <{1}> not to be a subset of <1, 2, 3>{2}, but it is anyhow.",
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
                int actualCount = ActualValue.Count;
                int expectedCount = otherCollection.Cast<object>().Count();

                VerifyThat(() => actualCount == expectedCount,
                           "Expected collection to have <{0}> items{2}, but found <{1}>.",
                           expectedCount, actualCount, reason, reasonParameters);

                return new AndConstraint<CollectionAssertions>(this);
            }
        }
    }
}