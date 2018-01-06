using System;

namespace FluentAssertions
{
    /// <summary>
    /// Marks a method as an extension to Fluent Assertions that either uses the built-in assertions
    /// internally, or directly uses the <c>Execute.Assertion</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CustomAssertionAttribute : Attribute
    {
    }
}
