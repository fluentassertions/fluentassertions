using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentAssertions.Common
{
    public static class TypeExtensions
    {
        private const BindingFlags PublicPropertiesFlag =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// Determines whether the specified method has been annotated with a specific attribute.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the specified method has attribute; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttribute<TAttribute>(this MemberInfo method) where TAttribute : Attribute
        {
            return (method.GetCustomAttributes(typeof(TAttribute), true).Any());
        }

        public static bool HasMatchingAttribute<TAttribute>(this Type type, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            Func<TAttribute, bool> isMatchingAttribute = isMatchingAttributePredicate.Compile();

            return GetCustomAttributes<TAttribute>(type).Any(isMatchingAttribute);
        }

        public static bool IsDecoratedWith<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return GetCustomAttributes<TAttribute>(type).Any();
        }

        private static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(Type type)
        {
            return type.GetCustomAttributes(false).OfType<TAttribute>();
        }

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
                   (actualType.IsAssignableFrom(expectedType))
                ;
        }

        public static bool Implements<TInterface>(this Type type)
        {
            return Implements(type, typeof (TInterface));
        }

        public static bool Implements(this Type type, Type expectedBaseType)
        {
            return
                expectedBaseType.IsAssignableFrom(type)
                && (type != expectedBaseType);
        }

        public static bool IsComplexType(this Type type)
        {
            return HasProperties(type) && (type.Namespace != typeof (int).Namespace);
        }

        private static bool HasProperties(Type type)
        {
            return type
                .GetProperties(PublicPropertiesFlag)
                .Any();
        }

        /// <summary>
        /// Finds the property by a case-sensitive name.
        /// </summary>
        /// <returns>
        /// Returns <c>null</c> if no such property exists.
        /// </returns>
        public static PropertyInfo FindProperty(this Type type, string propertyName, Type preferredType)
        {
            IEnumerable<PropertyInfo> properties =
                type.GetProperties(PublicPropertiesFlag)
                    .Where(pi => pi.Name == propertyName)
                    .ToList();
            
            return (properties.Count() > 1)
                ? properties.SingleOrDefault(p => p.PropertyType == preferredType)
                : properties.SingleOrDefault();
        }

        public static IEnumerable<PropertyInfo> GetNonPrivateProperties(this Type typeToReflect, IEnumerable<string> filter = null)
        {
            var query =
                from propertyInfo in GetPropertiesFromHierarchy(typeToReflect)
                where HasNonPrivateGetter(propertyInfo)
                where (filter == null) || filter.Contains(propertyInfo.Name)
                select propertyInfo;

            return query.ToArray();
        }

        private static IEnumerable<PropertyInfo> GetPropertiesFromHierarchy(Type typeToReflect)
        {
            if (IsInterface(typeToReflect))
            {
                var propertyInfos = new List<PropertyInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(typeToReflect);
                queue.Enqueue(typeToReflect);

                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in GetInterfaces(subType))
                    {
                        if (considered.Contains(subInterface)) continue;

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    IEnumerable<PropertyInfo> typeProperties = GetPublicProperties(subType);

                    IEnumerable<PropertyInfo> newPropertyInfos = typeProperties.Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }
            else
            {
                return GetPublicProperties(typeToReflect);
            }
        }

        private static bool IsInterface(Type typeToReflect)
        {
            return typeToReflect.IsInterface;
        }

        private static IEnumerable<Type> GetInterfaces(Type type)
        {
            return type.GetInterfaces();
        }

        private static IEnumerable<PropertyInfo> GetPublicProperties(Type type)
        {
            return type.GetProperties(PublicPropertiesFlag);
        }

        private static bool HasNonPrivateGetter(PropertyInfo propertyInfo)
        {
            MethodInfo getMethod = propertyInfo.GetGetMethod(true);
            return (getMethod != null) && !getMethod.IsPrivate && !getMethod.IsFamily;
        }

        public static MethodInfo GetMethodNamed(this Type type, string methodName)
        {
            return type.GetMethod(methodName);
        }
    }
}