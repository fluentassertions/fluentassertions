using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Selection rule that includes a particular property in the structural comparison.
    /// </summary>
    internal class IncludePropertyByPathSelectionRule : ISelectionRule
    {
        private readonly PropertyInfo propertyInfo;

        public IncludePropertyByPathSelectionRule(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
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
            var props = new List<PropertyInfo>(selectedProperties);

            if (!props.Any(p => p.IsEquivalentTo(propertyInfo)))
            {
                props.Add(propertyInfo);
            }

            return props;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Select property " + propertyInfo.DeclaringType + "." + propertyInfo.Name;
        }
    }
}