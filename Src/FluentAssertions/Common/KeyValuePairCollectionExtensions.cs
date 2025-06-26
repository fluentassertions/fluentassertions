using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Common;

internal static class KeyValuePairCollectionExtensions
{
    public static IEnumerable<TKey> GetKeys<TCollection, TKey, TValue>(this TCollection collection)
        where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        return collection switch
        {
            IDictionary<TKey, TValue> dictionary => dictionary.Keys,
            IReadOnlyDictionary<TKey, TValue> readOnlyDictionary => readOnlyDictionary.Keys,
            _ => collection.Select(kvp => kvp.Key).ToList(),
        };
    }

    public static IEnumerable<TValue> GetValues<TCollection, TKey, TValue>(this TCollection collection)
        where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        return collection switch
        {
            IDictionary<TKey, TValue> dictionary => dictionary.Values,
            IReadOnlyDictionary<TKey, TValue> readOnlyDictionary => readOnlyDictionary.Values,
            _ => collection.Select(kvp => kvp.Value).ToList(),
        };
    }

    public static bool ContainsKey<TCollection, TKey, TValue>(this TCollection collection, TKey key)
        where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        Func<TKey, TKey, bool> areSameOrEqual = ObjectExtensions.GetComparer<TKey>();
        return collection switch
        {
            IDictionary<TKey, TValue> dictionary => dictionary.ContainsKey(key),
            IReadOnlyDictionary<TKey, TValue> readOnlyDictionary => readOnlyDictionary.ContainsKey(key),
            _ => collection.Any(kvp => areSameOrEqual(kvp.Key, key)),
        };
    }

    public static bool TryGetValue<TCollection, TKey, TValue>(this TCollection collection, TKey key, out TValue value)
        where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        return collection switch
        {
            IDictionary<TKey, TValue> dictionary => dictionary.TryGetValue(key, out value),
            IReadOnlyDictionary<TKey, TValue> readOnlyDictionary => readOnlyDictionary.TryGetValue(key, out value),
            _ => TryGetValueLocal(collection, key, out value),
        };
    }

    private static bool TryGetValueLocal<TCollection, TKey, TValue>(TCollection collection, TKey key, out TValue value)
        where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        Func<TKey, TKey, bool> areSameOrEqual = ObjectExtensions.GetComparer<TKey>();

        foreach (var kvp in collection)
        {
            if (areSameOrEqual(kvp.Key, key))
            {
                value = kvp.Value;
                return true;
            }
        }

        value = default;
        return false;
    }

    public static TValue GetValue<TCollection, TKey, TValue>(this TCollection collection, TKey key)
        where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        Func<TKey, TKey, bool> areSameOrEqual = ObjectExtensions.GetComparer<TKey>();
        return collection switch
        {
            IDictionary<TKey, TValue> dictionary => dictionary[key],
            IReadOnlyDictionary<TKey, TValue> readOnlyDictionary => readOnlyDictionary[key],
            _ => collection.First(kvp => areSameOrEqual(kvp.Key, key)).Value,
        };
    }
}
