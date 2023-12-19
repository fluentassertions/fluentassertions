using System.Collections.Generic;
using System.Linq;

namespace FluentAssertionsAsync.Equivalency.Selection;

internal class ExcludeNonBrowsableMembersRule : IMemberSelectionRule
{
    public bool IncludesMembers => false;

    public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
        MemberSelectionContext context)
    {
        return selectedMembers.Where(member => member.IsBrowsable).ToList();
    }
}
