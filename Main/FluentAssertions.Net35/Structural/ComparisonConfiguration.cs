using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions.Common;

namespace FluentAssertions.Structural
{
    public class ComparisonConfiguration
    {
        private readonly List<ISelectionRule> selectionRules = new List<ISelectionRule>();
        private readonly List<IMatchingRule> matchingRules = new List<IMatchingRule>();
        private readonly List<IAssertionRule> assertionRules = new List<IAssertionRule>();
        private CyclicReferenceHandling cyclicReferenceHandling = CyclicReferenceHandling.ThrowException;

        public List<ISelectionRule> SelectionRules
        {
            get { return selectionRules; }
        }

        public List<IMatchingRule> MatchingRules
        {
            get { return matchingRules; }
        }

        public List<IAssertionRule> AssertionRules
        {
            get { return assertionRules; }
        }

        public bool Recurse { get; set; }

        public static ComparisonConfiguration Default
        {
            get
            {
                var config = new ComparisonConfiguration();
                config.AddRule(new MustMatchByNameRule());

                return config;
            }
        }

        public CyclicReferenceHandling CyclicReferenceHandling
        {
            get { return cyclicReferenceHandling; }
        }

        public void IncludeAllDeclaredProperties()
        {
            ClearAllSelectionRules();
            AddRule(new AllDeclaredPublicPropertiesSelectionRule());
        }

        private void ClearAllSelectionRules()
        {
            selectionRules.Clear();
        }

        public void IncludeAllRuntimeProperties()
        {
            ClearAllSelectionRules();
            AddRule(new AllRuntimePublicPropertiesSelectionRule());
        }

        public void TryMatchByName()
        {
            ClearAllMatchingRules();
            matchingRules.Add(new TryMatchByNameRule());
        }

        private void ClearAllMatchingRules()
        {
            matchingRules.Clear();
        }

        public void Recursive()
        {
            Recurse = true;
        }

        public void IgnoreCyclicReferences()
        {
            cyclicReferenceHandling = CyclicReferenceHandling.Ignore;
        }

        public void Ignore<T>(Expression<Func<T, object>> propertyExpression)
        {
            AddRule(new IgnorePropertySelectionRule(propertyExpression.GetPropertyInfo()));
        }

        public void Include<T>(Expression<Func<T, object>> propertyExpression)
        {
            AddRule(new IncludePropertySelectionRule(propertyExpression.GetPropertyInfo()));
        }

        public void AddRule(ISelectionRule selectionRule)
        {
            selectionRules.Add(selectionRule);
        }

        public void AddRule(IMatchingRule matchingRule)
        {
            matchingRules.Add(matchingRule);
        }
    }
}