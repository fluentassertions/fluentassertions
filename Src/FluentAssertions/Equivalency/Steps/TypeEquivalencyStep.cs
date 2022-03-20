using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps
{
    /// <summary>
    /// A equivalency step for Fluent Assertions that asserts an object must be the same type to be equivalent.
    /// This differs from Fluent Assertions default equivalency assertion that states 2 objects are equivalent if the have the same properties and values, reguardless of type.
    /// Useful when working with polymorphic models that may or may not have inner properties. - https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/polymorphism
    /// </summary>
    public class TypeEquivalencyStep : IEquivalencyStep
    {
        public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
        {
            // When comparing using a collection we want to compare the children of the collection not the collection itself (the root object)
            // Value Types should be directly compared otherwise `GetSelectedMembers(comparands, context).Any()` will return false and skip actual value checking
            if (comparands.Subject is IEnumerable || comparands.RuntimeType.IsValueType)
            {
                return EquivalencyResult.ContinueWithNext;
            }

            // If both are null or same reference or are comparable, no need to have this step check anything
            if (comparands.Subject == comparands.Expectation)
            {
                return EquivalencyResult.ContinueWithNext;
            }

            // The above checked if both were null, if only 1 is null then no need to have this step check anything
            if (comparands.Subject == null || comparands.Expectation == null)
            {
                return EquivalencyResult.ContinueWithNext;
            }

            Type expectedType = comparands.GetExpectedType(context.Options);
            
            Execute.Assertion
                .ForCondition(comparands.Subject.GetType() == expectedType)
                .FailWith("Expected {context:subject} to be of type {0}, but found {1}", expectedType, comparands.Subject.GetType());

            return EquivalencyResult.ContinueWithNext;
        }

        /// <summary>
        /// The members (fields and properties) on the context subject that is used to compare structural equivalency.
        /// </summary>
        private static IEnumerable<IMember> GetSelectedMembers(Comparands comparands, IEquivalencyValidationContext context)
        {
            IEnumerable<IMember> members = Enumerable.Empty<IMember>();

            foreach (IMemberSelectionRule selectionRule in context.Options.SelectionRules)
            {
                members = selectionRule.SelectMembers(context.CurrentNode, members, new MemberSelectionContext(comparands.CompileTimeType, comparands.RuntimeType, context.Options));
            }

            return members;
        }
    }
}
