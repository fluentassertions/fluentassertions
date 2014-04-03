using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Execution;

namespace FluentAssertions.Collections
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="IEnumerable"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NonGenericCollectionAssertions : CollectionAssertions<IEnumerable, NonGenericCollectionAssertions>
    {
        public NonGenericCollectionAssertions(IEnumerable collection)
        {
            if (collection != null)
            {
                Subject = collection;
            }
        }

        /// <summary>
        /// Asserts that the number of items in the collection matches the supplied <paramref name="expected" /> amount.
        /// </summary>
        /// <param name="expected">The expected number of items in the collection.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NonGenericCollectionAssertions> HaveCount(int expected, string because = "", params object[] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, reasonArgs)
                    .FailWith("Expected {context:collection} to contain {0} item(s){reason}, but found <null>.", expected);
            }

            int actualCount = GetMostLocalCount();

            Execute.Assertion
                .ForCondition(actualCount == expected)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected {context:collection} to contain {0} item(s){reason}, but found {1}.", expected, actualCount);

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
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NonGenericCollectionAssertions> HaveCount(Expression<Func<int, bool>> countPredicate, string because = "",
            params object[] reasonArgs)
        {
            if (countPredicate == null)
            {
                throw new NullReferenceException("Cannot compare collection count against a <null> predicate.");
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, reasonArgs)
                    .FailWith("Expected {context:collection} to contain {0} items{reason}, but found {1}.", countPredicate.Body, Subject);
            }

            Func<int, bool> compiledPredicate = countPredicate.Compile();

            int actualCount = GetMostLocalCount();

            if (!compiledPredicate(actualCount))
            {
                Execute.Assertion
                    .BecauseOf(because, reasonArgs)
                    .FailWith("Expected {context:collection} {0} to have a count {1}{reason}, but count is {2}.",
                        Subject, countPredicate.Body, actualCount);
            }

            return new AndConstraint<NonGenericCollectionAssertions>(this);
        }

        private int GetMostLocalCount()
        {
            ICollection castSubject = Subject as ICollection;
            if (castSubject != null)
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
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<NonGenericCollectionAssertions> Contain(object expected, string because = "",
            params object [] reasonArgs)
        {
            if (expected is IEnumerable)
            {
                return base.Contain((IEnumerable) expected, because, reasonArgs);
            }

            return base.Contain(new [] { expected }, because, reasonArgs);
        }
    }
}