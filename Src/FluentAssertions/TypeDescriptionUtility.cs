using System;

namespace FluentAssertions
{
    internal static class TypeDescriptionUtility
    {
        public static string GetDescriptionOfObjectType(object obj)
        {
            return (obj is null) ? "<null>" : GetTypeDescription(obj.GetType());
        }

        private static string GetTypeDescription(Type type)
        {
            return
                ((type.Namespace == "System.Linq") && type.IsGenericType)
                ? "an anonymous iterator from a LINQ expression with element type " + type.GetGenericArguments()[0].FullName
                : (type.IsValueType ? "a value of type " : "an instance of ") + type.FullName;
        }
    }
}
