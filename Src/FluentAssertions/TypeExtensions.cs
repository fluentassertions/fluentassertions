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

        /// <summary>
        /// Returns a read or read/write property for the current <see cref="System.Type"/> given a name.
        /// </summary>
        /// <remarks>
        /// Looks for a public implementation in current <see cref="System.Type"/>, base <see cref="System.Type"/>s and explicit interface implementations.
        /// </remarks>
        public static PropertyInfo GetPublicExplicitProperty(this Type type, string name)
        {
            var method = type.GetPublicProperty(name);
            if (method is object)
                return method;

            foreach (var @interface in type.GetInterfaces())
            {
                method = @interface.GetPublicProperty(name);
                if (method is object)
                    return method;
            }

            return null;
        }

        /// <summary>
        /// Returns a parameterless method for the current <see cref="System.Type"/> given a name.
        /// </summary>
        /// <remarks>
        /// Looks for a public implementation in current <see cref="System.Type"/>, base <see cref="System.Type"/>s and explicit interface implementations.
        /// </remarks>
        public static MethodInfo GetPublicExplicitParameterlessMethod(this Type type, string name)
        {
            var method = type.GetPublicParameterlessMethod(name);
            if (method is object)
                return method;

            foreach (var @interface in type.GetInterfaces())
            {
                method = @interface.GetPublicParameterlessMethod(name);
                if (method is object)
                    return method;
            }

            return null;
        }

        private static PropertyInfo GetPublicProperty(this Type type, string name)
            => type.GetProperty(
                name,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.FlattenHierarchy);

        private static MethodInfo GetPublicParameterlessMethod(this Type type, string name)
            => type.GetMethod(
                name,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy,
                null, new Type[0], new ParameterModifier[0]);
    }
}
