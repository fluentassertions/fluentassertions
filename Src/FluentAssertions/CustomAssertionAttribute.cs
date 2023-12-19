using System;

namespace FluentAssertionsAsync;

/// <summary>
/// Marks a method as an extension to Fluent Assertions that either uses the built-in assertions
/// internally, or directly uses the <c>Execute.Assertion</c>.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
#pragma warning disable CA1813 // Avoid unsealed attributes. This type has shipped.
public class CustomAssertionAttribute : Attribute
{
}
