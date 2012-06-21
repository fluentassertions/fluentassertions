using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAssertions.Structural
{
    /// <summary>
    /// Represents a rule that defines which properties of the subject-under-test to include while comparing
    /// two objects for structural equality.
    /// </summary>
    public interface ISelectionRule
    {
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
        IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> properties, TypeInfo info);
    }

    /// <summary>
    /// Represents detailed type information on the subject in a structural comparison.
    /// </summary>
    public class TypeInfo
    {
        /// <summary>
        /// Gets the type of the subject as it was specified on the call to start the structural comparison.
        /// </summary>
        public Type DeclaredType { get; internal set; }

        /// <summary>
        /// Gets the actual runtime type of the subject.
        /// </summary>
        public Type RuntimeType { get; internal set; }
    }

}