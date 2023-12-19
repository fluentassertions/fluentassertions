using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertionsAsync.Equivalency;

namespace FluentAssertionsAsync.Common;

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
        bool includeExplicitlyImplemented = memberVisibility.HasFlag(MemberVisibility.ExplicitlyImplemented);

        return GetMembersFromHierarchy(typeToReflect, type =>
        {
            return
                from p in type.GetProperties(AllInstanceMembersFlag | BindingFlags.DeclaredOnly)
                where p.GetMethod is { } getMethod
                    && (IsPublic(getMethod) || (includeExplicitlyImplemented && IsExplicitlyImplemented(getMethod)))
                    && (includeInternal || !IsInternal(getMethod))
                    && !p.IsIndexer()
                orderby IsExplicitImplementation(p)
                select p;
        });
    }

    private static bool IsPublic(MethodBase getMethod) =>
        !getMethod.IsPrivate && !getMethod.IsFamily && !getMethod.IsFamilyAndAssembly;

    private static bool IsExplicitlyImplemented(MethodBase getMethod) =>
        getMethod.IsPrivate && getMethod.IsFinal;

    private static bool IsInternal(MethodBase getMethod) =>
        getMethod.IsAssembly || getMethod.IsFamilyOrAssembly;

    private static bool IsExplicitImplementation(PropertyInfo property)
    {
        return property.GetMethod!.IsPrivate &&
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
                .Where(field => IsPublic(field))
                .Where(field => includeInternal || !IsInternal(field));
        });
    }

    private static bool IsPublic(FieldInfo field) =>
        !field.IsPrivate && !field.IsFamily && !field.IsFamilyAndAssembly;

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
}
