using System;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    internal class StringEqualityValidator : StringValidator
    {
        private readonly StringComparison comparisonMode;

        public StringEqualityValidator(string subject, string expected, StringComparison comparisonMode, string reason,
            object[] reasonArgs) :
            base(subject, expected, reason, reasonArgs)
        {
            this.comparisonMode = comparisonMode;
        }

        protected override void ValidateAgainstSuperfluousWhitespace()
        {
            if ((expected.Length > subject.Length) && expected.TrimEnd().Equals(subject, comparisonMode))
            {
                verification.FailWith(ExpectationDescription + "but it misses some extra whitespace at the end.",
                    expected, subject);
            }

            if ((subject.Length > expected.Length) && subject.TrimEnd().Equals(expected, comparisonMode))
            {
                verification.FailWith(ExpectationDescription + "but it has unexpected whitespace at the end.", expected,
                    subject);
            }
        }

        protected override void ValidateAgainstLengthDifferences()
        {
            if (subject.Length < expected.Length)
            {
                verification.FailWith(ExpectationDescription + "but {1} is too short.", expected, subject);
            }

            if (subject.Length > expected.Length)
            {
                verification.FailWith(ExpectationDescription + "but {1} is too long.", expected, subject);
            }
        }

        protected override void ValidateAgainstMismatch()
        {
            int indexOfMismatch = subject.IndexOfFirstMismatch(expected, comparisonMode);
            if (indexOfMismatch != -1)
            {
                verification.FailWith(
                    ExpectationDescription + "but {1} differs near " + subject.IndexedSegmentAt(indexOfMismatch) + ".",
                    expected, subject);
            }
        }

        protected override string ExpectationDescription
        {
            get
            {
                string predicateDescription = IgnoreCase ? "be equivalent to" : "be";
                return "Expected " + Verification.SubjectNameOr("string") + " to " + predicateDescription + " {0}{reason}, ";
            }
        }

        private bool IgnoreCase
        {
            get
            {
                return (comparisonMode == StringComparison.CurrentCultureIgnoreCase) ||
                    (comparisonMode == StringComparison.InvariantCultureIgnoreCase) ||
                        (comparisonMode == StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}