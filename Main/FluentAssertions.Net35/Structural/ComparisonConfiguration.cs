using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions.Common;

namespace FluentAssertions.Structural
{
    public class ComparisonConfiguration
    {
        #region Private Definitions

        private readonly List<ISelectionRule> selectionRules = new List<ISelectionRule>();
        private readonly List<IMatchingRule> matchingRules = new List<IMatchingRule>();
        private readonly List<IAssertionRule> assertionRules = new List<IAssertionRule>();
        private CyclicReferenceHandling cyclicReferenceHandling = CyclicReferenceHandling.ThrowException;

        #endregion

        public static ComparisonConfiguration Default
        {
            get
            {
                var config = new ComparisonConfiguration();

                config.AddRule(new MustMatchByNameRule());

                config.OverrideAssertionFor<string>(
                    ctx => ((string)ctx.Subject).Should().Be(ctx.Expectation.ToString(), ctx.Reason, ctx.ReasonArgs));

                return config;
            }
        }

        internal IEnumerable<ISelectionRule> SelectionRules
        {
            get { return selectionRules; }
        }

        internal IEnumerable<IMatchingRule> MatchingRules
        {
            get { return matchingRules; }
        }

        internal IEnumerable<IAssertionRule> AssertionRules
        {
            get { return assertionRules; }
        }

        public bool Recurse { get; set; }

        public CyclicReferenceHandling CyclicReferenceHandling
        {
            get { return cyclicReferenceHandling; }
        }

        public void IncludeAllDeclaredProperties()
        {
            ClearAllSelectionRules();
            AddRule(new AllDeclaredPublicPropertiesSelectionRule());
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
        
        public void MustMatchByName()
        {
            ClearAllMatchingRules();
            matchingRules.Add(new MustMatchByNameRule());
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

        private void OverrideAssertionFor<TSubject>(Action<AssertionContext> action)
        {
            assertionRules.Add(new AssertionRule(pi => pi.PropertyType.IsSameOrInherits(typeof (TSubject)), action));
        }

        private void ClearAllSelectionRules()
        {
            selectionRules.Clear();
        }

        private void ClearAllMatchingRules()
        {
            matchingRules.Clear();
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