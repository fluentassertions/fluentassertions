using System;
using System.Collections.Generic;
using System.ComponentModel;
using FluentAssertionsAsync.Equivalency.Tracing;

namespace FluentAssertionsAsync.Equivalency;

/// <summary>
/// Provides the run-time details of the <see cref="EquivalencyOptions{TExpectation}" /> class.
/// </summary>
public interface IEquivalencyOptions
{
    /// <summary>
    /// Gets an ordered collection of selection rules that define what members (e.g. properties or fields) are included.
    /// </summary>
    IEnumerable<IMemberSelectionRule> SelectionRules { get; }

    /// <summary>
    /// Gets an ordered collection of matching rules that determine which subject members are matched with which
    /// expectation properties.
    /// </summary>
    IEnumerable<IMemberMatchingRule> MatchingRules { get; }

    /// <summary>
    /// Gets a value indicating whether or not the assertion must perform a deep comparison.
    /// </summary>
    bool IsRecursive { get; }

    /// <summary>
    /// Gets a value indicating whether recursion is allowed to continue indefinitely.
    /// </summary>
    bool AllowInfiniteRecursion { get; }

    /// <summary>
    /// Gets value indicating how cyclic references should be handled. By default, it will throw an exception.
    /// </summary>
    CyclicReferenceHandling CyclicReferenceHandling { get; }

    /// <summary>
    /// Gets an ordered collection of rules that determine whether or not the order of collections is important. By default,
    /// ordering is irrelevant.
    /// </summary>
    OrderingRuleCollection OrderingRules { get; }

    /// <summary>
    /// Contains the rules for what properties to run an auto-conversion.
    /// </summary>
    ConversionSelector ConversionSelector { get; }

    /// <summary>
    /// Gets value indicating how the enums should be compared.
    /// </summary>
    EnumEquivalencyHandling EnumEquivalencyHandling { get; }

    /// <summary>
    /// Gets an ordered collection of Equivalency steps how a subject is compared with the expectation.
    /// </summary>
    IEnumerable<IEquivalencyStep> UserEquivalencySteps { get; }

    /// <summary>
    /// Gets a value indicating whether the runtime type of the expectation should be used rather than the declared type.
    /// </summary>
    bool UseRuntimeTyping { get; }

    /// <summary>
    /// Gets a value indicating whether and which properties should be considered.
    /// </summary>
    MemberVisibility IncludedProperties { get; }

    /// <summary>
    /// Gets a value indicating whether and which fields should be considered.
    /// </summary>
    MemberVisibility IncludedFields { get; }

    /// <summary>
    /// Gets a value indicating whether members on the subject marked with [<see cref="EditorBrowsableAttribute"/>]
    /// and <see cref="EditorBrowsableState.Never"/> should be treated as though they don't exist.
    /// </summary>
    bool IgnoreNonBrowsableOnSubject { get; }

    /// <summary>
    /// Gets a value indicating whether members on the expectation marked with [<see cref="EditorBrowsableAttribute"/>]
    /// and <see cref="EditorBrowsableState.Never"/> should be excluded.
    /// </summary>
    bool ExcludeNonBrowsableOnExpectation { get; }

    /// <summary>
    /// Gets a value indicating whether records should be compared by value instead of their members
    /// </summary>
    bool? CompareRecordsByValue { get; }

    /// <summary>
    /// Gets the currently configured tracer, or <see langword="null"/> if no tracing was configured.
    /// </summary>
    ITraceWriter TraceWriter { get; }

    /// <summary>
    /// Determines the right strategy for evaluating the equality of objects of this type.
    /// </summary>
    EqualityStrategy GetEqualityStrategy(Type type);
}
