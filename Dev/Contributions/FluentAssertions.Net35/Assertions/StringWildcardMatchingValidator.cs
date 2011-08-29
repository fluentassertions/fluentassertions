using System.Text;
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
                verification.FailWith(ExpectationDescription + "but {1} does not match.", expected, subject);
            }

            if (IsMatch && Negate)
            {
                verification.FailWith(ExpectationDescription + "but {1} matches.", expected, subject);
            }
        }

        private bool IsMatch
        {
            get
            {
                var options = IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;

                return Regex.IsMatch(subject, ConvertWildcardToRegEx(expected), options | RegexOptions.Singleline);
            }
        }

        private static string ConvertWildcardToRegEx(string wildcardExpression)
        {
            return "^" + Regex.Escape(wildcardExpression).Replace("\\*", ".*").Replace("\\?", ".") + "$";
        }

        protected override string ExpectationDescription
        {
            get
            {
                var builder = new StringBuilder();
                builder.Append(Negate ? "Did not expect " : "Expected ");
                builder.Append(Verification.SubjectNameOr("string"));
                builder.Append(IgnoreCase ? " to match the equivalent of" : " to match");
                builder.Append(" {0}{reason}, ");

                return builder.ToString();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the subject should not match the pattern.
        /// </summary>
        public bool Negate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the matching process should ignore any casing difference.
        /// </summary>
        public bool IgnoreCase
        {
            get; set;
        }
    }
}