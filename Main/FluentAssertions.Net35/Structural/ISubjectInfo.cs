using System;
using System.Reflection;

namespace FluentAssertions.Structural
{
    /// <summary>
    /// Provides details about the subject's root or nested property.
    /// </summary>
    public interface ISubjectInfo
    {
        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> of the property that returned the current object, or <c>null</c> if the current
        /// object represents the root object.
        /// </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets the full path from the root object until the current object separated by dots.
        /// </summary>
        string PropertyPath { get; }

        /// <summary>
        /// Gets a display-friendly representation of the <see cref="PropertyPath"/>.
        /// </summary>
        string PropertyDescription { get; }

        /// <summary>
        /// Gets the compile-time type of the current object. If the current object is not the root object, then it returns the 
        /// same <see cref="Type"/> as the <see cref="RuntimeType"/> property does.
        /// </summary>
        Type CompileTimeType { get;  }

        /// <summary>
        /// Gets the run-time type of the current object.
        /// </summary>
        Type RuntimeType { get; }
    }
}