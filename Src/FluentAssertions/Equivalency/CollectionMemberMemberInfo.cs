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
            return propertyPath.Substring(propertyPath.IndexOf('.', StringComparison.Ordinal) + 1);
        }

        public SelectedMemberInfo SelectedMemberInfo { get; }

        public string SelectedMemberPath { get; }

        public string SelectedMemberDescription { get; }

        public Type CompileTimeType { get; }

        public Type RuntimeType { get; }
    }
}
