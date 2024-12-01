using System;

namespace FluentAssertions.Equivalency.Execution;

internal class ObjectInfo : IObjectInfo
{
    public ObjectInfo(Comparands comparands, INode currentNode)
    {
        Type = currentNode.Type;
        ParentType = currentNode.ParentType;
        Path = currentNode.PathAndName;
        RuntimeType = comparands.RuntimeType;
    }

    public Type Type { get; }

    public Type ParentType { get; }

    public string Path { get; set; }

    public Type RuntimeType { get; }
}
