using System.Collections.Generic;
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

        public virtual bool IncludesMembers
        {
            get { return false; }
        }

        public IEnumerable<SelectedMemberInfo> SelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers, ISubjectInfo context,
            IEquivalencyAssertionOptions config)
        {
            string path = context.SelectedMemberPath;
            if (!ContainsIndexingQualifiers(selectedPath))
            {
                path = RemoveInitialIndexQualifier(path);
            }

            return OnSelectMembers(selectedMembers, path, context);
        }

        protected abstract IEnumerable<SelectedMemberInfo> OnSelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers, string currentPath, ISubjectInfo context);

        private static bool ContainsIndexingQualifiers(string path)
        {
            return path.Contains("[") && path.Contains("]");
        }

        private string RemoveInitialIndexQualifier(string propertyPath)
        {
            var indexQualifierRegex = new Regex(@"^\[\d+]");

            if (!indexQualifierRegex.IsMatch(selectedPath))
            {
                var match = indexQualifierRegex.Match(propertyPath);
                if (match.Success)
                {
                    propertyPath = propertyPath.Substring(match.Length);
                }
            }

            return propertyPath;
        }
    }
}