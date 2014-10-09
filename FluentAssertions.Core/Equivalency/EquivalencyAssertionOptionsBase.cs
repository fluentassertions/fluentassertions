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
    /// Represents the run-time behavior of a structural equivalency assertion.
    /// </summary>
    public abstract class EquivalencyAssertionOptionsBase<TSelf> : IEquivalencyAssertionOptions
        where TSelf : EquivalencyAssertionOptionsBase<TSelf>
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
        protected readonly OrderingRuleCollection orderingRules = new OrderingRuleCollection();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isRecursive;

        private bool allowInfiniteRecursion;

        #endregion

        internal EquivalencyAssertionOptionsBase()
        {
            Using(new MustMatchByNameRule());

            orderingRules.Add(new ByteArrayOrderingRule());
        }

        /// <summary>
        /// Creates an instance of the equivalency assertions options based on defaults previously configured by the caller.
        /// </summary>
        protected EquivalencyAssertionOptionsBase(IEquivalencyAssertionOptions defaults)
        {
            allowInfiniteRecursion = defaults.AllowInfiniteRecursion;
            isRecursive = defaults.IsRecursive;
            cyclicReferenceHandling = defaults.CyclicReferenceHandling;
            allowInfiniteRecursion = defaults.AllowInfiniteRecursion;

            selectionRules.AddRange(defaults.SelectionRules);
            matchingRules.AddRange(defaults.MatchingRules);
            assertionRules.AddRange(defaults.AssertionRules);
            orderingRules = new OrderingRuleCollection(defaults.OrderingRules);
        }

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

        /// <summary>
        /// Adds all public properties of the subject as far as they are defined on the declared type. 
        /// </summary>
        public TSelf IncludingAllDeclaredProperties()
        {
            WithoutSelectionRules();
            Using(new AllDeclaredPublicPropertiesSelectionRule());
            return (TSelf)this;
        }

        /// <summary>
        /// Adds all public properties of the subject based on its run-time type rather than its declared type.
        /// </summary>
        public TSelf IncludingAllRuntimeProperties()
        {
            WithoutSelectionRules();
            Using(new AllRuntimePublicPropertiesSelectionRule());
            return (TSelf)this;
        }

        /// <summary>
        /// Tries to match the properties of the subject with equally named properties on the expectation. Ignores those 
        /// properties that don't exist on the expectation.
        /// </summary>
        public TSelf ExcludingMissingProperties()
        {
            WithoutMatchingRules();
            matchingRules.Add(new TryMatchByNameRule());
            return (TSelf)this;
        }

        /// <summary>
        /// Requires the expectation to have properties which are equally named to properties on the subject.
        /// </summary>
        /// <returns></returns>
        public TSelf ThrowingOnMissingProperties()
        {
            WithoutMatchingRules();
            matchingRules.Add(new MustMatchByNameRule());
            return (TSelf)this;
        }

        
        /// <summary>
        /// Excludes a (nested) property based on a predicate from the structural equality check.
        /// </summary>
        public TSelf Excluding(Expression<Func<ISubjectInfo, bool>> predicate)
        {
            Using(new ExcludePropertyByPredicateSelectionRule(predicate));
            return (TSelf)this;
        }

        /// <param name="action">
        /// The assertion to execute when the predicate is met.
        /// </param>
        public Restriction<TProperty> Using<TProperty>(Action<IAssertionContext<TProperty>> action)
        {
            return new Restriction<TProperty>((TSelf)this, action);
        }

        /// <summary>
        /// Causes the structural equality check to include nested collections and complex types.
        /// </summary>
        public TSelf IncludingNestedObjects()
        {
            isRecursive = true;
            return (TSelf)this;
        }

        /// <summary>
        /// Causes the structural equality check to exclude nested collections and complex types.
        /// </summary>
        /// <remarks>
        /// Behaves similarly to the old property assertions API.
        /// </remarks>
        public TSelf ExcludingNestedObjects()
        {
            isRecursive = false;
            return (TSelf)this;
        }

        /// <summary>
        /// Causes the structural equality check to ignore any cyclic references.
        /// </summary>
        /// <remarks>
        /// By default, cyclic references within the object graph will cause an exception to be thrown.
        /// </remarks>
        public TSelf IgnoringCyclicReferences()
        {
            cyclicReferenceHandling = CyclicReferenceHandling.Ignore;
            return (TSelf)this;
        }


        /// <summary>
        /// Disables limitations on recursion depth when the structural equality check is configured to include nested objects
        /// </summary>
        public TSelf AllowingInfiniteRecursion()
        {
            allowInfiniteRecursion = true;
            return (TSelf)this;
        }

        protected void RemoveSelectionRule<T>() where T : ISelectionRule
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
        public TSelf Using(ISelectionRule selectionRule)
        {
            selectionRules.Add(selectionRule);
            return (TSelf)this;
        }

        /// <summary>
        /// Adds a matching rule to the ones already added by default, and which is evaluated before all existing rules.
        /// </summary>
        public TSelf Using(IMatchingRule matchingRule)
        {
            matchingRules.Insert(0, matchingRule);
            return (TSelf)this;
        }

        /// <summary>
        /// Adds a matching rule to the ones already added by default, and which is evaluated before all existing rules.
        /// NOTE: These matching rules do not apply to the root object.
        /// </summary>
        public TSelf Using(IAssertionRule assertionRule)
        {
            assertionRules.Insert(0, assertionRule);
            return (TSelf)this;
        }

        /// <summary>
        /// Causes all collections to be compared in the order in which the items appear in the expectation.
        /// </summary>
        public TSelf WithStrictOrdering()
        {
            orderingRules.Add(new MatchAllOrderingRule());
            return (TSelf)this;
        }

        /// <summary>
        /// Causes the collection identified by the provided <paramref name="predicate"/> to be compared in the order 
        /// in which the items appear in the expectation.
        /// </summary>
        public TSelf WithStrictOrderingFor(Expression<Func<ISubjectInfo, bool>> predicate)
        {
            orderingRules.Add(new PredicateBasedOrderingRule(predicate));
            return (TSelf)this;
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
        /// Defines additional overrides when used with <see cref="EquivalencyAssertionOptions.When"/>
        /// </summary>
        public class Restriction<TProperty>
        {
            private readonly Action<IAssertionContext<TProperty>> action;
            private readonly TSelf options;

            public Restriction(TSelf options, Action<IAssertionContext<TProperty>> action)
            {
                this.options = options;
                this.action = action;
            }

            /// <summary>
            /// Allows overriding the way structural equality is applied to (nested) objects of tyoe <typeparamref name="TPropertyType"/>
            /// </summary>
            public TSelf WhenTypeIs<TPropertyType>()
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
            public TSelf When(Expression<Func<ISubjectInfo, bool>> predicate)
            {
                options.Using(new AssertionRule<TProperty>(predicate, action));
                return options;
            }
        }
    }
}