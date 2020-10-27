using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAssertions.Collections
{
    [DebuggerNonUserCode]
    public class SubsequentOrderingGenericCollectionAssertions<T> :
        SubsequentOrderingGenericCollectionAssertions<IEnumerable<T>, T, SubsequentOrderingGenericCollectionAssertions<T>>
    {
        public SubsequentOrderingGenericCollectionAssertions(IEnumerable<T> actualValue, IOrderedEnumerable<T> previousOrderedEnumerable)
            : base(actualValue, previousOrderedEnumerable)
        {
        }
    }

    [DebuggerNonUserCode]
    public class SubsequentOrderingGenericCollectionAssertions<TCollection, T> :
        SubsequentOrderingGenericCollectionAssertions<TCollection, T, SubsequentOrderingGenericCollectionAssertions<TCollection, T>>
        where TCollection : IEnumerable<T>
    {
        public SubsequentOrderingGenericCollectionAssertions(TCollection actualValue, IOrderedEnumerable<T> previousOrderedEnumerable)
            : base(actualValue, previousOrderedEnumerable)
        {
        }
    }

    [DebuggerNonUserCode]
    public class SubsequentOrderingGenericCollectionAssertions<TCollection, T, TAssertions> :
        GenericCollectionAssertions<TCollection, T, TAssertions>
        where TCollection : IEnumerable<T>
        where TAssertions : SubsequentOrderingGenericCollectionAssertions<TCollection, T, TAssertions>
    {
        private readonly IOrderedEnumerable<T> previousOrderedEnumerable;
        private bool subsequentOrdering = false;

        public SubsequentOrderingGenericCollectionAssertions(TCollection actualValue, IOrderedEnumerable<T> previousOrderedEnumerable)
            : base(actualValue)
        {
            this.previousOrderedEnumerable = previousOrderedEnumerable;
        }

        public AndConstraint<SubsequentOrderingGenericCollectionAssertions<T>> ThenBeInAscendingOrder<TSelector>(
            Expression<Func<T, TSelector>> propertyExpression, string because = "", params object[] becauseArgs)
        {
            return ThenBeInAscendingOrder(propertyExpression, Comparer<TSelector>.Default, because, becauseArgs);
        }

        public AndConstraint<SubsequentOrderingGenericCollectionAssertions<T>> ThenBeInAscendingOrder<TSelector>(
            Expression<Func<T, TSelector>> propertyExpression, IComparer<TSelector> comparer, string because = "", params object[] becauseArgs)
        {
            return ThenBeOrderedBy(propertyExpression, comparer, SortOrder.Ascending, because, becauseArgs);
        }

        public AndConstraint<SubsequentOrderingGenericCollectionAssertions<T>> ThenBeInDescendingOrder<TSelector>(
            Expression<Func<T, TSelector>> propertyExpression, string because = "", params object[] becauseArgs)
        {
            return ThenBeInDescendingOrder(propertyExpression, Comparer<TSelector>.Default, because, becauseArgs);
        }

        public AndConstraint<SubsequentOrderingGenericCollectionAssertions<T>> ThenBeInDescendingOrder<TSelector>(
            Expression<Func<T, TSelector>> propertyExpression, IComparer<TSelector> comparer, string because = "", params object[] becauseArgs)
        {
            return ThenBeOrderedBy(propertyExpression, comparer, SortOrder.Descending, because, becauseArgs);
        }

        private AndConstraint<SubsequentOrderingGenericCollectionAssertions<T>> ThenBeOrderedBy<TSelector>(
            Expression<Func<T, TSelector>> propertyExpression,
            IComparer<TSelector> comparer,
            SortOrder direction,
            string because,
            object[] becauseArgs)
        {
            subsequentOrdering = true;
            return BeOrderedBy(propertyExpression, comparer, direction, because, becauseArgs);
        }

        internal sealed override IOrderedEnumerable<T> GetOrderedEnumerable<TSelector>(
            Expression<Func<T, TSelector>> propertyExpression,
            IComparer<TSelector> comparer,
            SortOrder direction,
            ICollection<T> unordered)
        {
            if (subsequentOrdering)
            {
                Func<T, TSelector> keySelector = propertyExpression.Compile();

                IOrderedEnumerable<T> expectation = (direction == SortOrder.Ascending)
                    ? previousOrderedEnumerable.ThenBy(keySelector, comparer)
                    : previousOrderedEnumerable.ThenByDescending(keySelector, comparer);

                return expectation;
            }

            return base.GetOrderedEnumerable(propertyExpression, comparer, direction, unordered);
        }
    }
}
