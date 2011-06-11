using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions.Common;
using FluentAssertions.Formatting;

namespace FluentAssertions.Assertions
{
    public abstract class CollectionAssertions<TSubject, TAssertions> : ReferenceTypeAssertions<TSubject, TAssertions>
        where TAssertions : CollectionAssertions<TSubject, TAssertions>
        where TSubject : IEnumerable
    {
        /// <summary>
        ///   Asserts that the number of items in the collection matches the supplied <paramref name = "expected" /> amount.
        /// </summary>
        public AndConstraint<TAssertions> HaveCount(int expected)
        {
            return HaveCount(expected, String.Empty);
        }

        /// <summary>
        ///   Asserts that the number of items in the collection matches the supplied <paramref name = "expected" /> amount.
        /// </summary>
        public AndConstraint<TAssertions> HaveCount(int expected, string reason, params object[] reasonParameters)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected {0} item(s){2}, but found {1}.", expected, Subject, reason, reasonParameters);
            }

            int actualCount = Subject.Cast<object>().Count();

            Execute.Verify(() => actualCount == expected, "Expected {0} item(s){2}, but found {1}.",
                expected, actualCount, reason, reasonParameters);

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Asserts that the number of items in the collection matches a condition stated by a predicate.
        /// </summary>
        public AndConstraint<TAssertions> HaveCount(Expression<Func<int, bool>> countPredicate)
        {
            return HaveCount(countPredicate, String.Empty);
        }

        /// <summary>
        ///   Asserts that the number of items in the collection matches a condition stated by a predicate.
        /// </summary>
        public AndConstraint<TAssertions> HaveCount(Expression<Func<int, bool>> countPredicate, string reason,
            params object[] reasonParameters)
        {
            if (countPredicate == null)
            {
                throw new NullReferenceException("Cannot compare collection count against a <null> predicate.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected {0} items{2}, but found {1}.", countPredicate.Body, Subject, reason,
                    reasonParameters);
            }

            Func<int, bool> compiledPredicate = countPredicate.Compile();

            int actualCount = Subject.Cast<object>().Count();

            if (!compiledPredicate(actualCount))
            {
                Execute.Fail(
                    "Expected collection {0} to have a count " + countPredicate.Body + "{2}, but count is {1}.", Subject,
                    actualCount, reason, reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Asserts that the collection does not contain any items.
        /// </summary>
        public AndConstraint<TAssertions> BeEmpty()
        {
            return BeEmpty(String.Empty);
        }

        /// <summary>
        ///   Asserts that the collection does not contain any items.
        /// </summary>
        public AndConstraint<TAssertions> BeEmpty(string reason, params object[] reasonParameters)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collection to be empty{2}, but found {1}.", "", Subject, reason, reasonParameters);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            Execute.Verify(() => !enumerable.Any(), "Expected no items{2}, but found {1}.",
                null, enumerable.Count(), reason, reasonParameters);

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Asserts that the collection contains at least 1 item.
        /// </summary>
        public AndConstraint<TAssertions> NotBeEmpty()
        {
            return NotBeEmpty(String.Empty);
        }

        /// <summary>
        ///   Asserts that the collection contains at least 1 item.
        /// </summary>
        public AndConstraint<TAssertions> NotBeEmpty(string reason, params object[] reasonParameters)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collection not to be empty{2}, but found {1}.", "", Subject, reason,
                    reasonParameters);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            Execute.Verify(() => enumerable.Any(), "Expected one or more items{2}.",
                null, 0, reason, reasonParameters);

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Asserts that the collection does not contain any duplicate items.
        /// </summary>
        public AndConstraint<TAssertions> OnlyHaveUniqueItems()
        {
            return OnlyHaveUniqueItems(String.Empty);
        }

        /// <summary>
        ///   Asserts that the collection does not contain any duplicate items.
        /// </summary>
        public AndConstraint<TAssertions> OnlyHaveUniqueItems(string reason, params object[] reasonParameters)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collection to only have unique items{2}, but found {1}.", "", Subject, reason,
                    reasonParameters);
            }

            var groupWithMultipleItems =
                Subject.Cast<object>().GroupBy(o => o).FirstOrDefault(g => Enumerable.Count<object>(g) > 1);
            if (groupWithMultipleItems != null)
            {
                Execute.Fail("Expected only unique items{2}, but item {1} was found multiple times.",
                    null, groupWithMultipleItems.Key, reason, reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Asserts that the collection does not contain any <c>null</c> items.
        /// </summary>
        public AndConstraint<TAssertions> NotContainNulls()
        {
            return NotContainNulls(String.Empty);
        }

        /// <summary>
        ///   Asserts that the collection does not contain any <c>null</c> items.
        /// </summary>
        public AndConstraint<TAssertions> NotContainNulls(string reason, params object[] reasonParameters)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collection to not contain nulls{2}, but found {1}.", "", Subject, reason,
                    reasonParameters);
            }

            var values = Subject.Cast<object>().ToArray();
            for (int index = 0; index < values.Length; index++)
            {
                if (ReferenceEquals(values[index], null))
                {
                    Execute.Fail("Expected no <null> in collection{2}, but found one at index {1}.", null, index, reason,
                        reasonParameters);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Expects the current collection to contain all the same elements in the same order as the collection identified by 
        ///   <param name = "expected" />. Elements are compared using their <see cref = "object.Equals(object)" />.
        /// </summary>
        public AndConstraint<TAssertions> Equal(IEnumerable expected)
        {
            return Equal(expected, String.Empty);
        }

        /// <summary>
        ///   Expects the current collection to contain all the same elements in the same order as the collection identified by 
        ///   <param name = "expected" />. Elements are compared using their <see cref = "object.Equals(object)" />.
        /// </summary>
        public AndConstraint<TAssertions> Equal(params object[] elements)
        {
            return Equal(elements, String.Empty);
        }

        /// <summary>
        ///   Expects the current collection to contain all the same elements in the same order as the collection identified by 
        ///   <param name = "expected" />. Elements are compared using their <see cref = "object.Equals(object)" />.
        /// </summary>
        public AndConstraint<TAssertions> Equal(IEnumerable expected, string reason, params object[] reasonParameters)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collections to be equal{2}, but found {1}.", expected, Subject, reason,
                    reasonParameters);
            }

            if (expected == null)
            {
                throw new ArgumentNullException("expected", "Cannot compare collection with <null>.");
            }

            var expectedItems = expected.Cast<object>().ToArray();
            var actualItems = Subject.Cast<object>().ToArray();

            for (int index = 0; index < expectedItems.Length; index++)
            {
                Execute.Verification
                    .ForCondition((index < actualItems.Length) && actualItems[index].Equals(expectedItems[index]))
                    .BecauseOf(reason, reasonParameters)
                    .FailWith(
                        "Expected " + Verification.SubjectNameOr("collection") +
                            " to be equal to {1}{0}, but {2} differs at index " + index + ".", expected, Subject);
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Expects the current collection not to contain all the same elements in the same order as the collection identified by 
        ///   <param name = "expected" />. Elements are compared using their <see cref = "object.Equals(object)" />.
        /// </summary>
        public AndConstraint<TAssertions> NotEqual(IEnumerable expected)
        {
            return NotEqual(expected, String.Empty);
        }

        /// <summary>
        ///   Expects the current collection not to contain all the same elements in the same order as the collection identified by 
        ///   <param name = "expected" />. Elements are compared using their <see cref = "object.Equals(object)" />.
        /// </summary>
        public AndConstraint<TAssertions> NotEqual(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collections not to be equal{2}, but found {1}.", expected, Subject, reason,
                    reasonParameters);
            }

            if (expected == null)
            {
                throw new ArgumentNullException("expected", "Cannot compare collection with <null>.");
            }

            var actualitems = Subject.Cast<object>().ToList();

            if (actualitems.SequenceEqual(expected.Cast<object>()))
            {
                Execute.Fail("Did not expect collections {0} and {1} to be equal{2}.", expected, Subject, reason,
                    reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Expects the current collection to contain all elements of the collection identified by <param name = "expected" />,
        ///   regardless of the order. Elements are compared using their <see cref = "object.Equals(object)" />.
        /// </summary>
        public AndConstraint<TAssertions> BeEquivalentTo(IEnumerable expected)
        {
            return BeEquivalentTo(expected, String.Empty);
        }

        /// <summary>
        ///   Expects the current collection to contain all elements of the collection identified by <param name = "expected" />,
        ///   regardless of the order. Elements are compared using their <see cref = "object.Equals(object)" />.
        /// </summary>
        public AndConstraint<TAssertions> BeEquivalentTo(params object[] elements)
        {
            return BeEquivalentTo(elements, String.Empty);
        }

        /// <summary>
        ///   Expects the current collection to contain all elements of the collection identified by <param name = "expected" />,
        ///   regardless of the order. Elements are compared using their <see cref = "object.Equals(object)" />.
        /// </summary>
        public AndConstraint<TAssertions> BeEquivalentTo(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify equivalence against a <null> collection.");
            }

            Execute.Verification
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonParameters)
                .FailWith("Expected collection to be equivalent to {1}{0}, but found {2}.", expected, Subject);

            var expectedItems = expected.Cast<object>().ToList();
            var actualItems = Subject.Cast<object>().ToList();

            Execute.Verification
                .ForCondition(AreEquivalent(expectedItems, actualItems))
                .BecauseOf(reason, reasonParameters)
                .FailWith("Expected collection {1} to be equivalent to {2}{0}.", actualItems, expectedItems);

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Expects the current collection not to contain all elements of the collection identified by <param name = "expected" />,
        ///   regardless of the order. Elements are compared using their <see cref = "object.Equals(object)" />.
        /// </summary>
        public AndConstraint<TAssertions> NotBeEquivalentTo(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify inequivalence against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collections not to be equivalent{2}, but found {1}.", expected, Subject, reason,
                    reasonParameters);
            }

            if (AreEquivalent(expected.Cast<object>(), Subject.Cast<object>()))
            {
                Execute.Fail("Expected collection {1} not be equivalent with collection {0}.", expected, Subject, reason,
                    reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Expects the current collection not to contain all elements of the collection identified by <param name = "expected" />,
        ///   regardless of the order. Elements are compared using their <see cref = "object.Equals(object)" />.
        /// </summary>
        public AndConstraint<TAssertions> NotBeEquivalentTo(IEnumerable expected)
        {
            return NotBeEquivalentTo(expected, String.Empty);
        }

        /// <summary>
        ///   Asserts that the current collection only contains items that are assignable to the type <typeparamref name = "T" />.
        /// </summary>
        public AndConstraint<TAssertions> ContainItemsAssignableTo<T>(string reason,
            params object[] reasonParameters)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collection to contain element assignable to type {0}{2}, but found {1}.",
                    typeof (T), Subject, reason, reasonParameters);
            }

            int index = 0;
            foreach (var item in Subject)
            {
                if (!typeof (T).IsAssignableFrom(item.GetType()))
                {
                    Execute.Fail(
                        "Expected only items of type {0} in collection{2}, but item " + item + " at index " + index +
                            " is of type {1}.",
                        typeof (T), item.GetType(), reason, reasonParameters);
                }

                ++index;
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        private static bool AreEquivalent(IEnumerable<object> expectedItems, IEnumerable<object> actualItems)
        {
            expectedItems = expectedItems.Distinct();
            actualItems = actualItems.Distinct();

            int expectedCount = expectedItems.Count();

            return (expectedItems.Intersect(actualItems).Count() == expectedCount) &&
                (expectedCount == actualItems.Count());
        }

        /// <summary>
        ///   Expects the current collection to contain the specified elements in any order. Elements are compared
        ///   using their <see cref = "object.Equals(object)" /> implementation.
        /// </summary>
        public AndConstraint<TAssertions> Contain(IEnumerable expected)
        {
            return Contain(expected, String.Empty);
        }

        /// <summary>
        ///   Expects the current collection to contain the specified elements in any order. Elements are compared
        ///   using their <see cref = "object.Equals(object)" /> implementation.
        /// </summary>
        public AndConstraint<TAssertions> Contain(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Connect verify containment against a <null> collection");
            }

            IEnumerable<object> expectedObjects = expected.Cast<object>();
            if (!expectedObjects.Any())
            {
                throw new ArgumentException("Connect verify containment against an empty collection");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collection to contain {0}, but found {1}.", expected, Subject, reason,
                    reasonParameters);
            }

            if (expected is string)
            {
                if (!Subject.Cast<object>().Contains(expected))
                {
                    Execute.Fail("Expected collection {1} to contain {0}{2}.", expected, Subject, reason,
                        reasonParameters);
                }
            }
            else
            {
                var missingItems = expectedObjects.Except(Subject.Cast<object>());
                if (missingItems.Any())
                {
                    if (expectedObjects.Count() > 1)
                    {
                        Execute.Verification
                            .BecauseOf(reason, reasonParameters)
                            .FailWith("Expected collection {1} to contain {2}{0}, but could not find {3}.", Subject,
                                expected, missingItems);
                    }
                    else
                    {
                        Execute.Verification
                            .BecauseOf(reason, reasonParameters)
                            .FailWith("Expected collection {1} to contain {2}{0}.", Subject,
                                expected.Cast<object>().First());
                    }
                }
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Expects the current collection to contain the specified elements in the exact same order. Elements are compared
        ///   using their <see cref = "object.Equals(object)" /> implementation.
        /// </summary>
        public AndConstraint<TAssertions> ContainInOrder(IEnumerable expected)
        {
            return ContainInOrder(expected, String.Empty);
        }

        /// <summary>
        ///   Expects the current collection to contain the specified elements in the exact same order. Elements are compared
        ///   using their <see cref = "object.Equals(object)" /> implementation.
        /// </summary>
        public AndConstraint<TAssertions> ContainInOrder(IEnumerable expected, string reason,
            params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify ordered containment against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collection to contain {0} in order, but found {1}.", expected, Subject, reason,
                    reasonParameters);
            }

            var expectedItems = expected.Cast<object>().ToList();
            var actualItems = Subject.Cast<object>();
            var missingItems = expectedItems.Except(actualItems);
            if (missingItems.Any())
            {
                Execute.Fail(
                    "Expected items {0} in ordered collection {1}{2}, but " + Formatter.ToString(missingItems).Escape() +
                        " did not appear.",
                    expected, Subject, reason, reasonParameters);
            }

            var actualMatchingItems = RemoveItemsThatWereNotExpected(actualItems, expectedItems);

            if (!expectedItems.SequenceEqual(actualMatchingItems))
            {
                Execute.Fail("Expected items {0} in ordered collection {1}{2}, but the order did not match.",
                    expected, Subject, reason, reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        private static IEnumerable<object> RemoveItemsThatWereNotExpected(IEnumerable<object> actualItems,
            IEnumerable<object> expectedItems)
        {
            return actualItems.Where(item => expectedItems.Any(expected => expected.Equals(item))).ToArray();
        }

        /// <summary>
        ///   Asserts that the collection is a subset of the <paramref name = "otherCollection" />.
        /// </summary>
        public AndConstraint<TAssertions> BeSubsetOf(IEnumerable otherCollection)
        {
            return BeSubsetOf(otherCollection, String.Empty);
        }

        /// <summary>
        ///   Asserts that the collection is a subset of the <paramref name = "otherCollection" />.
        /// </summary>
        /// <param name = "reason">        
        ///   A formatted phrase as is supported by <see cref = "string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "reasonArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "reason" />.
        /// </param>
        public AndConstraint<TAssertions> BeSubsetOf(IEnumerable otherCollection, string reason,
            params object[] reasonArgs)
        {
            if (otherCollection == null)
            {
                throw new NullReferenceException("Cannot verify a subset against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collection to be a subset of {0}{2}, but found {1}.", otherCollection, Subject,
                    reason, reasonArgs);
            }

            var expectedItems = otherCollection.Cast<object>();
            var actualItems = Subject.Cast<object>();

            var excessItems = actualItems.Except(expectedItems);

            if (excessItems.Any())
            {
                Execute.Fail(
                    "Expected collection to be a subset of {0}{2}, but items {1} are not part of the superset.",
                    otherCollection, excessItems, reason, reasonArgs);
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Asserts that the collection is not a subset of the <paramref name = "otherCollection" />.
        /// </summary>
        public AndConstraint<TAssertions> NotBeSubsetOf(IEnumerable otherCollection)
        {
            return NotBeSubsetOf(otherCollection, String.Empty);
        }

        /// <summary>
        ///   Asserts that the collection is not a subset of the <paramref name = "otherCollection" />.
        /// </summary>
        /// <param name = "reason">
        ///   A formatted phrase as is supported by <see cref = "string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "reasonArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "reason" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeSubsetOf(IEnumerable otherCollection, string reason,
            params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Cannot assert a {1} collection against a subset.", Subject);

            var expectedItems = otherCollection.Cast<object>();
            var actualItems = Subject.Cast<object>();

            if (actualItems.Intersect(expectedItems).Count() == actualItems.Count())
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Did not expect collection {1} to be a subset of {2}{0}.", actualItems, expectedItems);
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Assert that the current collection has the same number of elements as <paramref name = "otherCollection" />.
        /// </summary>
        public AndConstraint<TAssertions> HaveSameCount(IEnumerable otherCollection)
        {
            return HaveSameCount(otherCollection, "");
        }

        /// <summary>
        ///   Assert that the current collection has the same number of elements as <paramref name = "otherCollection" />.
        /// </summary>
        public AndConstraint<TAssertions> HaveSameCount(IEnumerable otherCollection, string reason,
            params object[] reasonParameters)
        {
            if (otherCollection == null)
            {
                throw new NullReferenceException("Cannot verify count against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collection to have the same count as {0}{2}, but found {1}.", otherCollection,
                    Subject, reason, reasonParameters);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            int actualCount = enumerable.Count();
            int expectedCount = otherCollection.Cast<object>().Count();

            Execute.Verify(() => actualCount == expectedCount,
                "Expected collection to have {0} item(s){2}, but found {1}.",
                expectedCount, actualCount, reason, reasonParameters);

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Asserts that the current collection has not been initialized yet with an actual collection.
        /// </summary>
        public AndConstraint<TAssertions> BeNull()
        {
            return BeNull("");
        }

        /// <summary>
        ///   Asserts that the current collection has not been initialized yet with an actual collection.
        /// </summary>
        public AndConstraint<TAssertions> BeNull(string reason, params object[] reasonParameters)
        {
            if (!ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collection to be <null>{2}, but found {1}.", null, Subject, reason,
                    reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Asserts that the current collection has been initialized with an actual collection.
        /// </summary>
        public AndConstraint<TAssertions> NotBeNull()
        {
            return NotBeNull("");
        }

        /// <summary>
        ///   Asserts that the current collection has been initialized with an actual collection.
        /// </summary>
        public AndConstraint<TAssertions> NotBeNull(string reason, params object[] reasonParameters)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collection not to be <null>{2}.", null, Subject, reason, reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Asserts that the current collection has the supplied <paramref name = "element" /> at the supplied <paramref
        ///    name = "index" />.
        /// </summary>
        public AndConstraint<TAssertions> HaveElementAt(int index, object element)
        {
            return HaveElementAt(index, element, String.Empty);
        }

        /// <summary>
        ///   Asserts that the current collection has the supplied <paramref name = "element" /> at the supplied <paramref
        ///    name = "index" />.
        /// </summary>
        public AndConstraint<TAssertions> HaveElementAt(int index, object element, string reason,
            params object[] reasonParameters)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collection to have element at index {0}{2}, but found {1}.", index, Subject,
                    reason, reasonParameters);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            if (index < enumerable.Count())
            {
                var actual = Subject.Cast<object>().ElementAt(index);

                Execute.Verify(actual.Equals(element),
                    "Expected {0} at index " + index + "{2}, but found {1}.",
                    element, actual, reason, reasonParameters);
            }
            else
            {
                Execute.Fail("Expected {0} at index " + index + "{2}, but found no element.",
                    element, null, reason, reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Asserts that the current collection only contains items that are assignable to the type <typeparamref name = "T" />.
        /// </summary>
        public AndConstraint<TAssertions> ContainItemsAssignableTo<T>()
        {
            return ContainItemsAssignableTo<T>(String.Empty);
        }

        /// <summary>
        ///   Asserts that the current collection does not contain the supplied <paramref name = "unexpected" /> item.
        /// </summary>
        public AndConstraint<TAssertions> NotContain(object unexpected)
        {
            return NotContain(unexpected, String.Empty);
        }

        /// <summary>
        ///   Asserts that the current collection does not contain the supplied <paramref name = "unexpected" /> item.
        /// </summary>
        public AndConstraint<TAssertions> NotContain(object unexpected, string reason,
            params object[] reasonParameters)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Fail("Expected collection not to contain element {0}{2}, but found {1}.", unexpected, Subject,
                    reason, reasonParameters);
            }

            if (Subject.Cast<object>().Contains(unexpected))
            {
                Execute.Fail("Collection {1} should not contain {0}{2}, but found it anyhow.",
                    unexpected, Subject, reason, reasonParameters);
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }
    }
}