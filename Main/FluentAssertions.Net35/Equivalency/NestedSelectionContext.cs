using System;
using System.Reflection;

using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents a selection context of a nested property
    /// </summary>
    internal class NestedSelectionContext : ISubjectInfo
    {
        public NestedSelectionContext(ISubjectInfo context, PropertyInfo propertyInfo)
        {
            PropertyPath = context.PropertyPath.Combine(propertyInfo.Name);
            PropertyDescription = context.PropertyDescription.Combine(propertyInfo.Name);
            CompileTimeType = propertyInfo.PropertyType;
            RuntimeType = propertyInfo.PropertyType;
            PropertyInfo = propertyInfo;
        }

        /// <summary>
        /// Gets the <see cref="ISubjectInfo.PropertyInfo"/> of the property that returned the current object, or 
        /// <c>null</c> if the current  object represents the root object.
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// Gets the full path from the root object until the current property, separated by dots.
        /// </summary>
        public string PropertyPath { get; private set; }

        /// <summary>
        /// Gets a textual description of the current property based on the <see cref="ISubjectInfo.PropertyPath"/>.
        /// </summary>
        public string PropertyDescription { get; private set; }

        /// <summary>
        /// Gets the compile-time type of the current object. If the current object is not the root object, then it returns the 
        /// same <see cref="Type"/> as the <see cref="ISubjectInfo.RuntimeType"/> property does.
        /// </summary>
        public Type CompileTimeType { get; private set; }

        /// <summary>
        /// Gets the run-time type of the current object.
        /// </summary>
        public Type RuntimeType { get; private set; }
    }
}