using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Equivalency.Steps;

/// <summary>
/// Represents a collection of indexed items of type <typeparamref name="T"/> that implements <see cref="IReadOnlyList{T}"/>.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
[System.Diagnostics.StackTraceHidden]
internal class IndexedItemCollection<T> : IReadOnlyList<IndexedItem<T>>
{
    private readonly List<IndexedItem<T>> items = new();

    public IndexedItemCollection(IEnumerable<T> source)
    {
        items = source.Select((item, index) => new IndexedItem<T>(item, index)).ToList();
    }

    public IndexedItemCollection(IEnumerable<IndexedItem<T>> source)
    {
        items.AddRange(source);
    }

    /// <inheritdoc />
    public IEnumerator<IndexedItem<T>> GetEnumerator() => items.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)items).GetEnumerator();

    public int Count => items.Count;

    /// <inheritdoc />
    public IndexedItem<T> this[int index] => items[index];

    public void Remove(IndexedItem<T> item)
    {
        items.Remove(item);
    }

    /// <summary>
    /// Removes items from the specified list of expectations that do not have a corresponding match
    /// in the current collection.
    /// </summary>
    /// <param name="expectations">The list of expectations to be filtered by removing unmatched items.</param>
    public void RemoveMatchedItemFrom(List<T> expectations)
    {
        var existingItems = new HashSet<T>(items.Select(x => x.Item));
        expectations.RemoveAll(expectation => !existingItems.Contains(expectation));
    }
}
