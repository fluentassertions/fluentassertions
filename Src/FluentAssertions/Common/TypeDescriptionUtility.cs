using System;

namespace FluentAssertions.Common
{
    internal static class TypeDescriptionUtility
    {
        public static string GetDescriptionOfObjectType(object obj)
        {
            return (obj is null) ? "<null>" : GetTypeDescription(obj.GetType(), describeValue: true);
        }

        public static string GetTypeDescription(Type type)
            => GetTypeDescription(type, describeValue: false);

        private static string GetTypeDescription(Type type, bool describeValue)
        {
            if ((type.Namespace == "System.Linq") && type.IsGenericType)
            {
                return "an anonymous iterator from a LINQ expression with element type " + type.GetGenericArguments()[0].FullName;
            }
            else
            {
                return
                    describeValue
                    ? (type.IsValueType ? "a value of type " : "an instance of ") + type.FullName
                    : type.FullName;
            }
        }
    }
}
