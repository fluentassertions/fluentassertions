namespace FluentAssertionsAsync.Formatting;

/// <summary>
/// Represents a method that can be used to format child values from inside an <see cref="IValueFormatter"/>.
/// </summary>
/// <param name="childPath">
/// Represents the path from the current location to the child value.
/// </param>
/// <param name="value">
/// The child value to format with the configured <see cref="IValueFormatter"/>s.
/// </param>
public delegate void FormatChild(string childPath, object value, FormattedObjectGraph formattedGraph);
