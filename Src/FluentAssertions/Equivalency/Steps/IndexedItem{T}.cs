namespace FluentAssertions.Equivalency.Steps;

/// <summary>
/// Encapsulates an item of type <typeparamref name="T"/> associated with an index.
/// </summary>
/// <typeparam name="T">The type of the item being indexed.</typeparam>
[System.Diagnostics.StackTraceHidden]
internal class IndexedItem<T>(T item, int index)
{
    public T Item { get; } = item;

    public int Index { get; } = index;
}
