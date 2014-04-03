using System;
using FluentAssertions.Common;

namespace FluentAssertions.Primitives
{
    internal class StringEqualityValidator : StringValidator
    {
        private readonly StringComparison comparisonMode;

        public StringEqualityValidator(string subject, string expected, StringComparison comparisonMode, string because,
            object[] reasonArgs) :
                base(subject, expected, because, reasonArgs)
        {
            this.comparisonMode = comparisonMode;
        }

        protected override void ValidateAgainstSuperfluousWhitespace()
        {
            if ((expected.Length > subject.Length) && expected.TrimEnd().Equals(subject, comparisonMode))
            {
                assertion.FailWith(ExpectationDescription + "{0}{reason}, but it misses some extra whitespace at the end.",
                    expected, subject);
            }

            if ((subject.Length > expected.Length) && subject.TrimEnd().Equals(expected, comparisonMode))
            {
                assertion.FailWith(ExpectationDescription + "{0}{reason}, but it has unexpected whitespace at the end.", expected,
                    subject);
            }
        }

        protected override void ValidateAgainstLengthDifferences()
        {
            if (subject.Length != expected.Length)
            {
                assertion.FailWith(
                    ExpectationDescription + "{0} with a length of {1}{reason}, but {2} has a length of {3}.", 
                    expected, expected.Length, subject, subject.Length);
            }
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