using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Selection rule that includes a particular property in the structural comparison.
    /// </summary>
    internal class IncludeMemberByPredicateSelectionRule : ISelectionRule
    {
        private readonly Func<ISubjectInfo, bool> predicate;
        private readonly string description;

        public IncludeMemberByPredicateSelectionRule(Expression<Func<ISubjectInfo, bool>> predicate)
        {
            description = predicate.Body.ToString();
            this.predicate = predicate.Compile();
        }

        /// <summary>
        /// Adds or removes properties to/from the collection of subject properties that must be included while
        /// comparing two objects for structural equality.
        /// </summary>
        /// <param name="selectedProperties">
        /// A collection of properties that was prepopulated by other selection rules. Can be empty.</param>
        /// <returns>
        /// The collection of properties after applying this rule. Can contain less or more than was passed in.
        /// </returns>
        public IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> selectedProperties, ISubjectInfo context)
        {
            var properties = new List<PropertyInfo>(selectedProperties);

            foreach (PropertyInfo propertyInfo in context.RuntimeType.GetNonPrivateProperties())
            {
                if (predicate(new NestedSelectionContext(context, propertyInfo)))
                {
                    if (!properties.Any(p => p.IsEquivalentTo(propertyInfo)))
                    {
                        properties.Add(propertyInfo);
                    }
                }
            }

            return properties;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return "Exclude property when " + description;
        }
    }
}