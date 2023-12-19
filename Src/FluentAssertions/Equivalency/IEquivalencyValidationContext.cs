using FluentAssertionsAsync.Equivalency.Tracing;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Equivalency;

/// <summary>
/// Provides information on a particular property or field during an assertion for structural equality of two object graphs.
/// </summary>
public interface IEquivalencyValidationContext
{
    /// <summary>
    /// Gets the <see cref="INode"/> of the member that returned the current object, or <see langword="null"/> if the current
    /// object represents the root object.
    /// </summary>
    INode CurrentNode { get; }

    /// <summary>
    /// A formatted phrase and the placeholder values explaining why the assertion is needed.
    /// </summary>
    public Reason Reason { get; }

    /// <summary>
    /// Gets an object that can be used by the equivalency algorithm to provide a trace when the
    /// <see cref="SelfReferenceEquivalencyOptions{TSelf}.WithTracing"/> option is used.
    /// </summary>
    Tracer Tracer { get; }

    IEquivalencyOptions Options { get; }

    /// <summary>
    /// Determines whether the specified object reference is a cyclic reference to the same object earlier in the
    /// equivalency validation.
    /// </summary>
    public bool IsCyclicReference(object expectation);

    /// <summary>
    /// Creates a context from the current object intended to assert the equivalency of a nested member.
    /// </summary>
    IEquivalencyValidationContext AsNestedMember(IMember expectationMember);

    /// <summary>
    /// Creates a context from the current object intended to assert the equivalency of a collection item identified by <paramref name="index"/>.
    /// </summary>
    IEquivalencyValidationContext AsCollectionItem<TItem>(string index);

    /// <summary>
    /// Creates a context from the current object intended to assert the equivalency of a collection item identified by <paramref name="key"/>.
    /// </summary>
    IEquivalencyValidationContext AsDictionaryItem<TKey, TExpectation>(TKey key);

    /// <summary>
    /// Creates a deep clone of the current context.
    /// </summary>
    IEquivalencyValidationContext Clone();
}
