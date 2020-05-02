using System;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    internal class StringEqualityValidator : StringValidator
    {
        private readonly StringComparison comparisonMode;

        public StringEqualityValidator(string subject, string expected, StringComparison comparisonMode, string because,
            object[] becauseArgs)
            : base(subject, expected, because, becauseArgs)
        {
            this.comparisonMode = comparisonMode;
        }

        protected override bool ValidateAgainstSuperfluousWhitespace()
        {
            return Assertion
                .ForCondition(!((Expected.Length > Subject.Length) && Expected.TrimEnd().Equals(Subject, comparisonMode)))
                .FailWith(ExpectationDescription + "{0}{reason}, but it misses some extra whitespace at the end.", Expected)
                .Then
                .ForCondition(!((Subject.Length > Expected.Length) && Subject.TrimEnd().Equals(Expected, comparisonMode)))
                .FailWith(ExpectationDescription + "{0}{reason}, but it has unexpected whitespace at the end.", Expected)
                .SourceSucceeded;
        }

        protected override bool ValidateAgainstLengthDifferences()
        {
            return Assertion
                .ForCondition(Subject.Length == Expected.Length)
                .FailWith(() =>
                {
                    string mismatchSegment = GetMismatchSegmentForStringsOfDifferentLengths();

                    string message = ExpectationDescription +
                                        "{0} with a length of {1}{reason}, but {2} has a length of {3}, differs near " + mismatchSegment + ".";

                    return new FailReason(message, Expected, Expected.Length, Subject, Subject.Length);
                })
                .SourceSucceeded;
        }

        private string GetMismatchSegmentForStringsOfDifferentLengths()
        {
            int indexOfMismatch = Subject.IndexOfFirstMismatch(Expected, comparisonMode);

            // If there is no difference it means that either
            // * subject starts with expected or
            // * expected starts with subject
            if (indexOfMismatch == -1)
            {
                // If subject is shorter we are sure that expected starts with subject
                if (Subject.Length < Expected.Length)
                {
                    // Subject is shorter so we point at its last character.
                    // We would like to point at next character as it is the real
                    // index of first mismatch, but we need to point at character existing in
                    // subject, so the next best thing is the last subject character.
                    indexOfMismatch = Math.Max(0, Subject.Length - 1);
                }
                else
                {
                    // If subject is longer we are sure that subject starts with expected
                    // and we point at first character after expected.
                    indexOfMismatch = Expected.Length;
                }
            }

            return Subject.IndexedSegmentAt(indexOfMismatch);
        }

        protected override void ValidateAgainstMismatch()
        {
            int indexOfMismatch = Subject.IndexOfFirstMismatch(Expected, comparisonMode);
            if (indexOfMismatch != -1)
            {
                Assertion.FailWith(
                    ExpectationDescription + "{0}{reason}, but {1} differs near " + Subject.IndexedSegmentAt(indexOfMismatch) + ".",
                    Expected, Subject);
            }
        }

        protected override string ExpectationDescription
        {
            get
            {
                string predicateDescription = IgnoreCase ? "be equivalent to" : "be";
                return "Expected {context:string} to " + predicateDescription + " ";
            }
        }

        private bool IgnoreCase
        {
            get
            {
                return comparisonMode == StringComparison.OrdinalIgnoreCase;
            }
        }
    }
}
