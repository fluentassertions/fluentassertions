using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FluentAssertions.Common;
using FluentAssertions.Equivalency.Matching;
using FluentAssertions.Equivalency.Ordering;
using FluentAssertions.Equivalency.Selection;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents the run-time behavior of a structural equivalency assertion.
    /// </summary>
    public abstract class SelfReferenceEquivalencyAssertionOptions<TSelf> : IEquivalencyAssertionOptions
        where TSelf : SelfReferenceEquivalencyAssertionOptions<TSelf>
    {
        #region Private Definitions

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<IMemberSelectionRule> selectionRules = new List<IMemberSelectionRule>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<IMemberMatchingRule> matchingRules = new List<IMemberMatchingRule>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<IEquivalencyStep> userEquivalencySteps = new List<IEquivalencyStep>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CyclicReferenceHandling cyclicReferenceHandling = CyclicReferenceHandling.ThrowException;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#pragma warning disable CA1051 // TODO: fix in 6.0
        protected readonly OrderingRuleCollection orderingRules = new OrderingRuleCollection();
#pragma warning restore CA1051

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isRecursive;

        private bool allowInfiniteRecursion;

        private EnumEquivalencyHandling enumEquivalencyHandling;

        private bool useRuntimeTyping;

        private bool includeProperties;

        private bool includeFields;

        private readonly ConcurrentDictionary<Type, bool> hasValueSemanticsMap = new ConcurrentDictionary<Type, bool>();

        private readonly List<Type> referenceTypes = new List<Type>();
        private readonly List<Type> valueTypes = new List<Type>();

        private readonly Func<Type, EqualityStrategy> getDefaultEqualityStrategy = null;

        #endregion

        internal SelfReferenceEquivalencyAssertionOptions()
        {
            AddMatchingRule(new MustMatchByNameRule());

            orderingRules.Add(new ByteArrayOrderingRule());
        }

        /// <summary>
        /// Creates an instance of the equivalency assertions options based on defaults previously configured by the caller.
        /// </summary>
        protected SelfReferenceEquivalencyAssertionOptions(IEquivalencyAssertionOptions defaults)
        {
            isRecursive = defaults.IsRecursive;
            cyclicReferenceHandling = defaults.CyclicReferenceHandling;
            allowInfiniteRecursion = defaults.AllowInfiniteRecursion;
            enumEquivalencyHandling = defaults.EnumEquivalencyHandling;
            useRuntimeTyping = defaults.UseRuntimeTyping;
            includeProperties = defaults.IncludeProperties;
            includeFields = defaults.IncludeFields;

            ConversionSelector = defaults.ConversionSelector.Clone();

            selectionRules.AddRange(defaults.SelectionRules);
            userEquivalencySteps.AddRange(defaults.GetUserEquivalencySteps(ConversionSelector));
            matchingRules.AddRange(defaults.MatchingRules);
            orderingRules = new OrderingRuleCollection(defaults.OrderingRules);

            getDefaultEqualityStrategy = defaults.GetEqualityStrategy;
            TraceWriter = defaults.TraceWriter;

            RemoveSelectionRule<AllPublicPropertiesSelectionRule>();
            RemoveSelectionRule<AllPublicFieldsSelectionRule>();
        }

        /// <summary>
        /// Gets an ordered collection of selection rules that define what members are included.
        /// </summary>
        IEnumerable<IMemberSelectionRule> IEquivalencyAssertionOptions.SelectionRules
        {
            get
            {
                bool hasConflictingRules = selectionRules.Any(rule => rule.IncludesMembers);

                if (includeProperties && !hasConflictingRules)
                {
                    yield return new AllPublicPropertiesSelectionRule();
                }

                if (includeFields && !hasConflictingRules)
                {
                    yield return new AllPublicFieldsSelectionRule();
                }

                foreach (IMemberSelectionRule rule in selectionRules)
                {
                    yield return rule;
                }
            }
        }

        /// <summary>
        /// Gets an ordered collection of matching rules that determine which subject members are matched with which
        /// expectation members.
        /// </summary>
        IEnumerable<IMemberMatchingRule> IEquivalencyAssertionOptions.MatchingRules => matchingRules;

        /// <summary>
        /// Gets an ordered collection of Equivalency steps how a subject is compared with the expectation.
        /// </summary>
        IEnumerable<IEquivalencyStep> IEquivalencyAssertionOptions.GetUserEquivalencySteps(ConversionSelector conversionSelector) =>
            userEquivalencySteps.Concat(new[] { new TryConversionStep(conversionSelector) });

        public ConversionSelector ConversionSelector { get; } = new ConversionSelector();

        /// <summary>
        /// Gets an ordered collection of rules that determine whether or not the order of collections is important. By
        /// default,
        /// ordering is irrelevant.
        /// </summary>
        OrderingRuleCollection IEquivalencyAssertionOptions.OrderingRules => orderingRules;

        /// <summary>
        /// Gets value indicating whether the equality check will include nested collections and complex types.
        /// </summary>
        bool IEquivalencyAssertionOptions.IsRecursive => isRecursive;

        bool IEquivalencyAssertionOptions.AllowInfiniteRecursion => allowInfiniteRecursion;

        /// <summary>
        /// Gets value indicating how cyclic references should be handled. By default, it will throw an exception.
        /// </summary>
        CyclicReferenceHandling IEquivalencyAssertionOptions.CyclicReferenceHandling => cyclicReferenceHandling;

        EnumEquivalencyHandling IEquivalencyAssertionOptions.EnumEquivalencyHandling => enumEquivalencyHandling;

        bool IEquivalencyAssertionOptions.UseRuntimeTyping => useRuntimeTyping;

        bool IEquivalencyAssertionOptions.IncludeProperties => includeProperties;

        bool IEquivalencyAssertionOptions.IncludeFields => includeFields;

        EqualityStrategy IEquivalencyAssertionOptions.GetEqualityStrategy(Type type)
        {
            EqualityStrategy strategy;

            if (referenceTypes.Any(type.IsSameOrInherits))
            {
                strategy = EqualityStrategy.ForceMembers;
            }
            else if (valueTypes.Any(type.IsSameOrInherits))
            {
                strategy = EqualityStrategy.ForceEquals;
            }
            else
            {
                if (getDefaultEqualityStrategy != null)
                {
                    strategy = getDefaultEqualityStrategy(type);
                }
                else
                {
                    bool hasValueSemantics = hasValueSemanticsMap.GetOrAdd(type, t => t.HasValueSemantics());

                    if (hasValueSemantics)
                    {
                        strategy = EqualityStrategy.Equals;
                    }
                    else
                    {
                        strategy = EqualityStrategy.Members;
                    }
                }
            }

            return strategy;
        }

        public ITraceWriter TraceWriter { get; private set; }

        /// <summary>
        /// Causes inclusion of only public properties of the subject as far as they are defined on the declared type.
        /// </summary>
        /// <remarks>
        /// This clears all previously registered selection rules.
        /// </remarks>
        public TSelf IncludingAllDeclaredProperties()
        {
            RespectingDeclaredTypes();

            ExcludingFields();
            IncludingProperties();

            WithoutSelectionRules();

            return (TSelf)this;
        }

        /// <summary>
        /// Causes inclusion of only public properties of the subject based on its run-time type rather than its declared type.
        /// </summary>
        /// <remarks>
        /// This clears all previously registered selection rules.
        /// </remarks>
        public TSelf IncludingAllRuntimeProperties()
        {
            RespectingRuntimeTypes();

            ExcludingFields();
            IncludingProperties();

            WithoutSelectionRules();

            return (TSelf)this;
        }

        /// <summary>
        /// Instructs the comparison to include fields.
        /// </summary>
        /// <remarks>
        /// This is part of the default behavior.
        /// </remarks>
        public TSelf IncludingFields()
        {
            includeFields = true;
            return (TSelf)this;
        }

        /// <summary>
        /// Instructs the comparison to exclude fields.
        /// </summary>
        /// <remarks>
        /// This does not preclude use of `Including`.
        /// </remarks>
        public TSelf ExcludingFields()
        {
            includeFields = false;
            return (TSelf)this;
        }

        /// <summary>
        /// Instructs the comparison to include properties.
        /// </summary>
        /// <remarks>
        /// This is part of the default behavior.
        /// </remarks>
        public TSelf IncludingProperties()
        {
            includeProperties = true;
            return (TSelf)this;
        }

        /// <summary>
        /// Instructs the comparison to exclude properties.
        /// </summary>
        /// <remarks>
        /// This does not preclude use of `Including`.
        /// </remarks>
        public TSelf ExcludingProperties()
        {
            includeProperties = false;
            return (TSelf)this;
        }

        /// <summary>
        /// Instructs the comparison to respect the expectation's runtime type.
        /// </summary>
        public TSelf RespectingRuntimeTypes()
        {
            useRuntimeTyping = true;
            return (TSelf)this;
        }

        /// <summary>
        /// Instructs the comparison to respect the expectation's declared type.
        /// </summary>
        public TSelf RespectingDeclaredTypes()
        {
            useRuntimeTyping = false;
            return (TSelf)this;
        }

        /// <summary>
        /// Excludes a (nested) property based on a predicate from the structural equality check.
        /// </summary>
        public TSelf Excluding(Expression<Func<IMemberInfo, bool>> predicate)
        {
            AddSelectionRule(new ExcludeMemberByPredicateSelectionRule(predicate));
            return (TSelf)this;
        }

        /// <summary>
        /// Tries to match the members of the subject with equally named members on the expectation. Ignores those
        /// members that don't exist on the expectation and previously registered matching rules.
        /// </summary>
        public TSelf ExcludingMissingMembers()
        {
            ClearMatchingRules();
            matchingRules.Add(new TryMatchByNameRule());
            return (TSelf)this;
        }

        /// <summary>
        /// Requires the expectation to have members which are equally named to members on the subject.
        /// </summary>
        /// <returns></returns>
        public TSelf ThrowingOnMissingMembers()
        {
            ClearMatchingRules();
            matchingRules.Add(new MustMatchByNameRule());
            return (TSelf)this;
        }

        /// <summary></summary>
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
            ClearMatchingRules();
        }

        /// <summary>
        /// Adds a selection rule to the ones already added by default, and which is evaluated after all existing rules.
        /// </summary>
        public TSelf Using(IMemberSelectionRule selectionRule)
        {
            return AddSelectionRule(selectionRule);
        }

        /// <summary>
        /// Adds a matching rule to the ones already added by default, and which is evaluated before all existing rules.
        /// </summary>
        public TSelf Using(IMemberMatchingRule matchingRule)
        {
            return AddMatchingRule(matchingRule);
        }

        /// <summary>
        /// Adds an assertion rule to the ones already added by default, and which is evaluated before all existing rules.
        /// </summary>
        public TSelf Using(IAssertionRule assertionRule)
        {
            userEquivalencySteps.Insert(0, new AssertionRuleEquivalencyStepAdapter(assertionRule));
            return (TSelf)this;
        }

        /// <summary>
        /// Adds an equivalency step rule to the ones already added by default, and which is evaluated before previous
        /// user-registered steps
        /// </summary>
        public TSelf Using(IEquivalencyStep equivalencyStep)
        {
            return AddEquivalencyStep(equivalencyStep);
        }

        /// <summary>
        /// Causes all collections to be compared in the order in which the items appear in the expectation.
        /// </summary>
        public TSelf WithStrictOrdering()
        {
            orderingRules.Clear();
            orderingRules.Add(new MatchAllOrderingRule());
            return (TSelf)this;
        }

        /// <summary>
        /// Causes the collection identified by the provided <paramref name="predicate" /> to be compared in the order
        /// in which the items appear in the expectation.
        /// </summary>
        public TSelf WithStrictOrderingFor(Expression<Func<IMemberInfo, bool>> predicate)
        {
            orderingRules.Add(new PredicateBasedOrderingRule(predicate));
            return (TSelf)this;
        }

        /// <summary>
        /// Causes all collections - except bytes - to be compared ignoring the order in which the items appear in the expectation.
        /// </summary>
        public TSelf WithoutStrictOrdering()
        {
            orderingRules.Clear();
            orderingRules.Add(new ByteArrayOrderingRule());
            return (TSelf)this;
        }

        /// <summary>
        /// Causes the collection identified by the provided <paramref name="predicate" /> to be compared ignoring the order
        /// in which the items appear in the expectation.
        /// </summary>
        public TSelf WithoutStrictOrderingFor(Expression<Func<IMemberInfo, bool>> predicate)
        {
            orderingRules.Add(new PredicateBasedOrderingRule(predicate)
            {
                Invert = true
            });

            return (TSelf)this;
        }

        /// <summary>
        /// Causes to compare Enum properties using the result of their ToString method.
        /// </summary>
        /// <remarks>
        /// By default, enums are compared by value.
        /// </remarks>
        public TSelf ComparingEnumsByName()
        {
            enumEquivalencyHandling = EnumEquivalencyHandling.ByName;
            return (TSelf)this;
        }

        /// <summary>
        /// Causes to compare Enum members using their underlying value only.
        /// </summary>
        /// <remarks>
        /// This is the default.
        /// </remarks>
        public TSelf ComparingEnumsByValue()
        {
            enumEquivalencyHandling = EnumEquivalencyHandling.ByValue;
            return (TSelf)this;
        }

        /// <summary>
        /// Marks the <typeparamref name="T" /> as a type that should be compared by its members even though it may override
        /// the <see cref="object.Equals(object)" /> method.
        /// </summary>
        public TSelf ComparingByMembers<T>()
        {
            if (valueTypes.Any(typeof(T).IsSameOrInherits))
            {
                throw new InvalidOperationException(
                    $"Can't compare {typeof(T).Name} by its members if it already setup to be compared by value");
            }

            referenceTypes.Add(typeof(T));
            return (TSelf)this;
        }

        /// <summary>
        /// Marks the <typeparamref name="T" /> as a value type which must be compared using its
        /// <see cref="object.Equals(object)" /> method, regardless of it overriding it or not.
        /// </summary>
        public TSelf ComparingByValue<T>()
        {
            if (referenceTypes.Any(typeof(T).IsSameOrInherits))
            {
                throw new InvalidOperationException(
                    $"Can't compare {typeof(T).Name} by value if it already setup to be compared by its members");
            }

            valueTypes.Add(typeof(T));
            return (TSelf)this;
        }

        /// <summary>
        /// Enables tracing the steps the equivalency validation followed to compare two graphs.
        /// </summary>
        public TSelf WithTracing(ITraceWriter writer = null)
        {
            TraceWriter = writer ?? new StringBuilderTraceWriter();
            return (TSelf)this;
        }

        /// <summary>
        /// Instructs the equivalency comparison to try to convert the values of
        /// matching properties before running any of the other steps.
        /// </summary>
        public TSelf WithAutoConversion()
        {
            ConversionSelector.IncludeAll();
            return (TSelf)this;
        }

        /// <summary>
        /// Instructs the equivalency comparison to try to convert the value of
        /// a specific property on the expectation object before running any of the other steps.
        /// </summary>
        public TSelf WithAutoConversionFor(Expression<Func<IMemberInfo, bool>> predicate)
        {
            ConversionSelector.Include(predicate);
            return (TSelf)this;
        }

        /// <summary>
        /// Instructs the equivalency comparison to prevent trying to convert the value of
        /// a specific property on the expectation object before running any of the other steps.
        /// </summary>
        public TSelf WithoutAutoConversionFor(Expression<Func<IMemberInfo, bool>> predicate)
        {
            ConversionSelector.Exclude(predicate);
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
            StringBuilder builder = new StringBuilder();

            builder.Append("- Use ")
                .Append(useRuntimeTyping ? "runtime" : "declared")
                .AppendLine(" types and members");

            if (isRecursive)
            {
                if (allowInfiniteRecursion)
                {
                    builder.AppendLine("- Recurse indefinitely");
                }
            }

            builder.AppendFormat("- Compare enums by {0}" + Environment.NewLine,
                enumEquivalencyHandling == EnumEquivalencyHandling.ByName ? "name" : "value");

            if (cyclicReferenceHandling == CyclicReferenceHandling.Ignore)
            {
                builder.AppendLine("- Ignoring cyclic references");
            }

            foreach (IMemberSelectionRule rule in selectionRules)
            {
                builder.Append("- ").AppendLine(rule.ToString());
            }

            foreach (IMemberMatchingRule rule in matchingRules)
            {
                builder.Append("- ").AppendLine(rule.ToString());
            }

            foreach (IEquivalencyStep step in userEquivalencySteps)
            {
                builder.Append("- ").AppendLine(step.ToString());
            }

            foreach (IOrderingRule rule in orderingRules)
            {
                builder.Append("- ").AppendLine(rule.ToString());
            }

            return builder.ToString();
        }

        /// <summary>
        /// Defines additional overrides when used with <see cref="EquivalencyAssertionOptions.When" />
        /// </summary>
        public class Restriction<TMember>
        {
            private readonly Action<IAssertionContext<TMember>> action;
            private readonly TSelf options;

            public Restriction(TSelf options, Action<IAssertionContext<TMember>> action)
            {
                this.options = options;
                this.action = action;
            }

            /// <summary>
            /// Allows overriding the way structural equality is applied to (nested) objects of type
            /// <typeparamref name="TMemberType" />
            /// </summary>
            public TSelf WhenTypeIs<TMemberType>()
            {
                When(info => info.RuntimeType.IsSameOrInherits(typeof(TMemberType)));
                return options;
            }

            /// <summary>
            /// Allows overriding the way structural equality is applied to particular members.
            /// </summary>
            /// <param name="predicate">
            /// A predicate based on the <see cref="IMemberInfo" /> of the subject that is used to identify the property for which
            /// the
            /// override applies.
            /// </param>
            public TSelf When(Expression<Func<IMemberInfo, bool>> predicate)
            {
                options.Using(new AssertionRule<TMember>(predicate, action));
                return options;
            }
        }

        #region Non-fluent API

        protected void RemoveSelectionRule<T>()
            where T : IMemberSelectionRule
        {
            selectionRules.RemoveAll(selectionRule => selectionRule is T);
        }

        private void ClearMatchingRules()
        {
            matchingRules.Clear();
        }

        protected TSelf AddSelectionRule(IMemberSelectionRule selectionRule)
        {
            selectionRules.Add(selectionRule);
            return (TSelf)this;
        }

        private TSelf AddMatchingRule(IMemberMatchingRule matchingRule)
        {
            matchingRules.Insert(0, matchingRule);
            return (TSelf)this;
        }

        private TSelf AddEquivalencyStep(IEquivalencyStep equivalencyStep)
        {
            userEquivalencySteps.Add(equivalencyStep);
            return (TSelf)this;
        }

        #endregion
    }
}
