using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions.Types;

namespace FluentAssertions
{
    /// <summary>
    /// Extension methods for getting method and property selectors for a type.
    /// </summary>
    [DebuggerNonUserCode]
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns the types that are visible outside the specified <see cref="Assembly"/>.
        /// </summary>
        public static TypeSelector Types(this Assembly assembly)
        {
            return new TypeSelector(assembly.GetTypes());
        }

        /// <summary>
        /// Returns a type selector for the current <see cref="System.Type"/>.
        /// </summary>
        public static TypeSelector Types(this Type type)
        {
            return new TypeSelector(type);
        }

        /// <summary>
        /// Returns a type selector for the current <see cref="System.Type"/>.
        /// </summary>
        public static TypeSelector Types(this IEnumerable<Type> types)
        {
            return new TypeSelector(types);
        }

        /// <summary>
        /// Returns a method selector for the current <see cref="System.Type"/>.
        /// </summary>
        public static MethodInfoSelector Methods(this Type type)
        {
            return new MethodInfoSelector(type);
        }

        /// <summary>
        /// Returns a method selector for the current <see cref="System.Type"/>.
        /// </summary>
        public static MethodInfoSelector Methods(this TypeSelector typeSelector)
        {
            return new MethodInfoSelector(typeSelector.ToList());
        }

        /// <summary>
        /// Returns a property selector for the current <see cref="System.Type"/>.
        /// </summary>
        public static PropertyInfoSelector Properties(this Type type)
        {
            return new PropertyInfoSelector(type);
        }

        /// <summary>
        /// Returns a property selector for the current <see cref="System.Type"/>.
        /// </summary>
        public static PropertyInfoSelector Properties(this TypeSelector typeSelector)
        {
            return new PropertyInfoSelector(typeSelector.ToList());
        }
    }
}
