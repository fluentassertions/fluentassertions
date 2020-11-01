using System;

namespace FluentAssertions.Equivalency
{
    internal class ObjectInfo : IObjectInfo
    {
        public ObjectInfo(IEquivalencyValidationContext context)
        {
            Type = context.CurrentNode.Type;
            Path = context.CurrentNode.PathAndName;
            CompileTimeType = context.CompileTimeType;
            RuntimeType = context.RuntimeType;
        }

        public Type Type { get; }

        public string Path { get; set; }

        public Type CompileTimeType { get; }

        public Type RuntimeType { get; }
    }
}
