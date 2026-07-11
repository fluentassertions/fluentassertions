using System;
using System.Diagnostics;

namespace FluentAssertions;

/// <summary>
/// Marks an assembly as containing extensions to Fluent Assertions that either uses the built-in assertions
/// internally, or directly uses <c>AssertionChain</c>.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly)]
[StackTraceHidden]
public sealed class CustomAssertionsAssemblyAttribute : Attribute;

