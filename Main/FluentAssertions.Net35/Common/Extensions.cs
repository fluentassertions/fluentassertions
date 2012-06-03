using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Formatting;

namespace FluentAssertions.Common
{
    internal static class ExpressionExtensions
    {
        public static PropertyInfo GetPropertyInfo<T>(this Expression<Func<T, object>> expression)
        {
            if (ReferenceEquals(expression, null))
            {
                throw new NullReferenceException("Expected a property expression, but found <null>.");
            }

            PropertyInfo propertyInfo = AttemptToGetPropertyInfoFromCastExpression(expression);
            if (propertyInfo == null)
            {
                propertyInfo = AttemptToGetPropertyInfoFromPropertyExpression(expression);
            }

            if (propertyInfo == null)
            {
                throw new ArgumentException("Cannot use <" + expression.Body + "> when a property expression is expected.");
            }

            return propertyInfo;
        }

        private static PropertyInfo AttemptToGetPropertyInfoFromPropertyExpression<T>(Expression<Func<T, object>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                return (PropertyInfo) memberExpression.Member;
            }

            return null;
        }

        private static PropertyInfo AttemptToGetPropertyInfoFromCastExpression<T>(Expression<Func<T, object>> expression)
        {
            var castExpression = expression.Body as UnaryExpression;
            if (castExpression != null)
            {
                return (PropertyInfo) ((MemberExpression) castExpression.Operand).Member;
            }

            return null;
        }
    }

    internal static class StringExtensions
    {
        /// <summary>
        /// Finds the first index at which the <paramref name="value"/> does not match the <paramref name="expected"/>
        /// string anymore, including the exact casing.
        /// </summary>
        public static int IndexOfFirstMismatch(this string value, string expected)
        {
            return IndexOfFirstMismatch(value, expected, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Finds the first index at which the <paramref name="value"/> does not match the <paramref name="expected"/>
        /// string anymore, accounting for the specified <paramref name="stringComparison"/>.
        /// </summary>
        public static int IndexOfFirstMismatch(this string value, string expected, StringComparison stringComparison)
        {
            for (int index = 0; index < value.Length; index++)
            {
                if ((index >= expected.Length) || !value[index].ToString().Equals(expected[index].ToString(), stringComparison))
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the quoted three characters at the specified index of a string, including the index itself.
        /// </summary>
        public static string IndexedSegmentAt(this string value, int index)
        {
            int length = Math.Min(value.Length - index, 3);

            return String.Format("{0} (index {1})", Formatter.ToString(value.Substring(index, length)), index);
        }

        /// <summary>
        /// Replaces all characters that might conflict with formatting placeholders and newlines with their escaped counterparts.
        /// </summary>
        public static string Escape(this string value)
        {
            return value.Replace("\"", "\\\"").Replace("\n", @"\n").Replace("\r", @"\r").Replace("{", "{{").Replace(
                "}", "}}");
        }
    }

    internal static class ObjectExtensions
    {
#if !WINRT
        private const BindingFlags PublicPropertiesFlag =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
#endif

        public static bool IsSameOrEqualTo(this object actual, object expected)
        {
            if (ReferenceEquals(actual, null) && ReferenceEquals(expected, null))
            {
                return true;
            }

            if (ReferenceEquals(actual, null))
            {
                return false;
            }

            return actual.Equals(expected);
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
            return Implements(type, typeof (TInterface));
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
            return HasProperties(type) && (type.Namespace != typeof (int).Namespace);
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

        public static PropertyInfo FindProperty(this object obj, string propertyName)
        {
            PropertyInfo property =
#if !WINRT
                obj.GetType().GetProperties(PublicPropertiesFlag)
#else
                obj.GetType().GetRuntimeProperties().Where(p => !p.GetMethod.IsStatic)
#endif
                    .SingleOrDefault(pi => pi.Name == propertyName);

            return property;
        }
    }

    internal static class TypeExtensions
    {
        public static bool IsEquivalentTo(this PropertyInfo property, PropertyInfo otherProperty)
        {
            return (property.DeclaringType == otherProperty.DeclaringType) && (property.Name == otherProperty.Name);
        }
    }
}