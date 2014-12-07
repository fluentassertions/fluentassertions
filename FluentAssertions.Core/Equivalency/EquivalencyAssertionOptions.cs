#region

using System;
using System.Linq.Expressions;
using FluentAssertions.Common;

#endregion

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents the run-time type-specific behavior of a structural equivalency assertion.
    /// </summary>
    public class EquivalencyAssertionOptions<TSubject> :
        EquivalencyAssertionOptionsBase<EquivalencyAssertionOptions<TSubject>>
    {
        /// <summary>
        /// Gets a configuration that by default doesn't include any of the subject's properties and doesn't consider any nested objects
        /// or collections.
        /// </summary>
        [Obsolete(
            "Will be removed in v4.0. Instead, use AssertionOptions.AssertEquivalencyUsing method to setup the equivalency assertion defaults"
            )] public static Func<EquivalencyAssertionOptions<TSubject>> Empty =
                () => new EquivalencyAssertionOptions<TSubject>();

        /// <summary>
        /// Gets a configuration that compares all declared properties of the subject with equally named properties of the expectation,
        /// and includes the entire object graph. The names of the properties between the subject and expectation must match.
        /// </summary>
        [Obsolete(
            "Will be removed in v4.0. Instead, use AssertionOptions.AssertEquivalencyUsing method to setup the equivalency assertion defaults"
            )] public static Func<EquivalencyAssertionOptions<TSubject>> Default =
                () => AssertionOptions.CloneDefaults<TSubject>();

        public EquivalencyAssertionOptions()
        {
        }

        internal EquivalencyAssertionOptions(IEquivalencyAssertionOptions defaults) : base(defaults)
        {
        }

        /// <summary>
        /// Excludes the specified (nested) property from the structural equality check.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> Excluding(Expression<Func<TSubject, object>> propertyExpression)
        {
            AddSelectionRule(new ExcludePropertyByPathSelectionRule(propertyExpression.GetPropertyPath()));
            return this;
        }

        /// <summary>
        /// Includes the specified property in the equality check.
        /// </summary>
        /// <remarks>
        /// This overrides the default behavior of including all declared properties.
        /// </remarks>
        public EquivalencyAssertionOptions<TSubject> Including(Expression<Func<TSubject, object>> propertyExpression)
        {
            RemoveSelectionRule<AllDeclaredPublicPropertiesSelectionRule>();
            RemoveSelectionRule<AllRuntimePublicPropertiesSelectionRule>();

            AddSelectionRule(new IncludePropertyByPathSelectionRule(propertyExpression.GetPropertyInfo()));
            return this;
        }

        /// <summary>
        /// Includes the specified property in the equality check.
        /// </summary>
        /// <remarks>
        /// This overrides the default behavior of including all declared properties.
        /// </remarks>
        public EquivalencyAssertionOptions<TSubject> Including(Expression<Func<ISubjectInfo, bool>> predicate)
        {
            RemoveSelectionRule<AllDeclaredPublicPropertiesSelectionRule>();
            RemoveSelectionRule<AllRuntimePublicPropertiesSelectionRule>();

            AddSelectionRule(new IncludePropertyByPredicateSelectionRule(predicate));
            return this;
        }

        /// <summary>
        /// Causes the collection identified by <paramref name="propertyExpression"/> to be compared in the order 
        /// in which the items appear in the expectation.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> WithStrictOrderingFor(
            Expression<Func<TSubject, object>> propertyExpression)
        {
            orderingRules.Add(new PropertyPathOrderingRule(propertyExpression.GetPropertyPath()));
            return this;
        }
    }

    /// <summary>
    /// Represents the run-time type-agnostic behavior of a structural equivalency assertion.
    /// </summary>
    public class EquivalencyAssertionOptions : EquivalencyAssertionOptionsBase<EquivalencyAssertionOptions>
    {
        public EquivalencyAssertionOptions()
        {
            IncludingNestedObjects();
            IncludingAllDeclaredProperties();
        }
    }
}