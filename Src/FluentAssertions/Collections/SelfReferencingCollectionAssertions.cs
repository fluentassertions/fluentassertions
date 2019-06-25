using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Collections
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="IEnumerable{T}"/> is in the expectation state.
    /// </summary>
    public class SelfReferencingCollectionAssertions<T, TAssertions> : CollectionAssertions<IEnumerable<T>, TAssertions>
        where TAssertions : SelfReferencingCollectionAssertions<T, TAssertions>
    {
        public SelfReferencingCollectionAssertions(IEnumerable<T> actualValue) : base(actualValue)
        {
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveCount(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToContainXItemsFormat + Resources.Common_CommaButFoundNull, expected);
            }

            int actualCount = Subject.Count();

            Execute.Assertion
                .ForCondition(actualCount == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Collection_ExpectedCollectionToContainXItemsFormat + Resources.Common_CommaButFoundYFormat, expected, actualCount);

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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveCount(Expression<Func<int, bool>> countPredicate, string because = "",
            params object[] becauseArgs)
        {
            if (countPredicate is null)
            {
                throw new ArgumentNullException(nameof(countPredicate), Resources.Collection_CannotCompareCollectionCountAgainstNullPredicate);
            }

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToContainXItemsFormat + Resources.Common_CommaButFoundYFormat, countPredicate.Body, Subject);
            }

            Func<int, bool> compiledPredicate = countPredicate.Compile();

            int actualCount = Subject.Count();

            if (!compiledPredicate(actualCount))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionXToHaveCountYButCountIsZFormat,
                        Subject, countPredicate.Body, actualCount);
            }

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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotHaveCount(int unexpected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToNotContainXItemsFormat + Resources.Common_CommaButFoundNull, unexpected);
            }

            int actualCount = Subject.Count();

            Execute.Assertion
                .ForCondition(actualCount != unexpected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Collection_ExpectedCollectionToNotContainXItemsFormat + Resources.Common_CommaButFoundYFormat, unexpected, actualCount);

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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveCountGreaterThan(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToContainMoreThanXItemsFormat + Resources.Common_CommaButFoundNull, expected);
            }

            int actualCount = Subject.Count();

            Execute.Assertion
                .ForCondition(actualCount > expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Collection_ExpectedCollectionToContainMoreThanXItemsFormat + Resources.Common_CommaButFoundYFormat, expected, actualCount);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the number of items in the collection is greater or equal to the supplied <paramref name="expected" /> amount.
        /// </summary>
        /// <param name="expected">The number to which the actual number items in the collection will be compared.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveCountGreaterOrEqualTo(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToContainAtLeastXItemsFormat + Resources.Common_CommaButFoundNull, expected);
            }

            int actualCount = Subject.Count();

            Execute.Assertion
                .ForCondition(actualCount >= expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Collection_ExpectedCollectionToContainAtLeastXItemsFormat + Resources.Common_CommaButFoundYFormat, expected, actualCount);

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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveCountLessThan(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToContainFewerThanXItemsFormat + Resources.Common_CommaButFoundNull, expected);
            }

            int actualCount = Subject.Count();

            Execute.Assertion
                .ForCondition(actualCount < expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Collection_ExpectedCollectionToContainFewerThanXItemsFormat + Resources.Common_CommaButFoundYFormat, expected, actualCount);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the number of items in the collection is less or equal to the supplied <paramref name="expected" /> amount.
        /// </summary>
        /// <param name="expected">The number to which the actual number items in the collection will be compared.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveCountLessOrEqualTo(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToContainAtMostXItemsFormat + Resources.Common_CommaButFoundNull, expected);
            }

            int actualCount = Subject.Count();

            Execute.Assertion
                .ForCondition(actualCount <= expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Collection_ExpectedCollectionToContainAtMostXItemsFormat + Resources.Common_CommaButFoundYFormat, expected, actualCount);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by
        /// <paramref name="elements" />. Elements are compared using their <see cref="T.Equals(object)" /> method.
        /// </summary>
        /// <param name="elements">A params array with the expected elements.</param>
        public AndConstraint<TAssertions> Equal(params T[] elements)
        {
            Func<T, T, bool> comparer = GetComparer();

            AssertSubjectEquality(elements, comparer, string.Empty);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private static Func<T, T, bool> GetComparer()
        {
            if (typeof(T).GetTypeInfo().IsValueType)
            {
                return (T s, T e) => s.Equals(e);
            }

            return (T s, T e) => Equals(s, e);
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
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<TAssertions> Equal<TExpected>(
            IEnumerable<TExpected> expectation, Func<T, TExpected, bool> equalityComparison, string because = "", params object[] becauseArgs)
        {
            AssertSubjectEquality(expectation, equalityComparison, because, becauseArgs);

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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> StartWith(IEnumerable<T> expectation, string because = "", params object[] becauseArgs)
        {
            if (expectation is null)
            {
                return base.StartWith(null, because, becauseArgs);
            }

            AssertCollectionStartsWith(Subject, expectation.ConvertOrCastToCollection(), EqualityComparer<T>.Default.Equals, because, becauseArgs);
            return new AndConstraint<TAssertions>((TAssertions)this);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> StartWith<TExpected>(
            IEnumerable<TExpected> expectation, Func<T, TExpected, bool> equalityComparison, string because = "", params object[] becauseArgs)
        {
            if (expectation is null)
            {
                throw new ArgumentNullException(nameof(expectation), Resources.Collection_CannotCompareCollectionWithNull);
            }

            AssertCollectionStartsWith(Subject, expectation.ConvertOrCastToCollection(), equalityComparison, because, becauseArgs);
            return new AndConstraint<TAssertions>((TAssertions)this);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> EndWith(IEnumerable<T> expectation, string because = "", params object[] becauseArgs)
        {
            if (expectation is null)
            {
                return base.EndWith(null, because, becauseArgs);
            }

            AssertCollectionEndsWith(Subject, expectation.ConvertOrCastToCollection(), EqualityComparer<T>.Default.Equals, because, becauseArgs);
            return new AndConstraint<TAssertions>((TAssertions)this);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> EndWith<TExpected>(
            IEnumerable<TExpected> expectation, Func<T, TExpected, bool> equalityComparison, string because = "", params object[] becauseArgs)
        {
            if (expectation is null)
            {
                throw new ArgumentNullException(nameof(expectation), Resources.Collection_CannotCompareCollectionWithNull);
            }

            AssertCollectionEndsWith(Subject, expectation.ConvertOrCastToCollection(), equalityComparison, because, becauseArgs);
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
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndWhichConstraint<TAssertions, T> Contain(T expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToContainXFormat + Resources.Common_CommaButFoundYFormat, expected, Subject);
            }

            if (!Subject.Contains(expected))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionXToContainYFormat, Subject, expected);
            }

            return new AndWhichConstraint<TAssertions, T>((TAssertions)this,
                Subject.Where(
                    item => EqualityComparer<T>.Default.Equals(item, expected)));
        }

        /// <summary>
        /// Asserts that the collection contains some extra items in addition to the original items.
        /// </summary>
        /// <param name="expectedItemsList">An <see cref="IEnumerable{T}"/> of expectation items.</param>
        /// <param name="additionalExpectedItems">Additional items that are expectation to be contained by the collection.</param>
        public AndConstraint<TAssertions> Contain(IEnumerable<T> expectedItemsList,
            params T[] additionalExpectedItems)
        {
            var list = new List<T>(expectedItemsList);
            list.AddRange(additionalExpectedItems);

            return Contain((IEnumerable)list);
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
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndWhichConstraint<TAssertions, T> Contain(Expression<Func<T, bool>> predicate, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToContainXFormat + Resources.Common_CommaButFoundYFormat, predicate.Body, Subject);
            }

            Func<T, bool> func = predicate.Compile();

            Execute.Assertion
                .ForCondition(Subject.Any(func))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Collection_ExpectedCollectionXToHaveItemMatchingYFormat, Subject, predicate.Body);

            return new AndWhichConstraint<TAssertions, T>((TAssertions)this, Subject.Where(func));
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
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<TAssertions> OnlyContain(
            Expression<Func<T, bool>> predicate, string because = "", params object[] becauseArgs)
        {
            Func<T, bool> compiledPredicate = predicate.Compile();

            Execute.Assertion
                .ForCondition(Subject.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Collection_ExpectedCollectionToContainOnlyItemsMatchingXFormat + Resources.Collection_CommaButCollectionIsEmpty,
                    predicate.Body);

            IEnumerable<T> mismatchingItems = Subject.Where(item => !compiledPredicate(item));
            if (mismatchingItems.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToContainOnlyItemsMatchingXButYDoesNotMatchFormat,
                        predicate.Body, mismatchingItems);
            }

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
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndWhichConstraint<TAssertions, T> NotContain(T unexpected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToNotXContainFormat + Resources.Common_CommaButFoundNull, unexpected);
            }

            if (Subject.Contains(unexpected))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionXToNotContainYDotFormat, Subject, unexpected);
            }

            return new AndWhichConstraint<TAssertions, T>((TAssertions)this,
                Subject.Where(
                    item => !EqualityComparer<T>.Default.Equals(item, unexpected)));
        }

        /// <summary>
        /// Asserts that the collection does not contain some extra items in addition to the original items.
        /// </summary>
        /// <param name="unexpectedItemsList">An <see cref="IEnumerable{T}"/> of unexpected items.</param>
        /// <param name="additionalUnexpectedItems">Additional items that are not expected to be contained by the collection.</param>
        public AndConstraint<TAssertions> NotContain(IEnumerable<T> unexpectedItemsList, params T[] additionalUnexpectedItems)
        {
            var list = new List<T>(unexpectedItemsList);
            list.AddRange(additionalUnexpectedItems);
            return NotContain((IEnumerable)list);
        }

        /// <summary>
        /// Asserts that the collection does not contain any items that match the predicate.
        /// </summary>
        /// <param name="predicate">A predicate to match the items in the collection against.</param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<TAssertions> NotContain(Expression<Func<T, bool>> predicate, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionNotToContainXFormat + Resources.Common_CommaButFoundYFormat, predicate.Body, Subject);
            }

            Func<T, bool> compiledPredicate = predicate.Compile();
            IEnumerable<T> unexpectedItems = Subject.Where(item => compiledPredicate(item));

            if (unexpectedItems.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionXToNotHaveItemsMatchingYFormat + Resources.Common_CommaButFoundZFormat,
                        Subject, predicate.Body, unexpectedItems);
            }

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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<TAssertions, T> ContainSingle(string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToContainSingleItem + Resources.Common_CommaButFoundNull);
            }

            switch (Subject.Count())
            {
                case 0: // Fail, Collection is empty
                    Execute.Assertion.BecauseOf(because, becauseArgs).FailWith(Resources.Collection_ExpectedCollectionToContainSingleItem + Resources.Collection_CommaButCollectionIsEmpty);
                    break;
                case 1: // Success Condition
                    break;
                default: // Fail, Collection contains more than a single item
                    Execute.Assertion.BecauseOf(because, becauseArgs).FailWith(Resources.Collection_ExpectedCollectionToContainSingleItem + Resources.Common_CommaButFoundXFormat, Subject);
                    break;
            }

            return new AndWhichConstraint<TAssertions, T>((TAssertions)this, Subject.SingleOrDefault());
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<TAssertions, T> ContainSingle(Expression<Func<T, bool>> predicate,
            string because = "", params object[] becauseArgs)
        {
            string expectationPrefix =
                string.Format(Resources.Collection_ExpectedCollectionToContainSingleItemMatchingXFormat, predicate.Body);

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(expectationPrefix + Resources.Common_ButFoundXFormat, Subject);
            }

            ICollection<T> actualItems = Subject.ConvertOrCastToCollection();
            Execute.Assertion
                .ForCondition(actualItems.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(expectationPrefix + Resources.Collection_ButTheCollectionIsEmpty);

            T[] matchingElements = actualItems.Where(predicate.Compile()).ToArray();
            int count = matchingElements.Length;
            if (count == 0)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(expectationPrefix + Resources.Common_ButNoSuchItemWasFound);
            }
            else if (count > 1)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(expectationPrefix + Resources.Collection_ButXSuchItemsWereFoundFormat, count);
            }
            else
            {
                // Exactly 1 item was expected
            }

            return new AndWhichConstraint<TAssertions, T>((TAssertions)this, matchingElements);
        }
    }
}
