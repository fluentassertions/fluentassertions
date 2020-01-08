using System.Collections;
using System.Collections.Generic;

namespace FluentAssertions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Helper method that adapts a <see cref="IDictionary{TKey,TValue}"/>
        /// to a <see cref="IReadOnlyDictionary{TKey,TValue}"/> interface. It can be useful
        /// for those dictionaries that do not implement read only interface but still
        /// would like to use dictionary specific assertions.
        ///
        /// Note, that contrary to the intuition <see cref="IDictionary{TKey,TValue}"/>
        /// does NOT extend <see cref="IReadOnlyDictionary{TKey,TValue}"/> interface.
        /// That is why this peculiar adapting extension may be helpful.
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <returns><see cref="IReadOnlyDictionary{TKey,TValue}"/> object with contents equal to input <paramref name="dictionary"/></returns>
        public static IReadOnlyDictionary<TKey, TValue>
            AsReadOnlyDictionary<TKey, TValue>(
                this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionaryAdapter<TKey, TValue>(dictionary);
        }

        internal class ReadOnlyDictionaryAdapter<TKey, TValue>
            : IReadOnlyDictionary<TKey, TValue>
        {
            private readonly IDictionary<TKey, TValue> dictionary;

            public ReadOnlyDictionaryAdapter(IDictionary<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
            }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
                dictionary.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => dictionary.GetEnumerator();

            public int Count => dictionary.Count;

            public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

            public bool TryGetValue(TKey key, out TValue value) =>
                dictionary.TryGetValue(key, out value);

            public TValue this[TKey key] => dictionary[key];

            public IEnumerable<TKey> Keys => dictionary.Keys;
            public IEnumerable<TValue> Values => dictionary.Values;
        }
    }
}
