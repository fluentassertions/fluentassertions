using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FluentAssertionsAsync.Equivalency.Selection;

internal abstract class SelectMemberByPathSelectionRule : IMemberSelectionRule
{
    public virtual bool IncludesMembers => false;

    public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
        MemberSelectionContext context)
    {
        var currentPath = RemoveRootIndexQualifier(currentNode.PathAndName);
        var members = selectedMembers.ToList();
        AddOrRemoveMembersFrom(members, currentNode, currentPath, context);

        return members;
    }

    protected abstract void AddOrRemoveMembersFrom(List<IMember> selectedMembers,
        INode parent, string parentPath,
        MemberSelectionContext context);

    private static string RemoveRootIndexQualifier(string path)
    {
        Match match = new Regex(@"^\[[0-9]+]").Match(path);

        if (match.Success)
        {
            path = path.Substring(match.Length);
        }

        return path;
    }
}
