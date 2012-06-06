using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Types
{
    public class AttributeConstraints<TAttribute>
    {
        private readonly Dictionary<PropertyInfo, object> expectedProperties = new Dictionary<PropertyInfo, object>();

        public AttributeConstraints<TAttribute> WithProperty<TProperty>(Expression<Func<TAttribute, TProperty>> propertyExpression, TProperty expectedValue)
        {
            PropertyInfo property = propertyExpression.GetPropertyInfo();
            expectedProperties.Add(property, expectedValue);
            return this;
        }

        public IDictionary<PropertyInfo, object> ExpectedProperties
        {
            get { return expectedProperties; }
        }
    }
}