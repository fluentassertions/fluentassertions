using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentAssertions.Common
{
    internal static class ExpressionExtensions
    {
        public static PropertyInfo GetPropertyInfo<T>(this Expression<Func<T, object>> expression)
        {
            if (ReferenceEquals(expression, null))
            {
                throw new NullReferenceException("Expected a property expression, but found <null>.");
            }

            PropertyInfo propertyInfo = AttemptToGetPropertyInfoFromCastExpression(expression);
            if (propertyInfo == null)
            {
                propertyInfo = AttemptToGetPropertyInfoFromPropertyExpression(expression);
            }

            if (propertyInfo == null)
            {
                throw new ArgumentException("Cannot use <" + expression.Body + "> when a property expression is expected.");
            }

            return propertyInfo;
        }

        private static PropertyInfo AttemptToGetPropertyInfoFromPropertyExpression<T>(Expression<Func<T, object>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                return (PropertyInfo) memberExpression.Member;
            }

            return null;
        }

        private static PropertyInfo AttemptToGetPropertyInfoFromCastExpression<T>(Expression<Func<T, object>> expression)
        {
            var castExpression = expression.Body as UnaryExpression;
            if (castExpression != null)
            {
                return (PropertyInfo) ((MemberExpression) castExpression.Operand).Member;
            }

            return null;
        }
    }
}