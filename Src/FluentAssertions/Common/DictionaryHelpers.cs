using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertionsAsync.Common;

internal static class DictionaryHelpers
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
        return collection switch
        {
            IDictionary<TKey, TValue> dictionary => dictionary.ContainsKey(key),
            IReadOnlyDictionary<TKey, TValue> readOnlyDictionary => readOnlyDictionary.ContainsKey(key),
            _ => ContainsKey(collection, key),
        };

        static bool ContainsKey(TCollection collection, TKey key)
        {
            Func<TKey, TKey, bool> areSameOrEqual = ObjectExtensions.GetComparer<TKey>();
            return collection.Any(kvp => areSameOrEqual(kvp.Key, key));
        }
    }

    public static bool TryGetValue<TCollection, TKey, TValue>(this TCollection collection, TKey key, out TValue value)
        where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        return collection switch
        {
            IDictionary<TKey, TValue> dictionary => dictionary.TryGetValue(key, out value),
            IReadOnlyDictionary<TKey, TValue> readOnlyDictionary => readOnlyDictionary.TryGetValue(key, out value),
            _ => TryGetValue(collection, key, out value),
        };

        static bool TryGetValue(TCollection collection, TKey key, out TValue value)
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
    }

    public static TValue GetValue<TCollection, TKey, TValue>(this TCollection collection, TKey key)
        where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        return collection switch
        {
            IDictionary<TKey, TValue> dictionary => dictionary[key],
            IReadOnlyDictionary<TKey, TValue> readOnlyDictionary => readOnlyDictionary[key],
            _ => GetValue(collection, key),
        };

        static TValue GetValue(TCollection collection, TKey key)
        {
            Func<TKey, TKey, bool> areSameOrEqual = ObjectExtensions.GetComparer<TKey>();
            return collection.First(kvp => areSameOrEqual(kvp.Key, key)).Value;
        }
    }
}
