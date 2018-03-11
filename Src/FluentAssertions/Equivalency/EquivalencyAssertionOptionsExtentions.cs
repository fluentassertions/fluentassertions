using System;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Equivalency
{
    public static class EquivalencyAssertionOptionsExtentions
    {
        /// <summary>
        ///     Returns either the run-time or compile-time type of the subject based on the options provided by the caller.
        /// </summary>
        /// <remarks>
        /// If the expectation is a nullable type, it should return the type of the wrapped object.
        /// </remarks>
        public static Type GetExpectationType(this IEquivalencyAssertionOptions config, IMemberInfo context)
        {
            Type type = config.UseRuntimeTyping ? context.RuntimeType : context.CompileTimeType;

            return NullableOrActualType(type);
        }

        private static Type NullableOrActualType(Type type)
        {
            if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments().First();
            }

            return type;
        }
    }
}
