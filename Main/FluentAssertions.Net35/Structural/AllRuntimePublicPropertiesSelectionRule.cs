using System.Collections.Generic;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Structural
{
    public class AllRuntimePublicPropertiesSelectionRule : ISelectionRule
    {
        public IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> properties, TypeInfo info)
        {
            return info.RuntimeType.GetNonPrivateProperties();
        }
    }
}