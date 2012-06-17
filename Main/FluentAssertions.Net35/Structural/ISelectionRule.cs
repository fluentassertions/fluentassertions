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
        IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> properties, TypeInfo info);
    }

    public class TypeInfo
    {
        public Type DeclaredType { get; set; }
        public Type RuntimeType { get; set; }
    }

}