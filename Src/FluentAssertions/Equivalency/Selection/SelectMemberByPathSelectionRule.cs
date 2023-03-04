using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FluentAssertions.Equivalency.Selection;

internal abstract class SelectMemberByPathSelectionRule : IMemberSelectionRule
{
    public virtual bool IncludesMembers => false;

    public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
        MemberSelectionContext context)
    {
        var currentPath = RemoveIndexQualifiers(currentNode.PathAndName);
        var members = selectedMembers.ToList();
        AddOrRemoveMembersFrom(members, currentNode, currentPath, context);

        return members;
    }

    protected abstract void AddOrRemoveMembersFrom(List<IMember> selectedMembers,
        INode parent, string parentPath,
        MemberSelectionContext context);

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
