using System;

namespace FluentAssertions.Equivalency.Execution
{
    internal class ObjectInfo : IObjectInfo
    {
        public ObjectInfo(Comparands comparands, INode currentNode)
        {
            Type = currentNode.Type;
            Path = currentNode.PathAndName;
            CompileTimeType = comparands.CompileTimeType;
            RuntimeType = comparands.RuntimeType;
        }

        public Type Type { get; }

        public string Path { get; set; }

        public Type CompileTimeType { get; }

        public Type RuntimeType { get; }
    }
}
