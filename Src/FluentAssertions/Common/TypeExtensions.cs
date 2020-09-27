using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Equivalency;

namespace FluentAssertions.Common
{
    internal static class TypeExtensions
    {
        private const BindingFlags PublicMembersFlag =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private const BindingFlags AllMembersFlag =
            PublicMembersFlag | BindingFlags.NonPublic | BindingFlags.Static;

        public static bool IsDecoratedWith<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return type.IsDefined(typeof(TAttribute), inherit: false);
        }

        public static bool IsDecoratedWith<TAttribute>(this MemberInfo type)
            where TAttribute : Attribute
        {
            // Do not use MemberInfo.IsDefined
            // There is an issue with PropertyInfo and EventInfo preventing the inherit option to work.
            // https://github.com/dotnet/runtime/issues/30219
            return Attribute.IsDefined(type, typeof(TAttribute), inherit: false);
        }

        public static bool IsDecoratedWithOrInherit<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return type.IsDefined(typeof(TAttribute), inherit: true);
        }

        public static bool IsDecoratedWithOrInherit<TAttribute>(this MemberInfo type)
            where TAttribute : Attribute
        {
            // Do not use MemberInfo.IsDefined
            // There is an issue with PropertyInfo and EventInfo preventing the inherit option to work.
            // https://github.com/dotnet/runtime/issues/30219
            return Attribute.IsDefined(type, typeof(TAttribute), inherit: true);
        }

        public static bool IsDecoratedWith<TAttribute>(this Type type, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            return GetCustomAttributes(type, isMatchingAttributePredicate).Any();
        }

        public static bool IsDecoratedWith<TAttribute>(this MemberInfo type, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            return GetCustomAttributes(type, isMatchingAttributePredicate).Any();
        }

        public static bool IsDecoratedWithOrInherit<TAttribute>(this Type type, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            return GetCustomAttributes(type, isMatchingAttributePredicate, inherit: true).Any();
        }

        public static IEnumerable<TAttribute> GetMatchingAttributes<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return GetCustomAttributes<TAttribute>(type);
        }

        public static IEnumerable<TAttribute> GetMatchingAttributes<TAttribute>(this Type type, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            return GetCustomAttributes(type, isMatchingAttributePredicate);
        }

        public static IEnumerable<TAttribute> GetMatchingOrInheritedAttributes<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return GetCustomAttributes<TAttribute>(type, inherit: true);
        }

        public static IEnumerable<TAttribute> GetMatchingOrInheritedAttributes<TAttribute>(this Type type, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            return GetCustomAttributes(type, isMatchingAttributePredicate, inherit: true);
        }

        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this MemberInfo type, bool inherit = false)
            where TAttribute : Attribute
        {
            // Do not use MemberInfo.GetCustomAttributes.
            // There is an issue with PropertyInfo and EventInfo preventing the inherit option to work.
            // https://github.com/dotnet/runtime/issues/30219
            return CustomAttributeExtensions.GetCustomAttributes<TAttribute>(type, inherit);
        }

        private static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(MemberInfo type,
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, bool inherit = false)
            where TAttribute : Attribute
        {
            Func<TAttribute, bool> isMatchingAttribute = isMatchingAttributePredicate.Compile();
            return GetCustomAttributes<TAttribute>(type, inherit).Where(isMatchingAttribute);
        }

        private static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this Type type, bool inherit = false)
            where TAttribute : Attribute
        {
            return (IEnumerable<TAttribute>)type.GetCustomAttributes(typeof(TAttribute), inherit);
        }

        private static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(Type type,
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, bool inherit = false)
            where TAttribute : Attribute
        {
            Func<TAttribute, bool> isMatchingAttribute = isMatchingAttributePredicate.Compile();
            return GetCustomAttributes<TAttribute>(type, inherit).Where(isMatchingAttribute);
        }

        /// <summary>
        /// Determines whether two <see cref="FluentAssertions.Equivalency.SelectedMemberInfo" /> objects refer to the same
        /// member.
        /// </summary>
        public static bool IsEquivalentTo(this SelectedMemberInfo property, SelectedMemberInfo otherProperty)
        {
            return (property.DeclaringType.IsSameOrInherits(otherProperty.DeclaringType) ||
                    otherProperty.DeclaringType.IsSameOrInherits(property.DeclaringType)) &&
                   property.Name == otherProperty.Name;
        }

        public static Type[] GetClosedGenericInterfaces(Type type, Type openGenericType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == openGenericType)
            {
                return new[] { type };
            }

            Type[] interfaces = type.GetInterfaces();
            return
                interfaces
                    .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == openGenericType)
                    .ToArray();
        }

        public static bool OverridesEquals(this Type type)
        {
            MethodInfo method = type
                .GetMethod("Equals", new[] { typeof(object) });

            return method != null
                && method.GetBaseDefinition().DeclaringType != method.DeclaringType;
        }

        /// <summary>
        /// Finds a member by its case-sensitive name.
        /// </summary>
        /// <returns>
        /// Returns <c>null</c> if no such member exists.
        /// </returns>
        public static SelectedMemberInfo FindMember(this Type type, string memberName, Type preferredType)
        {
            return SelectedMemberInfo.Create(FindProperty(type, memberName, preferredType)) ??
                   SelectedMemberInfo.Create(FindField(type, memberName, preferredType));
        }

        /// <summary>
        /// Finds the property by a case-sensitive name.
        /// </summary>
        /// <returns>
        /// Returns <c>null</c> if no such property exists.
        /// </returns>
        public static PropertyInfo FindProperty(this Type type, string propertyName, Type preferredType)
        {
            List<PropertyInfo> properties =
                type.GetProperties(PublicMembersFlag)
                    .Where(pi => pi.Name == propertyName)
                    .ToList();

            return properties.Count > 1
                ? properties.SingleOrDefault(p => p.PropertyType == preferredType)
                : properties.SingleOrDefault();
        }

        /// <summary>
        /// Finds the field by a case-sensitive name.
        /// </summary>
        /// <returns>
        /// Returns <c>null</c> if no such property exists.
        /// </returns>
        public static FieldInfo FindField(this Type type, string fieldName, Type preferredType)
        {
            List<FieldInfo> properties =
                type.GetFields(PublicMembersFlag)
                    .Where(pi => pi.Name == fieldName)
                    .ToList();

            return properties.Count > 1
                ? properties.SingleOrDefault(p => p.FieldType == preferredType)
                : properties.SingleOrDefault();
        }

        public static IEnumerable<SelectedMemberInfo> GetNonPrivateMembers(this Type typeToReflect)
        {
            return
                GetNonPrivateProperties(typeToReflect)
                    .Select(SelectedMemberInfo.Create)
                    .Concat(GetNonPrivateFields(typeToReflect).Select(SelectedMemberInfo.Create))
                    .ToArray();
        }

        public static IEnumerable<PropertyInfo> GetNonPrivateProperties(this Type typeToReflect,
            IEnumerable<string> filter = null)
        {
            IEnumerable<PropertyInfo> query =
                from propertyInfo in GetPropertiesFromHierarchy(typeToReflect)
                where HasNonPrivateGetter(propertyInfo)
                where !propertyInfo.IsIndexer()
                where filter is null || filter.Contains(propertyInfo.Name)
                select propertyInfo;

            return query.ToArray();
        }

        public static IEnumerable<FieldInfo> GetNonPrivateFields(this Type typeToReflect)
        {
            IEnumerable<FieldInfo> query =
                from fieldInfo in GetFieldsFromHierarchy(typeToReflect)
                where !fieldInfo.IsPrivate
                where !fieldInfo.IsFamily
                select fieldInfo;

            return query.ToArray();
        }

        private static IEnumerable<FieldInfo> GetFieldsFromHierarchy(Type typeToReflect)
        {
            return GetMembersFromHierarchy(typeToReflect, GetPublicFields);
        }

        private static IEnumerable<PropertyInfo> GetPropertiesFromHierarchy(Type typeToReflect)
        {
            return GetMembersFromHierarchy(typeToReflect, GetPublicProperties);
        }

        private static IEnumerable<TMemberInfo> GetMembersFromHierarchy<TMemberInfo>(
            Type typeToReflect,
            Func<Type, IEnumerable<TMemberInfo>> getMembers)
            where TMemberInfo : MemberInfo
        {
            if (typeToReflect.IsInterface)
            {
                var propertyInfos = new List<TMemberInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(typeToReflect);
                queue.Enqueue(typeToReflect);

                while (queue.Count > 0)
                {
                    Type subType = queue.Dequeue();
                    foreach (Type subInterface in GetInterfaces(subType))
                    {
                        if (considered.Contains(subInterface))
                        {
                            continue;
                        }

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    IEnumerable<TMemberInfo> typeProperties = getMembers(subType);

                    IEnumerable<TMemberInfo> newPropertyInfos = typeProperties.Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            return getMembers(typeToReflect);
        }

        private static Type[] GetInterfaces(Type type)
        {
            return type.GetInterfaces();
        }

        private static PropertyInfo[] GetPublicProperties(Type type)
        {
            return type.GetProperties(PublicMembersFlag);
        }

        private static FieldInfo[] GetPublicFields(Type type)
        {
            return type.GetFields(PublicMembersFlag);
        }

        private static bool HasNonPrivateGetter(PropertyInfo propertyInfo)
        {
            MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: true);
            return getMethod != null && !getMethod.IsPrivate && !getMethod.IsFamily;
        }

        /// <summary>
        /// Check if the type is declared as abstract.
        /// </summary>
        /// <param name="type">Type to be checked</param>
        public static bool IsCSharpAbstract(this Type type)
        {
            return type.IsAbstract && !type.IsSealed;
        }

        /// <summary>
        /// Check if the type is declared as sealed.
        /// </summary>
        /// <param name="type">Type to be checked</param>
        public static bool IsCSharpSealed(this Type type)
        {
            return type.IsSealed && !type.IsAbstract;
        }

        /// <summary>
        /// Check if the type is declared as static.
        /// </summary>
        /// <param name="type">Type to be checked</param>
        public static bool IsCSharpStatic(this Type type)
        {
            return type.IsSealed && type.IsAbstract;
        }

        public static MethodInfo GetMethod(this Type type, string methodName, IEnumerable<Type> parameterTypes)
        {
            return type.GetMethods(AllMembersFlag)
                .SingleOrDefault(m =>
                    m.Name == methodName && m.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes));
        }

        public static bool HasMethod(this Type type, string methodName, IEnumerable<Type> parameterTypes)
        {
            return type.GetMethod(methodName, parameterTypes) != null;
        }

        public static MethodInfo GetParameterlessMethod(this Type type, string methodName)
        {
            return type.GetMethod(methodName, Enumerable.Empty<Type>());
        }

        public static bool HasParameterlessMethod(this Type type, string methodName)
        {
            return type.GetParameterlessMethod(methodName) != null;
        }

        public static PropertyInfo GetPropertyByName(this Type type, string propertyName)
        {
            return type.GetProperty(propertyName, AllMembersFlag);
        }

        public static bool HasExplicitlyImplementedProperty(this Type type, Type interfaceType, string propertyName)
        {
            bool hasGetter = type.HasParameterlessMethod(string.Format("{0}.get_{1}", interfaceType.FullName, propertyName));
            bool hasSetter = type.GetMethods(AllMembersFlag)
                                 .SingleOrDefault(m =>
                                     m.Name == string.Format("{0}.set_{1}", interfaceType.FullName, propertyName) &&
                                     m.GetParameters().Length == 1) != null;

            return hasGetter || hasSetter;
        }

        public static PropertyInfo GetIndexerByParameterTypes(this Type type, IEnumerable<Type> parameterTypes)
        {
            return type.GetProperties(AllMembersFlag)
                .SingleOrDefault(p =>
                    p.IsIndexer() && p.GetIndexParameters().Select(i => i.ParameterType).SequenceEqual(parameterTypes));
        }

        public static bool IsIndexer(this PropertyInfo member)
        {
            return member.GetIndexParameters().Length != 0;
        }

        public static ConstructorInfo GetConstructor(this Type type, IEnumerable<Type> parameterTypes)
        {
            return type
                .GetConstructors(PublicMembersFlag)
                .SingleOrDefault(m => m.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes));
        }

        public static IEnumerable<MethodInfo> GetConversionOperators(this Type type, Type sourceType, Type targetType,
            Func<string, bool> predicate)
        {
            return type
                .GetMethods()
                .Where(m =>
                    m.IsPublic
                    && m.IsStatic
                    && m.IsSpecialName
                    && m.ReturnType == targetType
                    && predicate(m.Name)
                    && m.GetParameters().Length == 1
                    && m.GetParameters()[0].ParameterType == sourceType);
        }

        public static bool IsAssignableToOpenGeneric(this Type type, Type definition)
        {
            // The CLR type system does not consider anything to be assignable to an open generic type.
            // For the purposes of test assertions, the user probably means that the subject type is
            // assignable to any generic type based on the given generic type definition.
            if (definition.IsInterface)
            {
                return type.IsImplementationOfOpenGeneric(definition);
            }
            else
            {
                return type == definition || type.IsDerivedFromOpenGeneric(definition);
            }
        }

        private static bool IsImplementationOfOpenGeneric(this Type type, Type definition)
        {
            // check subject against definition
            if (type.IsInterface && type.IsGenericType &&
                type.GetGenericTypeDefinition() == definition)
            {
                return true;
            }

            // check subject's interfaces against definition
            return type.GetInterfaces()
                .Where(i => i.IsGenericType)
                .Select(i => i.GetGenericTypeDefinition())
                .Contains(definition);
        }

        public static bool IsDerivedFromOpenGeneric(this Type type, Type definition)
        {
            if (type == definition)
            {
                // do not consider a type to be derived from itself
                return false;
            }

            // check subject and its base types against definition
            for (Type baseType = type; baseType != null;
                    baseType = baseType.BaseType)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == definition)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsUnderNamespace(this Type type, string @namespace)
        {
            return IsGlobalNamespace()
                || IsExactNamespace()
                || IsParentNamespace();

            bool IsGlobalNamespace() => @namespace is null;
            bool IsExactNamespace() => IsNamespacePrefix() && type.Namespace.Length == @namespace.Length;
            bool IsParentNamespace() => IsNamespacePrefix() && type.Namespace[@namespace.Length] == '.';
            bool IsNamespacePrefix() => type.Namespace?.StartsWith(@namespace, StringComparison.Ordinal) == true;
        }
    }
}
