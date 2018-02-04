using System;
using System.Collections.Generic;

using FluentAssertions.Types;

namespace FluentAssertions
{
    /// <summary>
    /// Extension methods for filtering a collection of types.
    /// </summary>
    public static class TypeEnumerableExtensions
    {
        /// <summary>
        /// Filters to only include types decorated with a particular attribute.
        /// </summary>
        public static IEnumerable<Type> ThatAreDecoratedWith<TAttribute>(this IEnumerable<Type> types)
            where TAttribute : Attribute
        {
            return new TypeSelector(types).ThatAreDecoratedWith<TAttribute>();
        }

        /// <summary>
        /// Filters to only include types decorated with, or inherits from a parent class, a particular attribute.
        /// </summary>
        public static IEnumerable<Type> ThatAreDecoratedWithOrInherit<TAttribute>(this IEnumerable<Type> types)
            where TAttribute : Attribute
        {
            return new TypeSelector(types).ThatAreDecoratedWithOrInherit<TAttribute>();
        }

        /// <summary>
        /// Filters to only include types not decorated with a particular attribute.
        /// </summary>
        public static IEnumerable<Type> ThatAreNotDecoratedWith<TAttribute>(this IEnumerable<Type> types)
            where TAttribute : Attribute
        {
            return new TypeSelector(types).ThatAreNotDecoratedWith<TAttribute>();
        }

        /// <summary>
        /// Filters to only include types not decorated with and does not inherit from a parent class, a particular attribute.
        /// </summary>
        public static IEnumerable<Type> ThatAreNotDecoratedWithOrInherit<TAttribute>(this IEnumerable<Type> types)
            where TAttribute : Attribute
        {
            return new TypeSelector(types).ThatAreNotDecoratedWithOrInherit<TAttribute>();
        }

        /// <summary>
        /// Filters to only include types where the namespace of type is exactly <paramref name="namespace"/>.
        /// </summary>
        public static IEnumerable<Type> ThatAreInNamespace(this IEnumerable<Type> types, string @namespace)
        {
            return new TypeSelector(types).ThatAreInNamespace(@namespace);
        }

        /// <summary>
        /// Filters to only include types where the namespace of type is starts with <paramref name="namespace"/>.
        /// </summary>
        public static IEnumerable<Type> ThatAreUnderNamespace(this IEnumerable<Type> types, string @namespace)
        {
            return new TypeSelector(types).ThatAreUnderNamespace(@namespace);
        }

        /// <summary>
        /// Filters to only include types that subclass the specified type, but NOT the same type.
        /// </summary>
        public static IEnumerable<Type> ThatDeriveFrom<T>(this IEnumerable<Type> types)
        {
            return new TypeSelector(types).ThatDeriveFrom<T>();
        }

        /// <summary>
        /// Determines whether a type implements an interface (but is not the interface itself).
        /// </summary>
        public static IEnumerable<Type> ThatImplement<T>(this IEnumerable<Type> types)
        {
            return new TypeSelector(types).ThatImplement<T>();
        }
    }
}
