using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAssertions.Assertions
{
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
                Execute.Fail("Expected collection to contain {0}, but found {1}.", expected, Subject, reason, reasonArgs);
            }

            if (!Subject.Contains(expected))
            {
                Execute.Fail("Expected collection {1} to contain {0}{2}.", expected, Subject, reason, reasonArgs);
            }

            return new AndConstraint<GenericCollectionAssertions<T>>(this);
        }

        
        /// <summary>
        /// Asserts that the collection contains some extra items in addition to the original items.
        /// </summary>
        public AndConstraint<GenericCollectionAssertions<T>> Contain(IEnumerable<T> originalItems, params T[] additionalItems)
        {
            var list = new List<T>(originalItems);
            list.AddRange(additionalItems);

            return Contain((IEnumerable)list);
        }

        /// <summary>
        /// Asserts that the collection contains at least one item that matches the predicate.
        /// </summary>
        public AndConstraint<GenericCollectionAssertions<T>> Contain(Expression<Func<T, bool>> predicate)
        {
            return Contain(predicate, string.Empty);
        }

        /// <summary>
        /// Asserts that the collection contains at least one item that matches the predicate.
        /// </summary>
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
                Execute.Fail("Expected collection to contain {0}{2}, but found {1}.", predicate.Body, Subject, reason, reasonArgs);
            }

            if (!Subject.Any(item => predicate.Compile()(item)))
            {
                Execute.Fail("Collection {1} should have an item matching {0}{2}.", predicate.Body, Subject, reason, reasonArgs);
            }

            return new AndConstraint<GenericCollectionAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the collection only contains items that match a predicate.
        /// </summary>
        public AndConstraint<GenericCollectionAssertions<T>> OnlyContain(Expression<Func<T, bool>> predicate)
        {
            return OnlyContain(predicate, "");
        }

        /// <summary>
        /// Asserts that the collection only contains items that match a predicate.
        /// </summary>
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
            
            IEnumerable<T> mismatchingItems = Subject.Where(item => !compiledPredicate(item));
            if (mismatchingItems.Any())
            {
                Execute.Fail("Expected collection to contain only items matching {0}{2}, but {1} do(es) not match.",
                    predicate.Body, mismatchingItems, reason, reasonArgs);
            }

            return new AndConstraint<GenericCollectionAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the collection does not contain any items that match the predicate.
        /// </summary>
        public AndConstraint<GenericCollectionAssertions<T>> NotContain(Expression<Func<T, bool>> predicate)
        {
            return NotContain(predicate, string.Empty);
        }

        /// <summary>
        /// Asserts that the collection does not contain any items that match the predicate.
        /// </summary>
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
                Execute.Fail("Expected collection not to contain {0}{2}, but found {1}.", predicate.Body, Subject, reason, reasonArgs);
            }

            if (Subject.Any(item => predicate.Compile()(item)))
            {
                Execute.Fail("Collection {1} should not have any items matching {0}{2}.", predicate.Body, Subject, reason, reasonArgs);
            }

            return new AndConstraint<GenericCollectionAssertions<T>>(this);
        }
    }
}