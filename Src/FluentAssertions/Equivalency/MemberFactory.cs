using System;
using System.Reflection;

namespace FluentAssertions.Equivalency
{
    public static class MemberFactory
    {
        public static IMember Create(MemberInfo memberInfo, INode parent)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                return new Field((FieldInfo)memberInfo, parent);
            }

            if (memberInfo.MemberType == MemberTypes.Property)
            {
                return new Property((PropertyInfo)memberInfo, parent);
            }

            throw new NotSupportedException($"Don't know how to deal with a {memberInfo.MemberType}");
        }
    }
}
