using System;

namespace FluentAssertionsAsync.Formatting;

/// <summary>
/// Marks a static method as a kind of <see cref="IValueFormatter"/> for a particular type.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
#pragma warning disable CA1813 // Avoid unsealed attributes. This type has shipped.
public class ValueFormatterAttribute : Attribute
{
}
