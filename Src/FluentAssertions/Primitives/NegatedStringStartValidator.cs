using System;

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
                string predicateDescription = IgnoreCase ? "start with equivalent of" : "start with";
                return "Expected {context:string} that does not " + predicateDescription + " ";
            }
        }

        private bool IgnoreCase
        {
            get
            {
                return stringComparison == StringComparison.OrdinalIgnoreCase;
            }
        }

        protected override void ValidateAgainstMismatch()
        {
            bool isMatch = Subject.StartsWith(Expected, stringComparison);
            if (isMatch)
            {
                Assertion.FailWith(ExpectationDescription + "{0}{reason}, but found {1}.",
                    Expected, Subject);
            }
        }
    }
}
