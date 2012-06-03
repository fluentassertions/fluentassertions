using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;

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

            if (!props.Any(p => p.IsEquivalentTo(propertyInfo)))
            {
                props.Add(propertyInfo);
            }

            return props;
        }
    }
}