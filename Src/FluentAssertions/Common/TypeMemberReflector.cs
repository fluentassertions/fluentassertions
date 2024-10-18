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
    private readonly HashSet<string> collectedPropertyNames = new();
    private readonly HashSet<string> collectedFields = new();
    private readonly List<FieldInfo> fields = new();
    private List<PropertyInfo> properties = new();

    public TypeMemberReflector(Type typeToReflect, MemberVisibility visibility)
    {
        LoadProperties(typeToReflect, visibility);
        LoadFields(typeToReflect, visibility);

        Members = properties.Concat<MemberInfo>(fields).ToArray();
    }

#pragma warning disable MA0051
    private void LoadProperties(Type typeToReflect, MemberVisibility visibility)
#pragma warning restore MA0051
    {
        while (typeToReflect is not null && typeToReflect != typeof(object))
        {
            PropertyInfo[] allProperties = typeToReflect.GetProperties(
                BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic);

            AddNormalProperties(visibility, allProperties);

            AddExplicitlyImplementedProperties(visibility, allProperties);

            AddInterfaceProperties(typeToReflect, visibility);

            // Move to the base type
            typeToReflect = typeToReflect.BaseType;
        }

        properties = properties.Where(x => !x.IsIndexer()).ToList();
    }

    private void AddNormalProperties(MemberVisibility visibility, PropertyInfo[] allProperties)
    {
        if (visibility.HasFlag(MemberVisibility.Public) || visibility.HasFlag(MemberVisibility.Internal) ||
            visibility.HasFlag(MemberVisibility.ExplicitlyImplemented))
        {
            foreach (PropertyInfo property in allProperties)
            {
                if (!collectedPropertyNames.Contains(property.Name) && !IsExplicitlyImplemented(property) &&
                    HasVisibility(visibility, property))
                {
                    properties.Add(property);
                    collectedPropertyNames.Add(property.Name);
                }
            }
        }
    }

    private static bool HasVisibility(MemberVisibility visibility, PropertyInfo prop) =>
        (visibility.HasFlag(MemberVisibility.Public) && prop.GetMethod?.IsPublic is true) ||
        (visibility.HasFlag(MemberVisibility.Internal) &&
            (prop.GetMethod?.IsAssembly is true || prop.GetMethod?.IsFamilyOrAssembly is true));

    private void AddExplicitlyImplementedProperties(MemberVisibility visibility, PropertyInfo[] allProperties)
    {
        if (visibility.HasFlag(MemberVisibility.ExplicitlyImplemented))
        {
            foreach (var p in allProperties)
            {
                if (IsExplicitlyImplemented(p))
                {
                    var name = p.Name.Split('.').Last();

                    if (!collectedPropertyNames.Contains(name))
                    {
                        properties.Add(p);
                        collectedPropertyNames.Add(name);
                    }
                }
            }
        }
    }

    private void AddInterfaceProperties(Type typeToReflect, MemberVisibility visibility)
    {
        if (visibility.HasFlag(MemberVisibility.DefaultInterfaceProperties) || typeToReflect.IsInterface)
        {
            // Add explicitly implemented interface properties (not included above)
            var interfaces = typeToReflect.GetInterfaces();

            foreach (var iface in interfaces)
            {
                foreach (var prop in iface.GetProperties())
                {
                    if (!collectedPropertyNames.Contains(prop.Name) && (!prop.GetMethod!.IsAbstract || typeToReflect.IsInterface))
                    {
                        properties.Add(prop);
                        collectedPropertyNames.Add(prop.Name);
                    }
                }
            }
        }
    }

    private static bool IsExplicitlyImplemented(PropertyInfo prop)
    {
        return prop.Name.Contains('.', StringComparison.OrdinalIgnoreCase);
    }

    private void LoadFields(Type typeToReflect, MemberVisibility visibility)
    {
        while (typeToReflect is not null && typeToReflect != typeof(object))
        {
            FieldInfo[] files = typeToReflect.GetFields(
                BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in files)
            {
                if (!collectedFields.Contains(field.Name) && HasVisibility(visibility, field))
                {
                    fields.Add(field);
                    collectedFields.Add(field.Name);
                }
            }

            // Move to the base type
            typeToReflect = typeToReflect.BaseType;
        }
    }

    private static bool HasVisibility(MemberVisibility visibility, FieldInfo field) =>
        (visibility.HasFlag(MemberVisibility.Public) && field.IsPublic) ||
        (visibility.HasFlag(MemberVisibility.Internal) && (field.IsAssembly || field.IsFamilyOrAssembly));

    public MemberInfo[] Members { get; }

    public PropertyInfo[] Properties => properties.ToArray();

    public FieldInfo[] Fields => fields.ToArray();
}
