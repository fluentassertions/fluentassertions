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

        public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
            MemberSelectionContext context)
        {
            string currentPath = currentNode.PathAndName;

            // If we're part of a collection comparison, the selected path will not include an index,
            // so we need to remove it from the current node as well.
            if (!ContainsIndexingQualifiers(selectedPath))
            {
                currentPath = RemoveIndexQualifiers(currentPath);
            }

            var members = selectedMembers.ToList();
            AddOrRemoveMembersFrom(members, currentNode, currentPath, context);

            return members;
        }

        protected abstract void AddOrRemoveMembersFrom(List<IMember> selectedMembers,
            INode parent, string parentPath,
            MemberSelectionContext context);

        private static bool ContainsIndexingQualifiers(string path)
        {
            return path.Contains("[", StringComparison.Ordinal) && path.Contains("]", StringComparison.Ordinal);
        }

        private static string RemoveIndexQualifiers(string path)
        {
            Match match = new Regex(@"^\[\d+]").Match(path);
            if (match.Success)
            {
                path = path.Substring(match.Length);
            }

            return path;
        }
    }
}
