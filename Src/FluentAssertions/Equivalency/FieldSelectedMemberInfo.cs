using System;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Provides an ISelectedMemberInfo for FieldInfo objects
    /// </summary>
    internal class FieldSelectedMemberInfo : MemberInfoSelectedMemberInfo
    {
        private readonly FieldInfo fieldInfo;

        public FieldSelectedMemberInfo(FieldInfo fieldInfo)
            : base(fieldInfo)
        {
            this.fieldInfo = fieldInfo;
        }

        public override object GetValue(object obj, object[] index)
        {
            if (index?.Any() == true)
            {
                throw new TargetParameterCountException("Parameter count mismatch.");
            }

            return fieldInfo.GetValue(obj);
        }

        public override Type MemberType => fieldInfo.FieldType;

        internal override CSharpAccessModifier GetGetAccessModifier() => fieldInfo.GetCSharpAccessModifier();

        internal override CSharpAccessModifier GetSetAccessModifier() => fieldInfo.GetCSharpAccessModifier();
    }
}
