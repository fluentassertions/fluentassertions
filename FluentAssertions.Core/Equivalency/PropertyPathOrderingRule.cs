using System;
using System.Text.RegularExpressions;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents a rule for determining whether or not a certain collection within the object graph should be compared using
    /// strict ordering.
    /// </summary>
    public class PropertyPathOrderingRule : IOrderingRule
    {
        private readonly string propertyPath;

        public PropertyPathOrderingRule(string propertyPath)
        {
            this.propertyPath = propertyPath;
        }

        /// <summary>
        /// Determines if ordering of the property refered to by the current <paramref name="subjectInfo"/> is relevant.
        /// </summary>
        public bool AppliesTo(ISubjectInfo subjectInfo)
        {
            string currentPropertyPath = subjectInfo.PropertyPath;
            if (!ContainsIndexingQualifiers(propertyPath))
            {
                currentPropertyPath = RemoveInitialIndexQualifier(currentPropertyPath);
            }

            return currentPropertyPath.Equals(propertyPath, StringComparison.CurrentCultureIgnoreCase);
        }

        private bool ContainsIndexingQualifiers(string path)
        {
            return path.Contains("[") && path.Contains("]");
        }

        private string RemoveInitialIndexQualifier(string sourcePath)
        {
            var indexQualifierRegex = new Regex(@"^\[\d+]\.");

            if (!indexQualifierRegex.IsMatch(propertyPath))
            {
                var match = indexQualifierRegex.Match(sourcePath);
                if (match.Success)
                {
                    sourcePath = sourcePath.Substring(match.Length);
                }
            }

            return sourcePath;
        }
    }
}