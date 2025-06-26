using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentAssertions.Common;

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
            throw new ArgumentException($"Cannot use <{expression.Body}> when a property expression is expected.",
                nameof(expression));
        }

        return propertyInfo;
    }

    private static MemberInfo AttemptToGetMemberInfoFromExpression<T, TValue>(Expression<Func<T, TValue>> expression) =>
        (((expression.Body as UnaryExpression)?.Operand ?? expression.Body) as MemberExpression)?.Member;

    /// <summary>
    /// Gets one or more dotted paths of property names representing the property expression, including the declaring type.
    /// </summary>
    /// <example>
    /// E.g. ["Parent.Child.Sibling.Name"] or ["A.Dotted.Path1", "A.Dotted.Path2"].
    /// </example>
    /// <exception cref="ArgumentNullException"><paramref name="expression"/> is <see langword="null"/>.</exception>
    [SuppressMessage("Maintainability", "AV1500:Member or local function contains too many statements")]
    public static IEnumerable<MemberPath> GetMemberPaths<TDeclaringType, TPropertyType>(
        this Expression<Func<TDeclaringType, TPropertyType>> expression)
    {
        Guard.ThrowIfArgumentIsNull(expression, nameof(expression), "Expected an expression, but found <null>.");

        string singlePath = null;
        List<string> selectors = [];
        List<Type> declaringTypes = [];
        Expression node = expression;

        while (node is not null)
        {
#pragma warning disable IDE0010 // System.Linq.Expressions.ExpressionType has many members we do not care about
            switch (node.NodeType)
#pragma warning restore IDE0010
            {
                case ExpressionType.Lambda:
                {
                    node = ((LambdaExpression)node).Body;
                    break;
                }

                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                {
                    var unaryExpression = (UnaryExpression)node;
                    node = unaryExpression.Operand;
                    break;
                }

                case ExpressionType.MemberAccess:
                {
                    var memberExpression = (MemberExpression)node;
                    node = memberExpression.Expression;

                    singlePath = $"{memberExpression.Member.Name}.{singlePath}";
                    declaringTypes.Add(memberExpression.Member.DeclaringType);
                    break;
                }

                case ExpressionType.ArrayIndex:
                {
                    var binaryExpression = (BinaryExpression)node;
                    var indexExpression = (ConstantExpression)binaryExpression.Right;
                    node = binaryExpression.Left;

                    singlePath = $"[{indexExpression.Value}].{singlePath}";
                    break;
                }

                case ExpressionType.Parameter:
                {
                    node = null;
                    break;
                }

                case ExpressionType.Call:
                {
                    var methodCallExpression = (MethodCallExpression)node;

                    if (methodCallExpression is not
                        { Method.Name: "get_Item", Arguments: [ConstantExpression argumentExpression] })
                    {
                        throw new ArgumentException(GetUnsupportedExpressionMessage(expression.Body), nameof(expression));
                    }

                    node = methodCallExpression.Object;
                    singlePath = $"[{argumentExpression.Value}].{singlePath}";
                    break;
                }

                case ExpressionType.New:
                {
                    var newExpression = (NewExpression)node;

                    foreach (Expression member in newExpression.Arguments)
                    {
                        var expr = member.ToString();
                        selectors.Add(expr[expr.IndexOf('.', StringComparison.Ordinal)..]);
                        declaringTypes.Add(((MemberExpression)member).Member.DeclaringType);
                    }

                    node = null;
                    break;
                }

                default:
                {
                    throw new ArgumentException(GetUnsupportedExpressionMessage(expression.Body), nameof(expression));
                }
            }
        }

        Type declaringType = declaringTypes.FirstOrDefault() ?? typeof(TDeclaringType);

        if (singlePath is null)
        {
#if NET47 || NETSTANDARD2_0
            return selectors.Select(selector =>
                GetNewInstance<TDeclaringType>(declaringType, selector)).ToList();
#else
            return selectors.ConvertAll(selector =>
                GetNewInstance<TDeclaringType>(declaringType, selector));
#endif
        }

        return [GetNewInstance<TDeclaringType>(declaringType, singlePath)];
    }

    private static MemberPath GetNewInstance<TReflectedType>(Type declaringType, string dottedPath) =>
        new(typeof(TReflectedType), declaringType, dottedPath.Trim('.').Replace(".[", "[", StringComparison.Ordinal));

    /// <summary>
    /// Gets the first dotted path of property names collected by <see cref="GetMemberPaths{TDeclaringType,TPropertyType}"/>
    /// from a given property expression, including the declaring type.
    /// </summary>
    /// <example>
    /// E.g. Parent.Child.Sibling.Name.
    /// </example>
    /// <exception cref="ArgumentNullException"><paramref name="expression"/> is <see langword="null"/>.</exception>
    public static MemberPath GetMemberPath<TDeclaringType, TPropertyType>(
        this Expression<Func<TDeclaringType, TPropertyType>> expression)
    {
        return expression.GetMemberPaths().FirstOrDefault() ?? new MemberPath("");
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
                {
                    node = ((LambdaExpression)node).Body;
                    break;
                }

                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                {
                    var unaryExpression = (UnaryExpression)node;
                    node = unaryExpression.Operand;
                    break;
                }

                case ExpressionType.MemberAccess:
                {
                    var memberExpression = (MemberExpression)node;
                    node = memberExpression.Expression;

                    break;
                }

                case ExpressionType.ArrayIndex:
                {
                    var binaryExpression = (BinaryExpression)node;
                    node = binaryExpression.Left;

                    break;
                }

                case ExpressionType.Parameter:
                {
                    node = null;
                    break;
                }

                case ExpressionType.Call:
                {
                    var methodCallExpression = (MethodCallExpression)node;

                    if (methodCallExpression is not { Method.Name: "get_Item", Arguments: [ConstantExpression] })
                    {
                        throw new ArgumentException(GetUnsupportedExpressionMessage(expression.Body), nameof(expression));
                    }

                    node = methodCallExpression.Object;
                    break;
                }

                default:
                {
                    throw new ArgumentException(GetUnsupportedExpressionMessage(expression.Body), nameof(expression));
                }
            }
        }
    }

    private static string GetUnsupportedExpressionMessage(Expression expression) =>
        $"Expression <{expression}> cannot be used to select a member.";
}
