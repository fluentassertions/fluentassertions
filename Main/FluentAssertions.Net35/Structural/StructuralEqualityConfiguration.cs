using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;

namespace FluentAssertions.Structural
{
    public class StructuralEqualityConfiguration<TSubject> : IStructuralEqualityConfiguration
    {
        #region Private Definitions

        private readonly List<ISelectionRule> selectionRules = new List<ISelectionRule>();
        private readonly List<IMatchingRule> matchingRules = new List<IMatchingRule>();
        private readonly List<IAssertionRule> assertionRules = new List<IAssertionRule>();
        private CyclicReferenceHandling cyclicReferenceHandling = CyclicReferenceHandling.ThrowException;

        #endregion

        private StructuralEqualityConfiguration()
        {
            AddRule(new MustMatchByNameRule());

            OverrideAssertionFor<string>(
                ctx => ctx.Subject.Should().Be(ctx.Expectation, ctx.Reason, ctx.ReasonArgs));

            OverrideAssertionFor<DateTime>(
                ctx => ctx.Subject.Should().Be(ctx.Expectation, ctx.Reason, ctx.ReasonArgs));
        }

        public static StructuralEqualityConfiguration<TSubject> Default
        {
            get
            {
                var config = new StructuralEqualityConfiguration<TSubject>();
                config.Recursive();
                config.IncludeAllDeclaredProperties();

                return config;
            }
        }

        public static StructuralEqualityConfiguration<TSubject> Empty
        {
            get { return new StructuralEqualityConfiguration<TSubject>(); }
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

        public StructuralEqualityConfiguration<TSubject> IncludeAllDeclaredProperties()
        {
            ClearAllSelectionRules();
            AddRule(new AllDeclaredPublicPropertiesSelectionRule());
            return this;
        }

        public StructuralEqualityConfiguration<TSubject> IncludeAllRuntimeProperties()
        {
            ClearAllSelectionRules();
            AddRule(new AllRuntimePublicPropertiesSelectionRule());
            return this;
        }

        /// <summary>
        /// Tries to match the properties of the subject with equally named properties on the expectation. Ignores those 
        /// properties that don't exist on the expectation.
        /// </summary>
        public StructuralEqualityConfiguration<TSubject> TryMatchByName()
        {
            ClearAllMatchingRules();
            matchingRules.Add(new TryMatchByNameRule());
            return this;
        }

        /// <summary>
        /// Requires the expectation to have properties which are equally named to properties on the subject.
        /// </summary>
        /// <returns></returns>
        public StructuralEqualityConfiguration<TSubject> MustMatchByName()
        {
            ClearAllMatchingRules();
            matchingRules.Add(new MustMatchByNameRule());
            return this;
        }

        public StructuralEqualityConfiguration<TSubject> Recursive()
        {
            Recurse = true;
            return this;
        }

        public StructuralEqualityConfiguration<TSubject> IgnoreCyclicReferences()
        {
            cyclicReferenceHandling = CyclicReferenceHandling.Ignore;
            return this;
        }

        /// <summary>
        /// Excludes the specified (nested) property from the structural equality check.
        /// </summary>
        public StructuralEqualityConfiguration<TSubject> Exclude(Expression<Func<TSubject, object>> propertyExpression)
        {
            AddRule(new ExcludePropertyByPathSelectionRule(propertyExpression.GetPropertyPath()));
            return this;
        }

        /// <summary>
        /// Excludes a (nested) property based on a predicate from the structural equality check.
        /// </summary>
        public StructuralEqualityConfiguration<TSubject> Exclude(Func<ISubjectInfo, bool> predicate)
        {
            AddRule(new ExcludePropertyByPredicateSelectionRule(predicate));
            return this;
        }

        /// <summary>
        /// Includes the specified property in the equality check.
        /// </summary>
        /// <remarks>
        /// This overrides the default behavior of including all declared properties.
        /// </remarks>
        public StructuralEqualityConfiguration<TSubject> Include(Expression<Func<TSubject, object>> propertyExpression)
        {
            RemoveSelectionRule<AllDeclaredPublicPropertiesSelectionRule>();
            RemoveSelectionRule<AllRuntimePublicPropertiesSelectionRule>();

            AddRule(new IncludePropertySelectionRule(propertyExpression.GetPropertyInfo()));
            return this;
        }

        private void RemoveSelectionRule<T>() where T : ISelectionRule
        {
            foreach (T selectionRule in selectionRules.OfType<T>().ToArray())
            {
                selectionRules.Remove(selectionRule);
            }
        }

        public StructuralEqualityConfiguration<TSubject> OverrideAssertionFor<TPropertyType>(
            Action<IAssertionContext<TPropertyType>> action)
        {
            assertionRules.Insert(0, new AssertionRule<TPropertyType>(
                info => info.RuntimeType.IsSameOrInherits(typeof (TPropertyType)), action));

            return this;
        }

        /// <summary>
        /// Overrides the way a particular subject of type <typeparamref name="TSubject"/> is compared with 
        /// a matching property on the expectation object.
        /// </summary>
        /// <typeparam name="TPropertyType">
        /// The type of the subject (property).
        /// </typeparam>
        /// <param name="predicate">
        /// A predicate based on the <see cref="ISubjectInfo"/> of the subject that determines whether this override applies or not.
        /// </param>
        /// <param name="action">
        /// The assertion to execute if the <paramref name="predicate"/> matches.
        /// </param>
        public StructuralEqualityConfiguration<TSubject> OverrideAssertion<TPropertyType>(Func<ISubjectInfo, bool> predicate,
            Action<IAssertionContext<TPropertyType>> action)
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

        public StructuralEqualityConfiguration<TSubject> AddRule(ISelectionRule selectionRule)
        {
            selectionRules.Add(selectionRule);
            return this;
        }

        public StructuralEqualityConfiguration<TSubject> AddRule(IMatchingRule matchingRule)
        {
            matchingRules.Add(matchingRule);
            return this;
        }
    }
}