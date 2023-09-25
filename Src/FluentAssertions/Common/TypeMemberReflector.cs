using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Equivalency;

namespace FluentAssertions.Common;

/// <summary>
/// Helper class to get all the public and internal fields and properties from a type.
/// </summary>
internal sealed class TypeMemberReflector
{
    private const BindingFlags AllInstanceMembersFlag =
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    public TypeMemberReflector(Type typeToReflect, MemberVisibility visibility)
    {
        Properties = LoadProperties(typeToReflect, visibility);
        Fields = LoadFields(typeToReflect, visibility);
        Members = Properties.Concat<MemberInfo>(Fields).ToArray();
    }

    public MemberInfo[] Members { get; }

    public PropertyInfo[] Properties { get; }

    public FieldInfo[] Fields { get; }

    private static PropertyInfo[] LoadProperties(Type typeToReflect, MemberVisibility visibility)
    {
        List<PropertyInfo> query = GetPropertiesFromHierarchy(typeToReflect, visibility);

        return query.ToArray();
    }

    private static List<PropertyInfo> GetPropertiesFromHierarchy(Type typeToReflect, MemberVisibility memberVisibility)
    {
        bool includeInternal = memberVisibility.HasFlag(MemberVisibility.Internal);

        return GetMembersFromHierarchy(typeToReflect, type =>
        {
            return type
                .GetProperties(AllInstanceMembersFlag | BindingFlags.DeclaredOnly)
                .Where(p => HasGetter(p, memberVisibility) && !p.IsIndexer())
                .Where(property => includeInternal || !IsInternal(property))
                .OrderBy(property => IsExplicitImplementation(property));
        });
    }

    private static bool IsInternal(PropertyInfo property)
    {
        return property.GetMethod is { IsAssembly: true } or { IsFamilyOrAssembly: true };
    }

    private static bool IsExplicitImplementation(PropertyInfo property)
    {
        return property.GetMethod.IsPrivate &&
            property.SetMethod?.IsPrivate != false &&
            property.Name.Contains('.', StringComparison.Ordinal);
    }

    private static FieldInfo[] LoadFields(Type typeToReflect, MemberVisibility visibility)
    {
        List<FieldInfo> query = GetFieldsFromHierarchy(typeToReflect, visibility);

        return query.ToArray();
    }

    private static List<FieldInfo> GetFieldsFromHierarchy(Type typeToReflect, MemberVisibility memberVisibility)
    {
        bool includeInternal = memberVisibility.HasFlag(MemberVisibility.Internal);

        return GetMembersFromHierarchy(typeToReflect, type =>
        {
            return type
                .GetFields(AllInstanceMembersFlag)
                .Where(field => !field.IsPrivate && !field.IsFamily)
                .Where(field => includeInternal || !IsInternal(field));
        });
    }

    private static bool IsInternal(FieldInfo field)
    {
        return field.IsAssembly || field.IsFamilyOrAssembly;
    }

    private static List<TMemberInfo> GetMembersFromHierarchy<TMemberInfo>(
        Type typeToReflect,
        Func<Type, IEnumerable<TMemberInfo>> getMembers)
        where TMemberInfo : MemberInfo
    {
        if (typeToReflect.IsInterface)
        {
            return GetInterfaceMembers(typeToReflect, getMembers);
        }

        return GetClassMembers(typeToReflect, getMembers);
    }

    private static List<TMemberInfo> GetInterfaceMembers<TMemberInfo>(Type typeToReflect,
        Func<Type, IEnumerable<TMemberInfo>> getMembers)
        where TMemberInfo : MemberInfo
    {
        List<TMemberInfo> members = new();

        var considered = new List<Type>();
        var queue = new Queue<Type>();
        considered.Add(typeToReflect);
        queue.Enqueue(typeToReflect);

        while (queue.Count > 0)
        {
            Type subType = queue.Dequeue();

            foreach (Type subInterface in subType.GetInterfaces())
            {
                if (considered.Contains(subInterface))
                {
                    continue;
                }

                considered.Add(subInterface);
                queue.Enqueue(subInterface);
            }

            IEnumerable<TMemberInfo> typeMembers = getMembers(subType);

            IEnumerable<TMemberInfo> newPropertyInfos = typeMembers.Where(x => !members.Contains(x));

            members.InsertRange(0, newPropertyInfos);
        }

        return members;
    }

    private static List<TMemberInfo> GetClassMembers<TMemberInfo>(Type typeToReflect,
        Func<Type, IEnumerable<TMemberInfo>> getMembers)
        where TMemberInfo : MemberInfo
    {
        List<TMemberInfo> members = new();

        while (typeToReflect != null)
        {
            foreach (var memberInfo in getMembers(typeToReflect))
            {
                if (members.TrueForAll(mi => mi.Name != memberInfo.Name))
                {
                    members.Add(memberInfo);
                }
            }

            typeToReflect = typeToReflect.BaseType;
        }

        return members;
    }

    private static bool HasGetter(PropertyInfo propertyInfo, MemberVisibility visibility)
    {
        MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: true);

        if (visibility.HasFlag(MemberVisibility.ExplicitlyImplemented))
        {
            return getMethod is { IsPrivate: false, IsFamily: false } or { IsPrivate: true, IsFinal: true };
        }

        return getMethod is { IsPrivate: false, IsFamily: false };
    }
}
