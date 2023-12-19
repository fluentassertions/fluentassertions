using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertionsAsync.Equivalency.Ordering;
using FluentAssertionsAsync.Equivalency.Selection;
using FluentAssertionsAsync.Equivalency.Tracing;

namespace FluentAssertionsAsync.Equivalency.Execution;

/// <summary>
/// Ensures that all the rules remove the collection index from the path before processing it further.
/// </summary>
internal class CollectionMemberOptionsDecorator : IEquivalencyOptions
{
    private readonly IEquivalencyOptions inner;

    public CollectionMemberOptionsDecorator(IEquivalencyOptions inner)
    {
        this.inner = inner;
    }

    public IEnumerable<IMemberSelectionRule> SelectionRules
    {
        get
        {
            return inner.SelectionRules.Select(rule => new CollectionMemberSelectionRuleDecorator(rule)).ToArray();
        }
    }

    public IEnumerable<IMemberMatchingRule> MatchingRules
    {
        get { return inner.MatchingRules.ToArray(); }
    }

    public OrderingRuleCollection OrderingRules
    {
        get
        {
            return new OrderingRuleCollection(inner.OrderingRules.Select(rule =>
                new CollectionMemberOrderingRuleDecorator(rule)));
        }
    }

    public ConversionSelector ConversionSelector => inner.ConversionSelector;

    public IEnumerable<IEquivalencyStep> UserEquivalencySteps
    {
        get { return inner.UserEquivalencySteps; }
    }

    public bool IsRecursive => inner.IsRecursive;

    public bool AllowInfiniteRecursion => inner.AllowInfiniteRecursion;

    public CyclicReferenceHandling CyclicReferenceHandling => inner.CyclicReferenceHandling;

    public EnumEquivalencyHandling EnumEquivalencyHandling => inner.EnumEquivalencyHandling;

    public bool UseRuntimeTyping => inner.UseRuntimeTyping;

    public MemberVisibility IncludedProperties => inner.IncludedProperties;

    public MemberVisibility IncludedFields => inner.IncludedFields;

    public bool IgnoreNonBrowsableOnSubject => inner.IgnoreNonBrowsableOnSubject;

    public bool ExcludeNonBrowsableOnExpectation => inner.ExcludeNonBrowsableOnExpectation;

    public bool? CompareRecordsByValue => inner.CompareRecordsByValue;

    public EqualityStrategy GetEqualityStrategy(Type type)
    {
        return inner.GetEqualityStrategy(type);
    }

    public ITraceWriter TraceWriter => inner.TraceWriter;
}
