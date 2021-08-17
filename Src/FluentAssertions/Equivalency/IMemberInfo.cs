using System;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents a field or property in an object graph.
    /// </summary>
    public interface IMemberInfo
    {
        /// <summary>
        /// Gets the name of the current member.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the type of this member.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the type that declares the current member.
        /// </summary>
        Type DeclaringType { get; }

        /// <summary>
        /// Gets the full path from the root object until and including the current node separated by dots.
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Gets the access modifier for the getter of this member.
        /// </summary>
        CSharpAccessModifier GetterAccessibility { get; }

        /// <summary>
        /// Gets the access modifier for the setter of this member.
        /// </summary>
        CSharpAccessModifier SetterAccessibility { get; }
    }
}
