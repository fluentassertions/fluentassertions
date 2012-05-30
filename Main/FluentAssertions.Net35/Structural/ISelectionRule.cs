using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAssertions.Structural
{
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