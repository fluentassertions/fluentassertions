using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions
{
    internal static class ReflectionExtensions
    {
        public static bool IsSubclassOf(this Type type, Type parentType)
        {
            return type.GetTypeInfo().IsSubclassOf(parentType);
        }

        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static bool IsPrimitive(this Type type)
        {
            return type.GetTypeInfo().IsPrimitive;
        }

        public static Type GetBaseType(this Type type)
        {
            return type.GetTypeInfo().BaseType;
        }

        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public static bool IsInterface(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }
        
        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GetTypeInfo().GenericTypeArguments;
        }  
  
        public static bool IsGenericTypeDefinition(this Type type)
        {
            return type.GetTypeInfo().IsGenericTypeDefinition;
        }

        public static bool IsAssignableFrom(this Type type, Type fromType)
        {
            return type.GetTypeInfo().IsAssignableFrom(fromType.GetTypeInfo());
        }
    }
}
