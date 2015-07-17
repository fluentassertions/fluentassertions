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
            Array array = context.Subject as Array;
            return (array != null) && (array.Rank > 1);
        }

        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config)
        {
            Array subjectAsArray = (Array) context.Subject;

            if (AreComparable(context, subjectAsArray))
            {
                Digit digit = BuildDigitsRepresentingAllIndices(subjectAsArray);

                do
                {
                    var expectation = ((Array) context.Expectation).GetValue(digit.Indices);
                    IEquivalencyValidationContext itemContext = context.CreateForCollectionItem(
                        string.Join(",", digit.Indices),
                        subjectAsArray.GetValue(digit.Indices),
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
                .Aggregate((Digit) null, (next, rank) => new Digit(subjectAsArray.GetLength(rank), next));
        }

        private static bool AreComparable(IEquivalencyValidationContext context, Array subjectAsArray)
        {
            return
                IsArray(context.Expectation) &&
                HaveSameRank(context.Expectation, subjectAsArray) &&
                HaveSameDimensions(context.Expectation, subjectAsArray);
        }

        private static bool IsArray(object expectation)
        {
            return AssertionScope.Current
                .ForCondition(!ReferenceEquals(expectation, null))
                .FailWith("Can't compare a multi-dimensional array to {0}.", new object[] {null})
                .Then
                .ForCondition(expectation is Array)
                .FailWith("Can't compare a multi-dimensional array to something else.");
        }

        private static bool HaveSameDimensions(object expectation, Array actual)
        {
            bool sameDimensions = true;

            for (int dimension = 0; dimension < actual.Rank; dimension++)
            {
                int expectedLength = ((Array) expectation).GetLength(dimension);
                int actualLength = actual.GetLength(dimension);

                sameDimensions = sameDimensions & AssertionScope.Current
                    .ForCondition(expectedLength == actualLength)
                    .FailWith("Expected dimension {0} to contain {1} item(s), but found {2}.", dimension, expectedLength,
                        actualLength);
            }

            return sameDimensions;
        }

        private static bool HaveSameRank(object expectation, Array actual)
        {
            var expectationAsArray = (Array) expectation;

            return AssertionScope.Current
                .ForCondition(actual.Rank == expectationAsArray.Rank)
                .FailWith("Expected {context:array} to have {0} dimension(s), but it has {1}.", expectationAsArray.Rank,
                    actual.Rank);
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

        public int[] Indices
        {
            get
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
        }

        public bool Increment()
        {
            bool success = (nextDigit != null) && nextDigit.Increment();
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