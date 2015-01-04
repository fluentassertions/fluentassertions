using System;
using System.Reflection;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Provides details about the subject's root or nested member.
    /// </summary>
    public interface ISubjectInfo
    {
        /// <summary>
        /// Gets the <see cref="SelectedMemberInfo"/> of the member that returned the current object, or <c>null</c> if the current
        /// object represents the root object.
        /// </summary>
        ISelectedMemberInfo SelectedMemberInfo { get; }

        /// <summary>
        /// Gets the full path from the root object until the current object separated by dots.
        /// </summary>
        string SelectedMemberPath { get; }

        /// <summary>
        /// Gets a display-friendly representation of the <see cref="SelectedMemberPath"/>.
        /// </summary>
        string SelectedMemberDescription { get; }

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> of the property that returned the current object, or <c>null</c> if the current
        /// object represents the root object.
        /// </summary>
        [Obsolete("This property will be removed in a future version.  Use `SelectedMemberInfo` instead.")]
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets the full path from the root object until the current object separated by dots.
        /// </summary>
        [Obsolete("This property will be removed in a future version.  Use `SelectedMemberPath` instead.")]
        string PropertyPath { get; }

        /// <summary>
        /// Gets a display-friendly representation of the <see cref="PropertyPath"/>.
        /// </summary>
        [Obsolete("This property will be removed in a future version.  Use `SelectedMemberDescription` instead.")]
        string PropertyDescription { get; }

        /// <summary>
        /// Gets the compile-time type of the current object. If the current object is not the root object, then it returns the 
        /// same <see cref="Type"/> as the <see cref="RuntimeType"/> property does.
        /// </summary>
        Type CompileTimeType { get; }

        /// <summary>
        /// Gets the run-time type of the current object.
        /// </summary>
        Type RuntimeType { get; }
    }
}