using System;
using FluentAssertions.Localization;

namespace FluentAssertions.Primitives
{
    internal class NegatedStringStartValidator : StringValidator
    {
        private readonly StringComparison stringComparison;

        public NegatedStringStartValidator(string subject, string expected, StringComparison stringComparison, string because,
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
                    ? Resources.String_ExpectedStringThatDoesNotStartWithEquivalentOf
                    : Resources.String_ExpectedStringThatDoesNotStartWith;
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

        protected override void ValidateAgainstMismatch()
        {
            bool isMatch = subject.StartsWith(expected, stringComparison);
            if (isMatch)
            {
                assertion.FailWith(ExpectationDescription + Resources.String_XFormat + Resources.Common_CommaButFoundYFormat,
                    expected, subject);
            }
        }
    }
}
