using System;
using FluentAssertions.Equivalency.Tracing;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency;

/// <summary>
/// Is responsible for validating the equivalency of a subject with another object.
/// </summary>
internal class EquivalencyValidator : IValidateChildNodeEquivalency
{
    private const int MaxDepth = 10;

    public void AssertEquality(Comparands comparands, EquivalencyValidationContext context)
    {
        context.ResetTracing();

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
                assertionChain.FailWith("Expected {context:subject} to be {0}{reason}, but it contains a cyclic reference.", comparands.Expectation);
            }
            else
            {
                AssertEquivalencyForCyclicReference(comparands, assertionChain);
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

    private static void AssertEquivalencyForCyclicReference(Comparands comparands, AssertionChain assertionChain)
    {
        // We know that at this point the expectation is a non-null cyclic reference, so we don't want to continue the recursion.
        // We still want to compare the subject with the expectation though.

        // If they point at the same object, then equality is proven, and it doesn't matter that there's a cyclic reference.
        if (ReferenceEquals(comparands.Subject, comparands.Expectation))
        {
            return;
        }

        // If the expectation is non-null and the subject isn't, they would never be equivalent, regardless of how we deal with cyclic references,
        // so we can just throw an exception here.
        if (comparands.Subject is null)
        {
            assertionChain.ReuseOnce();
            comparands.Subject.Should().BeSameAs(comparands.Expectation);
        }

        // If they point at different objects, and the expectation is a cyclic reference, we would never be
        // able to prove that they are equal. And since we're supposed to ignore cyclic references, we can just return here.
    }

    private void TryToProveNodesAreEquivalent(Comparands comparands, IEquivalencyValidationContext context)
    {
        using var _ = context.Tracer.WriteBlock(node => node.Expectation.Description);

        foreach (IEquivalencyStep step in AssertionConfiguration.Current.Equivalency.Plan)
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
