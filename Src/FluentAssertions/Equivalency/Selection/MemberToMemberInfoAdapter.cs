using System;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Equivalency.Selection;

/// <summary>
/// Represents a selection context of a nested property
/// </summary>
internal class MemberToMemberInfoAdapter : IMemberInfo
{
    private readonly IMember member;

    public MemberToMemberInfoAdapter(IMember member)
    {
        this.member = member;
        DeclaringType = member.DeclaringType;
        Name = member.Name;
        Type = member.Type;
        Path = member.PathAndName;
    }

    public string Name { get; }

    public Type Type { get; }

    public Type DeclaringType { get; }

    public string Path { get; set; }

    public CSharpAccessModifier GetterAccessibility => member.GetterAccessibility;

    public CSharpAccessModifier SetterAccessibility => member.SetterAccessibility;
}
