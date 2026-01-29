using System;

namespace FluentAssertions;

/// <summary>
/// Marks a method as an extension to Fluent Assertions that either uses the built-in assertions
/// internally, or directly uses <c>AssertionChain</c>.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
#pragma warning disable CA1813 // Avoid unsealed attributes. This type has shipped.
[System.Diagnostics.StackTraceHidden]
public class CustomAssertionAttribute : Attribute;

