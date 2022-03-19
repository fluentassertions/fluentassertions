using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Equivalency.Selection;

namespace FluentAssertions.Equivalency
{
    public class NestedExclusionOptionBuilder<TExpectation, TCurrent> : EquivalencyAssertionOptions<TExpectation>
    {
        /// <summary>
        /// The selected path starting at the first <see cref="EquivalencyAssertionOptions{TExpectation}.For{TNext}"/>.
        /// </summary>
        private readonly ExcludeMemberByPathSelectionRule currentPathSelectionRule;

        internal NestedExclusionOptionBuilder(EquivalencyAssertionOptions<TExpectation> equivalencyAssertionOptions,
            ExcludeMemberByPathSelectionRule currentPathSelectionRule)
            : base(equivalencyAssertionOptions)
        {
            this.currentPathSelectionRule = currentPathSelectionRule;
        }

        /// <summary>
        /// Selects a property to use. This ends the <see cref="Exclude"/> chain.
        /// </summary>
        public EquivalencyAssertionOptions<TExpectation> Exclude(Expression<Func<TCurrent, object>> expression)
        {
            var nextPath = expression.GetMemberPath();
            currentPathSelectionRule.CombinePath(nextPath);
            return this;
        }

        /// <summary>
        /// Adds the selected collection to the <see cref="For{TNext}"/> chain.
        /// If this is the last call to <see cref="For{TNext}"/>, this ends the chain.
        /// </summary>
        public NestedExclusionOptionBuilder<TExpectation, TNext> For<TNext>(
            Expression<Func<TCurrent, IEnumerable<TNext>>> expression)
        {
            var nextPath = expression.GetMemberPath();
            currentPathSelectionRule.CombinePath(nextPath);
            return new NestedExclusionOptionBuilder<TExpectation, TNext>(this, currentPathSelectionRule);
        }
    }
}
