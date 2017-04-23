using System;
using System.Reflection;

namespace FluentAssertions.Equivalency
{
    internal class CollectionMemberSubjectInfo : ISubjectInfo
    {
        public CollectionMemberSubjectInfo(ISubjectInfo subjectInfo)
        {
            CompileTimeType = subjectInfo.CompileTimeType;
            SelectedMemberDescription = subjectInfo.SelectedMemberDescription;
            SelectedMemberInfo = subjectInfo.SelectedMemberInfo;
            SelectedMemberPath = GetAdjustedPropertyPath(subjectInfo.SelectedMemberPath);
            RuntimeType = subjectInfo.RuntimeType;
        }

        internal static string GetAdjustedPropertyPath(string propertyPath)
        {
            return propertyPath.Substring(propertyPath.IndexOf(".", StringComparison.Ordinal) + 1);
        }

        public SelectedMemberInfo SelectedMemberInfo { get; private set; }

        public string SelectedMemberPath { get; private set; }

        public string SelectedMemberDescription { get; private set; }

        public Type CompileTimeType { get; private set; }

        public Type RuntimeType { get; private set; }
    }
}