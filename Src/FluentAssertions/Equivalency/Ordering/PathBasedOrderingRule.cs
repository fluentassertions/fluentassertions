using System;
using System.Text.RegularExpressions;

namespace FluentAssertions.Equivalency.Ordering
{
    /// <summary>
    /// Represents a rule for determining whether or not a certain collection within the object graph should be compared using
    /// strict ordering.
    /// </summary>
    internal class PathBasedOrderingRule : IOrderingRule
    {
        private readonly string path;

        public PathBasedOrderingRule(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Determines if ordering of the member referred to by the current <paramref name="memberInfo"/> is relevant.
        /// </summary>
        public OrderStrictness Evaluate(IMemberInfo memberInfo)
        {
            string currentPropertyPath = memberInfo.SelectedMemberPath;
            if (!ContainsIndexingQualifiers(path))
            {
                currentPropertyPath = RemoveInitialIndexQualifier(currentPropertyPath);
            }

            if (currentPropertyPath.Equals(path, StringComparison.CurrentCultureIgnoreCase))
            {
                return OrderStrictness.Strict;
            }
            else
            {
                return OrderStrictness.Irrelevant;
            }
        }

        private static bool ContainsIndexingQualifiers(string path)
        {
            return path.Contains("[") && path.Contains("]");
        }

        private string RemoveInitialIndexQualifier(string sourcePath)
        {
            var indexQualifierRegex = new Regex(@"^\[\d+]\.");

            if (!indexQualifierRegex.IsMatch(path))
            {
                Match match = indexQualifierRegex.Match(sourcePath);
                if (match.Success)
                {
                    sourcePath = sourcePath.Substring(match.Length);
                }
            }

            return sourcePath;
        }

        public override string ToString()
        {
            return "Be strict about the order of collection items when path is " + path;
        }
    }
}
