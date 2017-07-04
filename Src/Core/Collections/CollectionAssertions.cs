using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Collections
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="IEnumerable"/> is in the expected state.
    /// </summary>
    public abstract class CollectionAssertions<TSubject, TAssertions> : ReferenceTypeAssertions<TSubject, TAssertions>
        where TAssertions : CollectionAssertions<TSubject, TAssertions>
        where TSubject : IEnumerable
    {
        /// <summary>
        /// Asserts that the collection does not contain any items.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeEmpty(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to be empty{reason}, ")
                .ForCondition(!ReferenceEquals(Subject, null))
                .FailWith("but found {0}.", Subject)
                .Then
                .Given(() => Subject.Cast<object>())
                .ForCondition(collection => !collection.Any())
                .FailWith("but found {0}.", collection => collection);

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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeEmpty(string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} not to be empty{reason}, but found {0}.", Subject);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            Execute.Assertion
                .ForCondition(enumerable.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} not to be empty{reason}.");

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection is null or does not contain any items.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeNullOrEmpty(string because = "", params object[] becauseArgs)
        {
            var nullOrEmpty = ReferenceEquals(Subject, null) || !Subject.Cast<object>().Any();

            Execute.Assertion.ForCondition(nullOrEmpty)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:collection} to be null or empty{reason}, but found {0}.",
                    Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection is not null and contains at least 1 item.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeNullOrEmpty(string because = "", params object[] becauseArgs)
        {
            return NotBeNull(because, becauseArgs)
                .And.NotBeEmpty(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the collection does not contain any duplicate items.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> OnlyHaveUniqueItems(string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to only have unique items{reason}, but found {0}.", Subject);
            }

            IGrouping<object, object> groupWithMultipleItems = Subject.Cast<object>()
                .GroupBy(o => o)
                .FirstOrDefault(g => g.Count() > 1);

            if (groupWithMultipleItems != null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to only have unique items{reason}, but item {0} is not unique.",
                        groupWithMultipleItems.Key);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection does not contain any <c>null</c> items.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotContainNulls(string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} not to contain nulls{reason}, but collection is <null>.");
            }

            object[] values = Subject.Cast<object>().ToArray();
            for (int index = 0; index < values.Length; index++)
            {
                if (ReferenceEquals(values[index], null))
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} not to contain nulls{reason}, but found one at index {0}.", index);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by
        /// <paramref name="elements" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="elements">A params array with the expected elements.</param>
        public AndConstraint<TAssertions> Equal(params object[] elements)
        {
            return Equal(elements, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by
        /// <paramref name="expected" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> Equal(IEnumerable expected, string because = "", params object[] becauseArgs)
        {
            AssertSubjectEquality<object, object>(expected, (s, e) => s.IsSameOrEqualTo(e), because, becauseArgs);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        protected void AssertSubjectEquality<TActual, TExpected>(IEnumerable expectation, Func<TActual, TExpected, bool> equalityComparison,
            string because = "", params object[] becauseArgs)
        {

            bool subjectIsNull = ReferenceEquals(Subject, null);
            bool expectationIsNull = ReferenceEquals(expectation, null);
            if (subjectIsNull && expectationIsNull)
            {
                return;
            }

            if (expectation == null)
            {
                throw new ArgumentNullException("expectation", "Cannot compare collection with <null>.");
            }

            TExpected[] expectedItems = expectation.Cast<TExpected>().ToArray();

            AssertionScope assertion = Execute.Assertion.BecauseOf(because, becauseArgs);
            if (subjectIsNull)
            {
                assertion.FailWith("Expected {context:collection} to be equal to {0}{reason}, but found <null>.", expectedItems);
            }

            assertion
                .WithExpectation("Expected {context:collection} to be equal to {0}{reason}, ", expectedItems)
                .Given(() => Subject.Cast<TActual>().ToList().AsEnumerable())
                .AssertCollectionsHaveSameCount(expectedItems.Length)
                .Then
                .AssertCollectionsHaveSameItems(expectedItems, (a, e) => a.IndexOfFirstDifferenceWith(e, equalityComparison));
        }

        /// <summary>
        /// Expects the current collection not to contain all the same elements in the same order as the collection identified by
        /// <paramref name="unexpected" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="unexpected">An <see cref="IEnumerable"/> with the elements that are not expected.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotEqual(IEnumerable unexpected, string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected collections not to be equal{reason}, but found <null>.");
            }

            if (unexpected == null)
            {
                throw new ArgumentNullException("unexpected", "Cannot compare collection with <null>.");
            }

            List<object> actualitems = Subject.Cast<object>().ToList();

            if (actualitems.SequenceEqual(unexpected.Cast<object>()))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect collections {0} and {1} to be equal{reason}.", unexpected, actualitems);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to contain all elements of the collection identified by <paramref name="elements" />,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="elements">A params array with the expected elements.</param>
        public AndConstraint<TAssertions> BeEquivalentTo(params object[] elements)
        {
            return BeEquivalentTo(elements, String.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain all elements of the collection identified by <paramref name="expected" />,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeEquivalentTo(IEnumerable expected, string because = "", params object[] becauseArgs)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify equivalence against a <null> collection.");
            }

            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to be equivalent to {0}{reason}, but found <null>.", expected);

            List<object> expectedItems = expected.Cast<object>().ToList();
            List<object> actualItems = Subject.Cast<object>().ToList();

            bool haveSameLength = Execute.Assertion
                .ForCondition(actualItems.Count <= expectedItems.Count)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} {0} to be equivalent to {1}{reason}, but it contains too many items.",
                    actualItems, expectedItems);

            if (haveSameLength)
            {
                object[] missingItems = GetMissingItems(expectedItems, actualItems);

                Execute.Assertion
                    .ForCondition(missingItems.Length == 0)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} {0} to be equivalent to {1}{reason}, but it misses {2}.",
                        actualItems, expectedItems, missingItems);
            }
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeEquivalentTo<T>(IEnumerable<T> expected, string because = "", params object[] becauseArgs)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify equivalence against a <null> collection.");
            }

            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to be equivalent to {0}{reason}, but found <null>.", expected);

            List<T> expectedItems = expected.ToList();
            List<T> actualItems = Subject.Cast<T>().ToList();

            bool haveSameLength = Execute.Assertion
                .ForCondition(actualItems.Count <= expectedItems.Count)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} {0} to be equivalent to {1}{reason}, but it contains too many items.",
                    actualItems, expectedItems);

            if (haveSameLength)
            {
                T[] missingItems = GetMissingItems(expectedItems, actualItems);

                Execute.Assertion
                    .ForCondition(missingItems.Length == 0)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} {0} to be equivalent to {1}{reason}, but it misses {2}.",
                        actualItems, expectedItems, missingItems);
            }
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection not to contain all elements of the collection identified by <paramref name="unexpected" />,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="unexpected">An <see cref="IEnumerable"/> with the unexpected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeEquivalentTo(IEnumerable unexpected, string because = "",
            params object[] becauseArgs)
        {
            if (unexpected == null)
            {
                throw new NullReferenceException("Cannot verify inequivalence against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} not to be equivalent{reason}, but found <null>.");
            }

            IEnumerable<object> actualItems = Subject.Cast<object>();
            IEnumerable<object> unexpectedItems = unexpected.Cast<object>();

            if (actualItems.Count() == unexpectedItems.Count())
            {
                object[] missingItems = GetMissingItems(unexpectedItems, actualItems);

                Execute.Assertion
                    .ForCondition(missingItems.Length > 0)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} {0} not be equivalent with collection {1}{reason}.", Subject,
                        unexpected);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current collection only contains items that are assignable to the type <typeparamref name="T" />.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> ContainItemsAssignableTo<T>(string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain element assignable to type {0}{reason}, but found <null>.",
                        typeof(T));
            }

            int index = 0;
            foreach (object item in Subject)
            {
                if (!(item is T))
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith(
                            "Expected {context:collection} to contain only items of type {0}{reason}, but item {1} at index {2} is of type {3}.",
                            typeof(T), item, index, item.GetType());
                }

                ++index;
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private static T[] GetMissingItems<T>(IEnumerable<T> expectedItems, IEnumerable<T> actualItems)
        {
            List<T> missingItems = new List<T>();
            List<T> subject = actualItems.ToList();

            while (expectedItems.Any())
            {
                T expectation = expectedItems.First();
                if (subject.Contains(expectation))
                {
                    subject.Remove(expectation);
                }
                else
                {
                    missingItems.Add(expectation);
                }

                expectedItems = expectedItems.Skip(1).ToArray();
            }

            return missingItems.ToArray();
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in any order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> Contain(IEnumerable expected, string because = "", params object[] becauseArgs)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify containment against a <null> collection");
            }

            IEnumerable<object> expectedObjects = expected.Cast<object>().ToArray();
            if (!expectedObjects.Any())
            {
                throw new ArgumentException("Cannot verify containment against an empty collection");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain {0}{reason}, but found <null>.", expected);
            }

            if (expected is string)
            {
                if (!Subject.Cast<object>().Contains(expected))
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} {0} to contain {1}{reason}.", Subject, expected);
                }
            }
            else
            {
                IEnumerable<object> missingItems = expectedObjects.Except(Subject.Cast<object>());
                if (missingItems.Any())
                {
                    if (expectedObjects.Count() > 1)
                    {
                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected {context:collection} {0} to contain {1}{reason}, but could not find {2}.", Subject,
                                expected, missingItems);
                    }
                    else
                    {
                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected {context:collection} {0} to contain {1}{reason}.", Subject,
                                expected.Cast<object>().First());
                    }
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order, not necessarily consecutive.
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        public AndConstraint<TAssertions> ContainInOrder(params object[] expected)
        {
            return ContainInOrder(expected, "");
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order, not necessarily consecutive.
        /// </summary>
        /// <remarks>
        /// Elements are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </remarks>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> ContainInOrder(IEnumerable expected, string because = "",
            params object[] becauseArgs)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot verify ordered containment against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain {0} in order{reason}, but found <null>.", expected);
            }

            object[] expectedItems = expected.Cast<object>().ToArray();
            object[] actualItems = Subject.Cast<object>().ToArray();

            for (int index = 0; index < expectedItems.Length; index++)
            {
                object expectedItem = expectedItems[index];
                actualItems = actualItems.SkipWhile(actualItem => !actualItem.IsSameOrEqualTo(expectedItem)).ToArray();
                if (actualItems.Any())
                {
                    actualItems = actualItems.Skip(1).ToArray();
                }
                else
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith(
                            "Expected {context:collection} {0} to contain items {1} in order{reason}, but {2} (index {3}) did not appear (in the right order).",
                            Subject, expected, expectedItem, index);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeInAscendingOrder(string because = "", params object[] becauseArgs)
        {
            return BeInAscendingOrder(Comparer<object>.Default, because, becauseArgs);
        }

        /// <summary>
        /// Expects the current collection to have all elements in ascending order. Elements are compared
        /// using the given <see cref="IComparer{T}" /> implementation.
        /// </summary>
        /// <param name="comparer">
        /// The object that should be used to determine the expected ordering.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeInAscendingOrder(IComparer<object> comparer, string because = "", params object[] becauseArgs)
        {
            return BeInOrder(comparer, SortOrder.Ascending, because, becauseArgs);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeInDescendingOrder(string because = "", params object[] becauseArgs)
        {
            return BeInDescendingOrder(Comparer<object>.Default, because, becauseArgs);
        }

        /// <summary>
        /// Expects the current collection to have all elements in descending order. Elements are compared
        /// using the given <see cref="IComparer{T}" /> implementation.
        /// </summary>
        /// <param name="comparer">
        /// The object that should be used to determine the expected ordering.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeInDescendingOrder(IComparer<object> comparer, string because = "", params object[] becauseArgs)
        {
            return BeInOrder(comparer, SortOrder.Descending, because, becauseArgs);
        }

        /// <summary>
        /// Expects the current collection to have all elements in the specified <paramref name="expectedOrder"/>.
        /// Elements are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        private AndConstraint<TAssertions> BeInOrder(
            IComparer<object> comparer, SortOrder expectedOrder, string because = "", params object[] becauseArgs)
        {
            string sortOrder = (expectedOrder == SortOrder.Ascending) ? "ascending" : "descending";

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain items in " + sortOrder + " order{reason}, but found {1}.",
                        Subject);
            }

            object[] actualItems = Subject.Cast<object>().ToArray();

            object[] orderedItems = (expectedOrder == SortOrder.Ascending)
                ? actualItems.OrderBy(item => item, comparer).ToArray()
                : actualItems.OrderByDescending(item => item, comparer).ToArray();

            for (int index = 0; index < orderedItems.Length; index++)
            {
                Execute.Assertion
                    .ForCondition(actualItems[index].IsSameOrEqualTo(orderedItems[index]))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain items in " + sortOrder +
                              " order{reason}, but found {0} where item at index {1} is in wrong order.",
                        Subject, index);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeAscendingInOrder(string because = "", params object[] becauseArgs)
        {
            return NotBeAscendingInOrder(Comparer<object>.Default, because, becauseArgs);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in ascending order. Elements are compared
        /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
        /// </summary>
        /// <param name="comparer">
        /// The object that should be used to determine the expected ordering.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeAscendingInOrder(IComparer<object> comparer, string because = "", params object[] becauseArgs)
        {
            return NotBeInOrder(comparer, SortOrder.Ascending, because, becauseArgs);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeDescendingInOrder(string because = "", params object[] becauseArgs)
        {
            return NotBeDescendingInOrder(Comparer<object>.Default, because, becauseArgs);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in descending order. Elements are compared
        /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
        /// </summary>
        /// <param name="comparer">
        /// The object that should be used to determine the expected ordering.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeDescendingInOrder(IComparer<object> comparer, string because = "", params object[] becauseArgs)
        {
            return NotBeInOrder(comparer, SortOrder.Descending, because, becauseArgs);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in ascending order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        private AndConstraint<TAssertions> NotBeInOrder(IComparer<object> comparer, SortOrder order, string because = "", params object[] becauseArgs)
        {
            string sortOrder = (order == SortOrder.Ascending) ? "ascending" : "descending";

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Did not expect {context:collection} to contain items in " + sortOrder + " order{reason}, but found {1}.",
                        Subject);
            }

            object[] orderedItems = (order == SortOrder.Ascending)
                ? Subject.Cast<object>().OrderBy(item => item, comparer).ToArray()
                : Subject.Cast<object>().OrderByDescending(item => item, comparer).ToArray();

            object[] actualItems = Subject.Cast<object>().ToArray();

            bool itemsAreUnordered = actualItems
                .Where((actualItem, index) => !actualItem.IsSameOrEqualTo(orderedItems[index]))
                .Any();

            if (!itemsAreUnordered)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Did not expect {context:collection} to contain items in " + sortOrder + " order{reason}, but found {0}.",
                        Subject);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection is a subset of the <paramref name="expectedSuperset" />.
        /// </summary>
        /// <param name="expectedSuperset">An <see cref="IEnumerable"/> with the expected superset.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeSubsetOf(IEnumerable expectedSuperset, string because = "",
            params object[] becauseArgs)
        {
            if (expectedSuperset == null)
            {
                throw new NullReferenceException("Cannot verify a subset against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to be a subset of {0}{reason}, but found {1}.", expectedSuperset,
                        Subject);
            }

            IEnumerable<object> expectedItems = expectedSuperset.Cast<object>();
            IEnumerable<object> actualItems = Subject.Cast<object>();

            IEnumerable<object> excessItems = actualItems.Except(expectedItems);

            if (excessItems.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:collection} to be a subset of {0}{reason}, but items {1} are not part of the superset.",
                        expectedSuperset, excessItems);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection is not a subset of the <paramref name="unexpectedSuperset" />.
        /// </summary>
        /// <param name="unexpectedSuperset">An <see cref="IEnumerable"/> with the unexpected superset.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeSubsetOf(IEnumerable unexpectedSuperset, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Cannot assert a <null> collection against a subset.");

            IEnumerable<object> expectedItems = unexpectedSuperset.Cast<object>();
            object[] actualItems = Subject.Cast<object>().ToArray();

            if (actualItems.Intersect(expectedItems).Count() == actualItems.Count())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {context:collection} {0} to be a subset of {1}{reason}.", actualItems, expectedItems);
            }

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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveSameCount(IEnumerable otherCollection, string because = "",
            params object[] becauseArgs)
        {
            if (otherCollection == null)
            {
                throw new NullReferenceException("Cannot verify count against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to have the same count as {0}{reason}, but found {1}.",
                        otherCollection,
                        Subject);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            int actualCount = enumerable.Count();
            int expectedCount = otherCollection.Cast<object>().Count();

            Execute.Assertion
                .ForCondition(actualCount == expectedCount)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to have {0} item(s){reason}, but found {1}.", expectedCount, actualCount);

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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<TAssertions, object> HaveElementAt(int index, object element, string because = "",
            params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to have element at index {0}{reason}, but found {1}.", index, Subject);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            object actual = null;
            if (index < enumerable.Count())
            {
                actual = Subject.Cast<object>().ElementAt(index);

                Execute.Assertion
                    .ForCondition(actual.IsSameOrEqualTo(element))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {0} at index {1}{reason}, but found {2}.", element, index, actual);
            }
            else
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {0} at index {1}{reason}, but found no element.", element, index);
            }

            return new AndWhichConstraint<TAssertions, object>((TAssertions)this, actual);
        }

        /// <summary>
        /// Asserts that the current collection does not contain the supplied items. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">An <see cref="IEnumerable"/> with the unexpected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotContain(IEnumerable unexpected, string because = "", params object[] becauseArgs)
        {
            if (unexpected == null)
            {
                throw new NullReferenceException("Cannot verify non-containment against a <null> collection");
            }

            IEnumerable<object> unexpectedObjects = unexpected.Cast<object>().ToArray();
            if (!unexpectedObjects.Any())
            {
                throw new ArgumentException("Cannot verify non-containment against an empty collection");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to not contain {0}{reason}, but found <null>.", unexpected);
            }

            if (unexpected is string)
            {
                if (Subject.Cast<object>().Contains(unexpected))
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} {0} to not contain {1}{reason}.", Subject, unexpected);
                }
            }
            else
            {
                IEnumerable<object> foundItems = unexpectedObjects.Intersect(Subject.Cast<object>());
                if (foundItems.Any())
                {
                    if (unexpectedObjects.Count() > 1)
                    {
                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected {context:collection} {0} to not contain {1}{reason}, but found {2}.", Subject,
                                unexpected, foundItems);
                    }
                    else
                    {
                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected {context:collection} {0} to not contain element {1}{reason}.", Subject,
                                unexpectedObjects.First());
                    }
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection shares one or more items with the specified <paramref name="otherCollection"/>.
        /// </summary>
        /// <param name="otherCollection">The <see cref="IEnumerable"/> with the expected shared items.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> IntersectWith(IEnumerable otherCollection, string because = "",
            params object[] becauseArgs)
        {
            if (otherCollection == null)
            {
                throw new NullReferenceException("Cannot verify intersection against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to intersect with {0}{reason}, but found {1}.", otherCollection,
                        Subject);
            }

            IEnumerable<object> otherItems = otherCollection.Cast<object>();
            IEnumerable<object> sharedItems = Subject.Cast<object>().Intersect(otherItems);

            if (!sharedItems.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:collection} to intersect with {0}{reason}, but {1} does not contain any shared items.",
                        otherCollection, Subject);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection does not share any items with the specified <paramref name="otherCollection"/>.
        /// </summary>
        /// <param name="otherCollection">The <see cref="IEnumerable"/> to compare to.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotIntersectWith(IEnumerable otherCollection, string because = "",
            params object[] becauseArgs)
        {
            if (otherCollection == null)
            {
                throw new NullReferenceException("Cannot verify intersection against a <null> collection.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {context:collection} to intersect with {0}{reason}, but found {1}.", otherCollection,
                        Subject);
            }

            IEnumerable<object> otherItems = otherCollection.Cast<object>();
            IEnumerable<object> sharedItems = Subject.Cast<object>().Intersect(otherItems);

            if (sharedItems.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Did not expect {context:collection} to intersect with {0}{reason}, but found the following shared items {1}.",
                        otherCollection, sharedItems);
            }

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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> StartWith(object element, string because = "", params object[] becauseArgs)
        {
            AssertCollectionStartsWith(Subject?.Cast<object>(), new[] { element }, ObjectExtensions.IsSameOrEqualTo, because, becauseArgs);
            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        protected void AssertCollectionStartsWith<TActual, TExpected>(IEnumerable<TActual> actualItems, TExpected[] expected, Func<TActual, TExpected, bool> equalityComparison, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to start with {0}{reason}, ", expected)
                .Given(() => actualItems)
                .AssertCollectionIsNotNullOrEmpty(expected.Length)
                .Then
                .AssertCollectionHasEnoughItems(expected.Length)
                .Then
                .AssertCollectionsHaveSameItems(expected, (a, e) => a.Take(e.Length).IndexOfFirstDifferenceWith(e, equalityComparison));
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> EndWith(object element, string because = "", params object[] becauseArgs)
        {
            AssertCollectionEndsWith(Subject?.Cast<object>(), new[] { element }, ObjectExtensions.IsSameOrEqualTo, because, becauseArgs);
            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        protected void AssertCollectionEndsWith<TActual, TExpected>(IEnumerable<TActual> actual, TExpected[] expected, Func<TActual, TExpected, bool> equalityComparison, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to end with {0}{reason}, ", expected)
                .Given(() => actual)
                .AssertCollectionIsNotNullOrEmpty(expected.Length)
                .Then
                .AssertCollectionHasEnoughItems(expected.Length)
                .Then
                .AssertCollectionsHaveSameItems(expected, (a, e) =>
                {
                    int firstIndexToCompare = a.Length - e.Length;
                    int index = a.Skip(firstIndexToCompare).IndexOfFirstDifferenceWith(e, equalityComparison);
                    return index >= 0 ? index + firstIndexToCompare : index;
                });
        }

        /// <summary>
        /// Asserts that the <paramref name="expectation"/> element directly precedes the <paramref name="successor"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveElementPreceding(object successor, object expectation, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to have {0} precede {1}{reason}, ", expectation, successor)
                .Given(() => Subject.Cast<object>())
                .ForCondition(subject => subject.Any())
                .FailWith("but the collection is empty.")
                .Then
                .ForCondition(subject => HasPredecessor(successor, subject))
                .FailWith("but found nothing.")
                .Then
                .Given(subject => PredecessorOf(successor, subject))
                .ForCondition(predecessor => predecessor.IsSameOrEqualTo(expectation))
                .FailWith("but found {0}.", predecessor => predecessor);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private bool HasPredecessor(object successor, IEnumerable<object> subject)
        {
            return !ReferenceEquals(subject.First(), successor);
        }

        private object PredecessorOf(object succesor, IEnumerable<object> subject)
        {
            object[] collection = subject.ToArray();
            int index = Array.IndexOf(collection, succesor);
            return (index > 0) ? collection[index - 1] : null;
        }

        /// <summary>
        /// Asserts that the <paramref name="expectation"/> element directly succeeds the <paramref name="predecessor"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveElementSucceeding(object predecessor, object expectation, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to have {0} succeed {1}{reason}, ", expectation, predecessor)
                .Given(() => Subject.Cast<object>())
                .ForCondition(subject => subject.Any())
                .FailWith("but the collection is empty.")
                .Then
                .ForCondition(subject => HasSuccessor(predecessor, subject))
                .FailWith("but found nothing.")
                .Then
                .Given(subject => SuccessorOf(predecessor, subject))
                .ForCondition(successor => successor.IsSameOrEqualTo(expectation))
                .FailWith("but found {0}.", successor => successor);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private bool HasSuccessor(object predecessor, IEnumerable<object> subject)
        {
            return !ReferenceEquals(subject.Last(), predecessor);
        }

        private object SuccessorOf(object predecessor, IEnumerable<object> subject)
        {
            object[] collection = subject.ToArray();
            int index = Array.IndexOf(collection, predecessor);
            return (index < (collection.Length - 1)) ? collection[index + 1] : null;
        }

        /// <summary>
        /// Asserts that all items in the collection are of the specified type <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">The expected type of the objects</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> AllBeAssignableTo<T>(string because = "", params object[] becauseArgs)
        {
            return AllBeAssignableTo(typeof(T), because, becauseArgs);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> AllBeAssignableTo(Type expectedType, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected type to be {0}{reason}, ", expectedType.FullName)
                .Given(() => Subject.Cast<object>())
                .ForCondition(subject => subject.All(x => x != null))
                .FailWith("but found a null element.")
                .Then
                .ForCondition(subject => subject.All(x => expectedType.IsAssignableFrom(x.GetType())))
                .FailWith("but found {0}.", subject => $"[{string.Join(", ", subject.Select(x => x.GetType().FullName))}]");

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that all items in the collection are of the exact specified type <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">The expected type of the objects</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> AllBeOfType<T>(string because = "", params object[] becauseArgs)
        {
            return AllBeOfType(typeof(T), because, becauseArgs);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> AllBeOfType(Type expectedType, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected type to be {0}{reason}, ", expectedType.FullName)
                .Given(() => Subject.Cast<object>())
                .ForCondition(subject => subject.All(x => x != null))
                .FailWith("but found a null element.")
                .Then
                .ForCondition(subject => subject.All(x => expectedType == x.GetType()))
                .FailWith("but found {0}.", subject => $"[{string.Join(", ", subject.Select(x => x.GetType().FullName))}]");

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Context
        {
            get { return "collection"; }
        }
    }
}