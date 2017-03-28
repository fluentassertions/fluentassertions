using System;
using System.Collections.Generic;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Provides the run-time details of the <see cref="EquivalencyAssertionOptions{TSubject}" /> class.
    /// </summary>
    public interface IEquivalencyAssertionOptions
    {
        /// <summary>
        /// Gets an ordered collection of selection rules that define what properties are included.
        /// </summary>
        IEnumerable<IMemberSelectionRule> SelectionRules { get; }

        /// <summary>
        /// Gets an ordered collection of matching rules that determine which subject properties are matched with which
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
        /// Gets value indicating how the enums should be compared.
        /// </summary>
        EnumEquivalencyHandling EnumEquivalencyHandling { get; }

        /// <summary>
        /// Gets an ordered collection of Equivalency steps how a subject is compared with the expectation.
        /// </summary>
        IEnumerable<IEquivalencyStep> UserEquivalencySteps { get; }

        /// <summary>
        /// Gets a value indicating whether the runtime type should be used rather than the declared type.
        /// </summary>
        bool UseRuntimeTyping { get; }

        /// <summary>
        /// Gets a value indicating whether properties should be considered.
        /// </summary>
        bool IncludeProperties { get; }

        /// <summary>
        /// Gets a value indicating whether fields should be considered.
        /// </summary>
        bool IncludeFields { get; }

        /// <summary>
        /// Gets the currently configured tracer, or <c>null</c> if no tracing was configured.
        /// </summary>
        ITraceWriter TraceWriter { get; }

        /// <summary>
        /// Gets a value indicating whether the <paramref name="type"/> should be treated as having value semantics.
        /// </summary>
        bool IsValueType(Type type);
    }
}