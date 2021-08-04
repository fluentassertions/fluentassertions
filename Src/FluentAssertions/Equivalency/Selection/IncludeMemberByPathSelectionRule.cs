using System.Collections.Generic;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Selection
{
    /// <summary>
    /// Selection rule that includes a particular property in the structural comparison.
    /// </summary>
    internal class IncludeMemberByPathSelectionRule : SelectMemberByPathSelectionRule
    {
        private readonly MemberPath memberToInclude;

        public IncludeMemberByPathSelectionRule(MemberPath pathToInclude)
            : base(pathToInclude.ToString())
        {
            memberToInclude = pathToInclude;
        }

        public override bool IncludesMembers => true;

        protected override void AddOrRemoveMembersFrom(List<IMember> selectedMembers, INode parent, string parentPath, MemberSelectionContext context)
        {
            foreach (MemberInfo memberInfo in context.Type.GetNonPrivateMembers(MemberVisibility.Public | MemberVisibility.Internal))
            {
                var memberPath = new MemberPath(context.Type, memberInfo.DeclaringType, parentPath.Combine(memberInfo.Name));
                if (memberToInclude.IsSameAs(memberPath) || memberToInclude.IsParentOrChildOf(memberPath))
                {
                    selectedMembers.Add(MemberFactory.Create(memberInfo, parent));
                }
            }
        }

        public override string ToString()
        {
            return "Include member root." + memberToInclude;
        }
    }
}
