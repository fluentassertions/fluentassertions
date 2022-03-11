using System;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Exposes information about an object's member
    /// </summary>
    public interface IMember : INode
    {
        /// <summary>
        /// Gets the type that declares the current member.
        /// </summary>
        Type DeclaringType { get; }

        /// <summary>
        /// Gets the type that was used to determine this member.
        /// </summary>
        Type ReflectedType { get; }

        /// <summary>
        /// Gets the value of the member from the provided <paramref name="obj"/>
        /// </summary>
        object GetValue(object obj);

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
