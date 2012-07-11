using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Common;
using System.Linq;

namespace FluentAssertions.Structural
{
    public interface IComparisonConfiguration
    {
        IEnumerable<ISelectionRule> SelectionRules { get; }
        IEnumerable<IMatchingRule> MatchingRules { get; }
        IEnumerable<IAssertionRule> AssertionRules { get; }
        bool Recurse { get; set; }
        CyclicReferenceHandling CyclicReferenceHandling { get; }
    }

    public class ComparisonConfiguration<TSubject> : IComparisonConfiguration
    {
        #region Private Definitions

        private readonly List<ISelectionRule> selectionRules = new List<ISelectionRule>();
        private readonly List<IMatchingRule> matchingRules = new List<IMatchingRule>();
        private readonly List<IAssertionRule> assertionRules = new List<IAssertionRule>();
        private CyclicReferenceHandling cyclicReferenceHandling = CyclicReferenceHandling.ThrowException;

        #endregion

        private ComparisonConfiguration()
        {
            AddRule(new MustMatchByNameRule());

            OverrideAssertionFor<string>(
                ctx => ctx.Subject.Should().Be(ctx.Expectation, ctx.Reason, ctx.ReasonArgs));

            OverrideAssertionFor<DateTime>(
                ctx => ctx.Subject.Should().Be(ctx.Expectation, ctx.Reason, ctx.ReasonArgs));
        }

        public static ComparisonConfiguration<TSubject> Default
        {
            get
            {
                var config = new ComparisonConfiguration<TSubject>();
                config.Recursive();
                config.IncludeAllDeclaredProperties();

                return config;
            }
        }
        
        public static ComparisonConfiguration<TSubject> Empty
        {
            get
            {
                return new ComparisonConfiguration<TSubject>();
            }
        }

        public IEnumerable<ISelectionRule> SelectionRules
        {
            get { return selectionRules; }
        }

        public IEnumerable<IMatchingRule> MatchingRules
        {
            get { return matchingRules; }
        }

        public IEnumerable<IAssertionRule> AssertionRules
        {
            get { return assertionRules; }
        }

        public bool Recurse { get; set; }

        public CyclicReferenceHandling CyclicReferenceHandling
        {
            get { return cyclicReferenceHandling; }
        }

        public ComparisonConfiguration<TSubject> IncludeAllDeclaredProperties()
        {
            ClearAllSelectionRules();
            AddRule(new AllDeclaredPublicPropertiesSelectionRule());
            return this;
        }

        public ComparisonConfiguration<TSubject> IncludeAllRuntimeProperties()
        {
            ClearAllSelectionRules();
            AddRule(new AllRuntimePublicPropertiesSelectionRule());
            return this;
        }

        /// <summary>
        /// Tries to match the properties of the subject with equally named properties on the expectation. Ignores those 
        /// properties that don't exist on the expectation.
        /// </summary>
        public ComparisonConfiguration<TSubject> TryMatchByName()
        {
            ClearAllMatchingRules();
            matchingRules.Add(new TryMatchByNameRule());
            return this;
        }

        /// <summary>
        /// Requires the expectation to have properties which are equally named to properties on the subject.
        /// </summary>
        /// <returns></returns>
        public ComparisonConfiguration<TSubject> MustMatchByName()
        {
            ClearAllMatchingRules();
            matchingRules.Add(new MustMatchByNameRule());
            return this;
        }

        public ComparisonConfiguration<TSubject> Recursive()
        {
            Recurse = true;
            return this;
        }

        public ComparisonConfiguration<TSubject> IgnoreCyclicReferences()
        {
            cyclicReferenceHandling = CyclicReferenceHandling.Ignore;
            return this;
        }

        /// <summary>
        /// Excludes the specified property from the equality assertion.
        /// </summary>
        public ComparisonConfiguration<TSubject> Exclude(Expression<Func<TSubject, object>> propertyExpression)
        {
            string propertyPath = propertyExpression.GetPropertyPath();

            AddRule(new IgnorePropertySelectionRule(propertyPath));
            return this;
        }

        /// <summary>
        /// Includes the specified property in the equality check.
        /// </summary>
        /// <remarks>
        /// This overrides the default behavior of including all declared properties.
        /// </remarks>
        public ComparisonConfiguration<TSubject> Include(Expression<Func<TSubject, object>> propertyExpression)
        {
            RemoveSelectionRule<AllDeclaredPublicPropertiesSelectionRule>();
            RemoveSelectionRule<AllRuntimePublicPropertiesSelectionRule>();

            AddRule(new IncludePropertySelectionRule(propertyExpression.GetPropertyInfo()));
            return this;
        }

        private void RemoveSelectionRule<T>() where T : ISelectionRule
        {
            foreach (var selectionRule in selectionRules.OfType<T>().ToArray())
            {
                selectionRules.Remove(selectionRule);
            }
        }

        public ComparisonConfiguration<TSubject> OverrideAssertionFor<TPropertyType>(Action<AssertionContext<TPropertyType>> action)
        {
            assertionRules.Insert(0, new AssertionRule<TPropertyType>(
                pi => pi.PropertyType.IsSameOrInherits(typeof(TPropertyType)), action));

            return this;
        }

        public ComparisonConfiguration<TSubject> OverrideAssertion<TPropertyType>(Func<PropertyInfo, bool> predicate, Action<AssertionContext<TPropertyType>> action)
        {
            assertionRules.Insert(0, new AssertionRule<TPropertyType>(predicate, action));
            return this;
        }

        public void ClearAllSelectionRules()
        {
            selectionRules.Clear();
        }

        public void ClearAllMatchingRules()
        {
            matchingRules.Clear();
        }

        public ComparisonConfiguration<TSubject> AddRule(ISelectionRule selectionRule)
        {
            selectionRules.Add(selectionRule);
            return this;
        }

        public ComparisonConfiguration<TSubject> AddRule(IMatchingRule matchingRule)
        {
            matchingRules.Add(matchingRule);
            return this;
        }
    }
}