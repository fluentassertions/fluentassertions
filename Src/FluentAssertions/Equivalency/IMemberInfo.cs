using System;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Provides details about the expectation's root or nested member.
    /// </summary>
    public interface IMemberInfo
    {
        /// <summary>
        /// Gets the <see cref="SelectedMemberInfo"/> of the member that returned the current object, or <c>null</c> if the current
        /// object represents the root object.
        /// </summary>
        SelectedMemberInfo SelectedMemberInfo { get; }

        /// <summary>
        /// Gets the full path from the root object until the current object separated by dots.
        /// </summary>
        string SelectedMemberPath { get; }

        /// <summary>
        /// Gets a display-friendly representation of the <see cref="SelectedMemberPath"/>.
        /// </summary>
        string SelectedMemberDescription { get; }

        /// <summary>
        /// Gets the compile-time type of the current object. If the current object is not the root object and the type is not <see cref="object"/>,
        /// then it returns the same <see cref="System.Type"/> as the <see cref="IMemberInfo.RuntimeType"/> property does.
        /// </summary>
        Type CompileTimeType { get; }

        /// <summary>
        /// Gets the run-time type of the current object.
        /// </summary>
        Type RuntimeType { get; }
    }
}
