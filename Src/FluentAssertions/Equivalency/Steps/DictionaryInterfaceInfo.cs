using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps
{
    /// <summary>
    /// Provides Reflection-backed meta-data information about a type implementing the <see cref="IDictionary{TKey,TValue}"/> interface.
    /// </summary>
    internal class DictionaryInterfaceInfo
    {
        // ReSharper disable once PossibleNullReferenceException
        private static readonly MethodInfo ConvertToDictionaryMethod =
            new Func<IEnumerable<KeyValuePair<object, object>>, IDictionary<object, object>>(ConvertToDictionaryInternal)
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
        public static bool TryGetFrom(Type target, string role, out DictionaryInterfaceInfo result)
        {
            result = null;

            var interfaces = GetDictionaryInterfacesFrom(target);
            if (interfaces.Length > 1)
            {
                throw new ArgumentException(
                    $"The {role} implements multiple dictionary types. It is not known which type should be " +
                    $"use for equivalence.{Environment.NewLine}The following IDictionary interfaces are implemented: {string.Join(", ", (IEnumerable<DictionaryInterfaceInfo>)interfaces)}");
            }

            if (interfaces.Length == 1)
            {
                result = interfaces.Single();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to reflect on the provided <paramref name="target"/> and returns an instance of the <see cref="DictionaryInterfaceInfo"/>
        /// representing the single dictionary interface keyed to <paramref name="key"/>.
        /// Will throw if the target implements more than one dictionary interface.
        /// </summary>
        /// <remarks>>
        /// The <paramref name="role"/> is used to describe the <paramref name="target"/> in failure messages.
        /// </remarks>
        public static bool TryGetFromWithKey(Type target, string role, Type key, out DictionaryInterfaceInfo result)
        {
            result = null;

            var suitableDictionaryInterfaces = GetDictionaryInterfacesFrom(target)
                .Where(info => info.Key.IsAssignableFrom(key))
                .ToArray();

            if (suitableDictionaryInterfaces.Length > 1)
            {
                // SMELL: Code could be written to handle this better, but is it really worth the effort?
                AssertionScope.Current.FailWith(
                    $"The {role} implements multiple IDictionary interfaces taking a key of {key}. ");

                return false;
            }

            if (suitableDictionaryInterfaces.Length == 0)
            {
                return false;
            }

            result = suitableDictionaryInterfaces.Single();
            return true;
        }

        private static DictionaryInterfaceInfo[] GetDictionaryInterfacesFrom(Type target)
        {
            return Cache.GetOrAdd(target, static key =>
            {
                if (Type.GetTypeCode(key) != TypeCode.Object)
                {
                    return new DictionaryInterfaceInfo[0];
                }
                else
                {
                    return key
                        .GetClosedGenericInterfaces(typeof(IDictionary<,>))
                        .Select(@interface => @interface.GetGenericArguments())
                        .Select(arguments => new DictionaryInterfaceInfo(arguments[0], arguments[1]))
                        .ToArray();
                }
            });
        }

        /// <summary>
        /// Tries to convert an object into a dictionary typed to the <see cref="Key"/> and <see cref="Value"/> of the current <see cref="DictionaryInterfaceInfo"/>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the conversion succeeded or <c>false</c> otherwise.
        /// </returns>
        public bool TryConvertFrom(object convertable, out object dictionary)
        {
            Type[] enumerables = convertable.GetType().GetClosedGenericInterfaces(typeof(IEnumerable<>));

            var suitableKeyValuePairCollection = enumerables
                .Select(enumerable => enumerable.GenericTypeArguments[0])
                .Where(itemType => itemType.IsGenericType && itemType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                .SingleOrDefault(itemType => itemType.GenericTypeArguments.First() == Key);

            if (suitableKeyValuePairCollection != null)
            {
                Type pairValueType = suitableKeyValuePairCollection.GenericTypeArguments.Last();

                var methodInfo = ConvertToDictionaryMethod.MakeGenericMethod(Key, pairValueType);
                dictionary = methodInfo.Invoke(null, new[] { convertable });
                return true;
            }

            dictionary = null;
            return false;
        }

        private static IDictionary<TKey, TValue> ConvertToDictionaryInternal<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            return collection.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public override string ToString() => $"IDictionary<{Key}, {Value}>";
    }
}
