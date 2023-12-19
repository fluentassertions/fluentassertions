using System;
using JetBrains.Annotations;

namespace FluentAssertionsAsync.Equivalency;

/// <summary>
/// Represents an object, dictionary key pair, collection item or member in an object graph.
/// </summary>
public interface IObjectInfo
{
    /// <summary>
    /// Gets the type of the object
    /// </summary>
    [Obsolete("Use CompileTimeType or RuntimeType instead")]
    Type Type { get; }

    /// <summary>
    /// Gets the type of the parent, e.g. the type that declares a property or field.
    /// </summary>
    /// <value>
    /// Is <see langword="null"/> for the root object.
    /// </value>
    [CanBeNull]
    Type ParentType { get; }

    /// <summary>
    /// Gets the full path from the root object until the current node separated by dots.
    /// </summary>
    string Path { get; set; }

    /// <summary>
    /// Gets the compile-time type of the current object. If the current object is not the root object and the type is not <see cref="object"/>,
    /// then it returns the same <see cref="System.Type"/> as the <see cref="IObjectInfo.RuntimeType"/> property does.
    /// </summary>
    Type CompileTimeType { get; }

    /// <summary>
    /// Gets the run-time type of the current object.
    /// </summary>
    Type RuntimeType { get; }
}
