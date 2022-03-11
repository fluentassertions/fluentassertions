using System;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// A specialized type of <see cref="INode  "/> that represents a property of an object in a structural equivalency assertion.
    /// </summary>
#pragma warning disable CA1716
    public class Property : Node, IMember
    {
        private readonly PropertyInfo propertyInfo;

        public Property(PropertyInfo propertyInfo, INode parent)
            : this(propertyInfo.ReflectedType, propertyInfo, parent)
        {
        }

        public Property(Type reflectedType, PropertyInfo propertyInfo, INode parent)
        {
            ReflectedType = reflectedType;
            this.propertyInfo = propertyInfo;
            DeclaringType = propertyInfo.DeclaringType;
            Name = propertyInfo.Name;
            Type = propertyInfo.PropertyType;
            Path = parent.PathAndName;
            GetSubjectId = parent.GetSubjectId;
            RootIsCollection = parent.RootIsCollection;
        }

        public object GetValue(object obj)
        {
            return propertyInfo.GetValue(obj);
        }

        public Type DeclaringType { get; private set; }

        public Type ReflectedType { get; }

        public override string Description => $"property {GetSubjectId().Combine(PathAndName)}";

        public CSharpAccessModifier GetterAccessibility => propertyInfo.GetGetMethod(nonPublic: true).GetCSharpAccessModifier();

        public CSharpAccessModifier SetterAccessibility => propertyInfo.GetSetMethod(nonPublic: true).GetCSharpAccessModifier();
    }
}
