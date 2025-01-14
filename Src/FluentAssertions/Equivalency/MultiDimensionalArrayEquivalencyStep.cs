using System;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency;

/// <summary>
/// Supports recursively comparing two multi-dimensional arrays for equivalency using strict order for the array items
/// themselves.
/// </summary>
internal class MultiDimensionalArrayEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
    {
        if (comparands.Expectation is not Array expectationAsArray || expectationAsArray.Rank == 1)
        {
            return EquivalencyResult.ContinueWithNext;
        }

        if (AreComparable(comparands, expectationAsArray, AssertionChain.GetOrCreate().For(context)))
        {
            if (expectationAsArray.Length == 0)
            {
                return EquivalencyResult.EquivalencyProven;
            }

            Digit digit = BuildDigitsRepresentingAllIndices(expectationAsArray);

            do
            {
                int[] indices = digit.GetIndices();
                object subject = ((Array)comparands.Subject).GetValue(indices);
                string listOfIndices = string.Join(",", indices);
                object expectation = expectationAsArray.GetValue(indices);

                IEquivalencyValidationContext itemContext = context.AsCollectionItem<object>(listOfIndices);

                valueChildNodes.AssertEquivalencyOf(new Comparands(subject, expectation, typeof(object)),
                    itemContext);
            }
            while (digit.Increment());
        }

        return EquivalencyResult.EquivalencyProven;
    }

    private static Digit BuildDigitsRepresentingAllIndices(Array subjectAsArray)
    {
        return Enumerable
            .Range(0, subjectAsArray.Rank)
            .Reverse()
            .Aggregate((Digit)null, (next, rank) => new Digit(subjectAsArray.GetLength(rank), next));
    }

    private static bool AreComparable(Comparands comparands, Array expectationAsArray, AssertionChain assertionChain)
    {
        return
            IsArray(comparands.Subject, assertionChain) &&
            HaveSameRank(comparands.Subject, expectationAsArray, assertionChain) &&
            HaveSameDimensions(comparands.Subject, expectationAsArray, assertionChain);
    }

    private static bool IsArray(object type, AssertionChain assertionChain)
    {
        assertionChain
            .ForCondition(type is not null)
            .FailWith("Cannot compare a multi-dimensional array to <null>.")
            .Then
            .ForCondition(type is Array)
            .FailWith("Cannot compare a multi-dimensional array to something else.");

        return assertionChain.Succeeded;
    }

    private static bool HaveSameDimensions(object subject, Array expectation, AssertionChain assertionChain)
    {
        bool sameDimensions = true;

        for (int dimension = 0; dimension < expectation.Rank; dimension++)
        {
            int actualLength = ((Array)subject).GetLength(dimension);
            int expectedLength = expectation.GetLength(dimension);

            assertionChain
                .ForCondition(expectedLength == actualLength)
                .FailWith("Expected dimension {0} to contain {1} item(s), but found {2}.", dimension, expectedLength,
                    actualLength);

            sameDimensions &= assertionChain.Succeeded;
        }

        return sameDimensions;
    }

    private static bool HaveSameRank(object subject, Array expectation, AssertionChain assertionChain)
    {
        var subjectAsArray = (Array)subject;

        assertionChain
            .ForCondition(subjectAsArray.Rank == expectation.Rank)
            .FailWith("Expected {context:array} to have {0} dimension(s), but it has {1}.", expectation.Rank,
                subjectAsArray.Rank);

        return assertionChain.Succeeded;
    }
}
