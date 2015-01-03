using System;
using System.Reflection;

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
            return fieldInfo.GetValue(obj);
        }

        public override Type MemberType
        {
            get { return fieldInfo.FieldType; }
        }
    }
}