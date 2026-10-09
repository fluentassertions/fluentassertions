namespace FluentAssertions.Collections;

/// <summary>
/// Defines the direction in which a collection is expected to be ordered.
/// </summary>
public enum SortOrder
{
    /// <summary>
    /// The collection is expected to be ordered from the smallest to the largest value.
    /// </summary>
    Ascending,

    /// <summary>
    /// The collection is expected to be ordered from the largest to the smallest value.
    /// </summary>
    Descending
}
