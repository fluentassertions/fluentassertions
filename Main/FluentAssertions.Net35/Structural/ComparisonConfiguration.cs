using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
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
                    ctx => ctx.Subject.Should().Be(ctx.Expectation, ctx.Reason, ctx.ReasonArgs));
                
                config.OverrideAssertionFor<DateTime>(
                    ctx => ctx.Subject.Should().Be(ctx.Expectation, ctx.Reason, ctx.ReasonArgs));

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

        public void OverrideAssertionFor<TSubject>(Action<AssertionContext<TSubject>> action)
        {
            assertionRules.Insert(0, new AssertionRule<TSubject>(
                pi => pi.PropertyType.IsSameOrInherits(typeof (TSubject)), action));
        }

        public void OverrideAssertion<TSubject>(Func<PropertyInfo, bool> predicate, Action<AssertionContext<TSubject>> action)
        {
            assertionRules.Insert(0, new AssertionRule<TSubject>(predicate, action));
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