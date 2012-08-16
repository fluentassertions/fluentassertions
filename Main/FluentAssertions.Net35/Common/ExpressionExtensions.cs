using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

            var propertyInfo = AttemptToGetPropertyInfoFromCastExpression(expression);
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

        private static PropertyInfo AttemptToGetPropertyInfoFromPropertyExpression<T, TValue>(
            Expression<Func<T, TValue>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                return (PropertyInfo)memberExpression.Member;
            }

            return null;
        }

        private static PropertyInfo AttemptToGetPropertyInfoFromCastExpression<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            var castExpression = expression.Body as UnaryExpression;
            if (castExpression != null)
            {
                return (PropertyInfo)((MemberExpression)castExpression.Operand).Member;
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
            Expression node = propertyExpression;

            while (node != null)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Lambda:
                        node = ((LambdaExpression)node).Body;
                        break;

                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                        var unaryExpression = (UnaryExpression)node;
                        node = unaryExpression.Operand;
                        break;
                    
                    case ExpressionType.MemberAccess:
                        var memberExpression = (MemberExpression)node;
                        node = memberExpression.Expression;
                        
                        segments.Add(memberExpression.Member.Name);
                        break;

                    case ExpressionType.ArrayIndex:
                        var binaryExpression = (BinaryExpression)node;
                        var constantExpression = (ConstantExpression)binaryExpression.Right;
                        node = binaryExpression.Left;

                        segments.Add("[" + constantExpression.Value + "]");
                        break;

                    case ExpressionType.Parameter:
                        node = null;
                        break;

                    default:
                        throw new ArgumentException(
                            "Expression {" + propertyExpression.Body + "} is not a valid property expression");
                }
            }

            return string.Join(".", segments.AsEnumerable().Reverse().ToArray()).Replace(".[", "[");
        }
    }
}