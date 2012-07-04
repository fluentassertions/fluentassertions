using System.Reflection;

namespace FluentAssertions.Structural
{
    public interface IAssertionRule
    {
        /// <summary>
        /// Defines how a subject's property is compared for equality with the same property of the expectation.
        /// </summary>
        /// <param name="subjectProperty">
        /// Provides details about the subject's property.
        /// </param>
        /// <param name="subject">
        /// The value of the subject's property.
        /// </param>
        /// <param name="expectation">
        /// The value of a property on expectation object that was identified 
        /// </param>
        /// <returns>
        /// Returns <c>true</c> if the rule was applied correctly and the assertion didn't cause any exceptions. 
        /// Returns <c>false</c> if this rule doesn't support the subject's type.
        /// Throws if the rule did support the data type but assertion fails.
        /// </returns>
        bool AssertEquality(PropertyInfo subjectProperty, object subject, object expectation);
    }
}