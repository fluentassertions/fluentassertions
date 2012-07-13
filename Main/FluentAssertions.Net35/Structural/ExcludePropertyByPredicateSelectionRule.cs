using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Structural
{
    /// <summary>
    /// Selection rule that removes a particular property from the structural comparison based on a predicate.
    /// </summary>
    internal class ExcludePropertyByPredicateSelectionRule : ISelectionRule
    {
        private readonly Func<ISelectionContext, bool> predicate;

        public ExcludePropertyByPredicateSelectionRule(Func<ISelectionContext, bool> predicate)
        {
            this.predicate = predicate;
        }

        /// <summary>
        /// Adds or removes properties to/from the collection of subject properties that must be included while
        /// comparing two objects for structural equality.
        /// </summary>
        /// <param name="properties">
        /// A collection of properties that was prepopulated by other selection rules. Can be empty.</param>
       /// <returns>
        /// The collection of properties after applying this rule. Can contain less or more than was passed in.
        /// </returns>
        public IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> properties, ISelectionContext context)
        {
            return properties.Where(p => !predicate(new NestedSelectionContext(context, p))).ToArray();
        }
    }
}
