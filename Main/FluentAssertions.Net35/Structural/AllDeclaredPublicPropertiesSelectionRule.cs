using System.Collections.Generic;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Structural
{
    public class AllDeclaredPublicPropertiesSelectionRule : ISelectionRule
    {
        public IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> properties, TypeInfo info)
        {
            return info.DeclaredType.GetNonPrivateProperties();
        }
    }
}