using System;
using System.ComponentModel;
using System.Reflection;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Equivalency;

/// <summary>
/// A specialized type of <see cref="INode  "/> that represents a property of an object in a structural equivalency assertion.
/// </summary>
#pragma warning disable CA1716
public class Property : Node, IMember
{
    private readonly PropertyInfo propertyInfo;
    private bool? isBrowsable;

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
        ParentType = propertyInfo.DeclaringType;
        Path = parent.PathAndName;
        GetSubjectId = parent.GetSubjectId;
        RootIsCollection = parent.RootIsCollection;
    }

    public object GetValue(object obj)
    {
        return propertyInfo.GetValue(obj);
    }

    public Type DeclaringType { get; }

    public Type ReflectedType { get; }

    public override string Description => $"property {GetSubjectId().Combine(PathAndName)}";

    public CSharpAccessModifier GetterAccessibility => propertyInfo.GetGetMethod(nonPublic: true).GetCSharpAccessModifier();

    public CSharpAccessModifier SetterAccessibility => propertyInfo.GetSetMethod(nonPublic: true).GetCSharpAccessModifier();

    public bool IsBrowsable
    {
        get
        {
            isBrowsable ??=
                propertyInfo.GetCustomAttribute<EditorBrowsableAttribute>() is not { State: EditorBrowsableState.Never };

            return isBrowsable.Value;
        }
    }
}
