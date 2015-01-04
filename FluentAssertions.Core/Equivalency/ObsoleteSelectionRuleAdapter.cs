using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Equivalency
{
    [Obsolete]
    internal class ObsoleteSelectionRuleAdapter : IMemberSelectionRule
    {
        private readonly ISelectionRule obsoleteSelectionRule;

        public ObsoleteSelectionRuleAdapter(ISelectionRule obsoleteSelectionRule)
        {
            this.obsoleteSelectionRule = obsoleteSelectionRule;
        }

        public IEnumerable<ISelectedMemberInfo> SelectMembers(IEnumerable<ISelectedMemberInfo> selectedMembers, ISubjectInfo context, IEquivalencyAssertionOptions config)
        {
            IEnumerable<PropertyInfo> propertyInfos =
                selectedMembers.OfType<PropertySelectedMemberInfo>().Select(info => info.PropertyInfo);

            return obsoleteSelectionRule.SelectProperties(propertyInfos, context).Select(SelectedMemberInfo.Create);
        }

        public override string ToString()
        {
            return obsoleteSelectionRule.ToString();
        }
    }
}