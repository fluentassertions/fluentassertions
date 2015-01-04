using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using FluentAssertions.Equivalency;

namespace FluentAssertions.Common
{
    public static class ExpressionExtensions
    {
        public static ISelectedMemberInfo GetSelectedMemberInfo<T, TValue>(this Expression<Func<T, TValue>> expression)
        {
            if (ReferenceEquals(expression, null))
            {
                throw new NullReferenceException("Expected an expression, but found <null>.");
            }

            MemberInfo memberInfo = AttemptToGetMemberInfoFromCastExpression(expression) ??
                                    AttemptToGetMemberInfoFromMemberExpression(expression);

            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                return SelectedMemberInfo.Create(propertyInfo);
            }

            throw new ArgumentException(
                string.Format("Expression <{0}> cannot be used to select a member.", expression.Body));
        }

        public static PropertyInfo GetPropertyInfo<T, TValue>(this Expression<Func<T, TValue>> expression)
        {
            if (ReferenceEquals(expression, null))
            {
                throw new NullReferenceException("Expected a property expression, but found <null>.");
            }

            var memberInfo = AttemptToGetMemberInfoFromCastExpression(expression) ??
                             AttemptToGetMemberInfoFromMemberExpression(expression);

            var propertyInfo = memberInfo as PropertyInfo;

            if (propertyInfo == null)
            {
                throw new ArgumentException("Cannot use <" + expression.Body + "> when a property expression is expected.");
            }

            return propertyInfo;
        }

        private static MemberInfo AttemptToGetMemberInfoFromMemberExpression<T, TValue>(
            Expression<Func<T, TValue>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                return memberExpression.Member;
            }

            return null;
        }

        private static MemberInfo AttemptToGetMemberInfoFromCastExpression<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            var castExpression = expression.Body as UnaryExpression;
            if (castExpression != null)
            {
                return ((MemberExpression)castExpression.Operand).Member;
            }

            return null;
        }

        /// <summary>
        /// Gets a dotted path of property names representing the property expression. E.g. Parent.Child.Sibling.Name.
        /// </summary>
        public static string GetMemberPath<TDeclaringType, TPropertyType>(
            this Expression<Func<TDeclaringType, TPropertyType>> expression)
        {
            var segments = new List<string>();
            Expression node = expression;

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
                            string.Format("Expression <{0}> cannot be used to select a member.", expression.Body));
                }
            }

            return string.Join(".", segments.AsEnumerable().Reverse().ToArray()).Replace(".[", "[");
        }

        [Obsolete("This method will be removed in a future version.  Use `GetMemberPath(expression)` instead.")]
        public static string GetPropertyPath<TDeclaringType, TPropertyType>(
            this Expression<Func<TDeclaringType, TPropertyType>> propertyExpression)
        {
            return GetMemberPath(propertyExpression);
        }
    }
}