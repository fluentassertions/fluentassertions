#region

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Equivalency.Execution;
using FluentAssertions.Equivalency.Matching;
using FluentAssertions.Equivalency.Ordering;
using FluentAssertions.Equivalency.Selection;

#endregion

namespace FluentAssertions.Equivalency
{
    // REFACTOR rename to EquivalencyOptions

    /// <summary>
    /// Represents the run-time type-specific behavior of a structural equivalency assertion.
    /// </summary>
    public class EquivalencyAssertionOptions<TExpectation> :
        SelfReferenceEquivalencyAssertionOptions<EquivalencyAssertionOptions<TExpectation>>
    {
        public EquivalencyAssertionOptions()
        {
        }

        public EquivalencyAssertionOptions(IEquivalencyAssertionOptions defaults)
            : base(defaults)
        {
        }

        /// <summary>
        /// Excludes the specified (nested) member from the structural equality check.
        /// </summary>
        public EquivalencyAssertionOptions<TExpectation> Excluding(Expression<Func<TExpectation, object>> expression)
        {
            AddSelectionRule(new ExcludeMemberByPathSelectionRule(expression.GetMemberPath()));
            return this;
        }

        /// <summary>
        /// Includes the specified member in the equality check.
        /// </summary>
        /// <remarks>
        /// This overrides the default behavior of including all declared members.
        /// </remarks>
        public EquivalencyAssertionOptions<TExpectation> Including(Expression<Func<TExpectation, object>> expression)
        {
            AddSelectionRule(new IncludeMemberByPathSelectionRule(expression.GetMemberPath()));
            return this;
        }

        /// <summary>
        /// Causes the collection identified by <paramref name="expression"/> to be compared in the order
        /// in which the items appear in the expectation.
        /// </summary>
        public EquivalencyAssertionOptions<TExpectation> WithStrictOrderingFor(
            Expression<Func<TExpectation, object>> expression)
        {
            string expressionMemberPath = expression.GetMemberPath().ToString();
            OrderingRules.Add(new PathBasedOrderingRule(expressionMemberPath));
            return this;
        }

        /// <summary>
        /// Creates a new set of options based on the current instance which acts on a a collection of the <typeparamref name="TExpectation"/>.
        /// </summary>
        public EquivalencyAssertionOptions<IEnumerable<TExpectation>> AsCollection()
        {
            return new EquivalencyAssertionOptions<IEnumerable<TExpectation>>(
                new CollectionMemberAssertionOptionsDecorator(this));
        }

        /// <summary>
        /// Maps a (nested) property or field of type <typeparamref name="TExpectation"/> to
        /// a (nested) property or field of <typeparamref name="TSubject"/> using lambda expressions.
        /// </summary>
        /// <param name="expectationMemberPath">A field or property expression indicating the (nested) member to map from.</param>
        /// <param name="subjectMemberPath">A field or property expression indicating the (nested) member to map to.</param>
        /// <remarks>
        /// The members of the subject and the expectation must have the same parent. Also, indexes in collections are ignored.
        /// If the types of the members are different, the usual logic applies depending or not if conversion options were specified.
        /// Fields can be mapped to properties and vice-versa.
        /// </remarks>
        public EquivalencyAssertionOptions<TExpectation> WithMapping<TSubject>(
            Expression<Func<TExpectation, object>> expectationMemberPath,
            Expression<Func<TSubject, object>> subjectMemberPath)
        {
            return WithMapping(
                expectationMemberPath.GetMemberPath().ToString().WithoutSpecificCollectionIndices(),
                subjectMemberPath.GetMemberPath().ToString().WithoutSpecificCollectionIndices());
        }

        /// <summary>
        /// Maps a (nested) property or field of the expectation to a (nested) property or field of the subject using a path string.
        /// </summary>
        /// <param name="expectationMemberPath">
        /// A field or property path indicating the (nested) member to map from in the format <c>Parent.Child.Collection[].Member</c>.
        /// </param>
        /// <param name="subjectMemberPath">
        /// A field or property path indicating the (nested) member to map to in the format <c>Parent.Child.Collection[].Member</c>.
        /// </param>
        /// <remarks>
        /// The members of the subject and the expectation must have the same parent. Also, indexes in collections are not allowed
        /// and must be written as "[]".  If the types of the members are different, the usual logic applies depending or not
        /// if conversion options were specified.
        /// Fields can be mapped to properties and vice-versa.
        /// </remarks>
        public EquivalencyAssertionOptions<TExpectation> WithMapping(
            string expectationMemberPath,
            string subjectMemberPath)
        {
            AddMatchingRule(new MappedPathMatchingRule(expectationMemberPath, subjectMemberPath));

            return this;
        }

        /// <summary>
        /// Maps a direct property or field of type <typeparamref name="TNestedExpectation"/> to
        /// a direct property or field of <typeparamref name="TNestedSubject"/> using lambda expressions.
        /// </summary>
        /// <param name="expectationMember">A field or property expression indicating the member to map from.</param>
        /// <param name="subjectMember">A field or property expression indicating the member to map to.</param>
        /// <remarks>
        /// Only direct members of <typeparamref name="TNestedExpectation"/> and <typeparamref name="TNestedSubject"/> can be
        /// mapped to each other. Those types can appear anywhere in the object graphs that are being compared.
        /// If the types of the members are different, the usual logic applies depending or not if conversion options were specified.
        /// Fields can be mapped to properties and vice-versa.
        /// </remarks>
        public EquivalencyAssertionOptions<TExpectation> WithMapping<TNestedExpectation, TNestedSubject>(
            Expression<Func<TNestedExpectation, object>> expectationMember,
            Expression<Func<TNestedSubject, object>> subjectMember)
        {
            return WithMapping<TNestedExpectation, TNestedSubject>(
                expectationMember.GetMemberPath().ToString(),
                subjectMember.GetMemberPath().ToString());
        }

        /// <summary>
        /// Maps a direct property or field of type <typeparamref name="TNestedExpectation"/> to
        /// a direct property or field of <typeparamref name="TNestedSubject"/> using member names.
        /// </summary>
        /// <param name="expectationMemberName">A field or property name indicating the member to map from.</param>
        /// <param name="subjectMemberName">A field or property name indicating the member to map to.</param>
        /// <remarks>
        /// Only direct members of <typeparamref name="TNestedExpectation"/> and <typeparamref name="TNestedSubject"/> can be
        /// mapped to each other, so no <c>.</c> or <c>[]</c> are allowed.
        /// Those types can appear anywhere in the object graphs that are being compared.
        /// If the types of the members are different, the usual logic applies depending or not if conversion options were specified.
        /// Fields can be mapped to properties and vice-versa.
        /// </remarks>
        public EquivalencyAssertionOptions<TExpectation> WithMapping<TNestedExpectation, TNestedSubject>(
            string expectationMemberName,
            string subjectMemberName)
        {
            AddMatchingRule(new MappedMemberMatchingRule<TNestedExpectation, TNestedSubject>(
                expectationMemberName,
                subjectMemberName));

            return this;
        }
    }

    /// <summary>
    /// Represents the run-time type-agnostic behavior of a structural equivalency assertion.
    /// </summary>
    public class EquivalencyAssertionOptions : SelfReferenceEquivalencyAssertionOptions<EquivalencyAssertionOptions>
    {
        public EquivalencyAssertionOptions()
        {
            IncludingNestedObjects();

            IncludingFields();
            IncludingProperties();

            RespectingDeclaredTypes();
        }
    }
}
