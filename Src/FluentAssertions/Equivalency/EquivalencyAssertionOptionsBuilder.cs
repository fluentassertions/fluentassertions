using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentAssertions.Equivalency
{
    public class EquivalencyAssertionOptionsBuilder<TExpectation, TCurrent> : EquivalencyAssertionOptions<TExpectation>
    {
        public EquivalencyAssertionOptionsBuilder(EquivalencyAssertionOptions<TExpectation> equivalencyAssertionOptions)
            : base(equivalencyAssertionOptions)
        {

        }

        /// <summary>
        /// Selects the actual property to exclude.
        /// </summary>
        public EquivalencyAssertionOptions<TExpectation> ThenExcluding(Expression<Func<TCurrent, object>> expression)
        {
            return this;
        }

        /// <summary>
        /// Walk further down.
        /// </summary>
        public EquivalencyAssertionOptionsBuilder<TExpectation, TNext> ThenExcluding<TNext>(
            Expression<Func<TCurrent, IEnumerable<TNext>>> expression)
        {
            return new EquivalencyAssertionOptionsBuilder<TExpectation, TNext>(this);
        }
    }
}
