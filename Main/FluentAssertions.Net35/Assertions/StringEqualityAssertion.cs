using System;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    internal class StringEqualityAssertion
    {
        private readonly string subject;
        private readonly string expected;
        private readonly Verification verification;

        public StringEqualityAssertion(string subject, string expected, string reason, object[] reasonArgs)
        {
            verification = Execute.Verification.BecauseOf(reason, reasonArgs);

            this.subject = subject;
            this.expected = expected;

            PositiveCase = true;
            CaseSensitive = true;
        }

        public bool PositiveCase { get; set; }

        public string PredicateDescription { get; set; }

        public bool CaseSensitive { get; set; }

        public void Assert()
        {
            if ((expected == null) && (subject == null))
            {
                return;
            }

            if (((expected == null) && (subject != null)) || ((expected != null) && (subject == null)))
            {
                verification.FailWith(ExpectationDescription + "but found {2}.", expected, subject);
            }

            if ((expected.Length > subject.Length) && expected.TrimEnd().Equals(subject, ComparisonMode))
            {
                verification.FailWith(ExpectationDescription + "but it misses some extra whitespace at the end.", expected, subject);
            }

            if ((subject.Length > expected.Length) && subject.TrimEnd().Equals(expected, ComparisonMode))
            {
                verification.FailWith(ExpectationDescription + "but it has unexpected whitespace at the end.", expected, subject);
            }

            if (subject.Length < expected.Length)
            {
                verification.FailWith(ExpectationDescription + "but {2} is too short.", expected, subject);
            }

            if (subject.Length > expected.Length)
            {
                verification.FailWith(ExpectationDescription + "but {2} is too long.", expected, subject);
            }

            int indexOfMismatch = subject.IndexOfFirstMismatch(expected, ComparisonMode);
            if (indexOfMismatch != -1)
            {
                verification.FailWith(ExpectationDescription + "but {2} differs near " + subject.IndexedSegmentAt(indexOfMismatch) + ".", expected, subject);
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
                string description = PositiveCase ? "Expected " : "Did not expect ";
                description += Verification.SubjectNameOr("string") + " to " + PredicateDescription + " {1}{0}, ";

                return description;
            }
        }
    }
}