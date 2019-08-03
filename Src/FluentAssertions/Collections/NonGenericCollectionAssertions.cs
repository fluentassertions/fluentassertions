using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;

namespace FluentAssertions.Collections
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="IEnumerable"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NonGenericCollectionAssertions : CollectionAssertions<IEnumerable, NonGenericCollectionAssertions>
    {
        public NonGenericCollectionAssertions(IEnumerable collection) : base(collection)
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
        public AndConstraint<NonGenericCollectionAssertions> HaveCount(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain {0} item(s){reason}, but found <null>.", expected);
            }

            int actualCount = GetMostLocalCount();

            Execute.Assertion
                .ForCondition(actualCount == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to contain {0} item(s){reason}, but found {1}.", expected, actualCount);

            return new AndConstraint<NonGenericCollectionAssertions>(this);
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
        public AndConstraint<NonGenericCollectionAssertions> NotHaveCount(int unexpected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to not contain {0} item(s){reason}, but found <null>.", unexpected);
            }

            int actualCount = GetMostLocalCount();

            Execute.Assertion
                .ForCondition(actualCount != unexpected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to not contain {0} item(s){reason}, but found {1}.", unexpected, actualCount);

            return new AndConstraint<NonGenericCollectionAssertions>(this);
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
        public AndConstraint<NonGenericCollectionAssertions> HaveCountGreaterThan(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain more than {0} item(s){reason}, but found <null>.", expected);
            }

            int actualCount = GetMostLocalCount();

            Execute.Assertion
                .ForCondition(actualCount > expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to contain more than {0} item(s){reason}, but found {1}.", expected, actualCount);

            return new AndConstraint<NonGenericCollectionAssertions>(this);
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
        public AndConstraint<NonGenericCollectionAssertions> HaveCountGreaterOrEqualTo(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain at least {0} item(s){reason}, but found <null>.", expected);
            }

            int actualCount = GetMostLocalCount();

            Execute.Assertion
                .ForCondition(actualCount >= expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to contain at least {0} item(s){reason}, but found {1}.", expected, actualCount);

            return new AndConstraint<NonGenericCollectionAssertions>(this);
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
        public AndConstraint<NonGenericCollectionAssertions> HaveCountLessThan(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain fewer than {0} item(s){reason}, but found <null>.", expected);
            }

            int actualCount = GetMostLocalCount();

            Execute.Assertion
                .ForCondition(actualCount < expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to contain fewer than {0} item(s){reason}, but found {1}.", expected, actualCount);

            return new AndConstraint<NonGenericCollectionAssertions>(this);
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
        public AndConstraint<NonGenericCollectionAssertions> HaveCountLessOrEqualTo(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain at most {0} item(s){reason}, but found <null>.", expected);
            }

            int actualCount = GetMostLocalCount();

            Execute.Assertion
                .ForCondition(actualCount <= expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to contain at most {0} item(s){reason}, but found {1}.", expected, actualCount);

            return new AndConstraint<NonGenericCollectionAssertions>(this);
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
        public AndConstraint<NonGenericCollectionAssertions> HaveCount(Expression<Func<int, bool>> countPredicate, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(countPredicate, nameof(countPredicate), "Cannot compare collection count against a <null> predicate.");

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain {0} items{reason}, but found {1}.", countPredicate.Body, Subject);
            }

            Func<int, bool> compiledPredicate = countPredicate.Compile();

            int actualCount = GetMostLocalCount();

            if (!compiledPredicate(actualCount))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} {0} to have a count {1}{reason}, but count is {2}.",
                        Subject, countPredicate.Body, actualCount);
            }

            return new AndConstraint<NonGenericCollectionAssertions>(this);
        }

        private int GetMostLocalCount()
        {
            if (Subject is ICollection castSubject)
            {
                return castSubject.Count;
            }
            else
            {
                return Subject.Cast<object>().Count();
            }
        }

        /// <summary>
        /// Asserts that the current collection contains the specified <paramref name="expected"/> object. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An object, or <see cref="IEnumerable"/> of objects that are expected to be in the collection.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NonGenericCollectionAssertions> Contain(object expected, string because = "",
            params object[] becauseArgs)
        {
            if (expected is IEnumerable enumerable)
            {
                return base.Contain(enumerable, because, becauseArgs);
            }

            return base.Contain(new[] { expected }, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current collection does not contain the supplied <paramref name="unexpected" /> item.
        /// Elements are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The element that is not expected to be in the collection</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NonGenericCollectionAssertions> NotContain(object unexpected, string because = "",
            params object[] becauseArgs)
        {
            if (unexpected is IEnumerable enumerable)
            {
                return base.NotContain(enumerable, because, becauseArgs);
            }

            return base.NotContain(new[] { unexpected }, because, becauseArgs);
        }
    }
}
