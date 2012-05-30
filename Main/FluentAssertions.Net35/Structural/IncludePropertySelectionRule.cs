using System.Collections.Generic;
using System.Reflection;

namespace FluentAssertions.Structural
{
    public class IncludePropertySelectionRule : ISelectionRule
    {
        private readonly PropertyInfo propertyInfo;

        public IncludePropertySelectionRule(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }

        public IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> properties, TypeInfo info)
        {
            var props = new List<PropertyInfo>(properties);

            if (!props.Contains(propertyInfo))
            {
                props.Add(propertyInfo);
            }

            return props;
        }
    }
}