using System;
using System.Linq;

namespace FluentAssertions.Equivalency
{
    internal static class EquivalencyAssertionOptionsExtensions
    {
        /// <summary>
        /// Returns either the run-time or compile-time type of the expectation based on the options provided by the caller.
        /// </summary>
        /// <remarks>
        /// If the expectation is a nullable type, it should return the type of the wrapped object.
        /// </remarks>
        public static Type GetExpectationType(this IEquivalencyAssertionOptions config, Type runTimeType, Type compileTimeType)
        {
            Type type = config.UseRuntimeTyping ? runTimeType : compileTimeType;

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
