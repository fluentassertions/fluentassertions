using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Collections
{
    [DebuggerNonUserCode]
    public class GenericCollectionAssertions<T> :
        SelfReferencingCollectionAssertions<T, GenericCollectionAssertions<T>>
    {
        public GenericCollectionAssertions(IEnumerable<T> actualValue) : base(actualValue)
        {
        }

        public AndConstraint<GenericCollectionAssertions<T>> BeInDescendingOrder(
            Expression<Func<T, object>> propertyExpression, string because = "", params object[] args)
        {
            return BeOrderedBy(propertyExpression, SortDirection.Descending, because, args);
        }

        public AndConstraint<GenericCollectionAssertions<T>> BeInAscendingOrder(
            Expression<Func<T, object>> propertyExpression, string because = "", params object[] args)
        {
            return BeOrderedBy(propertyExpression, SortDirection.Ascending, because, args);
        }

        private AndConstraint<GenericCollectionAssertions<T>> BeOrderedBy(
            Expression<Func<T, object>> propertyExpression, SortDirection direction, string because, object[] args)
        {
            if (IsValidProperty(propertyExpression, because, args))
            {
                IList<T> unordered = (Subject as IList<T>) ?? Subject.ToList();

                Func<T, object> keySelector = propertyExpression.Compile();

                IOrderedEnumerable<T> expectation = (direction == SortDirection.Ascending)
                    ? unordered.OrderBy(keySelector)
                    : unordered.OrderByDescending(keySelector);

                Execute.Assertion
                    .ForCondition(unordered.SequenceEqual(expectation))
                    .BecauseOf(because, args)
                    .FailWith("Expected collection {0} to be ordered by {1}{reason} and result in {2}.",
                        Subject, propertyExpression.GetPropertyPath(), expectation);
            }
            
            return new AndConstraint<GenericCollectionAssertions<T>>(this);
        }

        private bool IsValidProperty(Expression<Func<T, object>> propertyExpression, string because, object[] args)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression",
                    "Cannot assert collection ordering without specifying a property.");
            }

            return Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, args)
                .FailWith("Expected collection to be ordered by {0}{reason} but found <null>.",
                    propertyExpression.GetPropertyPath());
        }

        private enum SortDirection
        {
            Ascending,
            Descending
        }
    }
}