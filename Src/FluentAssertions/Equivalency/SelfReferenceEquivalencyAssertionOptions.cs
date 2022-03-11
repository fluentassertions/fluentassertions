using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FluentAssertions.Common;
using FluentAssertions.Equivalency.Matching;
using FluentAssertions.Equivalency.Ordering;
using FluentAssertions.Equivalency.Selection;
using FluentAssertions.Equivalency.Steps;
using FluentAssertions.Equivalency.Tracing;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents the run-time behavior of a structural equivalency assertion.
    /// </summary>
    public abstract class SelfReferenceEquivalencyAssertionOptions<TSelf> : IEquivalencyAssertionOptions
        where TSelf : SelfReferenceEquivalencyAssertionOptions<TSelf>
    {
        #region Private Definitions

        // REFACTOR: group the next three fields in a dedicated class
        private readonly List<Type> referenceTypes = new();
        private readonly List<Type> valueTypes = new();
        private readonly Func<Type, EqualityStrategy> getDefaultEqualityStrategy;

        private readonly ConcurrentDictionary<Type, EqualityStrategy> equalityStrategyCache = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<IMemberSelectionRule> selectionRules = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<IMemberMatchingRule> matchingRules = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<IEquivalencyStep> userEquivalencySteps = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CyclicReferenceHandling cyclicReferenceHandling = CyclicReferenceHandling.ThrowException;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected OrderingRuleCollection OrderingRules { get; } = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isRecursive;

        private bool allowInfiniteRecursion;

        private EnumEquivalencyHandling enumEquivalencyHandling;

        private bool useRuntimeTyping;

        private MemberVisibility includedProperties;
        private MemberVisibility includedFields;

        private bool compareRecordsByValue;

        #endregion

        internal SelfReferenceEquivalencyAssertionOptions()
        {
            AddMatchingRule(new MustMatchByNameRule());

            OrderingRules.Add(new ByteArrayOrderingRule());
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
            includedProperties = defaults.IncludedProperties;
            includedFields = defaults.IncludedFields;
            compareRecordsByValue = defaults.CompareRecordsByValue;

            ConversionSelector = defaults.ConversionSelector.Clone();

            selectionRules.AddRange(defaults.SelectionRules);
            userEquivalencySteps.AddRange(defaults.UserEquivalencySteps);
            matchingRules.AddRange(defaults.MatchingRules);
            OrderingRules = new OrderingRuleCollection(defaults.OrderingRules);

            getDefaultEqualityStrategy = defaults.GetEqualityStrategy;
            TraceWriter = defaults.TraceWriter;

            RemoveSelectionRule<AllPropertiesSelectionRule>();
            RemoveSelectionRule<AllFieldsSelectionRule>();
        }

        /// <summary>
        /// Gets an ordered collection of selection rules that define what members are included.
        /// </summary>
        IEnumerable<IMemberSelectionRule> IEquivalencyAssertionOptions.SelectionRules
        {
            get
            {
                bool hasConflictingRules = selectionRules.Any(rule => rule.IncludesMembers);

                if (includedProperties.HasFlag(MemberVisibility.Public) && !hasConflictingRules)
                {
                    yield return new AllPropertiesSelectionRule();
                }

                if (includedFields.HasFlag(MemberVisibility.Public) && !hasConflictingRules)
                {
                    yield return new AllFieldsSelectionRule();
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
        IEnumerable<IEquivalencyStep> IEquivalencyAssertionOptions.UserEquivalencySteps => userEquivalencySteps;

        public ConversionSelector ConversionSelector { get; } = new();

        /// <summary>
        /// Gets an ordered collection of rules that determine whether or not the order of collections is important. By
        /// default,
        /// ordering is irrelevant.
        /// </summary>
        OrderingRuleCollection IEquivalencyAssertionOptions.OrderingRules => OrderingRules;

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

        MemberVisibility IEquivalencyAssertionOptions.IncludedProperties => includedProperties;

        MemberVisibility IEquivalencyAssertionOptions.IncludedFields => includedFields;

        public bool CompareRecordsByValue => compareRecordsByValue;

        EqualityStrategy IEquivalencyAssertionOptions.GetEqualityStrategy(Type requestedType)
        {
            // As the valueFactory parameter captures instance members,
            // be aware if the cache must be cleared on mutating the members.
            return equalityStrategyCache.GetOrAdd(requestedType, type =>
            {
                EqualityStrategy strategy;

                if (!type.IsPrimitive && referenceTypes.Count > 0 && referenceTypes.Any(t => type.IsSameOrInherits(t)))
                {
                    strategy = EqualityStrategy.ForceMembers;
                }
                else if (valueTypes.Count > 0 && valueTypes.Any(t => type.IsSameOrInherits(t)))
                {
                    strategy = EqualityStrategy.ForceEquals;
                }
                else if (!type.IsPrimitive && referenceTypes.Count > 0 && referenceTypes.Any(t => type.IsAssignableToOpenGeneric(t)))
                {
                    strategy = EqualityStrategy.ForceMembers;
                }
                else if (valueTypes.Count > 0 && valueTypes.Any(t => type.IsAssignableToOpenGeneric(t)))
                {
                    strategy = EqualityStrategy.ForceEquals;
                }
                else if (type.IsRecord())
                {
                    strategy = compareRecordsByValue ? EqualityStrategy.ForceEquals : EqualityStrategy.ForceMembers;
                }
                else
                {
                    if (getDefaultEqualityStrategy is not null)
                    {
                        strategy = getDefaultEqualityStrategy(type);
                    }
                    else
                    {
                        strategy = type.HasValueSemantics() ? EqualityStrategy.Equals : EqualityStrategy.Members;
                    }
                }

                return strategy;
            });
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
        /// Instructs the comparison to include public fields.
        /// </summary>
        /// <remarks>
        /// This is part of the default behavior.
        /// </remarks>
        public TSelf IncludingFields()
        {
            includedFields = MemberVisibility.Public;
            return (TSelf)this;
        }

        /// <summary>
        /// Instructs the comparison to include public and internal fields.
        /// </summary>
        public TSelf IncludingInternalFields()
        {
            includedFields = MemberVisibility.Public | MemberVisibility.Internal;
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
            includedFields = MemberVisibility.None;
            return (TSelf)this;
        }

        /// <summary>
        /// Instructs the comparison to include public properties.
        /// </summary>
        /// <remarks>
        /// This is part of the default behavior.
        /// </remarks>
        public TSelf IncludingProperties()
        {
            includedProperties = MemberVisibility.Public;
            return (TSelf)this;
        }

        /// <summary>
        /// Instructs the comparison to include public and internal properties.
        /// </summary>
        public TSelf IncludingInternalProperties()
        {
            includedProperties = MemberVisibility.Public | MemberVisibility.Internal;
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
            includedProperties = MemberVisibility.None;
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
        /// Includes the specified member in the equality check.
        /// </summary>
        /// <remarks>
        /// This overrides the default behavior of including all declared members.
        /// </remarks>
        public TSelf Including(Expression<Func<IMemberInfo, bool>> predicate)
        {
            AddSelectionRule(new IncludeMemberByPredicateSelectionRule(predicate));
            return (TSelf)this;
        }

        /// <summary>
        /// Tries to match the members of the expectation with equally named members on the subject. Ignores those
        /// members that don't exist on the subject and previously registered matching rules.
        /// </summary>
        public TSelf ExcludingMissingMembers()
        {
            matchingRules.RemoveAll(x => x is MustMatchByNameRule);
            matchingRules.Add(new TryMatchByNameRule());
            return (TSelf)this;
        }

        /// <summary>
        /// Requires the subject to have members which are equally named to members on the expectation.
        /// </summary>
        public TSelf ThrowingOnMissingMembers()
        {
            matchingRules.RemoveAll(x => x is TryMatchByNameRule);
            matchingRules.Add(new MustMatchByNameRule());
            return (TSelf)this;
        }

        /// <summary>
        /// Overrides the comparison of subject and expectation to use provided <paramref name="action"/>
        /// when the predicate is met.
        /// </summary>
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
            matchingRules.Clear();
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
        /// Adds an ordering rule to the ones already added by default, and which is evaluated after all existing rules.
        /// </summary>
        public TSelf Using(IOrderingRule orderingRule)
        {
            return AddOrderingRule(orderingRule);
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
        /// Ensures the equivalency comparison will create and use an instance of <typeparamref name="TEqualityComparer"/>
        /// that implements <see cref="IEqualityComparer{T}"/>, any time
        /// when a property is of type <typeparamref name="T"/>.
        /// </summary>
        public TSelf Using<T, TEqualityComparer>()
            where TEqualityComparer : IEqualityComparer<T>, new()
        {
            return Using(new TEqualityComparer());
        }

        /// <summary>
        /// Ensures the equivalency comparison will use the specified implementation of <see cref="IEqualityComparer{T}"/>
        /// any time when a property is of type <typeparamref name="T"/>.
        /// </summary>
        public TSelf Using<T>(IEqualityComparer<T> comparer)
        {
            userEquivalencySteps.Insert(0, new EqualityComparerEquivalencyStep<T>(comparer));

            return (TSelf)this;
        }

        /// <summary>
        /// Causes all collections to be compared in the order in which the items appear in the expectation.
        /// </summary>
        public TSelf WithStrictOrdering()
        {
            OrderingRules.Clear();
            OrderingRules.Add(new MatchAllOrderingRule());
            return (TSelf)this;
        }

        /// <summary>
        /// Causes the collection identified by the provided <paramref name="predicate" /> to be compared in the order
        /// in which the items appear in the expectation.
        /// </summary>
        public TSelf WithStrictOrderingFor(Expression<Func<IObjectInfo, bool>> predicate)
        {
            OrderingRules.Add(new PredicateBasedOrderingRule(predicate));
            return (TSelf)this;
        }

        /// <summary>
        /// Causes all collections - except bytes - to be compared ignoring the order in which the items appear in the expectation.
        /// </summary>
        public TSelf WithoutStrictOrdering()
        {
            OrderingRules.Clear();
            OrderingRules.Add(new ByteArrayOrderingRule());
            return (TSelf)this;
        }

        /// <summary>
        /// Causes the collection identified by the provided <paramref name="predicate" /> to be compared ignoring the order
        /// in which the items appear in the expectation.
        /// </summary>
        public TSelf WithoutStrictOrderingFor(Expression<Func<IObjectInfo, bool>> predicate)
        {
            OrderingRules.Add(new PredicateBasedOrderingRule(predicate)
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
        /// Ensures records by default are compared by value instead of their members.
        /// </summary>
        public TSelf ComparingRecordsByValue()
        {
            compareRecordsByValue = true;
            equalityStrategyCache.Clear();
            return (TSelf)this;
        }

        /// <summary>
        /// Ensures records by default are compared by their members even though they override 
        /// the <see cref="object.Equals(object)" /> method.
        /// </summary>
        /// <remarks>
        /// This is the default.
        /// </remarks>
        public TSelf ComparingRecordsByMembers()
        {
            compareRecordsByValue = false;
            equalityStrategyCache.Clear();
            return (TSelf)this;
        }

        /// <summary>
        /// Marks the <typeparamref name="T" /> as a type that should be compared by its members even though it may override
        /// the <see cref="object.Equals(object)" /> method.
        /// </summary>
        public TSelf ComparingByMembers<T>() => ComparingByMembers(typeof(T));

        /// <summary>
        /// Marks <paramref name="type" /> as a type that should be compared by its members even though it may override
        /// the <see cref="object.Equals(object)" /> method.
        /// </summary>
        public TSelf ComparingByMembers(Type type)
        {
            Guard.ThrowIfArgumentIsNull(type, nameof(type));

            if (type.IsPrimitive)
            {
                throw new InvalidOperationException($"Cannot compare a primitive type such as {type.Name} by its members");
            }

            if (valueTypes.Any(t => type.IsSameOrInherits(t)))
            {
                throw new InvalidOperationException(
                    $"Can't compare {type.Name} by its members if it already setup to be compared by value");
            }

            referenceTypes.Add(type);
            equalityStrategyCache.Clear();
            return (TSelf)this;
        }

        /// <summary>
        /// Marks the <typeparamref name="T" /> as a value type which must be compared using its
        /// <see cref="object.Equals(object)" /> method, regardless of it overriding it or not.
        /// </summary>
        public TSelf ComparingByValue<T>() => ComparingByValue(typeof(T));

        /// <summary>
        /// Marks <paramref name="type" /> as a value type which must be compared using its
        /// <see cref="object.Equals(object)" /> method, regardless of it overriding it or not.
        /// </summary>
        public TSelf ComparingByValue(Type type)
        {
            Guard.ThrowIfArgumentIsNull(type, nameof(type));

            if (referenceTypes.Any(t => type.IsSameOrInherits(t)))
            {
                throw new InvalidOperationException(
                    $"Can't compare {type.Name} by value if it already setup to be compared by its members");
            }

            valueTypes.Add(type);
            equalityStrategyCache.Clear();
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
        /// a specific member on the expectation object before running any of the other steps.
        /// </summary>
        public TSelf WithAutoConversionFor(Expression<Func<IObjectInfo, bool>> predicate)
        {
            ConversionSelector.Include(predicate);
            return (TSelf)this;
        }

        /// <summary>
        /// Instructs the equivalency comparison to prevent trying to convert the value of
        /// a specific member on the expectation object before running any of the other steps.
        /// </summary>
        public TSelf WithoutAutoConversionFor(Expression<Func<IObjectInfo, bool>> predicate)
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
            var builder = new StringBuilder();

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

            builder.AppendFormat(CultureInfo.InvariantCulture,
                "- Compare enums by {0}" + Environment.NewLine,
                enumEquivalencyHandling == EnumEquivalencyHandling.ByName ? "name" : "value");

            if (cyclicReferenceHandling == CyclicReferenceHandling.Ignore)
            {
                builder.AppendLine("- Ignoring cyclic references");
            }

            builder.AppendLine($"- Compare tuples by their properties");
            builder.AppendLine($"- Compare anonymous types by their properties");

            if (compareRecordsByValue)
            {
                builder.AppendLine("- Compare records by value");
            }
            else
            {
                builder.AppendLine("- Compare records by their members");
            }

            foreach (Type valueType in valueTypes)
            {
                builder.AppendLine($"- Compare {valueType} by value");
            }

            foreach (Type type in referenceTypes)
            {
                builder.AppendLine($"- Compare {type} by its members");
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

            foreach (IOrderingRule rule in OrderingRules)
            {
                builder.Append("- ").AppendLine(rule.ToString());
            }

            builder.Append("- ").AppendLine(ConversionSelector.ToString());

            return builder.ToString();
        }

        /// <summary>
        /// Defines additional overrides when used with <see cref="SelfReferenceEquivalencyAssertionOptions{T}" />
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
                where TMemberType : TMember
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
            public TSelf When(Expression<Func<IObjectInfo, bool>> predicate)
            {
                options.userEquivalencySteps.Insert(0,
                    new AssertionRuleEquivalencyStep<TMember>(predicate, action));

                return options;
            }
        }

        #region Non-fluent API

        private void RemoveSelectionRule<T>()
            where T : IMemberSelectionRule
        {
            selectionRules.RemoveAll(selectionRule => selectionRule is T);
        }
        
        protected TSelf AddSelectionRule(IMemberSelectionRule selectionRule)
        {
            selectionRules.Add(selectionRule);
            return (TSelf)this;
        }

        protected TSelf AddMatchingRule(IMemberMatchingRule matchingRule)
        {
            matchingRules.Insert(0, matchingRule);
            return (TSelf)this;
        }

        private TSelf AddOrderingRule(IOrderingRule orderingRule)
        {
            OrderingRules.Add(orderingRule);
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
