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
    /// <summary>
    /// Represents the run-time type-specific behavior of a structural equivalency assertion.
    /// </summary>
    public class EquivalencyAssertionOptions<TSubject> :
        SelfReferenceEquivalencyAssertionOptions<EquivalencyAssertionOptions<TSubject>>
    {
        public EquivalencyAssertionOptions()
        {
        }

        internal EquivalencyAssertionOptions(IEquivalencyAssertionOptions defaults) : base(defaults)
        {
        }

        /// <summary>
        /// Excludes the specified (nested) member from the structural equality check.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> Excluding(Expression<Func<TSubject, object>> expression)
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
        public EquivalencyAssertionOptions<TSubject> Including(Expression<Func<TSubject, object>> expression)
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
        public EquivalencyAssertionOptions<TSubject> Including(Expression<Func<ISubjectInfo, bool>> predicate)
        {
            AddSelectionRule(new IncludeMemberByPredicateSelectionRule(predicate));
            return this;
        }

        /// <summary>
        /// Causes the collection identified by <paramref name="expression"/> to be compared in the order 
        /// in which the items appear in the expectation.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> WithStrictOrderingFor(
            Expression<Func<TSubject, object>> expression)
        {
            orderingRules.Add(new PathBasedOrderingRule(expression.GetMemberPath()));
            return this;
        }

        /// <summary>
        /// Creates a new set of options based on the current instance which acts on a <see cref="IEnumerable{TSubject}"/>
        /// </summary>
        /// <returns></returns>
        public EquivalencyAssertionOptions<IEnumerable<TSubject>> AsCollection()
        {
            return new EquivalencyAssertionOptions<IEnumerable<TSubject>>(
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