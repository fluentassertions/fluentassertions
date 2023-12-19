using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Equivalency.Steps;

/// <summary>
/// Provides Reflection-backed meta-data information about a type implementing the <see cref="IDictionary{TKey,TValue}"/> interface.
/// </summary>
internal sealed class DictionaryInterfaceInfo
{
    // ReSharper disable once PossibleNullReferenceException
    private static readonly MethodInfo ConvertToDictionaryMethod =
        new Func<IEnumerable<KeyValuePair<object, object>>, Dictionary<object, object>>(ConvertToDictionaryInternal)
            .GetMethodInfo().GetGenericMethodDefinition();

    private static readonly ConcurrentDictionary<Type, DictionaryInterfaceInfo[]> Cache = new();

    private DictionaryInterfaceInfo(Type key, Type value)
    {
        Key = key;
        Value = value;
    }

    public Type Value { get; }

    public Type Key { get; }

    /// <summary>
    /// Tries to reflect on the provided <paramref name="target"/> and returns an instance of the <see cref="DictionaryInterfaceInfo"/>
    /// representing the single dictionary interface. Will throw if the target implements more than one dictionary interface.
    /// </summary>
    /// <remarks>>
    /// The <paramref name="role"/> is used to describe the <paramref name="target"/> in failure messages.
    /// </remarks>
    public static DictionaryInterfaceInfo FindFrom(Type target, string role)
    {
        var interfaces = GetDictionaryInterfacesFrom(target);

        if (interfaces.Length > 1)
        {
            throw new ArgumentException(
                $"The {role} implements multiple dictionary types. It is not known which type should be " +
                $"use for equivalence.{Environment.NewLine}The following IDictionary interfaces are implemented: " +
                $"{string.Join(", ", (IEnumerable<DictionaryInterfaceInfo>)interfaces)}", nameof(role));
        }

        if (interfaces.Length == 0)
        {
            return null;
        }

        return interfaces[0];
    }

    /// <summary>
    /// Tries to reflect on the provided <paramref name="target"/> and returns an instance of the <see cref="DictionaryInterfaceInfo"/>
    /// representing the single dictionary interface keyed to <paramref name="key"/>.
    /// Will throw if the target implements more than one dictionary interface.
    /// </summary>
    /// <remarks>>
    /// The <paramref name="role"/> is used to describe the <paramref name="target"/> in failure messages.
    /// </remarks>
    public static DictionaryInterfaceInfo FindFromWithKey(Type target, string role, Type key)
    {
        var suitableDictionaryInterfaces = GetDictionaryInterfacesFrom(target)
            .Where(info => info.Key.IsAssignableFrom(key))
            .ToArray();

        if (suitableDictionaryInterfaces.Length > 1)
        {
            // SMELL: Code could be written to handle this better, but is it really worth the effort?
            AssertionScope.Current.FailWith(
                $"The {role} implements multiple IDictionary interfaces taking a key of {key}. ");

            return null;
        }

        if (suitableDictionaryInterfaces.Length == 0)
        {
            return null;
        }

        return suitableDictionaryInterfaces[0];
    }

    private static DictionaryInterfaceInfo[] GetDictionaryInterfacesFrom(Type target)
    {
        return Cache.GetOrAdd(target, static key =>
        {
            if (Type.GetTypeCode(key) != TypeCode.Object)
            {
                return Array.Empty<DictionaryInterfaceInfo>();
            }

            return key
                .GetClosedGenericInterfaces(typeof(IDictionary<,>))
                .Select(@interface => @interface.GetGenericArguments())
                .Select(arguments => new DictionaryInterfaceInfo(arguments[0], arguments[1]))
                .ToArray();
        });
    }

    /// <summary>
    /// Tries to convert an object into a dictionary typed to the <see cref="Key"/> and <see cref="Value"/> of the current <see cref="DictionaryInterfaceInfo"/>.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the conversion succeeded or <see langword="false"/> otherwise.
    /// </returns>
    public object ConvertFrom(object convertable)
    {
        Type[] enumerables = convertable.GetType().GetClosedGenericInterfaces(typeof(IEnumerable<>));

        var suitableKeyValuePairCollection = enumerables
            .Select(enumerable => enumerable.GenericTypeArguments[0])
            .Where(itemType => itemType.IsGenericType && itemType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
            .SingleOrDefault(itemType => itemType.GenericTypeArguments[0] == Key);

        if (suitableKeyValuePairCollection != null)
        {
            Type pairValueType = suitableKeyValuePairCollection.GenericTypeArguments[^1];

            var methodInfo = ConvertToDictionaryMethod.MakeGenericMethod(Key, pairValueType);
            return methodInfo.Invoke(null, new[] { convertable });
        }

        return null;
    }

    private static Dictionary<TKey, TValue> ConvertToDictionaryInternal<TKey, TValue>(
        IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
        return collection.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public override string ToString() => $"IDictionary<{Key}, {Value}>";
}
