using System;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Provides an ISelectedMemberInfo for PropertyInfo objects
    /// </summary>
    internal class PropertySelectedMemberInfo : MemberInfoSelectedMemberInfo
    {
        private readonly PropertyInfo propertyInfo;

        public PropertySelectedMemberInfo(PropertyInfo propertyInfo) : base(propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }

        public override Type MemberType
        {
            get { return propertyInfo.PropertyType; }
        }

        internal override CSharpAccessModifier GetAccessModifier
        {
            get { return propertyInfo.GetGetMethod(true).GetCSharpAccessModifier(); }
        }

        internal override CSharpAccessModifier SetAccessModifier
        {
            get { return propertyInfo.GetSetMethod(true).GetCSharpAccessModifier(); }
        }

        public override object GetValue(object obj, object[] index)
        {
            return propertyInfo.GetValue(obj, index);
        }
    }
}