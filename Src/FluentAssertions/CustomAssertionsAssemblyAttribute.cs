using System;

namespace FluentAssertions;

/// <summary>
/// Marks an assembly as containing extensions to Fluent Assertions that either uses the built-in assertions
/// internally, or directly uses the <c>assertion</c>.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly)]
public sealed class CustomAssertionsAssemblyAttribute : Attribute;
