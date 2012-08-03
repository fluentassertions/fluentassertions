using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;

namespace FluentAssertions.Structural
{
    /// <summary>
    /// Is responsible for the exact run-time behavior of a structural equality comparison.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
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

            WhenTypeIs<string>().Use(
                ctx => ctx.Subject.Should().Be(ctx.Expectation, ctx.Reason, ctx.ReasonArgs));

            WhenTypeIs<DateTime>().Use(
                ctx => ctx.Subject.Should().Be(ctx.Expectation, ctx.Reason, ctx.ReasonArgs));
        }

        /// <summary>
        /// Gets a configuration that compares all declared properties of the subject with equally named properties of the expectation,
        /// and includes the entire object graph. The names of the properties between the subject and expectation must match.
        /// </summary>
        public static StructuralEqualityConfiguration<TSubject> Default
        {
            get
            {
                var config = new StructuralEqualityConfiguration<TSubject>();
                config.Recurse();
                config.IncludeAllDeclaredProperties();

                return config;
            }
        }

        /// <summary>
        /// Gets a configuration that by default doesn't include any of the subject's properties and doesn't consider any nested objects
        /// or collections.
        /// </summary>
        public static StructuralEqualityConfiguration<TSubject> Empty
        {
            get { return new StructuralEqualityConfiguration<TSubject>(); }
        }

        /// <summary>
        /// Gets an ordered collection of selection rules that define what properties are included.
        /// </summary>
        public IEnumerable<ISelectionRule> SelectionRules
        {
            get { return selectionRules; }
        }

        /// <summary>
        /// Gets an ordered collection of matching rules that determine which subject properties are matched with which
        /// expectation properties.
        /// </summary>
        public IEnumerable<IMatchingRule> MatchingRules
        {
            get { return matchingRules; }
        }

        /// <summary>
        /// Gets an ordered collection of assertion rules that determine how subject properties are compared for equality with
        /// expectation properties.
        /// </summary>
        public IEnumerable<IAssertionRule> AssertionRules
        {
            get { return assertionRules; }
        }

        /// <summary>
        /// Gets value indicating whether the equality check will include nested collections and complex types.
        /// </summary>
        public bool IsRecursive { get; private set; }

        /// <summary>
        /// Gets value indicating how cyclic references should be handled. By default, it will throw an exception.
        /// </summary>
        public CyclicReferenceHandling CyclicReferenceHandling
        {
            get { return cyclicReferenceHandling; }
        }

        /// <summary>
        /// Adds all public properties of the subject as far as they are defined on the declared type. 
        /// </summary>
        public StructuralEqualityConfiguration<TSubject> IncludeAllDeclaredProperties()
        {
            ClearAllSelectionRules();
            AddRule(new AllDeclaredPublicPropertiesSelectionRule());
            return this;
        }

        /// <summary>
        /// Adds all public properties of the subject based on its run-time type rather than its declared type.
        /// </summary>
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

        /// <summary>
        /// Allows overriding the way structural equality is applied to (nested) objects of tyoe <typeparamref name="TSubjectType"/>
        /// </summary>
        public Restriction<TSubjectType> WhenTypeIs<TSubjectType>()
        {
            return new Restriction<TSubjectType>(info => info.RuntimeType.IsSameOrInherits(typeof(TSubjectType)), this);
        }

        /// <summary>
        /// Allows overriding the way structural equality is applied to particular properties.
        /// </summary>
        /// <param name="predicate">
        /// A predicate based on the <see cref="ISubjectInfo"/> of the subject that is used to identify the property for which the
        /// override applies.
        /// </param>
        public Restriction When(Func<ISubjectInfo, bool> predicate)
        {
            return new Restriction(predicate, this);
        }

        /// <summary>
        /// Causes the structural equality check to include nested collections and complex types.
        /// </summary>
        public StructuralEqualityConfiguration<TSubject> Recurse()
        {
            IsRecursive = true;
            return this;
        }

        /// <summary>
        /// Causes the structural equality check to ignore any cyclic references.
        /// </summary>
        /// <remarks>
        /// By default, cyclic references within the object graph will cause an exception to be thrown.
        /// </remarks>
        public StructuralEqualityConfiguration<TSubject> IgnoreCyclicReferences()
        {
            cyclicReferenceHandling = CyclicReferenceHandling.Ignore;
            return this;
        }

        private void RemoveSelectionRule<T>() where T : ISelectionRule
        {
            foreach (T selectionRule in selectionRules.OfType<T>().ToArray())
            {
                selectionRules.Remove(selectionRule);
            }
        }

        /// <summary>
        /// Clears all selection rules, including those that were added by default.
        /// </summary>
        public void ClearAllSelectionRules()
        {
            selectionRules.Clear();
        }

        /// <summary>
        /// Clears all matching rules, including those that were added by default.
        /// </summary>
        public void ClearAllMatchingRules()
        {
            matchingRules.Clear();
        }

        /// <summary>
        /// Adds a selection rule to the ones allready added by default and which is evaluated after all existing rules.
        /// </summary>
        public StructuralEqualityConfiguration<TSubject> AddRule(ISelectionRule selectionRule)
        {
            selectionRules.Add(selectionRule);
            return this;
        }

        /// <summary>
        /// Adds a matching rule to the ones allready added by default and which is evaluated before all existing rules.
        /// </summary>
        public StructuralEqualityConfiguration<TSubject> AddRule(IMatchingRule matchingRule)
        {
            matchingRules.Insert(0, matchingRule);
            return this;
        }

        /// <summary>
        /// Adds a matching rule to the ones allready added by default and which is evaluated before all existing rules
        /// </summary>
        public StructuralEqualityConfiguration<TSubject> AddRule(IAssertionRule assertionRule)
        {
            assertionRules.Insert(0, assertionRule);
            return this;
        }

        /// <summary>
        /// Defines additional overrides when used with <see cref="StructuralEqualityConfiguration{TSubject}.When"/>
        /// </summary>
        public class Restriction
        {
            private readonly Func<ISubjectInfo, bool> predicate;
            private readonly StructuralEqualityConfiguration<TSubject> config;

            internal Restriction(Func<ISubjectInfo, bool> predicate, StructuralEqualityConfiguration<TSubject> config)
            {
                this.predicate = predicate;
                this.config = config;
            }

            /// <param name="action">
            /// The assertion to execute for the predicate.
            /// </param>
            public StructuralEqualityConfiguration<TSubject> Use<T>(Action<IAssertionContext<T>> action)
            {
                config.assertionRules.Insert(0, new AssertionRule<T>(predicate, action));
                return config;
            }
        }

        /// <summary>
        /// Defines additional overrides when used with <see cref="StructuralEqualityConfiguration{TSubject}.When"/>
        /// </summary>
        public class Restriction<T>
        {
            private readonly Func<ISubjectInfo, bool> predicate;
            private readonly StructuralEqualityConfiguration<TSubject> config;

            internal Restriction(Func<ISubjectInfo, bool> predicate, StructuralEqualityConfiguration<TSubject> config)
            {
                this.predicate = predicate;
                this.config = config;
            }

            /// <param name="action">
            /// The assertion to execute for the predicate.
            /// </param>
            public StructuralEqualityConfiguration<TSubject> Use(Action<IAssertionContext<T>> action)
            {
                config.assertionRules.Insert(0, new AssertionRule<T>(predicate, action));
                return config;
            }
        }
    }
}