using System.Collections;
using System.Collections.Generic;

namespace FluentAssertions.Extensions
{
    public static class DictionaryExtensions
    {
        internal class ReadOnlyDictionaryAdapter<TKey, TValue>
            : IReadOnlyDictionary<TKey, TValue>
        {
            private readonly IDictionary<TKey, TValue> Dictionary;

            public ReadOnlyDictionaryAdapter(IDictionary<TKey, TValue> dictionary)
            {
                Dictionary = dictionary;
            }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
                Dictionary.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public int Count => Dictionary.Count;

            public bool ContainsKey(TKey key) => Dictionary.ContainsKey(key);

            public bool TryGetValue(TKey key, out TValue value) => Dictionary.TryGetValue(key, out value);

            public TValue this[TKey key] => Dictionary[key];

            public IEnumerable<TKey> Keys => Dictionary.Keys;
            public IEnumerable<TValue> Values => Dictionary.Values;
        }

        public static IReadOnlyDictionary<TKey, TValue>
            AsReadOnlyDictionary<TKey, TValue>(
                this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionaryAdapter<TKey, TValue>(dictionary);
        }
    }
}
