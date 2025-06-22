using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using FluentAssertions.Common;
using FluentAssertions.Equivalency.Matching;
using FluentAssertions.Equivalency.Ordering;
using FluentAssertions.Equivalency.Selection;
using FluentAssertions.Equivalency.Steps;
using FluentAssertions.Equivalency.Tracing;

namespace FluentAssertions.Equivalency;

#pragma warning disable CA1033 //An unsealed externally visible type provides an explicit method implementation of a public interface and does not provide an alternative externally visible method that has the same name.

/// <summary>
/// Represents the run-time behavior of a structural equivalency assertion.
/// </summary>
public abstract class SelfReferenceEquivalencyOptions<TSelf> : IEquivalencyOptions
    where TSelf : SelfReferenceEquivalencyOptions<TSelf>
{
    #region Private Definitions

    private readonly EqualityStrategyProvider equalityStrategyProvider;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly List<IMemberSelectionRule> selectionRules = [];

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly List<IMemberMatchingRule> matchingRules = [];

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly List<IEquivalencyStep> userEquivalencySteps = [];

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private CyclicReferenceHandling cyclicReferenceHandling = CyclicReferenceHandling.ThrowException;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected OrderingRuleCollection OrderingRules { get; } = [];

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool isRecursive;

    private bool allowInfiniteRecursion;

    private EnumEquivalencyHandling enumEquivalencyHandling;

    private bool useRuntimeTyping;

    private MemberVisibility includedProperties;
    private MemberVisibility includedFields;
    private bool ignoreNonBrowsableOnSubject;
    private bool excludeNonBrowsableOnExpectation;

    private IEqualityComparer<string> stringComparer;

    #endregion

    private protected SelfReferenceEquivalencyOptions()
    {
        equalityStrategyProvider = new EqualityStrategyProvider();

        AddMatchingRule(new MustMatchByNameRule());

        OrderingRules.Add(new ByteArrayOrderingRule());
    }

    /// <summary>
    /// Creates an instance of the equivalency assertions options based on defaults previously configured by the caller.
    /// </summary>
    protected SelfReferenceEquivalencyOptions(IEquivalencyOptions defaults)
    {
        equalityStrategyProvider = new EqualityStrategyProvider(defaults.GetEqualityStrategy)
        {
            CompareRecordsByValue = defaults.CompareRecordsByValue
        };

        isRecursive = defaults.IsRecursive;
        cyclicReferenceHandling = defaults.CyclicReferenceHandling;
        allowInfiniteRecursion = defaults.AllowInfiniteRecursion;
        enumEquivalencyHandling = defaults.EnumEquivalencyHandling;
        useRuntimeTyping = defaults.UseRuntimeTyping;
        includedProperties = defaults.IncludedProperties;
        includedFields = defaults.IncludedFields;
        ignoreNonBrowsableOnSubject = defaults.IgnoreNonBrowsableOnSubject;
        excludeNonBrowsableOnExpectation = defaults.ExcludeNonBrowsableOnExpectation;
        IgnoreLeadingWhitespace = defaults.IgnoreLeadingWhitespace;
        IgnoreTrailingWhitespace = defaults.IgnoreTrailingWhitespace;
        IgnoreCase = defaults.IgnoreCase;
        IgnoreNewlineStyle = defaults.IgnoreNewlineStyle;

        ConversionSelector = defaults.ConversionSelector.Clone();

        selectionRules.AddRange(defaults.SelectionRules);
        userEquivalencySteps.AddRange(defaults.UserEquivalencySteps);
        matchingRules.AddRange(defaults.MatchingRules);
        OrderingRules = new OrderingRuleCollection(defaults.OrderingRules);

        TraceWriter = defaults.TraceWriter;

        RemoveSelectionRule<AllPropertiesSelectionRule>();
        RemoveSelectionRule<AllFieldsSelectionRule>();
    }

    /// <summary>
    /// Gets an ordered collection of selection rules that define what members are included.
    /// </summary>
    IEnumerable<IMemberSelectionRule> IEquivalencyOptions.SelectionRules
    {
        get
        {
            bool hasConflictingRules = selectionRules.Exists(rule => rule.IncludesMembers);

            if (includedProperties.HasFlag(MemberVisibility.Public) && !hasConflictingRules)
            {
                yield return new AllPropertiesSelectionRule();
            }

            if (includedFields.HasFlag(MemberVisibility.Public) && !hasConflictingRules)
            {
                yield return new AllFieldsSelectionRule();
            }

            if (excludeNonBrowsableOnExpectation)
            {
                yield return new ExcludeNonBrowsableMembersRule();
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
    IEnumerable<IMemberMatchingRule> IEquivalencyOptions.MatchingRules => matchingRules;

    /// <summary>
    /// Gets an ordered collection of Equivalency steps how a subject is compared with the expectation.
    /// </summary>
    IEnumerable<IEquivalencyStep> IEquivalencyOptions.UserEquivalencySteps => userEquivalencySteps;

    public ConversionSelector ConversionSelector { get; } = new();

    /// <summary>
    /// Gets an ordered collection of rules that determine whether or not the order of collections is important. By
    /// default,
    /// ordering is irrelevant.
    /// </summary>
    OrderingRuleCollection IEquivalencyOptions.OrderingRules => OrderingRules;

    /// <summary>
    /// Gets value indicating whether the equality check will include nested collections and complex types.
    /// </summary>
    bool IEquivalencyOptions.IsRecursive => isRecursive;

    bool IEquivalencyOptions.AllowInfiniteRecursion => allowInfiniteRecursion;

    /// <summary>
    /// Gets value indicating how cyclic references should be handled. By default, it will throw an exception.
    /// </summary>
    CyclicReferenceHandling IEquivalencyOptions.CyclicReferenceHandling => cyclicReferenceHandling;

    EnumEquivalencyHandling IEquivalencyOptions.EnumEquivalencyHandling => enumEquivalencyHandling;

    bool IEquivalencyOptions.UseRuntimeTyping => useRuntimeTyping;

    MemberVisibility IEquivalencyOptions.IncludedProperties => includedProperties;

    MemberVisibility IEquivalencyOptions.IncludedFields => includedFields;

    bool IEquivalencyOptions.IgnoreNonBrowsableOnSubject => ignoreNonBrowsableOnSubject;

    bool IEquivalencyOptions.ExcludeNonBrowsableOnExpectation => excludeNonBrowsableOnExpectation;

    public bool? CompareRecordsByValue => equalityStrategyProvider.CompareRecordsByValue;

    EqualityStrategy IEquivalencyOptions.GetEqualityStrategy(Type type)
        => equalityStrategyProvider.GetEqualityStrategy(type);

    public bool IgnoreLeadingWhitespace { get; private set; }

    public bool IgnoreTrailingWhitespace { get; private set; }

    public bool IgnoreCase { get; private set; }

    public bool IgnoreNewlineStyle { get; private set; }

    public ITraceWriter TraceWriter { get; private set; }

    /// <summary>
    /// Causes inclusion of only public properties of the subject as far as they are defined on the declared type.
    /// </summary>
    /// <remarks>
    /// This clears all previously registered selection rules.
    /// </remarks>
    public TSelf IncludingAllDeclaredProperties()
    {
        PreferringDeclaredMemberTypes();

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
        PreferringRuntimeMemberTypes();

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
        includedProperties = MemberVisibility.Public | MemberVisibility.ExplicitlyImplemented |
            MemberVisibility.DefaultInterfaceProperties;

        return (TSelf)this;
    }

    /// <summary>
    /// Instructs the comparison to include public and internal properties.
    /// </summary>
    public TSelf IncludingInternalProperties()
    {
        includedProperties = MemberVisibility.Public | MemberVisibility.Internal | MemberVisibility.ExplicitlyImplemented |
            MemberVisibility.DefaultInterfaceProperties;

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
    /// Excludes the specified member(s) from the structural equality check anywhere in the object graph.
    /// </summary>
    public TSelf ExcludingMembersNamed(params string[] memberNames)
    {
        Guard.ThrowIfArgumentIsNull(memberNames, nameof(memberNames), "Member names cannot be null.");
        Guard.ThrowIfArgumentIsEmpty(memberNames, nameof(memberNames), "At least one member name must be specified.");

        AddSelectionRule(new ExcludeMembersByNameSelectionRule(memberNames));
        return (TSelf)this;
    }

    /// <summary>
    /// Excludes properties that are explicitly implemented from the equivalency comparison.
    /// </summary>
    public TSelf ExcludingExplicitlyImplementedProperties()
    {
        includedProperties &= ~MemberVisibility.ExplicitlyImplemented;
        return (TSelf)this;
    }

    /// <summary>
    /// Instructs the comparison to exclude non-browsable members in the expectation (members set to
    /// <see cref="EditorBrowsableState.Never"/>). It is not required that they be marked non-browsable in the subject. Use
    /// <see cref="IgnoringNonBrowsableMembersOnSubject"/> to ignore non-browsable members in the subject.
    /// </summary>
    public TSelf ExcludingNonBrowsableMembers()
    {
        excludeNonBrowsableOnExpectation = true;
        return (TSelf)this;
    }

    /// <summary>
    /// Instructs the comparison to treat non-browsable members in the subject as though they do not exist. If you need to
    /// ignore non-browsable members in the expectation, use <see cref="ExcludingNonBrowsableMembers"/>.
    /// </summary>
    public TSelf IgnoringNonBrowsableMembersOnSubject()
    {
        ignoreNonBrowsableOnSubject = true;
        return (TSelf)this;
    }

    /// <summary>
    /// Instructs the structural equality comparison to use the run-time types
    /// of the members to drive the assertion.
    /// </summary>
    public TSelf PreferringRuntimeMemberTypes()
    {
        useRuntimeTyping = true;
        return (TSelf)this;
    }

    /// <summary>
    /// Instructs the structural equality comparison to prefer the declared types
    /// of the members when executing assertions.
    /// </summary>
    /// <remarks>
    /// In reality, the declared types of the members are only known for the members of the root object,
    /// or the objects in a root collection. Beyond that, FluentAssertions only knows the run-time types of the members.
    /// </remarks>
    public TSelf PreferringDeclaredMemberTypes()
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
    /// Causes the structural equality comparison to recursively traverse the object graph and compare the fields and
    /// properties of any nested objects and objects in collections.
    /// </summary>
    /// <remarks>
    /// This is the default behavior. You can override this using <see cref="WithoutRecursing"/>.
    /// </remarks>
    public TSelf IncludingNestedObjects()
    {
        isRecursive = true;
        return (TSelf)this;
    }

    /// <summary>
    /// Stops the structural equality check from recursively comparing the members of any nested objects.
    /// </summary>
    /// <remarks>
    /// If a property or field points to a complex type or collection, a simple <see cref="object.Equals(object)"/> call will
    /// be done instead of recursively looking at the members of the nested object.
    /// </remarks>
    public TSelf WithoutRecursing()
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
    public TSelf WithoutSelectionRules()
    {
        selectionRules.Clear();
        return (TSelf)this;
    }

    /// <summary>
    /// Clears all matching rules, including those that were added by default.
    /// </summary>
    public TSelf WithoutMatchingRules()
    {
        matchingRules.Clear();
        return (TSelf)this;
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
    /// Ensures the equivalency comparison will use the specified implementation of <see cref="IEqualityComparer{String}"/>
    /// any time when a property is a <see langword="string" />.
    /// </summary>
    public TSelf Using(IEqualityComparer<string> comparer)
    {
        userEquivalencySteps.Insert(0, new EqualityComparerEquivalencyStep<string>(comparer));
        stringComparer = comparer;

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
        equalityStrategyProvider.CompareRecordsByValue = true;
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
        equalityStrategyProvider.CompareRecordsByValue = false;
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
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
    public TSelf ComparingByMembers(Type type)
    {
        Guard.ThrowIfArgumentIsNull(type);

        if (type.IsPrimitive)
        {
            throw new InvalidOperationException($"Cannot compare a primitive type such as {type.Name} by its members");
        }

        if (!equalityStrategyProvider.AddReferenceType(type))
        {
            throw new InvalidOperationException(
                $"Can't compare {type.Name} by its members if it already setup to be compared by value");
        }

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
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
    public TSelf ComparingByValue(Type type)
    {
        Guard.ThrowIfArgumentIsNull(type);

        if (!equalityStrategyProvider.AddValueType(type))
        {
            throw new InvalidOperationException(
                $"Can't compare {type.Name} by value if it already setup to be compared by its members");
        }

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
    /// Instructs the comparison to ignore leading whitespace when comparing <see langword="string" />s.
    /// </summary>
    /// <remarks>
    /// Note: This affects the index of first mismatch, as the removed whitespace is also ignored for the index calculation!
    /// </remarks>
    public TSelf IgnoringLeadingWhitespace()
    {
        IgnoreLeadingWhitespace = true;
        return (TSelf)this;
    }

    /// <summary>
    /// Instructs the comparison to ignore trailing whitespace when comparing <see langword="string" />s.
    /// </summary>
    public TSelf IgnoringTrailingWhitespace()
    {
        IgnoreTrailingWhitespace = true;
        return (TSelf)this;
    }

    /// <summary>
    /// Instructs the comparison to compare <see langword="string" />s case-insensitive.
    /// </summary>
    public TSelf IgnoringCase()
    {
        IgnoreCase = true;
        return (TSelf)this;
    }

    /// <summary>
    /// Instructs the comparison to ignore the newline style when comparing <see langword="string" />s.
    /// </summary>
    /// <remarks>
    /// Enabling this option will replace all occurences of <c>\r\n</c> and <c>\r</c> with <c>\n</c> in the strings before comparing them.
    /// </remarks>
    public TSelf IgnoringNewlineStyle()
    {
        IgnoreNewlineStyle = true;
        return (TSelf)this;
    }

    /// <summary>
    /// Returns the comparer for strings, which is either an explicitly specified comparer via <see cref="Using{T}(System.Collections.Generic.IEqualityComparer{T})"/> or an ordinal comparer depending on <see cref="IgnoringCase()" />.
    /// </summary>
    internal IEqualityComparer<string> GetStringComparerOrDefault()
    {
        return stringComparer ?? (IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>
    /// A string that represents the current object.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    [SuppressMessage("Design", "MA0051:Method is too long")]
    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.Append("- Prefer the ")
            .Append(useRuntimeTyping ? "runtime" : "declared")
            .AppendLine(" type of the members");

        if (ignoreNonBrowsableOnSubject)
        {
            builder.AppendLine("- Do not consider members marked non-browsable on the subject");
        }

        if (isRecursive && allowInfiniteRecursion)
        {
            builder.AppendLine("- Recurse indefinitely");
        }

        builder.AppendFormat(CultureInfo.InvariantCulture,
            "- Compare enums by {0}" + Environment.NewLine,
            enumEquivalencyHandling == EnumEquivalencyHandling.ByName ? "name" : "value");

        if (cyclicReferenceHandling == CyclicReferenceHandling.Ignore)
        {
            builder.AppendLine("- Ignoring cyclic references");
        }

        builder
            .AppendLine("- Compare tuples by their properties")
            .AppendLine("- Compare anonymous types by their properties")
            .Append(equalityStrategyProvider);

        if (excludeNonBrowsableOnExpectation)
        {
            builder.AppendLine("- Exclude non-browsable members");
        }
        else
        {
            builder.AppendLine("- Include non-browsable members");
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
    /// Defines additional overrides when used with <see cref="SelfReferenceEquivalencyOptions{TSelf}" />
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

    protected internal TSelf AddSelectionRule(IMemberSelectionRule selectionRule)
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
