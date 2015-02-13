using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Selection rule that removes a particular property from the structural comparison.
    /// </summary>
    internal class ExcludeMemberByPathSelectionRule : IMemberSelectionRule
    {
        private readonly string pathToExclude;

        public ExcludeMemberByPathSelectionRule(string pathToExclude)
        {
            this.pathToExclude = pathToExclude;
        }

        public IEnumerable<SelectedMemberInfo> SelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers, ISubjectInfo context, IEquivalencyAssertionOptions config)
        {
            string path = context.SelectedMemberPath;
            if (!ContainsIndexingQualifiers(pathToExclude))
            {
                path = RemoveInitialIndexQualifier(path);
            }

            return selectedMembers.Where(memberInfo => (path.Combine(memberInfo.Name) != pathToExclude)).ToArray();
        }

        private static bool ContainsIndexingQualifiers(string path)
        {
            return path.Contains("[") && path.Contains("]");
        }

        private string RemoveInitialIndexQualifier(string propertyPath)
        {
            var indexQualifierRegex = new Regex(@"^\[\d+]");

            if (!indexQualifierRegex.IsMatch(pathToExclude))
            {
                var match = indexQualifierRegex.Match(propertyPath);
                if (match.Success)
                {
                    propertyPath = propertyPath.Substring(match.Length);
                }
            }

            return propertyPath;
        }

        public override string ToString()
        {
            return "Exclude member " + pathToExclude;
        }
    }
}