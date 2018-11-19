#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions.Common;
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

        internal EquivalencyAssertionOptions(IEquivalencyAssertionOptions defaults)
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
        /// Includes the specified member in the equality check.
        /// </summary>
        /// <remarks>
        /// This overrides the default behavior of including all declared members.
        /// </remarks>
        public EquivalencyAssertionOptions<TExpectation> Including(Expression<Func<IMemberInfo, bool>> predicate)
        {
            AddSelectionRule(new IncludeMemberByPredicateSelectionRule(predicate));
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
            orderingRules.Add(new PathBasedOrderingRule(expressionMemberPath));
            return this;
        }

        /// <summary>
        /// Creates a new set of options based on the current instance which acts on a <see cref="IEnumerable{TSubject}"/>
        /// </summary>
        /// <returns></returns>
        public EquivalencyAssertionOptions<IEnumerable<TExpectation>> AsCollection()
        {
            return new EquivalencyAssertionOptions<IEnumerable<TExpectation>>(
                new CollectionMemberAssertionOptionsDecorator(this));
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
