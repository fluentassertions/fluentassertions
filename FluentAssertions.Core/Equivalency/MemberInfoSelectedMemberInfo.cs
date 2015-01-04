using System;
using System.Reflection;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// A partial ISelectedMemberInfo implementation that delegates to a MemberInfo object
    /// </summary>
    internal abstract class MemberInfoSelectedMemberInfo : ISelectedMemberInfo
    {
        private readonly MemberInfo memberInfo;

        protected MemberInfoSelectedMemberInfo(MemberInfo memberInfo)
        {
            this.memberInfo = memberInfo;
        }

        public string Name
        {
            get { return memberInfo.Name; }
        }

        public Type DeclaringType
        {
            get { return memberInfo.DeclaringType; }
        }

        public abstract Type MemberType { get; }
        public abstract object GetValue(object obj, object[] index);

        protected bool Equals(MemberInfoSelectedMemberInfo other)
        {
            return memberInfo.Equals(other.memberInfo);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MemberInfoSelectedMemberInfo) obj);
        }

        public override int GetHashCode()
        {
            return memberInfo.GetHashCode();
        }
    }
}