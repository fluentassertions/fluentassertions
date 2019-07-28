using System;
using System.Reflection;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// A partial ISelectedMemberInfo implementation that delegates to a MemberInfo object
    /// </summary>
    internal abstract class MemberInfoSelectedMemberInfo : SelectedMemberInfo
    {
        private readonly MemberInfo memberInfo;

        protected MemberInfoSelectedMemberInfo(MemberInfo memberInfo)
        {
            this.memberInfo = memberInfo;
        }

        public override string Name => memberInfo.Name;

        public override Type DeclaringType => memberInfo.DeclaringType;

        protected bool Equals(MemberInfoSelectedMemberInfo other)
        {
            return memberInfo.Equals(other.memberInfo);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((MemberInfoSelectedMemberInfo)obj);
        }

        public override int GetHashCode()
        {
            return memberInfo.GetHashCode();
        }
    }
}
