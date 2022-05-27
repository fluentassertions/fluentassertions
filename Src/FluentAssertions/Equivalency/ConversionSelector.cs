using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FluentAssertions.Common;
using FluentAssertions.Equivalency.Execution;
using FluentAssertions.Equivalency.Steps;

namespace FluentAssertions.Equivalency;

/// <summary>
/// Collects the members that need to be converted by the <see cref="AutoConversionStep"/>.
/// </summary>
public class ConversionSelector
{
    private class ConversionSelectorRule
    {
        public Func<IObjectInfo, bool> Predicate { get; }

        public string Description { get; }

        public ConversionSelectorRule(Func<IObjectInfo, bool> predicate, string description)
        {
            Predicate = predicate;
            Description = description;
        }
    }

    private readonly List<ConversionSelectorRule> inclusions;
    private readonly List<ConversionSelectorRule> exclusions;

    public ConversionSelector()
        : this(new List<ConversionSelectorRule>(), new List<ConversionSelectorRule>())
    {
    }

    private ConversionSelector(List<ConversionSelectorRule> inclusions, List<ConversionSelectorRule> exclusions)
    {
        this.inclusions = inclusions;
        this.exclusions = exclusions;
    }

    public void IncludeAll()
    {
        inclusions.Add(new ConversionSelectorRule(_ => true, "Try conversion of all members. "));
    }

    public void Include(Expression<Func<IObjectInfo, bool>> predicate)
    {
        Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate));

        inclusions.Add(new ConversionSelectorRule(
            predicate.Compile(),
            $"Try conversion of member {predicate.Body}. "));
    }

    public void Exclude(Expression<Func<IObjectInfo, bool>> predicate)
    {
        Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate));

        exclusions.Add(new ConversionSelectorRule(
            predicate.Compile(),
            $"Do not convert member {predicate.Body}."));
    }

    public bool RequiresConversion(Comparands comparands, INode currentNode)
    {
        if (inclusions.Count == 0)
        {
            return false;
        }

        var objectInfo = new ObjectInfo(comparands, currentNode);

        return inclusions.Any(p => p.Predicate(objectInfo)) && !exclusions.Any(p => p.Predicate(objectInfo));
    }

    public override string ToString()
    {
        if (inclusions.Count == 0 && exclusions.Count == 0)
        {
            return "Without automatic conversion.";
        }

        var descriptionBuilder = new StringBuilder();

        foreach (ConversionSelectorRule inclusion in inclusions)
        {
            descriptionBuilder.Append(inclusion.Description);
        }

        foreach (ConversionSelectorRule exclusion in exclusions)
        {
            descriptionBuilder.Append(exclusion.Description);
        }

        return descriptionBuilder.ToString();
    }

    public ConversionSelector Clone()
    {
        return new ConversionSelector(new List<ConversionSelectorRule>(inclusions), new List<ConversionSelectorRule>(exclusions));
    }
}
