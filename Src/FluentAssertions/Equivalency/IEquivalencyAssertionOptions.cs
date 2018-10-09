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
        IEnumerable<IEquivalencyStep> GetUserEquivalencySteps(ConversionSelector conversionSelector);

        /// <summary>
        /// Gets a value indicating whether the runtime type of the expectation should be used rather than the declared type.
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
        /// Determines the right strategy for evaluating the equality of objects of this type.
        /// </summary>
        EqualityStrategy GetEqualityStrategy(Type type);
    }

    public enum EqualityStrategy
    {
        /// <summary>
        /// The object overrides <see cref="object.Equals"/>, so use that.
        /// </summary>
        Equals,

        /// <summary>
        /// The object does not seem to override <see cref="object.Equals"/>, so compare by members
        /// </summary>
        Members,

        /// <summary>
        /// Compare using <see cref="object.Equals(object)"/>, whether or not the object overrides it.
        /// </summary>
        ForceEquals,

        /// <summary>
        /// Compare the members, regardless of an <see cref="object.Equals(object)"/> override exists or not.
        /// </summary>
        ForceMembers,
    }
}
