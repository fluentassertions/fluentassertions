using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Equivalency.Selection;

public class ExcludeIgnoredDataMembersRule : IMemberSelectionRule
{
    public bool IncludesMembers => false;

    public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
        MemberSelectionContext context)
    {
        return selectedMembers.Where(member => member.ReflectedType.IsSerializable || !member.IsIgnoredDataMember).ToList();
    }
}
