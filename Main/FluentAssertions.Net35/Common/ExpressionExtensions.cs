using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;

namespace FluentAssertions.Common
{
    internal static class ExpressionExtensions
    {
        public static PropertyInfo GetPropertyInfo<T, TValue>(this Expression<Func<T, TValue>> expression)
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

        private static PropertyInfo AttemptToGetPropertyInfoFromPropertyExpression<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                return (PropertyInfo) memberExpression.Member;
            }

            return null;
        }

        private static PropertyInfo AttemptToGetPropertyInfoFromCastExpression<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            var castExpression = expression.Body as UnaryExpression;
            if (castExpression != null)
            {
                return (PropertyInfo) ((MemberExpression) castExpression.Operand).Member;
            }

            return null;
        }

        /// <summary>
        /// Gets a dotted path of property names representing the property expression. E.g. Parent.Child.Sibling.Name.
        /// </summary>
        public static string GetPropertyPath<TDeclaringType, TPropertyType>(
            this Expression<Func<TDeclaringType, TPropertyType>> propertyExpression)
        {
            var segments = new List<string>();

            MemberExpression member = GetMemberExpression(propertyExpression);
            while (member != null)
            {
                segments.Add(member.Member.Name);

                member = member.Expression as MemberExpression;
            }
            
            return string.Join(".", segments.AsEnumerable().Reverse().ToArray());
        }

        private static MemberExpression GetMemberExpression<TDeclaringType, TPropertyType>(Expression<Func<TDeclaringType, TPropertyType>> expr)
        {
            MemberExpression member;
            
            switch (expr.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = expr.Body as UnaryExpression;
                    member = ((ue != null) ? ue.Operand : null) as MemberExpression;
                    break;
                
                default:
                    member = expr.Body as MemberExpression;
                    break;
            }

            return member;
        }
    }
}