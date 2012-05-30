using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Structural
{
    public class IgnorePropertySelectionRule : ISelectionRule
    {
        private readonly PropertyInfo propertyInfo;

        public IgnorePropertySelectionRule(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }

        public IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> properties, TypeInfo info)
        {
            return properties.Where(pi => pi != propertyInfo).ToArray();
        }
    }
}