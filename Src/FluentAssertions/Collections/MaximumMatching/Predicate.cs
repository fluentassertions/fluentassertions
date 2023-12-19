using System;
using System.Linq.Expressions;
using FluentAssertionsAsync.Formatting;

namespace FluentAssertionsAsync.Collections.MaximumMatching;

/// <summary>
/// Stores a predicate's expression and index in the maximum matching problem.
/// </summary>
/// <typeparam name="TValue">The type of the element values in the maximum matching problems.</typeparam>
internal class Predicate<TValue>
{
    private readonly Func<TValue, bool> compiledExpression;

    public Predicate(Expression<Func<TValue, bool>> expression, int index)
    {
        Index = index;
        Expression = expression;
        compiledExpression = expression.Compile();
    }

    /// <summary>
    /// The index of the predicate in the maximum matching problem.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// The expression of the predicate.
    /// </summary>
    public Expression<Func<TValue, bool>> Expression { get; }

    /// <summary>
    /// Determines whether the predicate matches the specified element.
    /// </summary>
    public bool Matches(TValue element) => compiledExpression(element);

    public override string ToString() => $"Index: {Index}, Expression: {Formatter.ToString(Expression)}";
}
