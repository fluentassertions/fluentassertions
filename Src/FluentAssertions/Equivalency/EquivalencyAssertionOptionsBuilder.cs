using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Equivalency.Selection;

namespace FluentAssertions.Equivalency
{
    public class EquivalencyAssertionOptionsBuilder<TExpectation, TCurrent> : EquivalencyAssertionOptions<TExpectation>
    {
        /// <summary>
        /// The selected path staring at the first <see cref="EquivalencyAssertionOptions{TExpectation}.Excluding{TNext}"/>.
        /// </summary>
        private readonly ExcludeMemberByPathSelectionRule currentPathSelectionRule;

        internal EquivalencyAssertionOptionsBuilder(EquivalencyAssertionOptions<TExpectation> equivalencyAssertionOptions,
            ExcludeMemberByPathSelectionRule currentPathSelectionRule)
            : base(equivalencyAssertionOptions)
        {
            this.currentPathSelectionRule = currentPathSelectionRule;
        }

        /// <summary>
        /// Selects a property to use. This exists the <see cref="ThenExcluding"/> chain.
        /// </summary>
        public EquivalencyAssertionOptions<TExpectation> ThenExcluding(Expression<Func<TCurrent, object>> expression)
        {
            var nextPath = expression.GetMemberPath();
            currentPathSelectionRule.ExtendPath(nextPath);
            return this;
        }

        /// <summary>
        /// Adds the selected collection to the <see cref="ThenExcluding{TNext}"/> chain.
        /// If this is the last call to <see cref="ThenExcluding{TNext}"/>, this exists the chain.
        /// </summary>
        public EquivalencyAssertionOptionsBuilder<TExpectation, TNext> ThenExcluding<TNext>(
            Expression<Func<TCurrent, IEnumerable<TNext>>> expression)
        {
            var nextPath = expression.GetMemberPath();
            currentPathSelectionRule.ExtendPath(nextPath);
            return new EquivalencyAssertionOptionsBuilder<TExpectation, TNext>(this, currentPathSelectionRule);
        }
    }
}
