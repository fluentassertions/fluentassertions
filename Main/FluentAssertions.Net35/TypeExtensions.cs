using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using FluentAssertions.Types;

namespace FluentAssertions
{
    /// <summary>
    /// Extension methods for getting method and property selectors for a type.
    /// </summary>
    [DebuggerNonUserCode]
    public static class TypeExtensions
    {
#if !WINRT
        private const BindingFlags PublicPropertiesFlag =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
#endif

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

        public static bool IsSameOrInherits(this Type actualType, Type expectedType)
        {
            return (actualType == expectedType) ||
#if !WINRT
                (expectedType.IsSubclassOf(actualType))
#else
                   (actualType.GetTypeInfo().IsAssignableFrom(expectedType.GetTypeInfo()))
#endif
                ;
        }

        public static bool Implements<TInterface>(this Type type)
        {
            return Implements(type, typeof (TInterface));
        }

        public static bool Implements(this Type type, Type expectedBaseType)
        {
            return
#if !WINRT
                expectedBaseType.IsAssignableFrom(type)
#else
                expectedBaseType.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo())
#endif
                && (type != expectedBaseType);
        }

        public static bool IsComplexType(this Type type)
        {
            return HasProperties(type) && (type.Namespace != typeof (int).Namespace);
        }

        private static bool HasProperties(Type type)
        {
            return type
#if !WINRT
                .GetProperties(PublicPropertiesFlag)
#else
                .GetRuntimeProperties().Where(p => (p.GetMethod != null) && !p.GetMethod.IsStatic)
#endif
                .Any();
        }
    }
}