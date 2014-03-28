using System.Reflection;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents a rule that defines how to map the properties from the subject-under-test with the properties 
    /// on the expectation object. 
    /// </summary>
    public interface IMatchingRule
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
        PropertyInfo Match(PropertyInfo subjectProperty, object expectation, string propertyPath);
    }
}