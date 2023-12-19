using System;
using System.Linq.Expressions;

namespace FluentAssertionsAsync.Equivalency.Ordering;

internal class PredicateBasedOrderingRule : IOrderingRule
{
    private readonly Func<IObjectInfo, bool> predicate;
    private readonly string description;

    public PredicateBasedOrderingRule(Expression<Func<IObjectInfo, bool>> predicate)
    {
        description = predicate.Body.ToString();
        this.predicate = predicate.Compile();
    }

    public bool Invert { get; init; }

    public OrderStrictness Evaluate(IObjectInfo objectInfo)
    {
        if (predicate(objectInfo))
        {
            return Invert ? OrderStrictness.NotStrict : OrderStrictness.Strict;
        }

        return OrderStrictness.Irrelevant;
    }

    public override string ToString()
    {
        return $"Be {(Invert ? "not strict" : "strict")} about the order of collections when {description}";
    }
}
