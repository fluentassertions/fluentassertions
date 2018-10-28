using System;

namespace FluentAssertions.Equivalency
{
    internal class CollectionMemberMemberInfo : IMemberInfo
    {
        public CollectionMemberMemberInfo(IMemberInfo memberInfo)
        {
            CompileTimeType = memberInfo.CompileTimeType;
            SelectedMemberDescription = memberInfo.SelectedMemberDescription;
            SelectedMemberInfo = memberInfo.SelectedMemberInfo;
            SelectedMemberPath = GetAdjustedPropertyPath(memberInfo.SelectedMemberPath);
            RuntimeType = memberInfo.RuntimeType;
        }

        internal static string GetAdjustedPropertyPath(string propertyPath)
        {
            return propertyPath.Substring(propertyPath.IndexOf('.') + 1);
        }

        public SelectedMemberInfo SelectedMemberInfo { get; private set; }

        public string SelectedMemberPath { get; private set; }

        public string SelectedMemberDescription { get; private set; }

        public Type CompileTimeType { get; private set; }

        public Type RuntimeType { get; private set; }
    }
}
