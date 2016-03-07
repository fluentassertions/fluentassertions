using System;
using System.Collections.Generic;

namespace FluentAssertions.Common
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Searches for the first different element in two sequences using specified <paramref name="predicate" />
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of the <paramref name="first" /> sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the <paramref name="second" /> sequence.</typeparam>
        /// <param name="first">The first sequence to compare.</param>
        /// <param name="second">The second sequence to compare.</param>
        /// <param name="predicate">Method that is used to compare 2 elements with the same index.</param>
        /// <returns>Index at which two sequences have elements that are not equal, or -1 if enumerables are equal</returns>
        public static int IndexOfFirstDifferenceWith<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, bool> predicate)
        {
            using (var firstEnumerator = first.GetEnumerator())
            using (var secondEnumerator = second.GetEnumerator())
            {
                var index = 0;
                while (true)
                {
                    var isFirstCompleted = !firstEnumerator.MoveNext();
                    var isSecondCompleted = !secondEnumerator.MoveNext();

                    if (isFirstCompleted && isSecondCompleted)
                    {
                        return -1;
                    }

                    if (isFirstCompleted ^ isSecondCompleted)
                    {
                        return index;
                    }

                    if (!predicate(firstEnumerator.Current, secondEnumerator.Current))
                    {
                        return index;
                    }     
                                   
                    index++;
                }   
            }
        }
    }
}