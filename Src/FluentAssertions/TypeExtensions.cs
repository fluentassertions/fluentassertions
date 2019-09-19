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
        /// Returns the read property Current for the current <see cref="System.Type"/>.
        /// </summary>
        /// <remarks>
        /// Looks for explicit interface implementations.
        /// </remarks>
        public static PropertyInfo GetPropertyCurrent(this Type type)
        {
            var method = type.GetPublicPropertyCurrent();
            if (method is object)
                return method;

            foreach (var @interface in type.GetInterfaces())
            {
                method = @interface.GetPublicPropertyCurrent();
                if (method is object)
                    return method;
            }

            return null;
        }

        /// <summary>
        /// Returns the parameterless method GetEnumerator for the current <see cref="System.Type"/>.
        /// </summary>
        /// <remarks>
        /// Looks for explicit interface implementations.
        /// </remarks>
        public static MethodInfo GetMethodGetEnumerator(this Type type)
        {
            var method = type.GetPublicMethodGetEnumerator();
            if (method is object)
                return method;

            foreach (var @interface in type.GetInterfaces())
            {
                method = @interface.GetPublicMethodGetEnumerator();
                if (method is object)
                    return method;
            }

            return null;
        }

        /// <summary>
        /// Returns the parameterless method MoveNext for the current <see cref="System.Type"/>.
        /// </summary>
        /// <remarks>
        /// Looks for explicit interface implementations.
        /// </remarks>
        public static MethodInfo GetMethodMoveNext(this Type type)
        {
            var method = type.GetPublicMethodMoveNext();
            if (method is object)
                return method;

            foreach (var @interface in type.GetInterfaces())
            {
                method = @interface.GetPublicMethodMoveNext();
                if (method is object)
                    return method;
            }

            return null;
        }

        /// <summary>
        /// Returns the parameterless method Reset for the current <see cref="System.Type"/>.
        /// </summary>
        /// <remarks>
        /// Looks for explicit interface implementations.
        /// </remarks>
        public static MethodInfo GetMethodReset(this Type type)
        {
            var method = type.GetPublicMethodReset();
            if (method is object)
                return method;

            foreach (var @interface in type.GetInterfaces())
            {
                method = @interface.GetPublicMethodReset();
                if (method is object)
                    return method;
            }

            return null;
        }

        /// <summary>
        /// Returns the parameterless method Dispose for the current <see cref="System.Type"/>.
        /// </summary>
        /// <remarks>
        /// Looks for explicit interface implementations.
        /// </remarks>
        public static MethodInfo GetMethodDispose(this Type type)
        {
            var method = type.GetPublicMethodDispose();
            if (method is object)
                return method;

            foreach (var @interface in type.GetInterfaces())
            {
                method = @interface.GetPublicMethodDispose();
                if (method is object)
                    return method;
            }

            return null;
        }

        static PropertyInfo GetPublicPropertyCurrent(this Type type)
            => type.GetProperty("Current", BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        static MethodInfo GetPublicMethodGetEnumerator(this Type type)
            => type.GetMethod("GetEnumerator", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        static MethodInfo GetPublicMethodMoveNext(this Type type)
            => type.GetMethod("MoveNext", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        static MethodInfo GetPublicMethodReset(this Type type)
            => type.GetMethod("Reset", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        static MethodInfo GetPublicMethodDispose(this Type type)
            => type.GetMethod("Dispose", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
    }
}
