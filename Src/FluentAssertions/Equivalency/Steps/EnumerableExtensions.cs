using System.Collections.Generic;

namespace FluentAssertions.Equivalency.Steps;

[System.Diagnostics.StackTraceHidden]
internal static class EnumerableExtensions
{
    /// <summary>
    /// Converts an enumerable collection of <see cref="IndexedItem{T}"/> into an <see cref="IndexedItemCollection{T}"/>.
    /// </summary>
    public static IndexedItemCollection<T> ToIndexedList<T>(this IEnumerable<IndexedItem<T>> source) => new(source);
}
