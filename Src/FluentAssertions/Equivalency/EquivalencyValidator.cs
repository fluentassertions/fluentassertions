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

        var getIdentifierOnce = new Lazy<string>(() => scope.GetIdentifier());

        var assertion = Assertion.GetOrCreate(() => scope, () => getIdentifierOnce.Value);

        assertion.WithReportable("configuration", () => context.Options.ToString());
        assertion.BecauseOf(context.Reason);

        RecursivelyAssertEquivalencyOf(comparands, assertion, context);

        if (context.TraceWriter is not null)
        {
            assertion.AppendTracing(context.TraceWriter.ToString());
        }
    }

    private void RecursivelyAssertEquivalencyOf(Comparands comparands, Assertion assertion, IEquivalencyValidationContext context)
    {
        AssertEquivalencyOf(comparands, assertion, context);
    }

    public void AssertEquivalencyOf(Comparands comparands, Assertion assertion, IEquivalencyValidationContext context)
    {
        if (ShouldContinueThisDeep(context.CurrentNode, context.Options, assertion))
        {
            TrackWhatIsNeededToProvideContextToFailures(assertion, comparands, context.CurrentNode);

            if (!context.IsCyclicReference(comparands.Expectation))
            {
                TryToProveNodesAreEquivalent(assertion, comparands, context);
            }
        }
    }

    private static bool ShouldContinueThisDeep(INode currentNode, IEquivalencyOptions options,
        Assertion assertion)
    {
        bool shouldRecurse = options.AllowInfiniteRecursion || currentNode.Depth <= MaxDepth;
        if (!shouldRecurse)
        {
            // This will throw, unless we're inside an AssertionScope
            assertion.FailWith($"The maximum recursion depth of {MaxDepth} was reached.  ");
        }

        return shouldRecurse;
    }

    private static void TrackWhatIsNeededToProvideContextToFailures(Assertion assertion, Comparands comparands, INode currentNode)
    {
        assertion.Context = new Lazy<string>(() => currentNode.Description);
        assertion.TrackComparands(comparands.Subject, comparands.Expectation);
    }

    private void TryToProveNodesAreEquivalent(Assertion assertion, Comparands comparands, IEquivalencyValidationContext context)
    {
        using var _ = context.Tracer.WriteBlock(node => node.Description);

        Func<IEquivalencyStep, GetTraceMessage> getMessage = step => _ => $"Equivalency was proven by {step.GetType().Name}";

        foreach (IEquivalencyStep step in AssertionOptions.EquivalencyPlan)
        {
            var result = step.Handle(comparands, assertion, context, this);
            if (result == EquivalencyResult.EquivalencyProven)
            {
                context.Tracer.WriteLine(getMessage(step));
                return;
            }
        }

        throw new NotSupportedException(
            $"Do not know how to compare {comparands.Subject} and {comparands.Expectation}. Please report an issue through https://www.fluentassertions.com.");
    }
}
