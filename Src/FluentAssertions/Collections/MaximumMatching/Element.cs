namespace FluentAssertionsAsync.Collections.MaximumMatching;

/// <summary>
/// Stores an element's value and index in the maximum matching problem.
/// </summary>
/// <typeparam name="TValue">The type of the element value.</typeparam>
internal class Element<TValue>
{
    public Element(TValue value, int index)
    {
        Index = index;
        Value = value;
    }

    /// <summary>
    /// The index of the element in the maximum matching problem.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// The value of the element in the maximum matching problem.
    /// </summary>
    public TValue Value { get; }
}
