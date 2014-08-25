using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    internal class StructuralEqualityEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(EquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return (context.IsRoot || config.IsRecursive);
        }

        /// <summary>
        /// Applies a step as part of the task to compare two objects for structural equality.
        /// </summary>
        /// <value>
        /// Should return <c>true</c> if the subject matches the expectation or if no additional assertions
        /// have to be executed. Should return <c>false</c> otherwise.
        /// </value>
        /// <remarks>
        /// May throw when preconditions are not met or if it detects mismatching data.
        /// </remarks>
        public bool Handle(EquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            bool expectationIsNotNull = AssertionScope.Current
                .ForCondition(!ReferenceEquals(context.Expectation, null))
                .FailWith(
                    "Expected {context:subject} to be <null>, but found {0}.",
                    context.Subject);

            bool subjectIsNotNull =
                AssertionScope.Current.ForCondition(
                    !ReferenceEquals(context.Subject, null))
                    .FailWith(
                        "Expected {context:object} to be {0}{reason}, but found {1}.",
                        context.Expectation,
                        context.Subject);

            IEnumerable<PropertyInfo> selectedProperties = GetSelectedProperties(context, config).ToArray();
            if (context.IsRoot && !selectedProperties.Any())
            {
                throw new InvalidOperationException(
                    "No properties were found for comparison. " +
                    "Please specify some properties to include in the comparison or choose a more meaningful assertion.");
            }

            if (expectationIsNotNull && subjectIsNotNull)
            {
                foreach (PropertyInfo propertyInfo in selectedProperties)
                {
                    AssertPropertyEquality(context, parent, propertyInfo, config);
                }
            }
            return true;
        }

        private void AssertPropertyEquality(EquivalencyValidationContext context, IEquivalencyValidator parent, PropertyInfo propertyInfo, IEquivalencyAssertionOptions config)
        {
            var matchingProperty = FindMatchFor(propertyInfo, context, config.MatchingRules);
            if (matchingProperty != null)
            {
                EquivalencyValidationContext nestedContext = context.CreateForNestedProperty(propertyInfo, matchingProperty);
                if (nestedContext != null)
                {
                    parent.AssertEqualityUsing(nestedContext);
                }
            }
        }

        private PropertyInfo FindMatchFor(PropertyInfo propertyInfo, EquivalencyValidationContext context, IEnumerable<IMatchingRule> matchingRules)
        {
            var query =
                from rule in matchingRules
                let match = rule.Match(propertyInfo, context.Expectation, context.PropertyDescription)
                where match != null
                select match;

            return query.FirstOrDefault();
        }

        internal IEnumerable<PropertyInfo> GetSelectedProperties(EquivalencyValidationContext context, 
            IEquivalencyAssertionOptions config)
        {
            IEnumerable<PropertyInfo> properties = Enumerable.Empty<PropertyInfo>();

            foreach (ISelectionRule selectionRule in config.SelectionRules)
            {
                properties = selectionRule.SelectProperties(properties, context);
            }

            return properties.Where(IsNotIndexer);
        }

        private static bool IsNotIndexer(PropertyInfo property)
        {
            return (property.GetIndexParameters().Length == 0);
        }
    }
}