using System;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Is responsible for validating the equality of one or more properties of a subject with another object.
    /// </summary>
    public class EquivalencyValidator : IEquivalencyValidator
    {
        private const int MaxDepth = 10;

        public void AssertEquality(Comparands comparands, EquivalencyValidationContext context)
        {
            using var scope = new AssertionScope();
            scope.AddReportable("configuration", context.Options.ToString());
            scope.BecauseOf(context.Reason);

            RecursivelyAssertEquality(comparands, context);

            if (context.TraceWriter is not null)
            {
                scope.AddReportable("trace", context.TraceWriter.ToString());
            }
        }

        public void RecursivelyAssertEquality(Comparands comparands, IEquivalencyValidationContext context)
        {
            var scope = AssertionScope.Current;

            if (ShouldCompareMembersThisDeep(context.CurrentNode, context.Options, scope))
            {
                UpdateScopeWithReportableContext(scope, comparands, context.CurrentNode);

                if (!context.IsCyclicReference(comparands.Expectation))
                {
                    RunStepsUntilEquivalencyIsProven(comparands, context);
                }
            }
        }

        private static bool ShouldCompareMembersThisDeep(INode currentNode, IEquivalencyAssertionOptions options,
            AssertionScope assertionScope)
        {
            bool shouldRecurse = options.AllowInfiniteRecursion || currentNode.Depth < MaxDepth;
            if (!shouldRecurse)
            {
                assertionScope.FailWith("The maximum recursion depth was reached.  ");
            }

            return shouldRecurse;
        }

        private static void UpdateScopeWithReportableContext(AssertionScope scope, Comparands comparands, INode currentNode)
        {
            scope.Context = new Lazy<string>(() => currentNode.Description);

            scope.TrackComparands(comparands.Subject, comparands.Expectation);
        }

        private void RunStepsUntilEquivalencyIsProven(Comparands comparands, IEquivalencyValidationContext context)
        {
            foreach (IEquivalencyStep step in AssertionOptions.EquivalencyPlan)
            {
                if (step.Handle(comparands, context, this) == EquivalencyResult.AssertionCompleted)
                {
                    return;
                }
            }

            throw new NotImplementedException(
                $"Do not know how to compare {comparands.Subject} and {comparands.Expectation}. Please report an issue through https://www.fluentassertions.com.");
        }
    }
}
