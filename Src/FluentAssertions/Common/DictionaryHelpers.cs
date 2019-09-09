using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Common
{
    internal static class DictionaryHelpers
    {
        public static IEnumerable<TKey> GetKeys<TCollection, TKey, TValue>(this TCollection collection)
            where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
        {
            return collection switch
            {
                IDictionary<TKey, TValue> dictionary => dictionary.Keys,
                IReadOnlyDictionary<TKey, TValue> readOnlyDictionary => readOnlyDictionary.Keys,
                _ => collection.Select(kvp => kvp.Key),
            };
        }

        public static IEnumerable<TValue> GetValues<TCollection, TKey, TValue>(this TCollection collection)
            where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
        {
            return collection switch
            {
                IDictionary<TKey, TValue> dictionary => dictionary.Values,
                IReadOnlyDictionary<TKey, TValue> readOnlyDictionary => readOnlyDictionary.Values,
                _ => collection.Select(kvp => kvp.Value),
            };
        }

        public static bool ContainsKey<TCollection, TKey, TValue>(this TCollection collection, TKey key)
            where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
        {
            return collection switch
            {
                IDictionary<TKey, TValue> dictionary => dictionary.ContainsKey(key),
                IReadOnlyDictionary<TKey, TValue> readOnlyDictionary => readOnlyDictionary.ContainsKey(key),
                _ => collection.Any(kvp => kvp.Key.IsSameOrEqualTo(key)),
            };
        }

        public static bool TryGetValue<TCollection, TKey, TValue>(this TCollection collection, TKey key, out TValue value)
            where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
        {
            return collection switch
            {
                IDictionary<TKey, TValue> dictionary => dictionary.TryGetValue(key, out value),
                IReadOnlyDictionary<TKey, TValue> readOnlyDictionary => readOnlyDictionary.TryGetValue(key, out value),
                _ => TryGetValueInternal(collection, key, out value),
            };
        }

        private static bool TryGetValueInternal<TCollection, TKey, TValue>(this TCollection collection, TKey key, out TValue value)
            where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
        {
            KeyValuePair<TKey, TValue> matchingPair = collection.FirstOrDefault(kvp => kvp.Key.IsSameOrEqualTo(key));
            value = matchingPair.Value;
            return matchingPair.Equals(default(KeyValuePair<TKey, TValue>));
        }

        public static TValue GetValue<TCollection, TKey, TValue>(this TCollection collection, TKey key)
            where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
        {
            return collection switch
            {
                IDictionary<TKey, TValue> dictionary => dictionary[key],
                IReadOnlyDictionary<TKey, TValue> readOnlyDictionary => readOnlyDictionary[key],
                _ => collection.First(kvp => kvp.Key.IsSameOrEqualTo(key)).Value,
            };
        }
    }
}
