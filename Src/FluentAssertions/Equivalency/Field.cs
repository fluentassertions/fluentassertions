using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency;

/// <summary>
/// A specialized type of <see cref="INode"/> that represents a field of an object in a structural equivalency assertion.
/// </summary>
internal class Field : Node, IMember
{
    private readonly FieldInfo fieldInfo;
    private bool? isBrowsable;

    public Field(FieldInfo fieldInfo, INode parent)
    {
        this.fieldInfo = fieldInfo;
        DeclaringType = fieldInfo.DeclaringType;
        ReflectedType = fieldInfo.ReflectedType;
        Subject = new Pathway(parent.Subject.PathAndName, fieldInfo.Name,  pathAndName => $"field {parent.GetSubjectId().Combine(pathAndName)}");
        Expectation = new Pathway(parent.Expectation.PathAndName, fieldInfo.Name, pathAndName => $"field {pathAndName}");
        GetSubjectId = parent.GetSubjectId;
        Type = fieldInfo.FieldType;
        ParentType = fieldInfo.DeclaringType;
        RootIsCollection = parent.RootIsCollection;
    }

    public Type ReflectedType { get; }

    public object GetValue(object obj)
    {
        return fieldInfo.GetValue(obj);
    }

    public Type DeclaringType { get; set; }

    public CSharpAccessModifier GetterAccessibility => fieldInfo.GetCSharpAccessModifier();

    public CSharpAccessModifier SetterAccessibility => fieldInfo.GetCSharpAccessModifier();

    public bool IsBrowsable =>
        isBrowsable ??= fieldInfo.GetCustomAttribute<EditorBrowsableAttribute>() is not { State: EditorBrowsableState.Never };
}
