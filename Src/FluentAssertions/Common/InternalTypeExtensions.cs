using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FluentAssertions.Common
{
    // NOTE: These extension methods are covered by tests separately and therefore need to be public.
    public static class InternalTypeExtensions
    {
        /// <summary>
        /// For internal use only.
        /// </summary>
        public static bool IsSameOrInherits(this Type actualType, Type expectedType)
        {
            return actualType == expectedType ||
                   expectedType.IsAssignableFrom(actualType);
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        public static MethodInfo GetExplicitConversionOperator(this Type type, Type sourceType, Type targetType)
        {
            return type
                .GetConversionOperators(sourceType, targetType, name => name == "op_Explicit")
                .SingleOrDefault();
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        public static MethodInfo GetImplicitConversionOperator(this Type type, Type sourceType, Type targetType)
        {
            return type
                .GetConversionOperators(sourceType, targetType, name => name == "op_Implicit")
                .SingleOrDefault();
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        public static bool HasValueSemantics(this Type type)
        {
            return type.IsSimpleType() &&
                   !type.IsAnonymousType() && !type.IsTuple() && !IsKeyValuePair(type);
        }

        private static bool IsTuple(this Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }

            Type openType = type.GetGenericTypeDefinition();
            return openType == typeof(ValueTuple<>)
                   || openType == typeof(ValueTuple<,>)
                   || openType == typeof(ValueTuple<,,>)
                   || openType == typeof(ValueTuple<,,,>)
                   || openType == typeof(ValueTuple<,,,,>)
                   || openType == typeof(ValueTuple<,,,,,>)
                   || openType == typeof(ValueTuple<,,,,,,>)
                   || (openType == typeof(ValueTuple<,,,,,,,>) && IsTuple(type.GetGenericArguments()[7]))
                   || openType == typeof(Tuple<>)
                   || openType == typeof(Tuple<,>)
                   || openType == typeof(Tuple<,,>)
                   || openType == typeof(Tuple<,,,>)
                   || openType == typeof(Tuple<,,,,>)
                   || openType == typeof(Tuple<,,,,,>)
                   || openType == typeof(Tuple<,,,,,,>)
                   || (openType == typeof(Tuple<,,,,,,,>) && IsTuple(type.GetGenericArguments()[7]));
        }

        private static bool IsAnonymousType(this Type type)
        {
            bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType", StringComparison.Ordinal);

            if (!nameContainsAnonymousType)
            {
                return false;
            }

            bool hasCompilerGeneratedAttribute =
                type.IsDecoratedWith<CompilerGeneratedAttribute>();

            return hasCompilerGeneratedAttribute;
        }

        private static bool IsKeyValuePair(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>);
        }
    }
}
