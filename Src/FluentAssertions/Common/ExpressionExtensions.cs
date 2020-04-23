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
            Guard.ThrowIfArgumentIsNull(expression, nameof(expression), "Expected a property expression, but found <null>.");

            MemberInfo memberInfo = AttemptToGetMemberInfoFromExpression(expression);

            if (!(memberInfo is PropertyInfo propertyInfo))
            {
                throw new ArgumentException("Cannot use <" + expression.Body + "> when a property expression is expected.",
                    nameof(expression));
            }

            return propertyInfo;
        }

        private static MemberInfo AttemptToGetMemberInfoFromExpression<T, TValue>(Expression<Func<T, TValue>> expression) =>
            (((expression.Body as UnaryExpression)?.Operand ?? expression.Body) as MemberExpression)?.Member;

        /// <summary>
        /// Gets a dotted path of property names representing the property expression, including the declaring type.
        /// </summary>
        /// <example>
        /// E.g. Parent.Child.Sibling.Name.
        /// </example>
        public static MemberPath GetMemberPath<TDeclaringType, TPropertyType>(
            this Expression<Func<TDeclaringType, TPropertyType>> expression)
        {
            Guard.ThrowIfArgumentIsNull(expression, nameof(expression), "Expected an expression, but found <null>.");

            var segments = new List<string>();
            var declaringTypes = new List<Type>();
            Expression node = expression;

            var unsupportedExpressionMessage = $"Expression <{expression.Body}> cannot be used to select a member.";

            while (node != null)
            {
#pragma warning disable IDE0010 // System.Linq.Expressions.ExpressionType has many members we do not care about
                switch (node.NodeType)
#pragma warning restore IDE0010
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
                        declaringTypes.Add(memberExpression.Member.DeclaringType);
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

                    case ExpressionType.Call:
                        var methodCallExpression = (MethodCallExpression)node;
                        if (methodCallExpression.Method.Name != "get_Item" || methodCallExpression.Arguments.Count != 1 || !(methodCallExpression.Arguments[0] is ConstantExpression))
                        {
                            throw new ArgumentException(unsupportedExpressionMessage, nameof(expression));
                        }

                        constantExpression = (ConstantExpression)methodCallExpression.Arguments[0];
                        node = methodCallExpression.Object;
                        segments.Add("[" + constantExpression.Value + "]");
                        break;

                    default:
                        throw new ArgumentException(unsupportedExpressionMessage, nameof(expression));
                }
            }

            // If any members were accessed in the expression, the first one found is the last member.
            Type declaringType = declaringTypes.FirstOrDefault() ?? typeof(TDeclaringType);

            string[] reversedSegments = segments.AsEnumerable().Reverse().ToArray();
            string segmentPath = string.Join(".", reversedSegments);

            return new MemberPath(declaringType, segmentPath.Replace(".[", "[", StringComparison.Ordinal));
        }
    }
}
