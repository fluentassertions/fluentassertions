using System.Text.RegularExpressions;

namespace FluentAssertions.Assertions
{
    internal class StringWildcardMatchingValidator : StringValidator
    {
        public StringWildcardMatchingValidator(string subject, string expected, string reason, object[] reasonArgs)
            : base(subject, expected, reason, reasonArgs)
        {
        }

        protected override void ValidateAgainstMismatch()
        {
            if (!IsMatch && !Negate)
            {
                verification.FailWith(ExpectationDescription + "but {2} does not match.", expected, subject);
            }

            if (IsMatch && Negate)
            {
                verification.FailWith(ExpectationDescription + "but {2} matches.", expected, subject);
            }
        }

        private bool IsMatch
        {
            get { return Regex.IsMatch(subject, ConvertWildcardToRegEx(expected)); }
        }

        private static string ConvertWildcardToRegEx(string wildcardExpression)
        {
            return "^" + Regex.Escape(wildcardExpression).Replace("\\*", ".*").Replace("\\?", ".") + "$";
        }

        protected override string ExpectationDescription
        {
            get
            {
                return (Negate ? "Did not expect " : "Expected ") + Verification.SubjectNameOr("string") + " to match {1}{0}, ";
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the subject should not match the pattern.
        /// </summary>
        public bool Negate { get; set; }
    }
}