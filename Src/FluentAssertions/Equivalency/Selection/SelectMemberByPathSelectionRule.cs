using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FluentAssertions.Equivalency.Selection
{
    internal abstract class SelectMemberByPathSelectionRule : IMemberSelectionRule
    {
        private readonly string selectedPath;

        protected SelectMemberByPathSelectionRule(string selectedPath)
        {
            this.selectedPath = selectedPath;
        }

        public virtual bool IncludesMembers => false;

        public IEnumerable<SelectedMemberInfo> SelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers, IMemberInfo context,
            IEquivalencyAssertionOptions config)
        {
            string path = context.SelectedMemberPath;
            if (!ContainsIndexingQualifiers(selectedPath))
            {
                path = RemoveInitialIndexQualifier(path);
            }

            return OnSelectMembers(selectedMembers, path, context);
        }

        protected abstract IEnumerable<SelectedMemberInfo> OnSelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers, string currentPath, IMemberInfo context);

        protected static IReadOnlyDictionary<string, SelectedMemberInfo> GetPreferredMembers(IEnumerable<SelectedMemberInfo> selectedMembers, IMemberInfo context)
        {
            var preferredMembers = selectedMembers
                .GroupBy(m => m.Name)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g.Where(m => m.DeclaringType == context.CompileTimeType))
                .ToDictionary(m => m.Name, m => m);
            return preferredMembers;
        }

        private static bool ContainsIndexingQualifiers(string path)
        {
            return path.Contains("[", StringComparison.Ordinal) && path.Contains("]", StringComparison.Ordinal);
        }

        private string RemoveInitialIndexQualifier(string propertyPath)
        {
            var indexQualifierRegex = new Regex(@"^\[\d+]");

            if (!indexQualifierRegex.IsMatch(selectedPath))
            {
                Match match = indexQualifierRegex.Match(propertyPath);
                if (match.Success)
                {
                    propertyPath = propertyPath.Substring(match.Length);
                }
            }

            return propertyPath;
        }
    }
}
