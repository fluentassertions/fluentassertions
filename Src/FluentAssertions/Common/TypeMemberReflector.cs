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
        IEnumerable<PropertyInfo> query =
            from propertyInfo in GetPropertiesFromHierarchy(typeToReflect, visibility)
            where HasGetter(propertyInfo, visibility)
            where !propertyInfo.IsIndexer()
            select propertyInfo;

        return query.ToArray();
    }

    private static IEnumerable<PropertyInfo> GetPropertiesFromHierarchy(Type typeToReflect, MemberVisibility memberVisibility)
    {
        bool includeInternal = memberVisibility.HasFlag(MemberVisibility.Internal);

        return GetMembersFromHierarchy(typeToReflect, type =>
        {
            return type
                .GetProperties(AllInstanceMembersFlag | BindingFlags.DeclaredOnly)
                .Where(property => includeInternal || !IsInternal(property))
                .OrderBy(property => IsExplicitImplementation(property))
                .ToArray();
        });
    }

    private static bool IsInternal(PropertyInfo property)
    {
        return property.GetMethod is { IsAssembly: true } or { IsFamilyOrAssembly: true };
    }

    private static bool IsExplicitImplementation(PropertyInfo property)
    {
        return property.GetMethod?.IsPrivate == true &&
            property.SetMethod?.IsPrivate != false &&
            property.Name.Contains('.', StringComparison.Ordinal);
    }

    private static FieldInfo[] LoadFields(Type typeToReflect, MemberVisibility visibility)
    {
        IEnumerable<FieldInfo> query =
            from fieldInfo in GetFieldsFromHierarchy(typeToReflect, visibility)
            where !fieldInfo.IsPrivate
            where !fieldInfo.IsFamily
            select fieldInfo;

        return query.ToArray();
    }

    private static IEnumerable<FieldInfo> GetFieldsFromHierarchy(Type typeToReflect, MemberVisibility memberVisibility)
    {
        bool includeInternal = memberVisibility.HasFlag(MemberVisibility.Internal);

        return GetMembersFromHierarchy(typeToReflect, type =>
        {
            return type
                .GetFields(AllInstanceMembersFlag)
                .Where(field => !field.IsPrivate)
                .Where(field => includeInternal || !IsInternal(field))
                .ToArray();
        });
    }

    private static bool IsInternal(FieldInfo field)
    {
        return field.IsAssembly || field.IsFamilyOrAssembly;
    }

    private static IEnumerable<TMemberInfo> GetMembersFromHierarchy<TMemberInfo>(
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

    private static IEnumerable<TMemberInfo> GetInterfaceMembers<TMemberInfo>(Type typeToReflect,
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

    private static IEnumerable<TMemberInfo> GetClassMembers<TMemberInfo>(Type typeToReflect,
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
