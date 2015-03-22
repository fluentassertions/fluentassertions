using System.Collections.Generic;

namespace FluentAssertions.Equivalency.Selection
{
    internal class CollectionMemberSelectionRuleDecorator : IMemberSelectionRule
    {
        private readonly IMemberSelectionRule selectionRule;

        public CollectionMemberSelectionRuleDecorator(IMemberSelectionRule selectionRule)
        {
            this.selectionRule = selectionRule;
        }

        public bool IncludesMembers
        {
            get { return selectionRule.IncludesMembers; }
        }

        public IEnumerable<SelectedMemberInfo> SelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers,
            ISubjectInfo context, IEquivalencyAssertionOptions config)
        {
            return selectionRule.SelectMembers(selectedMembers, new CollectionMemberSubjectInfo(context), config);
        }

        public override string ToString()
        {
            return selectionRule.ToString();
        }
    }
}