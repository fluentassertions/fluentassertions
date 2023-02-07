using System;

namespace FluentAssertions.Formatting;

/// <summary>
/// Marks a static method as a kind of <see cref="IValueFormatter"/> for a particular type.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ValueFormatterAttribute : Attribute
{
}
