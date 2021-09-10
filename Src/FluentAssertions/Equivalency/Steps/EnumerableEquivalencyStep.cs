using System;
using System.Collections;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps
{
    public class EnumerableEquivalencyStep : IEquivalencyStep
    {
        public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
        {
            if (!IsCollection(comparands.GetExpectedType(context.Options)))
            {
                return EquivalencyResult.ContinueWithNext;
            }

            if (AssertSubjectIsCollection(comparands.Subject))
            {
                var validator = new EnumerableEquivalencyValidator(nestedValidator, context)
                {
                    Recursive = context.CurrentNode.IsRoot || context.Options.IsRecursive,
                    OrderingRules = context.Options.OrderingRules
                };

                validator.Execute(ToArray(comparands.Subject), ToArray(comparands.Expectation));
            }

            return EquivalencyResult.AssertionCompleted;
        }

        private static bool AssertSubjectIsCollection(object subject)
        {
            bool conditionMet = AssertionScope.Current
                .ForCondition(subject is not null)
                .FailWith("Expected a collection, but {context:Subject} is <null>.");

            if (conditionMet)
            {
                conditionMet = AssertionScope.Current
                .ForCondition(IsCollection(subject.GetType()))
                .FailWith("Expected a collection, but {context:Subject} is of a non-collection type.");
            }

            return conditionMet;
        }

        private static bool IsCollection(Type type)
        {
            return !typeof(string).IsAssignableFrom(type) && typeof(IEnumerable).IsAssignableFrom(type);
        }

        internal static object[] ToArray(object value)
        {
            if (value == null)
            {
                return null;
            }

            try
            {
                return ((IEnumerable)value).Cast<object>().ToArray();
            }
            catch (InvalidOperationException) when (value.GetType().Name.Equals("ImmutableArray`1", StringComparison.Ordinal))
            {
                // This is probably a default ImmutableArray<T>
                return Array.Empty<object>();
            }
        }
    }
}
