using System;

namespace FluentAssertionsAsync.Equivalency.Ordering;

internal class CollectionMemberObjectInfo : IObjectInfo
{
    public CollectionMemberObjectInfo(IObjectInfo context)
    {
        Path = GetAdjustedPropertyPath(context.Path);

#pragma warning disable CS0618
        Type = context.Type;
#pragma warning restore CS0618

        ParentType = context.ParentType;
        RuntimeType = context.RuntimeType;
        CompileTimeType = context.CompileTimeType;
    }

    private static string GetAdjustedPropertyPath(string propertyPath)
    {
        return propertyPath.Substring(propertyPath.IndexOf('.', StringComparison.Ordinal) + 1);
    }

    public Type Type { get; }

    public Type ParentType { get; }

    public string Path { get; set; }

    public Type CompileTimeType { get; }

    public Type RuntimeType { get; }
}
