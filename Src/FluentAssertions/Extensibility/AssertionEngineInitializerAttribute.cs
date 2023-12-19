using System;
using System.Reflection;

namespace FluentAssertionsAsync.Extensibility;

/// <summary>
/// Can be added to an assembly so it gets a change to initialize Fluent Assertions before the first assertion happens.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class AssertionEngineInitializerAttribute : Attribute
{
    private readonly string methodName;
    private readonly Type type;

    /// <summary>
    /// Defines the static void-returning and parameterless method that should be invoked before the first assertion happens.
    /// </summary>
#pragma warning disable CA1019
    public AssertionEngineInitializerAttribute(Type type, string methodName)
#pragma warning restore CA1019
    {
        this.type = type;
        this.methodName = methodName;
    }

    internal void Initialize()
    {
        type?.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static)?.Invoke(obj: null, parameters: null);
    }
}
