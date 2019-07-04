using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Equivalency
{
    [Obsolete("This class is deprecated and will be removed in version 6.X.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class EquivalencyAssertionOptionsExtentions // TODO: Rename class into EquivalencyAssertionOptionsExtensions and make internal
    {
        /// <summary>
        /// Returns either the run-time or compile-time type of the subject based on the options provided by the caller.
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
