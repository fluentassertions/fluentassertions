using System.Collections.Generic;

namespace FluentAssertionsAsync.Equivalency.Selection;

internal class CollectionMemberSelectionRuleDecorator : IMemberSelectionRule
{
    private readonly IMemberSelectionRule selectionRule;

    public CollectionMemberSelectionRuleDecorator(IMemberSelectionRule selectionRule)
    {
        this.selectionRule = selectionRule;
    }

    public bool IncludesMembers => selectionRule.IncludesMembers;

    public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
        MemberSelectionContext context)
    {
        return selectionRule.SelectMembers(currentNode, selectedMembers, context);
    }

    public override string ToString()
    {
        return selectionRule.ToString();
    }
}
