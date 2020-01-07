using System.Collections.Generic;

namespace FluentAssertions.Extensions
{
    public static class DictionaryExtensions
    {
        public static IReadOnlyDictionary<TKey, TValue>
            AsReadOnlyDictionary<TKey, TValue>(
                this IDictionary<TKey, TValue> dictionary)
        {
            var result = new Dictionary<TKey, TValue>();
            foreach (var kvp in dictionary)
            {
                result.Add(kvp.Key, kvp.Value);    
            }
            return result;
        }
    }
}
