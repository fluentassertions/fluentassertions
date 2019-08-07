using System.Text;
using System.Text.RegularExpressions;

using FluentAssertions.Common;

namespace FluentAssertions.Primitives
{
    internal class StringWildcardMatchingValidator : StringValidator
    {
        public StringWildcardMatchingValidator(string subject, string expected, string because, object[] becauseArgs)
            : base(subject, expected, because, becauseArgs)
        {
        }

        protected override void ValidateAgainstMismatch()
        {
            bool isMatch = IsMatch();

            if (!isMatch && !Negate)
            {
                assertion.FailWith(ExpectationDescription + "but {1} does not.", expected, subject);
            }

            if (isMatch && Negate)
            {
                assertion.FailWith(ExpectationDescription + "but {1} matches.", expected, subject);
            }
        }

        private bool IsMatch()
        {
            RegexOptions options = IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;

            string input = CleanNewLines(subject);
            string pattern = ConvertWildcardToRegEx(CleanNewLines(expected));

            return Regex.IsMatch(input, pattern, options | RegexOptions.Singleline);
        }

        private static string ConvertWildcardToRegEx(string wildcardExpression)
        {
            return "^" + Regex.Escape(wildcardExpression).Replace("\\*", ".*").Replace("\\?", ".") + "$";
        }

        private string CleanNewLines(string input)
        {
            if (input is null)
            {
                return null;
            }

            return IgnoreNewLineDifferences ? input.RemoveNewLines() : input;
        }

        protected override string ExpectationDescription
        {
            get
            {
                var builder = new StringBuilder();
                builder.Append(Negate ? "Did not expect " : "Expected ");
                builder.Append("{context:string}");
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
        public bool IgnoreCase { get; set; }

        /// <summary>
        /// Ignores the difference between environment newline differences
        /// </summary>
        public bool IgnoreNewLineDifferences { get; set; }
    }
}
