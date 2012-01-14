using System;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    internal class StringStartValidator : StringValidator
    {
        private readonly StringComparison stringComparison;

        public StringStartValidator(string subject, string expected, StringComparison stringComparison, string reason,
            object [] reasonArgs) :
                base(subject, expected, reason, reasonArgs)
        {
            this.stringComparison = stringComparison;
        }

        protected override string ExpectationDescription
        {
            get
            {
                string predicateDescription = IgnoreCase ? "start with equivalent of" : "start with";
                return "Expected " + Verification.SubjectNameOr("string") + " to " + predicateDescription + " {0}{reason}, ";
            }
        }

        private bool IgnoreCase
        {
            get
            {
                return (stringComparison == StringComparison.CurrentCultureIgnoreCase) ||
                    (stringComparison == StringComparison.InvariantCultureIgnoreCase) ||
                        (stringComparison == StringComparison.OrdinalIgnoreCase);
            }
        }

        protected override void ValidateAgainstMismatch()
        {
            bool isMismatch = !subject.StartsWith(expected, stringComparison);
            if (isMismatch)
            {
                int indexOfMismatch = subject.IndexOfFirstMismatch(expected, stringComparison);

                verification.FailWith(
                    ExpectationDescription + "but {1} differs near " + subject.IndexedSegmentAt(indexOfMismatch) + ".",
                    expected, subject);
            }
        }
    }
}