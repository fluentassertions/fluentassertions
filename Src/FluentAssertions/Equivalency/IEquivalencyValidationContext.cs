using System;
using FluentAssertions.Equivalency.Tracing;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Provides information on a particular property or field during an assertion for structural equality of two object graphs.
    /// </summary>
    public interface IEquivalencyValidationContext
    {
        /// <summary>
        /// Gets the <see cref="INode"/> of the member that returned the current object, or <c>null</c> if the current
        /// object represents the root object.
        /// </summary>
        INode CurrentNode { get; }

        /// <summary>
        /// Gets the compile-time type of the current object. If the current object is not the root object and the type is not <see cref="object"/>,
        /// then it returns the same <see cref="System.Type"/> as the <see cref="RuntimeType"/> property does.
        /// </summary>
        Type CompileTimeType { get; }

        /// <summary>
        /// Gets the run-time type of the current object.
        /// </summary>
        Type RuntimeType { get; }

        /// <summary>
        /// Gets the value of the expected object graph.
        /// </summary>
        object Expectation { get; }

        /// <summary>
        /// Gets the value of the subject object graph.
        /// </summary>
        object Subject { get; set; }

        /// <summary>
        /// A formatted phrase and the placeholder values explaining why the assertion is needed.
        /// </summary>
        public Reason Reason { get; }

        /// <summary>
        /// Gets an object that can be used by the equivalency algorithm to provide a trace when the
        /// <see cref="SelfReferenceEquivalencyAssertionOptions{TSelf}.WithTracing"/> option is used.
        /// </summary>
        Tracer Tracer { get; }

        /// <summary>
        /// Creates a context from the current object intended to assert the equivalency of a nested member.
        /// </summary>
        IEquivalencyValidationContext AsNestedMember(IMember expectationMember, IMember matchingSubjectMember);

        /// <summary>
        /// Creates a context from the current object intended to assert the equivalency of a collection item identified by <paramref name="index"/>.
        /// </summary>
        IEquivalencyValidationContext AsCollectionItem<T>(string index, object subject, T expectation);

        /// <summary>
        /// Creates a context from the current object intended to assert the equivalency of a collection item identified by <paramref name="key"/>.
        /// </summary>
        IEquivalencyValidationContext AsDictionaryItem<TKey, TExpectation>(TKey key, object subject, TExpectation expectation);

        /// <summary>
        /// Creates a deep clone of the current context.
        /// </summary>
        IEquivalencyValidationContext Clone();
    }
}
