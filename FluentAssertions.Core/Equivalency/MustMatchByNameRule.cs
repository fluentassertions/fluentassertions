using System.Reflection;

using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Requires the expectation object to have a property with the exact same name.
    /// </summary>
    public class MustMatchByNameRule : IMatchingRule
    {
        /// <summary>
        /// Attempts to find a property on the expectation that should be compared with the 
        /// <paramref name="subjectProperty"/> during a structural equality.
        /// </summary>
        /// <remarks>
        /// Whether or not a match is required or optional is up to the specific rule. If no match is found and this is not an issue,
        /// simply return <c>null</c>.
        /// </remarks>
        /// <param name="subjectProperty">
        /// The <see cref="PropertyInfo"/> of the subject's property for which a match must be found. Can never
        /// be <c>null</c>.
        /// </param>
        /// <param name="expectation">
        /// The expectation object for which a matching property must be returned. Can never be <c>null</c>.
        /// </param>
        /// <param name="propertyPath">
        /// The dotted path from the root object to the current property. Will never  be <c>null</c>.
        /// </param>
        /// <returns>
        /// Returns the <see cref="PropertyInfo"/> of the property with which to compare the subject with, or <c>null</c>
        /// if no match was found.
        /// </returns>
        public PropertyInfo Match(PropertyInfo subjectProperty, object expectation, string propertyPath)
        {
            PropertyInfo compareeProperty = expectation.GetType().FindProperty(subjectProperty.Name, subjectProperty.PropertyType);
            if (compareeProperty == null)
            {
                string path = (propertyPath.Length > 0) ? propertyPath + "." : "property ";

                Execute.Assertion.FailWith(
                    "Subject has " + path + subjectProperty.Name + " that the other object does not have.");
            }

            return compareeProperty;
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
            return "Match property by name (or throw)";
        }
    }
}