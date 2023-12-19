using System;
using System.ComponentModel;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Equivalency;

/// <summary>
/// Exposes information about an object's member
/// </summary>
public interface IMember : INode
{
    /// <summary>
    /// Gets the type that declares the current member.
    /// </summary>
    Type DeclaringType { get; }

    /// <summary>
    /// Gets the type that was used to determine this member.
    /// </summary>
    Type ReflectedType { get; }

    /// <summary>
    /// Gets the value of the member from the provided <paramref name="obj"/>
    /// </summary>
    object GetValue(object obj);

    /// <summary>
    /// Gets the access modifier for the getter of this member.
    /// </summary>
    CSharpAccessModifier GetterAccessibility { get; }

    /// <summary>
    /// Gets the access modifier for the setter of this member.
    /// </summary>
    CSharpAccessModifier SetterAccessibility { get; }

    /// <summary>
    /// Gets a value indicating whether the member is browsable in the source code editor. This is controlled with
    /// <see cref="EditorBrowsableAttribute"/>.
    /// </summary>
    bool IsBrowsable { get; }
}
