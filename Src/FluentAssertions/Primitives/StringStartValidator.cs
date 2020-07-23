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
                return stringComparison == StringComparison.OrdinalIgnoreCase;
            }
        }

        protected override bool ValidateAgainstLengthDifferences()
        {
            return Assertion
                .ForCondition(Subject.Length >= Expected.Length)
                .FailWith(ExpectationDescription + "{0}{reason}, but {1} is too short.", Expected, Subject);
        }

        protected override void ValidateAgainstMismatch()
        {
            bool isMismatch = !Subject.StartsWith(Expected, stringComparison);
            if (isMismatch)
            {
                int indexOfMismatch = Subject.IndexOfFirstMismatch(Expected, stringComparison);

                Assertion.FailWith(
                    ExpectationDescription + "{0}{reason}, but {1} differs near " + Subject.IndexedSegmentAt(indexOfMismatch) + ".",
                    Expected, Subject);
            }
        }
    }
}
