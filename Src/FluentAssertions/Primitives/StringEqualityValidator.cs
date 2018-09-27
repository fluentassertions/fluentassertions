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
            return assertion
                .ForCondition(!((expected.Length > subject.Length) && expected.TrimEnd().Equals(subject, comparisonMode)))
                .FailWith(ExpectationDescription + "{0}{reason}, but it misses some extra whitespace at the end.", expected)
                .Then
                .ForCondition(!((subject.Length > expected.Length) && subject.TrimEnd().Equals(expected, comparisonMode)))
                .FailWith(ExpectationDescription + "{0}{reason}, but it has unexpected whitespace at the end.", expected)
                .SourceSucceeded;
        }

        protected override bool ValidateAgainstLengthDifferences()
        {
            return assertion
                .ForCondition(subject.Length == expected.Length)
                .FailWith(() =>
                    {
                        string mismatchSegment = GetMismatchSegmentForStringsOfDifferentLengths();

                        string message = ExpectationDescription +
                                         "{0} with a length of {1}{reason}, but {2} has a length of {3}, differs near " + mismatchSegment + ".";

                        return new FailReason(message, expected, expected.Length, subject, subject.Length);
                    }
               ).SourceSucceeded;
        }

        private string GetMismatchSegmentForStringsOfDifferentLengths()
        {
            int indexOfMismatch = subject.IndexOfFirstMismatch(expected, comparisonMode);

            // If there is no difference it means that either
            // * subject starts with expected or
            // * expected starts with subject
            if (indexOfMismatch == -1)
            {
                // If subject is shorter we are sure that expected starts with subject
                if (subject.Length < expected.Length)
                {
                    // Subject is shorter so we point at its last character.
                    // We would like to point at next character as it is the real
                    // index of first mismatch, but we need to point at character existing in
                    // subject, so the next best thing is the last subject character.
                    indexOfMismatch = Math.Max(0, subject.Length - 1);
                }
                else
                {
                    // If subject is longer we are sure that subject starts with expected
                    // and we point at first character after expected.
                    indexOfMismatch = expected.Length;
                }
            }

            return subject.IndexedSegmentAt(indexOfMismatch);
        }

        protected override void ValidateAgainstMismatch()
        {
            int indexOfMismatch = subject.IndexOfFirstMismatch(expected, comparisonMode);
            if (indexOfMismatch != -1)
            {
                assertion.FailWith(
                    ExpectationDescription + "{0}{reason}, but {1} differs near " + subject.IndexedSegmentAt(indexOfMismatch) + ".",
                    expected, subject);
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
                return (comparisonMode == StringComparison.CurrentCultureIgnoreCase) ||
                    (comparisonMode == StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
