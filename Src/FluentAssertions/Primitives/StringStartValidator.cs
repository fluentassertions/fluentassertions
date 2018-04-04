using System;
using FluentAssertions.Common;

namespace FluentAssertions.Primitives
{
    internal class StringStartValidator : StringValidator
    {
        private readonly StringComparison stringComparison;

        public StringStartValidator(string subject, string expected, StringComparison stringComparison, string because,
            object[] becauseArgs)
            : base(subject, expected, because, becauseArgs)
        {
            this.stringComparison = stringComparison;
        }

        protected override string ExpectationDescription
        {
            get
            {
                string predicateDescription = IgnoreCase ? "start with equivalent of" : "start with";
                return "Expected {context:string} to " + predicateDescription + " ";
            }
        }

        private bool IgnoreCase
        {
            get
            {
                return (stringComparison == StringComparison.CurrentCultureIgnoreCase) ||
                    (stringComparison == StringComparison.OrdinalIgnoreCase);
            }
        }

        protected override bool ValidateAgainstLengthDifferences()
        {
            return assertion
                .ForCondition(subject.Length >= expected.Length)
                .FailWith(ExpectationDescription + "{0}{reason}, but {1} is too short.", expected, subject)
                .SourceSucceeded;
        }

        protected override void ValidateAgainstMismatch()
        {
            bool isMismatch = !subject.StartsWith(expected, stringComparison);
            if (isMismatch)
            {
                int indexOfMismatch = subject.IndexOfFirstMismatch(expected, stringComparison);

                assertion.FailWith(
                    ExpectationDescription + "{0}{reason}, but {1} differs near " + subject.IndexedSegmentAt(indexOfMismatch) + ".",
                    expected, subject);
            }
        }
    }
}
