using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Selects a nested property to exclude. This ends the <see cref="For{TNext}"/> chain.
        /// </summary>
        public EquivalencyAssertionOptions<TExpectation> Exclude(Expression<Func<TCurrent, object>> expression)
        {
            var nextPath = expression.GetMemberPath();
            currentPathSelectionRule.AppendPath(nextPath);
            AddSelectionRule(currentPathSelectionRule);
            return this;
        }

        /// <summary>
        /// Adds the selected collection to the <see cref="For{TNext}"/> chain.
        /// </summary>
        public NestedExclusionOptionBuilder<TExpectation, TNext> For<TNext>(
            Expression<Func<TCurrent, IEnumerable<TNext>>> expression)
        {
            var nextPath = expression.GetMemberPath();
            currentPathSelectionRule.AppendPath(nextPath);
            return new NestedExclusionOptionBuilder<TExpectation, TNext>(this, currentPathSelectionRule);
        }
    }
}
