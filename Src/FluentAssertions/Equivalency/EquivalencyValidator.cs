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

        var assertion = AssertionChain.GetOrCreate();

        assertion.WithReportable("configuration", () => context.Options.ToString());
        assertion.BecauseOf(context.Reason);

        RecursivelyAssertEquivalencyOf(comparands, assertion, context);

        if (context.TraceWriter is not null)
        {
            assertion.AppendTracing(context.TraceWriter.ToString());
        }
    }

    private void RecursivelyAssertEquivalencyOf(Comparands comparands, AssertionChain assertionChain, IEquivalencyValidationContext context)
    {
        AssertEquivalencyOf(comparands, assertionChain, context);
    }

    public void AssertEquivalencyOf(Comparands comparands, AssertionChain assertionChain, IEquivalencyValidationContext context)
    {
        if (ShouldContinueThisDeep(context.CurrentNode, context.Options, assertionChain))
        {
            TrackWhatIsNeededToProvideContextToFailures(assertionChain, comparands, context.CurrentNode);

            if (!context.IsCyclicReference(comparands.Expectation))
            {
                TryToProveNodesAreEquivalent(assertionChain, comparands, context);
            }
            else if (context.Options.CyclicReferenceHandling == CyclicReferenceHandling.ThrowException)
            {
                assertionChain
                    .BecauseOf(context.Reason)
                    .FailWith("Expected {context:subject} to be {expectation}{reason}, but it contains a cyclic reference.");
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

    private static void TrackWhatIsNeededToProvideContextToFailures(AssertionChain assertionChain, Comparands comparands, INode currentNode)
    {
        var getCallerIdentifier = new Lazy<string>(() => currentNode.Description);
        assertionChain.OverrideCallerIdentifier(() => getCallerIdentifier.Value);
        assertionChain.TrackComparands(comparands.Subject, comparands.Expectation);
    }

    private void TryToProveNodesAreEquivalent(AssertionChain assertionChain, Comparands comparands, IEquivalencyValidationContext context)
    {
        using var _ = context.Tracer.WriteBlock(node => node.Description);

        foreach (IEquivalencyStep step in AssertionOptions.EquivalencyPlan)
        {
            var result = step.Handle(comparands, assertionChain, context, this);
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
