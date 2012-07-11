using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Structural
{
    /// <summary>
    /// Selection rule that removes a particular property from the structural comparison.
    /// </summary>
    public class IgnorePropertySelectionRule : ISelectionRule
    {
        private readonly string propertyPath;

        public IgnorePropertySelectionRule(string propertyPath)
        {
            this.propertyPath = propertyPath;
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
            return properties.Where(pi => propertyPath != (info.PropertyPath + "." + pi.Name));
        }
    }
}