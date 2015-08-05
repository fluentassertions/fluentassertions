using System;
using System.Reflection;

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

        [Obsolete]
        internal PropertyInfo PropertyInfo
        {
            get { return propertyInfo; }
        }

        public override object GetValue(object obj, object[] index)
        {
            return propertyInfo.GetValue(obj, index);
        }
    }
}