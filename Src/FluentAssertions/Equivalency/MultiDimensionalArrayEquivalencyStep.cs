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
                .FailWith(Resources.Array_CannotCompareMultiDimArrayToXFormat, new object[] { null })
                .Then
                .ForCondition(type is Array)
                .FailWith(Resources.Array_CannotCompareMultiDimArrayToSomethingElse);
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
                    .FailWith(Resources.Array_ExpectedDimensionXToContainYItemsFormat + Resources.Common_CommaButFoundZFormat,
                        dimension, expectedLength, actualLength);
            }

            return sameDimensions;
        }

        private static bool HaveSameRank(object subject, Array expectation)
        {
            var subjectAsArray = (Array)subject;

            return AssertionScope.Current
                .ForCondition(subjectAsArray.Rank == expectation.Rank)
                .FailWith(Resources.Array_ExpectedArrayToHaveXDimensionsButItHasYFormat, expectation.Rank,
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
