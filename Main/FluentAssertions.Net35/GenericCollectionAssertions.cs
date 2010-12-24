using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAssertions
{
    public class GenericCollectionAssertions<T> : CollectionAssertions<IEnumerable<T>, GenericCollectionAssertions<T>>
    {
        public GenericCollectionAssertions(IEnumerable<T> actualValue)
        {
            if (actualValue != null)
            {
                Subject = actualValue;
            }
        }

        public AndConstraint<GenericCollectionAssertions<T>> Contain(T expected)
        {
            return Contain(expected, string.Empty);
        }

        public AndConstraint<GenericCollectionAssertions<T>> Contain(T expected, string reason, params object[] reasonParameters)
        {
            VerifySubjectCollectionAgainstNull("Expected collection to contain {0}, but found {1}", expected, reason,
                                               reasonParameters);

            if (!Subject.Contains(expected))
            {
                Verification.Fail("Expected collection {1} to contain {0}{2}.", expected, Subject, reason, reasonParameters);
            }

            return new AndConstraint<GenericCollectionAssertions<T>>(this);
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
        public AndConstraint<GenericCollectionAssertions<T>> Contain(Expression<Func<T, bool>> predicate, string reason, params object[] reasonParameters)
        {
            VerifySubjectCollectionAgainstNull("Expected collection to contain {0}{2}, but found {1}", predicate.Body, reason,
                                               reasonParameters);

            if (!Subject.Any(item => predicate.Compile()(item)))
            {
                Verification.Fail("Collection {1} should have an item matching {0}{2}.", predicate.Body, Subject, reason, reasonParameters);
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
        public AndConstraint<GenericCollectionAssertions<T>> NotContain(Expression<Func<T, bool>> predicate, string reason, params object[] reasonParameters)
        {
            VerifySubjectCollectionAgainstNull("Expected collection not to contain {0}{2}, but found {1}", predicate.Body, reason,
                                               reasonParameters);

            if (Subject.Any(item => predicate.Compile()(item)))
            {
                Verification.Fail("Collection {1} should not have any items matching {0}{2}.", predicate.Body, Subject, reason, reasonParameters);
            }

            return new AndConstraint<GenericCollectionAssertions<T>>(this);
        }
    }
}