using System.Collections.Generic;
using System.Diagnostics;

namespace FluentAssertions.Equivalency.Steps;

[StackTraceHidden]
internal static class EnumerableExtensions
{
    /// <summary>
    /// Converts an enumerable collection of <see cref="IndexedItem{T}"/> into an <see cref="IndexedItemCollection{T}"/>.
    /// </summary>
    public static IndexedItemCollection<T> ToIndexedList<T>(this IEnumerable<IndexedItem<T>> source) => new(source);
}
