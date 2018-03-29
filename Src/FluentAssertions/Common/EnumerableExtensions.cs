using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
}
