using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Equivalency.Steps;

[System.Diagnostics.StackTraceHidden]
internal static class ReadOnlyCollectionExtensions
{
    /// <summary>
    /// Generates all possible permutations of the given collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="items">The collection of items for which permutations are to be generated.</param>
    /// <returns>A sequence of all possible permutations, where each permutation is represented as a read-only list of elements.</returns>
    public static IEnumerable<IReadOnlyList<T>> Permute<T>(this IReadOnlyList<T> items)
    {
        int[] indices = Enumerable.Range(0, items.Count).ToArray();

        do
        {
            var result = new T[indices.Length];
            for (int index = 0; index < indices.Length; index++)
            {
                result[index] = items[indices[index]];
            }

            yield return result;
        }
        while (NextPermutation(indices));
    }

    private static bool NextPermutation(int[] indices)
    {
        int index = indices.Length - 2;
        while (index >= 0 && indices[index] >= indices[index + 1])
        {
            index--;
        }

        if (index < 0)
        {
            return false;
        }

        int j = indices.Length - 1;
        while (indices[j] <= indices[index])
        {
            j--;
        }

        (indices[index], indices[j]) = (indices[j], indices[index]);
        Array.Reverse(indices, index + 1, indices.Length - index - 1);
        return true;
    }

}
