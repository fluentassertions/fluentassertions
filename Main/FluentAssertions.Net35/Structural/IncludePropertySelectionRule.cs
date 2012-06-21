using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Structural
{
    /// <summary>
    /// Selection rule that includes a particular property in the structural comparison.
    /// </summary>
    public class IncludePropertySelectionRule : ISelectionRule
    {
        private readonly PropertyInfo propertyInfo;

        public IncludePropertySelectionRule(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }

        /// <summary>
        /// Adds or removes properties to/from the collection of subject properties that must be included while
        /// comparing two objects for structural equality.
        /// </summary>
        /// <param name="properties">
        /// A collection of properties that was prepopulated by other selection rules. Can be empty.</param>
        /// <param name="info">
        /// Type info about the subject.
        /// </param>
        /// <returns>
        /// The collection of properties after applying this rule. Can contain less or more than was passed in.
        /// </returns>
        public IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> properties, TypeInfo info)
        {
            var props = new List<PropertyInfo>(properties);

            if (!props.Any(p => p.IsEquivalentTo(propertyInfo)))
            {
                props.Add(propertyInfo);
            }

            return props;
        }
    }
}