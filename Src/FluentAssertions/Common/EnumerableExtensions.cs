using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Common
{
    internal static class EnumerableExtensions
    {
        public static ICollection<T> ConvertOrCastToCollection<T>(this IEnumerable<T> source)
        {
            return (source as ICollection<T>) ?? source.ToList();
        }

        public static ICollection<T> ConvertOrCastToCollection<T>(this IEnumerable source)
        {
            return (source as ICollection<T>) ?? source.Cast<T>().ToList();
        }

        public static IList<T> ConvertOrCastToList<T>(this IEnumerable<T> source)
        {
            return (source as IList<T>) ?? source.ToList();
        }

        public static IList<T> ConvertOrCastToList<T>(this IEnumerable source)
        {
            return (source as IList<T>) ?? source.Cast<T>().ToList();
        }

        /// <summary>
        /// Searches for the first different element in two sequences using specified <paramref name="equalityComparison" />
        /// </summary>
        /// <param name="first">The first sequence to compare.</param>
        /// <param name="second">The second sequence to compare.</param>
        /// <param name="equalityComparison">Method that is used to compare 2 elements with the same index.</param>
        /// <returns>Index at which two sequences have elements that are not equal, or -1 if enumerables are equal</returns>
        public static int IndexOfFirstDifferenceWith(this object first, IEnumerable second, Func<object, object, bool> equalityComparison)
        {

            Type enumerableType = first.GetType();
            MethodInfo getEnumerator = enumerableType.GetPublicExplicitParameterlessMethod("GetEnumerator");

            Type enumeratorType = getEnumerator.ReturnType;
            PropertyInfo current = enumeratorType.GetPublicExplicitProperty("Current");
            MethodInfo moveNext = enumeratorType.GetPublicExplicitParameterlessMethod("MoveNext");
            MethodInfo dispose = enumeratorType.GetPublicExplicitParameterlessMethod("Dispose");

#if !NETSTANDARD2_1
            // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
            if (current.PropertyType.IsByRef)
                return -1; // what should we do here?????
#endif

            object firstEnumerator = getEnumerator.Invoke(first, new object[0]);
            IEnumerator secondEnumerator = second.GetEnumerator();
            try
            {
                int index = 0;
                checked
                {
                    while (true)
                    {
                        bool isFirstCompleted = !(bool)moveNext.Invoke(firstEnumerator, new object[0]);
                        bool isSecondCompleted = !secondEnumerator.MoveNext();

                        if (isFirstCompleted && isSecondCompleted)
                        {
                            return -1;
                        }

                        if (isFirstCompleted ^ isSecondCompleted)
                        {
                            return index;
                        }

                        if (!equalityComparison(current.GetValue(firstEnumerator), secondEnumerator.Current))
                        {
                            return index;
                        }

                        index++;
                    }
                }
            }
            finally
            {
                dispose?.Invoke(firstEnumerator, new object[0]);
            }
        }

        /// <summary>
        /// Searches for the first different element in two sequences using specified <paramref name="equalityComparison" />
        /// </summary>
        /// <param name="first">The first sequence to compare.</param>
        /// <param name="second">The second sequence to compare.</param>
        /// <param name="equalityComparison">Method that is used to compare 2 elements with the same index.</param>
        /// <returns>Index at which two sequences have elements that are not equal, or -1 if enumerables are equal</returns>
        public static int IndexOfFirstDifferenceWith(this IEnumerable first, IEnumerable second, Func<object, object, bool> equalityComparison)
        {
            IEnumerator firstEnumerator = first.GetEnumerator();
            IEnumerator secondEnumerator = second.GetEnumerator();
            int index = 0;
            checked
            {
                while (true)
                {
                    bool isFirstCompleted = !firstEnumerator.MoveNext();
                    bool isSecondCompleted = !secondEnumerator.MoveNext();

                    if (isFirstCompleted && isSecondCompleted)
                    {
                        return -1;
                    }

                    if (isFirstCompleted ^ isSecondCompleted)
                    {
                        return index;
                    }

                    if (!equalityComparison(firstEnumerator.Current, secondEnumerator.Current))
                    {
                        return index;
                    }

                    index++;
                }
            }
        }

        /// <summary>
        /// Searches for the first different element in two sequences using specified <paramref name="equalityComparison" />
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of the <paramref name="first" /> sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the <paramref name="second" /> sequence.</typeparam>
        /// <param name="first">The first sequence to compare.</param>
        /// <param name="second">The second sequence to compare.</param>
        /// <param name="equalityComparison">Method that is used to compare 2 elements with the same index.</param>
        /// <returns>Index at which two sequences have elements that are not equal, or -1 if enumerables are equal</returns>
        public static int IndexOfFirstDifferenceWith<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, bool> equalityComparison)
        {
            using (IEnumerator<TFirst> firstEnumerator = first.GetEnumerator())
            using (IEnumerator<TSecond> secondEnumerator = second.GetEnumerator())
            {
                int index = 0;
                checked
                {
                    while (true)
                    {
                        bool isFirstCompleted = !firstEnumerator.MoveNext();
                        bool isSecondCompleted = !secondEnumerator.MoveNext();

                        if (isFirstCompleted && isSecondCompleted)
                        {
                            return -1;
                        }

                        if (isFirstCompleted ^ isSecondCompleted)
                        {
                            return index;
                        }

                        if (!equalityComparison(firstEnumerator.Current, secondEnumerator.Current))
                        {
                            return index;
                        }

                        index++;
                    }
                }
            }
        }

        /// <summary>
        /// Searches for the first different element in two sequences using specified <paramref name="equalityComparison" />
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of the <paramref name="first" /> sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the <paramref name="second" /> sequence.</typeparam>
        /// <param name="first">The first sequence to compare.</param>
        /// <param name="second">The second sequence to compare.</param>
        /// <param name="equalityComparison">Method that is used to compare 2 elements with the same index.</param>
        /// <returns>Index at which two sequences have elements that are not equal, or -1 if enumerables are equal</returns>
        public static int IndexOfFirstDifferenceWith<TFirst, TSecond>(this IReadOnlyList<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, bool> equalityComparison)
        {
            using (IEnumerator<TSecond> secondEnumerator = second.GetEnumerator())
            {
                int index = 0;
                checked
                {
                    while (true)
                    {
                        bool isFirstCompleted = (index == first.Count);
                        bool isSecondCompleted = !secondEnumerator.MoveNext();

                        if (isFirstCompleted && isSecondCompleted)
                        {
                            return -1;
                        }

                        if (isFirstCompleted ^ isSecondCompleted)
                        {
                            return index;
                        }

                        if (!equalityComparison(first[index], secondEnumerator.Current))
                        {
                            return index;
                        }

                        index++;
                    }
                }
            }
        }
    }
}
