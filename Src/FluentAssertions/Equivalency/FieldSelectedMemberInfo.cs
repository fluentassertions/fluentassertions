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

        public FieldSelectedMemberInfo(FieldInfo fieldInfo) : base(fieldInfo)
        {
            this.fieldInfo = fieldInfo;
        }

        public override object GetValue(object obj, object[] index)
        {
            if ((index != null) && index.Any())
            {
                throw new TargetParameterCountException("Parameter count mismatch.");
            }

            return fieldInfo.GetValue(obj);
        }

        public override Type MemberType
        {
            get { return fieldInfo.FieldType; }
        }

        internal override CSharpAccessModifier GetAccessModifier
        {
            get { return fieldInfo.GetCSharpAccessModifier(); }
        }

        internal override CSharpAccessModifier SetAccessModifier
        {
            get { return fieldInfo.GetCSharpAccessModifier(); }
        }
    }
}