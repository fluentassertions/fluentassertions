using System.Reflection;

namespace FluentAssertions.Structural
{
    /// <summary>
    /// Represents a rule that defines how to map the properties from the subject-under-test with the properties 
    /// on the expectation. 
    /// </summary>
    public interface IMatchingRule
    {
        /// <summary>
        /// Attempts to find the property on the expectation that must be compared with the 
        /// subject's property named <paramref name="propertyPath"/>.
        /// <paramref name="propertyPath"/>.
        /// </summary>
        /// <param name="subjectProperty">
        /// The <see cref="PropertyInfo"/> of the subject's property for which a match must be found. Will never
        /// be <c>null</c>.
        /// </param>
        /// <param name="expectation">
        /// The expectation on which the search is performed. Will never be <c>null</c>.
        /// </param>
        /// <param name="propertyPath">
        /// The dotted path from the root object to the current property. Will never  be <c>null</c>.
        /// </param>
        /// <returns></returns>
        PropertyInfo FindMatch(PropertyInfo subjectProperty, object expectation, string propertyPath);
    }
}