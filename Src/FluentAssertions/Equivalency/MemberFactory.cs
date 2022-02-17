using System;
using System.Reflection;
using FluentAssertions.Common;

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

        internal static IMember Find(object target, string memberName, Type preferredMemberType, INode parent)
        {
            PropertyInfo property = target.GetType().FindProperty(memberName, preferredMemberType);
            if ((property is not null) && !property.IsIndexer())
            {
                return new Property(property, parent);
            }

            FieldInfo field = target.GetType().FindField(memberName, preferredMemberType);
            return (field is not null) ? new Field(field, parent) : null;
        }
    }
}
