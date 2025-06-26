using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FluentAssertions.Common;
using JetBrains.Annotations;

namespace FluentAssertions.Equivalency;

internal sealed class EqualityStrategyProvider
{
    private readonly List<Type> referenceTypes = [];
    private readonly List<Type> valueTypes = [];
    private readonly ConcurrentDictionary<Type, EqualityStrategy> typeCache = new();

    [CanBeNull]
    private readonly Func<Type, EqualityStrategy> defaultStrategy;

    private bool? compareRecordsByValue;

    public EqualityStrategyProvider()
    {
    }

    public EqualityStrategyProvider(Func<Type, EqualityStrategy> defaultStrategy)
    {
        this.defaultStrategy = defaultStrategy;
    }

    public bool? CompareRecordsByValue
    {
        get => compareRecordsByValue;
        set
        {
            compareRecordsByValue = value;
            typeCache.Clear();
        }
    }

    public EqualityStrategy GetEqualityStrategy(Type type)
    {
        // As the valueFactory parameter captures instance members,
        // be aware if the cache must be cleared on mutating the members.
        return typeCache.GetOrAdd(type, typeKey =>
        {
            if (!typeKey.IsPrimitive && referenceTypes.Count > 0 && referenceTypes.Exists(t => typeKey.IsSameOrInherits(t)))
            {
                return EqualityStrategy.ForceMembers;
            }

            if (valueTypes.Count > 0 && valueTypes.Exists(t => typeKey.IsSameOrInherits(t)))
            {
                return EqualityStrategy.ForceEquals;
            }

            if (!typeKey.IsPrimitive && referenceTypes.Count > 0 &&
                referenceTypes.Exists(t => typeKey.IsAssignableToOpenGeneric(t)))
            {
                return EqualityStrategy.ForceMembers;
            }

            if (valueTypes.Count > 0 && valueTypes.Exists(t => typeKey.IsAssignableToOpenGeneric(t)))
            {
                return EqualityStrategy.ForceEquals;
            }

            if ((compareRecordsByValue != null || defaultStrategy is null) && typeKey.IsRecord())
            {
                return compareRecordsByValue is true ? EqualityStrategy.ForceEquals : EqualityStrategy.ForceMembers;
            }

            if (defaultStrategy is not null)
            {
                return defaultStrategy(typeKey);
            }

            return typeKey.HasValueSemantics() ? EqualityStrategy.Equals : EqualityStrategy.Members;
        });
    }

    public bool AddReferenceType(Type type)
    {
        if (valueTypes.Exists(t => type.IsSameOrInherits(t)))
        {
            return false;
        }

        referenceTypes.Add(type);
        typeCache.Clear();
        return true;
    }

    public bool AddValueType(Type type)
    {
        if (referenceTypes.Exists(t => type.IsSameOrInherits(t)))
        {
            return false;
        }

        valueTypes.Add(type);
        typeCache.Clear();
        return true;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        if (compareRecordsByValue is true)
        {
            builder.AppendLine("- Compare records by value");
        }
        else
        {
            builder.AppendLine("- Compare records by their members");
        }

        foreach (Type valueType in valueTypes)
        {
            builder.AppendLine(CultureInfo.InvariantCulture, $"- Compare {valueType} by value");
        }

        foreach (Type type in referenceTypes)
        {
            builder.AppendLine(CultureInfo.InvariantCulture, $"- Compare {type} by its members");
        }

        return builder.ToString();
    }
}
