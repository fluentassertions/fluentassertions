using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Specs.Common
{
    internal static class TypeExtensions
    {
        private const BindingFlags AllMembersFlag =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        public static MethodInfo GetParameterlessMethod(this Type type, string methodName)
        {
            return type.GetMethod(methodName, Enumerable.Empty<Type>());
        }

        private static MethodInfo GetMethod(this Type type, string methodName, IEnumerable<Type> parameterTypes)
        {
            return type.GetMethods(AllMembersFlag)
                .SingleOrDefault(m =>
                    m.Name == methodName && m.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes));
        }

        public static PropertyInfo GetPropertyByName(this Type type, string propertyName)
        {
            return type.GetProperty(propertyName, AllMembersFlag);
        }
    }
}
