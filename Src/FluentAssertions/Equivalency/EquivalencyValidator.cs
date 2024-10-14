using System;
using FluentAssertions.Equivalency.Tracing;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency;

/// <summary>
/// Is responsible for validating the equivalency of a subject with another object.
/// </summary>
public class EquivalencyValidator : IValidateChildNodeEquivalency
{
    private const int MaxDepth = 10;

    public void AssertEquality(Comparands comparands, EquivalencyValidationContext context)
    {
        using var scope = new AssertionScope();

        RecursivelyAssertEquivalencyOf(comparands, context);

        if (context.TraceWriter is not null)
        {
            scope.AppendTracing(context.TraceWriter.ToString());
        }
    }

    private void RecursivelyAssertEquivalencyOf(Comparands comparands, IEquivalencyValidationContext context)
    {
        AssertEquivalencyOf(comparands, context);
    }

    public void AssertEquivalencyOf(Comparands comparands, IEquivalencyValidationContext context)
    {
        var assertionChain = AssertionChain.GetOrCreate()
            .For(context)
            .BecauseOf(context.Reason);

        if (ShouldContinueThisDeep(context.CurrentNode, context.Options, assertionChain))
        {
            if (!context.IsCyclicReference(comparands.Expectation))
            {
                TryToProveNodesAreEquivalent(comparands, context);
            }
            else if (context.Options.CyclicReferenceHandling == CyclicReferenceHandling.ThrowException)
            {
                assertionChain.FailWith("Expected {context:subject} to be {expectation}{reason}, but it contains a cyclic reference.");
            }
            else
            {
                // If cyclic references are allowed, we consider the objects to be equivalent
            }
        }
    }

    private static bool ShouldContinueThisDeep(INode currentNode, IEquivalencyOptions options,
        AssertionChain assertionChain)
    {
        bool shouldRecurse = options.AllowInfiniteRecursion || currentNode.Depth <= MaxDepth;
        if (!shouldRecurse)
        {
            // This will throw, unless we're inside an AssertionScope
            assertionChain.FailWith($"The maximum recursion depth of {MaxDepth} was reached.  ");
        }

        return shouldRecurse;
    }

    private void TryToProveNodesAreEquivalent(Comparands comparands, IEquivalencyValidationContext context)
    {
        using var _ = context.Tracer.WriteBlock(node => node.Description);

        foreach (IEquivalencyStep step in AssertionOptions.EquivalencyPlan)
        {
            var result = step.Handle(comparands, context, this);
            if (result == EquivalencyResult.EquivalencyProven)
            {
                context.Tracer.WriteLine(GetMessage(step));

                static GetTraceMessage GetMessage(IEquivalencyStep step) =>
                    _ => $"Equivalency was proven by {step.GetType().Name}";

                return;
            }
        }

        throw new NotSupportedException(
            $"Do not know how to compare {comparands.Subject} and {comparands.Expectation}. Please report an issue through https://www.fluentassertions.com.");
    }
}
