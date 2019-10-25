using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Supports recursively comparing two multi-dimensional arrays for equivalency using strict order for the array items
    /// themselves.
    /// </summary>
    internal class MultiDimensionalArrayEquivalencyStep : IEquivalencyStep
    {
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Array array = context.Expectation as Array;
            return array?.Rank > 1;
        }

        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config)
        {
            Array expectationAsArray = (Array)context.Expectation;

            if (AreComparable(context, expectationAsArray))
            {
                if (expectationAsArray.Length == 0)
                {
                    return true;
                }

                Digit digit = BuildDigitsRepresentingAllIndices(expectationAsArray);

                do
                {
                    int[] indices = digit.GetIndices();
                    object subject = ((Array)context.Subject).GetValue(indices);
                    string listOfIndices = string.Join(",", indices);
                    object expectation = expectationAsArray.GetValue(indices);
                    IEquivalencyValidationContext itemContext = context.CreateForCollectionItem(
                        listOfIndices,
                        subject,
                        expectation);

                    parent.AssertEqualityUsing(itemContext);
                }
                while (digit.Increment());
            }

            return true;
        }

        private static Digit BuildDigitsRepresentingAllIndices(Array subjectAsArray)
        {
            return Enumerable
                .Range(0, subjectAsArray.Rank)
                .Reverse()
                .Aggregate((Digit)null, (next, rank) => new Digit(subjectAsArray.GetLength(rank), next));
        }

        private static bool AreComparable(IEquivalencyValidationContext context, Array expectationAsArray)
        {
            return
                IsArray(context.Subject) &&
                HaveSameRank(context.Subject, expectationAsArray) &&
                HaveSameDimensions(context.Subject, expectationAsArray);
        }

        private static bool IsArray(object type)
        {
            return AssertionScope.Current
                .ForCondition(!(type is null))
                .FailWith("Cannot compare a multi-dimensional array to <null>.")
                .Then
                .ForCondition(type is Array)
                .FailWith("Cannot compare a multi-dimensional array to something else.");
        }

        private static bool HaveSameDimensions(object subject, Array expectation)
        {
            bool sameDimensions = true;

            for (int dimension = 0; dimension < expectation.Rank; dimension++)
            {
                int actualLength = ((Array)subject).GetLength(dimension);
                int expectedLength = expectation.GetLength(dimension);

                sameDimensions &= AssertionScope.Current
                    .ForCondition(expectedLength == actualLength)
                    .FailWith("Expected dimension {0} to contain {1} item(s), but found {2}.", dimension, expectedLength,
                        actualLength);
            }

            return sameDimensions;
        }

        private static bool HaveSameRank(object subject, Array expectation)
        {
            var subjectAsArray = (Array)subject;

            return AssertionScope.Current
                .ForCondition(subjectAsArray.Rank == expectation.Rank)
                .FailWith("Expected {context:array} to have {0} dimension(s), but it has {1}.", expectation.Rank,
                    subjectAsArray.Rank);
        }
    }

    internal class Digit
    {
        private readonly int length;
        private readonly Digit nextDigit;
        private int index;

        public Digit(int length, Digit nextDigit)
        {
            this.length = length;
            this.nextDigit = nextDigit;
        }

        public int[] GetIndices()
        {
            var indices = new List<int>();

            Digit digit = this;
            while (digit != null)
            {
                indices.Add(digit.index);
                digit = digit.nextDigit;
            }

            return indices.ToArray();
        }

        public bool Increment()
        {
            bool success = nextDigit?.Increment() == true;
            if (!success)
            {
                if (index < (length - 1))
                {
                    index++;
                    success = true;
                }
                else
                {
                    index = 0;
                }
            }

            return success;
        }
    }
}
