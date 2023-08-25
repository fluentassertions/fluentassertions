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
        NonPrivateProperties = LoadNonPrivateProperties(typeToReflect, visibility);
        NonPrivateFields = LoadNonPrivateFields(typeToReflect, visibility);
        NonPrivateMembers = NonPrivateProperties.Concat<MemberInfo>(NonPrivateFields).ToArray();
    }

    public MemberInfo[] NonPrivateMembers { get; }

    public PropertyInfo[] NonPrivateProperties { get; }

    public FieldInfo[] NonPrivateFields { get; }

    private static PropertyInfo[] LoadNonPrivateProperties(Type typeToReflect, MemberVisibility visibility)
    {
        IEnumerable<PropertyInfo> query =
            from propertyInfo in GetPropertiesFromHierarchy(typeToReflect, visibility)
            where HasNonPrivateGetter(propertyInfo)
            where !propertyInfo.IsIndexer()
            select propertyInfo;

        return query.ToArray();
    }

    private static List<PropertyInfo> GetPropertiesFromHierarchy(Type typeToReflect, MemberVisibility memberVisibility)
    {
        bool includeInternals = memberVisibility.HasFlag(MemberVisibility.Internal);

        return GetMembersFromHierarchy(typeToReflect, type =>
        {
            return type
                .GetProperties(AllInstanceMembersFlag | BindingFlags.DeclaredOnly)
                .Where(property => property.GetMethod?.IsPrivate == false)
                .Where(property => includeInternals || property.GetMethod is { IsAssembly: false, IsFamilyOrAssembly: false })
                .ToArray();
        });
    }

    private static FieldInfo[] LoadNonPrivateFields(Type typeToReflect, MemberVisibility visibility)
    {
        IEnumerable<FieldInfo> query =
            from fieldInfo in GetFieldsFromHierarchy(typeToReflect, visibility)
            where !fieldInfo.IsPrivate
            where !fieldInfo.IsFamily
            select fieldInfo;

        return query.ToArray();
    }

    private static List<FieldInfo> GetFieldsFromHierarchy(Type typeToReflect, MemberVisibility memberVisibility)
    {
        bool includeInternals = memberVisibility.HasFlag(MemberVisibility.Internal);

        return GetMembersFromHierarchy(typeToReflect, type =>
        {
            return type
                .GetFields(AllInstanceMembersFlag)
                .Where(field => !field.IsPrivate)
                .Where(field => includeInternals || (!field.IsAssembly && !field.IsFamilyOrAssembly))
                .ToArray();
        });
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

    private static bool HasNonPrivateGetter(PropertyInfo propertyInfo)
    {
        MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: true);
        return getMethod is { IsPrivate: false, IsFamily: false };
    }
}
