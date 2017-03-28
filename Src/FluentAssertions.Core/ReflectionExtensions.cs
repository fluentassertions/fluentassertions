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

        public static bool IsEnum(this Type type)
        {
            return type.IsEnum;
        }

        public static bool IsPrimitive(this Type type)
        {
            return type.IsPrimitive;
        }

        public static Type GetBaseType(this Type type)
        {
            return type.BaseType;
        }

        public static bool IsGenericType(this Type type)
        {
            return type.IsGenericType;
        }

        public static bool IsInterface(this Type type)
        {
            return type.IsInterface;
        }

        public static bool IsGenericTypeDefinition(this Type type)
        {
            return type.IsGenericTypeDefinition;
        }

        public static Type GetTypeInfo(this Type type)
        {
            return type;
        }
      
    }

}
