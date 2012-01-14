using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="IEnumerable{T}"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class GenericCollectionAssertions<T> : CollectionAssertions<IEnumerable<T>, GenericCollectionAssertions<T>>
    {
        protected internal GenericCollectionAssertions(IEnumerable<T> actualValue)
        {
            if (actualValue != null)
            {
                Subject = actualValue;
            }
        }

        /// <summary>
        /// Asserts that the collection contains the specified item.
        /// </summary>
        public AndConstraint<GenericCollectionAssertions<T>> Contain(T expected)
        {
            return Contain(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the collection contains the specified item.
        /// </summary>
        /// <param name="expected">The expected item.</param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<GenericCollectionAssertions<T>> Contain(T expected, string reason, params object[] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected collection to contain {0}{reason}, but found {1}.", expected, Subject);
            }

            if (!Subject.Contains(expected))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected collection {0} to contain {1}{reason}.", Subject, expected);
            }

            return new AndConstraint<GenericCollectionAssertions<T>>(this);
        }

        
        /// <summary>
        /// Asserts that the collection contains some extra items in addition to the original items.
        /// </summary>
        /// <param name="expectedItemsList">An <see cref="IEnumerable{T}"/> of expected items.</param>
        /// <param name="additionalExpectedItems">Additional items that are expected to be contained by the collection.</param>
        public AndConstraint<GenericCollectionAssertions<T>> Contain(IEnumerable<T> expectedItemsList,
            params T [] additionalExpectedItems)
        {
            var list = new List<T>(expectedItemsList);
            list.AddRange(additionalExpectedItems);

            return Contain((IEnumerable)list);
        }

        /// <summary>
        /// Asserts that the collection contains at least one item that matches the predicate.
        /// </summary>
        /// <param name="predicate">A predicate to match the items in the collection against.</param>
        public AndConstraint<GenericCollectionAssertions<T>> Contain(Expression<Func<T, bool>> predicate)
        {
            return Contain(predicate, string.Empty);
        }

        /// <summary>
        /// Asserts that the collection contains at least one item that matches the predicate.
        /// </summary>
        /// <param name="predicate">A predicate to match the items in the collection against.</param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<GenericCollectionAssertions<T>> Contain(Expression<Func<T, bool>> predicate, string reason, params object[] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected collection to contain {0}{reason}, but found {1}.", predicate.Body, Subject);
            }

            if (!Subject.Any(item => predicate.Compile()(item)))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Collection {0} should have an item matching {1}{reason}.", Subject, predicate.Body);
            }

            return new AndConstraint<GenericCollectionAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the collection only contains items that match a predicate.
        /// </summary>
        /// <param name="predicate">A predicate to match the items in the collection against.</param>
        public AndConstraint<GenericCollectionAssertions<T>> OnlyContain(Expression<Func<T, bool>> predicate)
        {
            return OnlyContain(predicate, string.Empty);
        }

        /// <summary>
        /// Asserts that the collection only contains items that match a predicate.
        /// </summary>
        /// <param name="predicate">A predicate to match the items in the collection against.</param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<GenericCollectionAssertions<T>> OnlyContain(
            Expression<Func<T, bool>> predicate, string reason, params object[] reasonArgs)
        {
            Func<T, bool> compiledPredicate = predicate.Compile();

            Execute.Verification
                .ForCondition(Subject.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected collection to contain only items matching {0}{reason}, but the collection is empty.",
                    predicate.Body);
            
            IEnumerable<T> mismatchingItems = Subject.Where(item => !compiledPredicate(item));
            if (mismatchingItems.Any())
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected collection to contain only items matching {0}{reason}, but {1} do(es) not match.",
                        predicate.Body, mismatchingItems);
            }

            return new AndConstraint<GenericCollectionAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the collection does not contain any items that match the predicate.
        /// </summary>
        /// <param name="predicate">A predicate to match the items in the collection against.</param>
        public AndConstraint<GenericCollectionAssertions<T>> NotContain(Expression<Func<T, bool>> predicate)
        {
            return NotContain(predicate, string.Empty);
        }

        /// <summary>
        /// Asserts that the collection does not contain any items that match the predicate.
        /// </summary>
        /// <param name="predicate">A predicate to match the items in the collection against.</param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<GenericCollectionAssertions<T>> NotContain(Expression<Func<T, bool>> predicate, string reason, params object[] reasonArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected collection not to contain {0}{reason}, but found {1}.", predicate.Body, Subject);
            }

            if (Subject.Any(item => predicate.Compile()(item)))
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Collection {0} should not have any items matching {1}{reason}.", Subject, predicate.Body);
            }

            return new AndConstraint<GenericCollectionAssertions<T>>(this);
        }
    }
}