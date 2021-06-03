using System;

namespace FluentAssertions.Equivalency.Ordering
{
    internal class CollectionMemberObjectInfo : IObjectInfo
    {
        public CollectionMemberObjectInfo(IObjectInfo context)
        {
            Path = GetAdjustedPropertyPath(context.Path);
            Type = context.Type;
            RuntimeType = context.RuntimeType;
            CompileTimeType = context.CompileTimeType;
        }

        private static string GetAdjustedPropertyPath(string propertyPath)
        {
            return propertyPath.Substring(propertyPath.IndexOf('.', StringComparison.Ordinal) + 1);
        }

        public Type Type { get; }

        public string Path { get; set; }

        public Type CompileTimeType { get; }

        public Type RuntimeType { get; }
    }
}
