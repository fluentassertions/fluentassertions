using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Common
{
    internal static class TypeExtensions
    {
#if !WINRT
        private const BindingFlags PublicPropertiesFlag =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
#endif

        /// <summary>
        /// Determines whether two <see cref="PropertyInfo"/> objects refer to the same property.
        /// </summary>
        public static bool IsEquivalentTo(this PropertyInfo property, PropertyInfo otherProperty)
        {
            return (property.DeclaringType.IsSameOrInherits(otherProperty.DeclaringType) ||
                    otherProperty.DeclaringType.IsSameOrInherits(property.DeclaringType)) && 
                   (property.Name == otherProperty.Name);
        }

        public static bool IsSameOrInherits(this Type actualType, Type expectedType)
        {
            return (actualType == expectedType) ||
#if !WINRT
                   (expectedType.IsSubclassOf(actualType))
#else
                   (actualType.GetTypeInfo().IsAssignableFrom(expectedType.GetTypeInfo()))
#endif
                ;
        }

        public static bool Implements<TInterface>(this Type type)
        {
            return Implements(type, typeof(TInterface));
        }

        public static bool Implements(this Type type, Type expectedBaseType)
        {
            return
#if !WINRT
                expectedBaseType.IsAssignableFrom(type)
#else
                expectedBaseType.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo())
#endif
                && (type != expectedBaseType);
        }

        public static bool IsComplexType(this Type type)
        {
            return HasProperties(type) && (type.Namespace != typeof(int).Namespace);
        }

        private static bool HasProperties(Type type)
        {
            return type
#if !WINRT
                .GetProperties(PublicPropertiesFlag)
#else
                .GetRuntimeProperties().Where(p => (p.GetMethod != null) && !p.GetMethod.IsStatic)
#endif
                .Any();
        }


#if !WINRT
        public static IEnumerable<PropertyInfo> GetNonPrivateProperties(this Type typeToReflect, IEnumerable<string> filter = null)
        {
            var query =
                from propertyInfo in GetProperties(typeToReflect, PublicPropertiesFlag)
                let getMethod = propertyInfo.GetGetMethod(true)
                where (getMethod != null) && !getMethod.IsPrivate
                where (filter == null) || filter.Contains(propertyInfo.Name)
                select propertyInfo;

            return query.ToArray();
        }

        private static IEnumerable<PropertyInfo> GetProperties(Type typeToReflect, BindingFlags bindingFlags)
        {
            if (typeToReflect.IsInterface)
            {
                var propertyInfos = new List<PropertyInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(typeToReflect);
                queue.Enqueue(typeToReflect);

                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (considered.Contains(subInterface)) continue;

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    PropertyInfo[] typeProperties = subType.GetProperties(bindingFlags);

                    IEnumerable<PropertyInfo> newPropertyInfos = typeProperties.Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }
            else
            {
                return typeToReflect.GetProperties(bindingFlags);
            }
        }

#else
        public static IEnumerable<PropertyInfo> GetNonPrivateProperties(this Type typeToReflect, IEnumerable<string> filter = null)
        {
            var query =
                from propertyInfo in GetProperties(typeToReflect)
                let getMethod = propertyInfo.GetMethod
                where (getMethod != null) && !getMethod.IsPrivate && !getMethod.IsStatic
                where (filter == null) || filter.Contains(propertyInfo.Name)
                select propertyInfo;

            return query.ToList();
        }

        private static IEnumerable<PropertyInfo> GetProperties(Type typeToReflect)
        {
            if (typeToReflect.GetTypeInfo().IsInterface)
            {
                var propertyInfos = new List<PropertyInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(typeToReflect);
                queue.Enqueue(typeToReflect);

                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetTypeInfo().ImplementedInterfaces)
                    {
                        if (considered.Contains(subInterface)) continue;

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    IEnumerable<PropertyInfo> newPropertyInfos = subType
                        .GetRuntimeProperties()
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }
            else
            {
                return typeToReflect.GetRuntimeProperties();
            }
        }
#endif


    }
}