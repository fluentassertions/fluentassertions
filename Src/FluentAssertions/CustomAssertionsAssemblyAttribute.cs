using System;

namespace FluentAssertionsAsync;

/// <summary>
/// Marks an assembly as containing extensions to Fluent Assertions that either uses the built-in assertions
/// internally, or directly uses the <c>Execute.Assertion</c>.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly)]
public sealed class CustomAssertionsAssemblyAttribute : Attribute
{
}
