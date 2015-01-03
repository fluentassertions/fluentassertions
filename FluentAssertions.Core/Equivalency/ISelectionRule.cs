using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents a rule that defines which properties of the subject-under-test to include while comparing
    /// two objects for structural equality.
    /// </summary>
    [Obsolete("This interface will be removed in a future version.  Use `IMemberSelectionRule` instead.")]
    public interface ISelectionRule
    {
        /// <summary>
        /// Adds or removes properties to/from the collection of subject properties that must be included while
        /// comparing two objects for structural equality.
        /// </summary>
        /// <param name="selectedProperties">
        /// A collection of properties that was prepopulated by other selection rules. Can be empty.</param>
        /// <param name="info">
        /// Type info about the subject.
        /// </param>
        /// <returns>
        /// The collection of properties after applying this rule. Can contain less or more than was passed in.
        /// </returns>
        IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> selectedProperties, ISubjectInfo context);
    }
}