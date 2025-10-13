---
title: Serialization and Ignored Members
permalink: /serializationignoredmembers/
layout: single
toc: true
sidebar:
  nav: "sidebar"

---

The `BeXmlSerializable` and `BeDataContractSerializable` assertions exist to verify that a particular instance can round-trip through serialization. This is done by serializing the object, deserializing the result, and then comparing the objects for equivalency.

The XML serialization infrastructure supports marking fields and properties with the `[XmlIgnore]` attribute. Similarly, members in a `[DataContract]` can be marked `[IgnoreDataMember]`. With legacy binary serialization, fields can be marked `[NonSerialized]`, and the DataContract serializer will respect this when serializing classes marked `[Serializable]`.

When members are marked in this way, the serialized form produced omits the specified members. When deserializing it, the members are not populated with anything. This is the case even if the serialized form is XML that contains elements matching ignored members. By default, Fluent Assertions will include these fields in comparisons, causing `BeXmlSerializable`/`BeDataContractSerializable` assertions to fail.

You can specify that these members should be ignored using a custom implementation of `IMemberSelectionRule`. This can then be configured via the `EquivalencyOptions` object, using an overload that `BeXmlSerializable` or `BeDataContractSerializable` that takes a configuration functor.

The following implementations exclude members marked with `[XmlIgnore]` (`ExcludeXmlIgnoredMembersRule`), `[IgnoreDataMember]` (`ExcludeIgnoredDataMembersRule`), and `[NonSerialized]` (`ExcludeNonSerializedFieldsRule`). If you are encountering assertion failures in `BeXmlSerializable`/`BeDataContractSerializable` assertions due to ignored members, you can include these in your project.

- Use `ExcludeXmlIgnoredMembers` for `BeXmlSerializable` assertions.
- The DataContract serializer respects the `[NonSerialized]` attribute from legacy formatter-based serialization. For full compatibility, use both `ExcludeIgnoredDataMembersRule` and `ExcludeNonSerializedFieldsRule` with `BeDataContractSerializable` assertions.

These rules can also be used when calling `BeEquivalentTo` on object graphs produced explicitly via serialization.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using FluentAssertions.Equivalency;

public abstract class ExcludeByAttributeRuleBase<TAttribute> : IMemberSelectionRule
{
    public bool IncludesMembers => false;

    private static readonly object s_cacheSync = new object();

    private static readonly Dictionary<Type, bool> s_typeCache = new Dictionary<Type, bool>();

    private static readonly Dictionary<MemberInfo, bool> s_memberCache = new Dictionary<MemberInfo, bool>();

    protected virtual bool ShouldProcessType(Type type) => true;

    protected virtual bool ShouldExcludeMember(MemberInfo member)
        => Attribute.IsDefined(member, typeof(TAttribute));

    private bool GetOrComputeShouldProcessType(Type type)
    {
        lock (s_cacheSync)
        {
            if (s_typeCache.TryGetValue(type, out var cachedValue))
                return cachedValue;

            bool shouldProcessType = ShouldProcessType(type);

            s_typeCache[type] = shouldProcessType;

            return shouldProcessType;
        }
    }

    private bool GetOrComputeShouldExcludeMember(IMember member)
    {
        var memberInfo = member.DeclaringType.GetMember(member.Subject.Name);

        bool excludeMember = false;

        if (memberInfo.Length == 1)
        {
            lock (s_cacheSync)
            {
                if (!s_memberCache.TryGetValue(memberInfo[0], out excludeMember))
                {
                    excludeMember = ShouldExcludeMember(memberInfo[0]);

                    s_memberCache[memberInfo[0]] = excludeMember;
                }
            }
        }

        return excludeMember;
    }

    public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers, MemberSelectionContext context)
    {
        return selectedMembers.Where(member => !GetOrComputeShouldProcessType(member.DeclaringType) || !GetOrComputeShouldExcludeMember(member)).ToList();
    }
}

public class ExcludeXmlIgnoredMembersRule : ExcludeByAttributeRuleBase<XmlIgnoreAttribute>
{
}

public class ExcludeIgnoredDataMembersRule : ExcludeByAttributeRuleBase<IgnoreDataMemberAttribute>
{
    protected override bool ShouldProcessType(Type type) => !type.IsSerializable;
}

public class ExcludeNonSerializedFieldsRule : ExcludeByAttributeRuleBase<NonSerializedAttribute>
{
    protected override bool ShouldProcessType(Type type) => type.IsSerializable;
}
```

Example usage:

### XML Serialization

```csharp
public class XmlRecord
{
    public string Name { get; }

    [XmlIgnore]
    public int CachedValue { get; }
}

XmlRecord subject = XmlGetRecord();

subject.Should().BeXmlSerializable(options => options
    .Using(new ExcludeXmlIgnoredMembersRule()));
```

### DataContract

```csharp
[DataContract]
public class Record
{
    [DataMember]
    public string Name { get; }

    [IgnoreDataMember]
    public int CachedValue { get; }
}

Record subject = GetRecord();

subject.Should().BeDataContractSerializable(options => options
    .Using(new ExcludeIgnoredDataMembersRule())
    .Using(new ExcludeNonSerializedFieldsRule()));
```

### BinaryFormatter (and DataContract serialization)

```csharp
[Serializable]
public class Record
{
    // Note, these are fields. By design, BinaryFormatter only serializes fields.
    public string Name;

    [NonSerialized]
    public int CachedValue;
}

Record original = GetRecord();

Record derived = BinaryFormatterDeserialize(BinaryFormatterSerialize(original)); // user-supplied methods

derived.Should().BeEquivalentTo(original, options => options
    .IncludingFields()
    .Using(new ExcludeNonSerializedFieldsRule()));
```
