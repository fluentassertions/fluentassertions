using System;
using FluentAssertions.Common;

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
            // Logic is a little bit convoluted because I want to avoid calculation
            // of mismatch segment in case of equalLength == true for performance reason.
            // If lazy version of FailWith would be introduced, calculation of mismatch
            // segment can be moved directly to FailWith's argument
            bool equalLength = subject.Length == expected.Length;

            string mismatchSegment = "";
            if (!equalLength)
            {
                int indexOfMismatch = subject.IndexOfFirstMismatch(expected, comparisonMode);
                if(indexOfMismatch == -1)
                {
                    indexOfMismatch = string.IsNullOrEmpty(subject) ? 0 : subject.Length - 1;
                }

                mismatchSegment = subject.IndexedSegmentAt(indexOfMismatch);
            }

            return assertion
                .ForCondition(equalLength)
                .FailWith(
                    ExpectationDescription + "{0} with a length of {1}{reason}, but {2} has a length of {3}, differs near " + mismatchSegment +  ".",
                    expected, expected.Length, subject, subject.Length)
                .SourceSucceeded;
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
