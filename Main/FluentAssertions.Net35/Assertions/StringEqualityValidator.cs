using System;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Dedicated class for comparing two strings and generating consistent error messages.
    /// </summary>
    internal class StringEqualityValidator
    {
        #region Private Definition

        private readonly string subject;
        private readonly string expected;
        private readonly Verification verification;

        #endregion
        
        public StringEqualityValidator(string subject, string expected, string reason, object[] reasonArgs)
        {
            verification = Execute.Verification.BecauseOf(reason, reasonArgs);

            this.subject = subject;
            this.expected = expected;

            CaseSensitive = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the comparison is case sensitive. 
        /// </summary>
        public bool CaseSensitive { get; set; }

        public void Validate()
        {
            if ((expected != null) || (subject != null))
            {
                ValidateAgainstNulls();

                ValidateAgainstSuperfluousWhitespace();

                ValidateAgainstLengthDifferences();

                ValidateAgainstMismatch();
            }
        }

        private void ValidateAgainstNulls()
        {
            if (((expected == null) && (subject != null)) || ((expected != null) && (subject == null)))
            {
                verification.FailWith(ExpectationDescription + "but found {2}.", expected, subject);
            }
        }

        private void ValidateAgainstSuperfluousWhitespace()
        {
            if ((expected.Length > subject.Length) && expected.TrimEnd().Equals(subject, ComparisonMode))
            {
                verification.FailWith(ExpectationDescription + "but it misses some extra whitespace at the end.",
                    expected, subject);
            }

            if ((subject.Length > expected.Length) && subject.TrimEnd().Equals(expected, ComparisonMode))
            {
                verification.FailWith(ExpectationDescription + "but it has unexpected whitespace at the end.", expected,
                    subject);
            }
        }

        private void ValidateAgainstLengthDifferences()
        {
            if (subject.Length < expected.Length)
            {
                verification.FailWith(ExpectationDescription + "but {2} is too short.", expected, subject);
            }

            if (subject.Length > expected.Length)
            {
                verification.FailWith(ExpectationDescription + "but {2} is too long.", expected, subject);
            }
        }

        private void ValidateAgainstMismatch()
        {
            int indexOfMismatch = subject.IndexOfFirstMismatch(expected, ComparisonMode);
            if (indexOfMismatch != -1)
            {
                verification.FailWith(
                    ExpectationDescription + "but {2} differs near " + subject.IndexedSegmentAt(indexOfMismatch) + ".",
                    expected, subject);
            }
        }

        private StringComparison ComparisonMode
        {
            get { return CaseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase; }
        }

        private string ExpectationDescription
        {
            get
            {
                string predicateDescription = CaseSensitive ? "be" : "be equivalent to";
                return "Expected " + Verification.SubjectNameOr("string") + " to " + predicateDescription + " {1}{0}, ";
            }
        }
    }
}