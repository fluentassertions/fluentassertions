using System.Collections.Generic;

namespace FluentAssertions.Equivalency.Selection;

internal class CollectionMemberSelectionRuleDecorator(IMemberSelectionRule selectionRule) : IPathBasedSelectionRule
{
    public bool IncludesMembers => selectionRule.IncludesMembers;

    public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
        MemberSelectionContext context)
    {
        return selectionRule.SelectMembers(currentNode, selectedMembers, context);
    }

    public bool SelectsMembersOf(INode currentNode)
    {
        return selectionRule is IPathBasedSelectionRule pathBasedRule &&
            pathBasedRule.SelectsMembersOf(currentNode);
    }

    public override string ToString()
    {
        return selectionRule.ToString();
    }
}
