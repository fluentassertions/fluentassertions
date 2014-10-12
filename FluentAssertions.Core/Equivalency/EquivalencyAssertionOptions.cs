using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Is responsible for the exact run-time behavior of a structural equality comparison.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    public class EquivalencyAssertionOptions<TSubject> : IEquivalencyAssertionOptions
    {
        #region Private Definitions

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<ISelectionRule> selectionRules = new List<ISelectionRule>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<IMatchingRule> matchingRules = new List<IMatchingRule>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<IAssertionRule> assertionRules = new List<IAssertionRule>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CyclicReferenceHandling cyclicReferenceHandling = CyclicReferenceHandling.ThrowException;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly OrderingRuleCollection orderingRules = new OrderingRuleCollection();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isRecursive;

        private bool allowInfiniteRecursion;

        private EnumEquivalencyHandling enumEquivalencyHandling;

        #endregion

        private EquivalencyAssertionOptions()
        {
            Using(new MustMatchByNameRule());

            orderingRules.Add(new ByteArrayOrderingRule());
        }

        /// <summary>
        /// Gets a configuration that by default doesn't include any of the subject's properties and doesn't consider any nested objects
        /// or collections.
        /// </summary>
        public static Func<EquivalencyAssertionOptions<TSubject>> Empty =
            () => new EquivalencyAssertionOptions<TSubject>();

        /// <summary>
        /// Gets a configuration that compares all declared properties of the subject with equally named properties of the expectation,
        /// and includes the entire object graph. The names of the properties between the subject and expectation must match.
        /// </summary>
        public static Func<EquivalencyAssertionOptions<TSubject>> Default = () =>
        {
            var config = new EquivalencyAssertionOptions<TSubject>();
            config.IncludingNestedObjects();
            config.IncludingAllDeclaredProperties();

            return config;
        };

        /// <summary>
        /// Gets an ordered collection of selection rules that define what properties are included.
        /// </summary>
        IEnumerable<ISelectionRule> IEquivalencyAssertionOptions.SelectionRules
        {
            get { return selectionRules; }
        }

        /// <summary>
        /// Gets an ordered collection of matching rules that determine which subject properties are matched with which
        /// expectation properties.
        /// </summary>
        IEnumerable<IMatchingRule> IEquivalencyAssertionOptions.MatchingRules
        {
            get { return matchingRules; }
        }

        /// <summary>
        /// Gets an ordered collection of assertion rules that determine how subject properties are compared for equality with
        /// expectation properties.
        /// </summary>
        IEnumerable<IAssertionRule> IEquivalencyAssertionOptions.AssertionRules
        {
            get { return assertionRules; }
        }

        /// <summary>
        /// Gets an ordered collection of rules that determine whether or not the order of collections is important. By default,
        /// ordering is irrelevant.
        /// </summary>
        OrderingRuleCollection IEquivalencyAssertionOptions.OrderingRules
        {
            get { return orderingRules; }
        }

        /// <summary>
        /// Gets value indicating whether the equality check will include nested collections and complex types.
        /// </summary>
        bool IEquivalencyAssertionOptions.IsRecursive
        {
            get { return isRecursive; }
        }

        bool IEquivalencyAssertionOptions.AllowInfiniteRecursion
        {
            get { return allowInfiniteRecursion; }
        }

        /// <summary>
        /// Gets value indicating how cyclic references should be handled. By default, it will throw an exception.
        /// </summary>
        CyclicReferenceHandling IEquivalencyAssertionOptions.CyclicReferenceHandling
        {
            get { return cyclicReferenceHandling; }
        }

        EnumEquivalencyHandling IEquivalencyAssertionOptions.EnumEquivalencyHandling
        {
            get { return enumEquivalencyHandling; }
        }

        /// <summary>
        /// Adds all public properties of the subject as far as they are defined on the declared type. 
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> IncludingAllDeclaredProperties()
        {
            WithoutSelectionRules();
            Using(new AllDeclaredPublicPropertiesSelectionRule());
            return this;
        }

        /// <summary>
        /// Adds all public properties of the subject based on its run-time type rather than its declared type.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> IncludingAllRuntimeProperties()
        {
            WithoutSelectionRules();
            Using(new AllRuntimePublicPropertiesSelectionRule());
            return this;
        }

        /// <summary>
        /// Tries to match the properties of the subject with equally named properties on the expectation. Ignores those 
        /// properties that don't exist on the expectation.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> ExcludingMissingProperties()
        {
            WithoutMatchingRules();
            matchingRules.Add(new TryMatchByNameRule());
            return this;
        }

        /// <summary>
        /// Requires the expectation to have properties which are equally named to properties on the subject.
        /// </summary>
        /// <returns></returns>
        public EquivalencyAssertionOptions<TSubject> ThrowingOnMissingProperties()
        {
            WithoutMatchingRules();
            matchingRules.Add(new MustMatchByNameRule());
            return this;
        }

        /// <summary>
        /// Excludes the specified (nested) property from the structural equality check.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> Excluding(Expression<Func<TSubject, object>> propertyExpression)
        {
            Using(new ExcludePropertyByPathSelectionRule(propertyExpression.GetPropertyPath()));
            return this;
        }

        /// <summary>
        /// Excludes a (nested) property based on a predicate from the structural equality check.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> Excluding(Expression<Func<ISubjectInfo, bool>> predicate)
        {
            Using(new ExcludePropertyByPredicateSelectionRule(predicate));
            return this;
        }

        /// <summary>
        /// Includes the specified property in the equality check.
        /// </summary>
        /// <remarks>
        /// This overrides the default behavior of including all declared properties.
        /// </remarks>
        public EquivalencyAssertionOptions<TSubject> Including(Expression<Func<TSubject, object>> propertyExpression)
        {
            RemoveSelectionRule<AllDeclaredPublicPropertiesSelectionRule>();
            RemoveSelectionRule<AllRuntimePublicPropertiesSelectionRule>();

            Using(new IncludePropertyByPathSelectionRule(propertyExpression.GetPropertyInfo()));
            return this;
        }

        /// <summary>
        /// Includes the specified property in the equality check.
        /// </summary>
        /// <remarks>
        /// This overrides the default behavior of including all declared properties.
        /// </remarks>
        public EquivalencyAssertionOptions<TSubject> Including(Expression<Func<ISubjectInfo, bool>> predicate)
        {
            RemoveSelectionRule<AllDeclaredPublicPropertiesSelectionRule>();
            RemoveSelectionRule<AllRuntimePublicPropertiesSelectionRule>();

            Using(new IncludePropertyByPredicateSelectionRule(predicate));
            return this;
        }

        /// <param name="action">
        /// The assertion to execute when the predicate is met.
        /// </param>
        public Restriction<TProperty> Using<TProperty>(Action<IAssertionContext<TProperty>> action)
        {
            return new Restriction<TProperty>(this, action);
        }

        /// <summary>
        /// Causes the structural equality check to include nested collections and complex types.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> IncludingNestedObjects()
        {
            isRecursive = true;
            return this;
        }

        /// <summary>
        /// Causes the structural equality check to exclude nested collections and complex types.
        /// </summary>
        /// <remarks>
        /// Behaves similarly to the old property assertions API.
        /// </remarks>
        public EquivalencyAssertionOptions<TSubject> ExcludingNestedObjects()
        {
            isRecursive = false;
            return this;
        }

        /// <summary>
        /// Causes the structural equality check to ignore any cyclic references.
        /// </summary>
        /// <remarks>
        /// By default, cyclic references within the object graph will cause an exception to be thrown.
        /// </remarks>
        public EquivalencyAssertionOptions<TSubject> IgnoringCyclicReferences()
        {
            cyclicReferenceHandling = CyclicReferenceHandling.Ignore;
            return this;
        }


        /// <summary>
        /// Disables limitations on recursion depth when the structural equality check is configured to include nested objects
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> AllowingInfiniteRecursion()
        {
            allowInfiniteRecursion = true;
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
        public void WithoutSelectionRules()
        {
            selectionRules.Clear();
        }

        /// <summary>
        /// Clears all matching rules, including those that were added by default.
        /// </summary>
        public void WithoutMatchingRules()
        {
            matchingRules.Clear();
        }

        /// <summary>
        /// Adds a selection rule to the ones already added by default, and which is evaluated after all existing rules.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> Using(ISelectionRule selectionRule)
        {
            selectionRules.Add(selectionRule);
            return this;
        }

        /// <summary>
        /// Adds a matching rule to the ones already added by default, and which is evaluated before all existing rules.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> Using(IMatchingRule matchingRule)
        {
            matchingRules.Insert(0, matchingRule);
            return this;
        }

        /// <summary>
        /// Adds a matching rule to the ones already added by default, and which is evaluated before all existing rules.
        /// NOTE: These matching rules do not apply to the root object.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> Using(IAssertionRule assertionRule)
        {
            assertionRules.Insert(0, assertionRule);
            return this;
        }

        /// <summary>
        /// Causes all collections to be compared in the order in which the items appear in the expectation.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> WithStrictOrdering()
        {
            orderingRules.Add(new MatchAllOrderingRule());
            return this;
        }

        /// <summary>
        /// Causes the collection identified by <paramref name="propertyExpression"/> to be compared in the order 
        /// in which the items appear in the expectation.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> WithStrictOrderingFor(Expression<Func<TSubject, object>> propertyExpression)
        {
            orderingRules.Add(new PropertyPathOrderingRule(propertyExpression.GetPropertyPath()));
            return this;
        }

        /// <summary>
        /// Causes the collection identified by the provided <paramref name="predicate"/> to be compared in the order 
        /// in which the items appear in the expectation.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> WithStrictOrderingFor(Expression<Func<ISubjectInfo, bool>> predicate)
        {
            orderingRules.Add(new PredicateBasedOrderingRule(predicate));
            return this;
        }

        /// <summary>
        /// Causes to compare Enum properties using the result of their ToString method.
        /// </summary>
        /// <remarks>
        /// By default, enums are compared by value.
        /// </remarks>
        public EquivalencyAssertionOptions<TSubject> ComparingEnumsByName()
        {
            enumEquivalencyHandling = EnumEquivalencyHandling.ByName;
            return this;
        }

        /// <summary>
        /// Causes to compare Enum properties using their underlying value only.
        /// </summary>
        /// <remarks>
        /// This is the default.
        /// </remarks>
        public EquivalencyAssertionOptions<TSubject> ComapringEnumsByValue()
        {
            enumEquivalencyHandling = EnumEquivalencyHandling.ByValue;
            return this;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var rule in selectionRules)
            {
                builder.AppendLine("- " + rule);
            }

            foreach (var rule in matchingRules)
            {
                builder.AppendLine("- " + rule);
            }

            foreach (var rule in assertionRules)
            {
                builder.AppendLine("- " + rule);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Defines additional overrides when used with <see cref="EquivalencyAssertionOptions{TSubject}.When"/>
        /// </summary>
        public class Restriction<TProperty>
        {
            private readonly Action<IAssertionContext<TProperty>> action;
            private readonly EquivalencyAssertionOptions<TSubject> options;

            public Restriction(EquivalencyAssertionOptions<TSubject> options, Action<IAssertionContext<TProperty>> action)
            {
                this.options = options;
                this.action = action;
            }

            /// <summary>
            /// Allows overriding the way structural equality is applied to (nested) objects of tyoe <typeparamref name="TPropertyType"/>
            /// </summary>
            public EquivalencyAssertionOptions<TSubject> WhenTypeIs<TPropertyType>()
            {
                When(info => info.RuntimeType.IsSameOrInherits(typeof(TPropertyType)));
                return options;
            }

            /// <summary>
            /// Allows overriding the way structural equality is applied to particular properties.
            /// </summary>
            /// <param name="predicate">
            /// A predicate based on the <see cref="ISubjectInfo"/> of the subject that is used to identify the property for which the
            /// override applies.
            /// </param>
            public EquivalencyAssertionOptions<TSubject> When(Expression<Func<ISubjectInfo, bool>> predicate)
            {
                options.Using(new AssertionRule<TProperty>(predicate, action));
                return options;
            }
        }
    }
}