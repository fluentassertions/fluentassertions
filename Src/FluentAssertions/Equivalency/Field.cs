using System;
using System.ComponentModel;
using System.Reflection;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Equivalency;

/// <summary>
/// A specialized type of <see cref="INode  "/> that represents a field of an object in a structural equivalency assertion.
/// </summary>
public class Field : Node, IMember
{
    private readonly FieldInfo fieldInfo;
    private bool? isBrowsable;

    public Field(FieldInfo fieldInfo, INode parent)
        : this(fieldInfo.ReflectedType, fieldInfo, parent)
    {
    }

    public Field(Type reflectedType, FieldInfo fieldInfo, INode parent)
    {
        this.fieldInfo = fieldInfo;
        DeclaringType = fieldInfo.DeclaringType;
        ReflectedType = reflectedType;
        Path = parent.PathAndName;
        GetSubjectId = parent.GetSubjectId;
        Name = fieldInfo.Name;
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

    public override string Description => $"field {GetSubjectId().Combine(PathAndName)}";

    public CSharpAccessModifier GetterAccessibility => fieldInfo.GetCSharpAccessModifier();

    public CSharpAccessModifier SetterAccessibility => fieldInfo.GetCSharpAccessModifier();

    public bool IsBrowsable =>
        isBrowsable ??= fieldInfo.GetCustomAttribute<EditorBrowsableAttribute>() is not { State: EditorBrowsableState.Never };
}
