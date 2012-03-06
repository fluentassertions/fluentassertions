using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace FluentAssertions.Assertions
{
#if WINRT
    internal static class ReflectionTypeExtensions
    {
        /// <summary>
        /// Returns properties from a given type and all of its base types
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>All properties from type</returns>
        public static IEnumerable<PropertyInfo> AllProperties(this Type type)
        {
            while (type != typeof(object))
            {
                var ti = type.GetTypeInfo();

                foreach (var pi in ti.DeclaredProperties)
                    yield return pi;

                type = ti.BaseType;
            }
        }

        /// <summary>
        /// Returns Events from a given type and all of its base types
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>All Events from type</returns>
        public static IEnumerable<EventInfo> AllEvents(this Type type)
        {
            while (type != typeof(object))
            {
                var ti = type.GetTypeInfo();

                foreach (var pi in ti.DeclaredEvents)
                    yield return pi;

                type = ti.BaseType;
            }
        }
    }
#endif
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
            return new TypeSelector(
#if !WINRT
                assembly.GetTypes()
#else
                assembly.DefinedTypes
#endif
                );
        }

        /// <summary>
        /// Returns a method selector for the current <see cref="Type"/>.
        /// </summary>
        public static MethodInfoSelector Methods(this Type type)
        {
            return new MethodInfoSelector(type);
        }

        /// <summary>
        /// Returns a method selector for the current <see cref="Type"/>.
        /// </summary>
        public static MethodInfoSelector Methods(this TypeSelector typeSelector)
        {
            return new MethodInfoSelector(typeSelector.ToList());
        }

        /// <summary>
        /// Returns a property selector for the current <see cref="Type"/>.
        /// </summary>
        public static PropertyInfoSelector Properties(this Type type)
        {
            return new PropertyInfoSelector(type);
        }

        /// <summary>
        /// Returns a property selector for the current <see cref="Type"/>.
        /// </summary>
        public static PropertyInfoSelector Properties(this TypeSelector typeSelector)
        {
            return new PropertyInfoSelector(typeSelector.ToList());
        }
    }
}