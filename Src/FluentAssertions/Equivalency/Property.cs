using System;
using System.ComponentModel;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency;

/// <summary>
/// A specialized type of <see cref="INode  "/> that represents a property of an object in a structural equivalency assertion.
/// </summary>
#pragma warning disable CA1716
internal class Property : Node, IMember
{
    private readonly PropertyInfo propertyInfo;
    private bool? isXmlIgnored;
    private bool? isIgnoredDataMember;
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
        Subject = new Pathway(parent.Subject.PathAndName, propertyInfo.Name,  pathAndName => $"property {parent.GetSubjectId().Combine(pathAndName)}");
        Expectation = new Pathway(parent.Expectation.PathAndName, propertyInfo.Name, pathAndName => $"property {pathAndName}");
        Type = propertyInfo.PropertyType;
        ParentType = propertyInfo.DeclaringType;
        GetSubjectId = parent.GetSubjectId;
        RootIsCollection = parent.RootIsCollection;
    }

    public object GetValue(object obj)
    {
        return propertyInfo.GetValue(obj);
    }

    public Type DeclaringType { get; }

    public Type ReflectedType { get; }

    public CSharpAccessModifier GetterAccessibility => propertyInfo.GetGetMethod(nonPublic: true).GetCSharpAccessModifier();

    public CSharpAccessModifier SetterAccessibility => propertyInfo.GetSetMethod(nonPublic: true).GetCSharpAccessModifier();

    public bool IsXmlIgnored
    {
        get
        {
            isXmlIgnored ??=
                propertyInfo.GetCustomAttribute<System.Xml.Serialization.XmlIgnoreAttribute>() != null;

            return isXmlIgnored.Value;
        }
    }

    public bool IsIgnoredDataMember
    {
        get
        {
            isIgnoredDataMember ??=
                propertyInfo.GetCustomAttribute<System.Runtime.Serialization.IgnoreDataMemberAttribute>() != null;

            return isIgnoredDataMember.Value;
        }
    }

    public bool IsNonSerialized
    {
        get
        {
            // [NonSerialized] only applies to fields.
            return false;
        }
    }

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
