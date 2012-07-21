using System.Collections.Generic;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Structural
{
    /// <summary>
    /// Selection rule that adds all public properties of the subject based on its run-time type rather than its
    /// declared type.
    /// </summary>
    internal class AllRuntimePublicPropertiesSelectionRule : ISelectionRule
    {
        /// <summary>
        /// Adds or removes properties to/from the collection of subject properties that must be included while
        /// comparing two objects for structural equality.
        /// </summary>
        /// <param name="properties">
        /// A collection of properties that was prepopulated by other selection rules. Can be empty.</param>
        /// <returns>
        /// The collection of properties after applying this rule. Can contain less or more than was passed in.
        /// </returns>
        public IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> properties, ISubjectInfo context)
        {
            return context.RuntimeType.GetNonPrivateProperties();
        }
    }
}