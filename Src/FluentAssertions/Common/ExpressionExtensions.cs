using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentAssertionsAsync.Common;

internal static class ExpressionExtensions
{
    /// <summary>
    /// Gets the <see cref="PropertyInfo"/> of an <see cref="Expression{T}" /> returning a property.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="expression"/> is <see langword="null"/>.</exception>
    public static PropertyInfo GetPropertyInfo<T, TValue>(this Expression<Func<T, TValue>> expression)
    {
        Guard.ThrowIfArgumentIsNull(expression, nameof(expression), "Expected a property expression, but found <null>.");

        MemberInfo memberInfo = AttemptToGetMemberInfoFromExpression(expression);

        if (memberInfo is not PropertyInfo propertyInfo)
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
    /// <exception cref="ArgumentNullException"><paramref name="expression"/> is <see langword="null"/>.</exception>
#pragma warning disable MA0051
    public static MemberPath GetMemberPath<TDeclaringType, TPropertyType>(
        this Expression<Func<TDeclaringType, TPropertyType>> expression)
#pragma warning restore MA0051
    {
        Guard.ThrowIfArgumentIsNull(expression, nameof(expression), "Expected an expression, but found <null>.");

        var segments = new List<string>();
        var declaringTypes = new List<Type>();
        Expression node = expression;

        while (node is not null)
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
                    var indexExpression = (ConstantExpression)binaryExpression.Right;
                    node = binaryExpression.Left;

                    segments.Add("[" + indexExpression.Value + "]");
                    break;

                case ExpressionType.Parameter:
                    node = null;
                    break;

                case ExpressionType.Call:
                    var methodCallExpression = (MethodCallExpression)node;

                    if (methodCallExpression is not { Method.Name: "get_Item", Arguments: [ConstantExpression argumentExpression] })
                    {
                        throw new ArgumentException(GetUnsupportedExpressionMessage(expression.Body), nameof(expression));
                    }

                    node = methodCallExpression.Object;
                    segments.Add("[" + argumentExpression.Value + "]");
                    break;

                default:
                    throw new ArgumentException(GetUnsupportedExpressionMessage(expression.Body), nameof(expression));
            }
        }

        // If any members were accessed in the expression, the first one found is the last member.
        Type declaringType = declaringTypes.FirstOrDefault() ?? typeof(TDeclaringType);

        IEnumerable<string> reversedSegments = segments.AsEnumerable().Reverse();
        string segmentPath = string.Join(".", reversedSegments);

        return new MemberPath(typeof(TDeclaringType), declaringType, segmentPath.Replace(".[", "[", StringComparison.Ordinal));
    }

    /// <summary>
    /// Validates that the expression can be used to construct a <see cref="MemberPath"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="expression"/> is <see langword="null"/>.</exception>
    public static void ValidateMemberPath<TDeclaringType, TPropertyType>(
        this Expression<Func<TDeclaringType, TPropertyType>> expression)
    {
        Guard.ThrowIfArgumentIsNull(expression, nameof(expression), "Expected an expression, but found <null>.");

        Expression node = expression;

        while (node is not null)
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

                    break;

                case ExpressionType.ArrayIndex:
                    var binaryExpression = (BinaryExpression)node;
                    node = binaryExpression.Left;

                    break;

                case ExpressionType.Parameter:
                    node = null;
                    break;

                case ExpressionType.Call:
                    var methodCallExpression = (MethodCallExpression)node;

                    if (methodCallExpression is not { Method.Name: "get_Item", Arguments: [ConstantExpression] })
                    {
                        throw new ArgumentException(GetUnsupportedExpressionMessage(expression.Body), nameof(expression));
                    }

                    node = methodCallExpression.Object;
                    break;

                default:
                    throw new ArgumentException(GetUnsupportedExpressionMessage(expression.Body), nameof(expression));
            }
        }
    }

    private static string GetUnsupportedExpressionMessage(Expression expression) =>
        $"Expression <{expression}> cannot be used to select a member.";
}
