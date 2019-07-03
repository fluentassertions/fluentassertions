using System;
using FluentAssertions.Common;
using FluentAssertions.Localization;

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
                return IgnoreCase
                    ? Resources.String_ExpectedStringToStartWithEquivalentOf
                    : Resources.String_ExpectedStringToStartWith;
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
                .FailWith(ExpectationDescription + Resources.String_XButYIsTooShortFormat,
                    expected, subject)
                .SourceSucceeded;
        }

        protected override void ValidateAgainstMismatch()
        {
            bool isMismatch = !subject.StartsWith(expected, stringComparison);
            if (isMismatch)
            {
                int indexOfMismatch = subject.IndexOfFirstMismatch(expected, stringComparison);

                assertion.FailWith(ExpectationDescription + Resources.String_XButYDiffersNearZFormat,
                    expected, subject, subject.IndexedSegmentAt(indexOfMismatch));
            }
        }
    }
}
