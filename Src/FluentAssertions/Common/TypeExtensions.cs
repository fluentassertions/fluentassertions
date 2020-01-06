using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentAssertions.Equivalency;

namespace FluentAssertions.Common
{
    public static class TypeExtensions
    {
        private const BindingFlags PublicMembersFlag =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private const BindingFlags AllMembersFlag =
            PublicMembersFlag | BindingFlags.NonPublic | BindingFlags.Static;

        /// <summary>
        /// Determines whether the specified method has been annotated with a specific attribute.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the specified method has attribute; otherwise, <c>false</c>.
        /// </returns>
        [Obsolete("This method is deprecated and will be removed on the next major version. Please use <IsDecoratedWithOrInherits> instead.")]
        public static bool HasAttribute<TAttribute>(this MemberInfo method)
            where TAttribute : Attribute
        {
            return method.GetCustomAttributes(typeof(TAttribute), true).Any();
        }

        [Obsolete("This method is deprecated and will be removed on the next major version. Please use <IsDecoratedWith> instead.")]
        public static bool HasMatchingAttribute<TAttribute>(this MemberInfo type,
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            Func<TAttribute, bool> isMatchingAttribute = isMatchingAttributePredicate.Compile();

            return GetCustomAttributes<TAttribute>(type).Any(isMatchingAttribute);
        }

        [Obsolete("This method is deprecated and will be removed on the next major version. Please use <IsDecoratedWithOrInherits> or <IsDecoratedWith> instead.")]
        public static bool HasMatchingAttribute<TAttribute>(this Type type,
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, bool inherit = false)
            where TAttribute : Attribute
        {
            Func<TAttribute, bool> isMatchingAttribute = isMatchingAttributePredicate.Compile();

            return GetCustomAttributes<TAttribute>(type, inherit).Any(isMatchingAttribute);
        }

        public static bool IsDecoratedWith<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return GetCustomAttributes<TAttribute>(type).Any();
        }

        public static bool IsDecoratedWith<TAttribute>(this TypeInfo type)
            where TAttribute : Attribute
        {
            return GetCustomAttributes<TAttribute>(type).Any();
        }

        public static bool IsDecoratedWith<TAttribute>(this MemberInfo type)
            where TAttribute : Attribute
        {
            return GetCustomAttributes<TAttribute>(type).Any();
        }

        public static bool IsDecoratedWithOrInherit<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return GetCustomAttributes<TAttribute>(type, true).Any();
        }

        public static bool IsDecoratedWithOrInherit<TAttribute>(this TypeInfo type)
            where TAttribute : Attribute
        {
            return GetCustomAttributes<TAttribute>(type, true).Any();
        }

        public static bool IsDecoratedWithOrInherit<TAttribute>(this MemberInfo type)
            where TAttribute : Attribute
        {
            return GetCustomAttributes<TAttribute>(type, true).Any();
        }

        public static bool IsDecoratedWith<TAttribute>(this Type type, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            Func<TAttribute, bool> isMatchingAttribute = isMatchingAttributePredicate.Compile();
            return GetCustomAttributes<TAttribute>(type).Any(isMatchingAttribute);
        }

        public static bool IsDecoratedWith<TAttribute>(this TypeInfo type, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            Func<TAttribute, bool> isMatchingAttribute = isMatchingAttributePredicate.Compile();
            return GetCustomAttributes<TAttribute>(type).Any(isMatchingAttribute);
        }

        public static bool IsDecoratedWith<TAttribute>(this MemberInfo type, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            Func<TAttribute, bool> isMatchingAttribute = isMatchingAttributePredicate.Compile();
            return GetCustomAttributes<TAttribute>(type).Any(isMatchingAttribute);
        }

        public static bool IsDecoratedWithOrInherit<TAttribute>(this Type type, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            Func<TAttribute, bool> isMatchingAttribute = isMatchingAttributePredicate.Compile();
            return GetCustomAttributes<TAttribute>(type, true).Any(isMatchingAttribute);
        }

        public static bool IsDecoratedWithOrInherit<TAttribute>(this TypeInfo type, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            Func<TAttribute, bool> isMatchingAttribute = isMatchingAttributePredicate.Compile();
            return GetCustomAttributes<TAttribute>(type, true).Any(isMatchingAttribute);
        }

        public static bool IsDecoratedWithOrInherit<TAttribute>(this MemberInfo type, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            Func<TAttribute, bool> isMatchingAttribute = isMatchingAttributePredicate.Compile();
            return GetCustomAttributes<TAttribute>(type, true).Any(isMatchingAttribute);
        }

        [Obsolete("This overload is deprecated and will be removed on the next major version. Please use <IsDecoratedWithOrInherits> or <IsDecoratedWith> instead.")]
        public static bool IsDecoratedWith<TAttribute>(this Type type, bool inherit = false)
            where TAttribute : Attribute
        {
            return GetCustomAttributes<TAttribute>(type, inherit).Any();
        }

        private static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(MemberInfo type, bool inherit = false)
            where TAttribute : Attribute
        {
            // Do not use as extension method here, there is an issue with PropertyInfo and EventInfo
            // preventing the inherit option to work.
            return CustomAttributeExtensions.GetCustomAttributes(type, inherit).OfType<TAttribute>();
        }

        private static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(Type type, bool inherit = false)
            where TAttribute : Attribute
        {
            return GetCustomAttributes<TAttribute>(type.GetTypeInfo(), inherit);
        }

        private static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(TypeInfo typeInfo, bool inherit = false)
            where TAttribute : Attribute
        {
            return typeInfo.GetCustomAttributes(inherit).OfType<TAttribute>();
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

        public static bool IsSameOrInherits(this Type actualType, Type expectedType)
        {
            return actualType == expectedType ||
                   expectedType.IsAssignableFrom(actualType);
        }

        /// <summary>
        /// NOTE: This method does not give the expected results with open generics
        /// </summary>
        public static bool Implements(this Type type, Type expectedBaseType)
        {
            return
                expectedBaseType.IsAssignableFrom(type)
                && type != expectedBaseType;
        }

        internal static Type[] GetClosedGenericInterfaces(Type type, Type openGenericType)
        {
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == openGenericType)
            {
                return new[] { type };
            }

            Type[] interfaces = type.GetInterfaces();
            return
                interfaces
                    .Where(t => t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == openGenericType)
                    .ToArray();
        }

        public static bool OverridesEquals(this Type type)
        {
#if NETSTANDARD1_3
            MethodInfo[] methods = type
                .GetMethods(BindingFlags.Public | BindingFlags.Instance);

            return methods.Any(m => m.Name == "Equals"
                && m.GetParameters().SingleOrDefault()?.ParameterType == typeof(object)
                && m.GetBaseDefinition().DeclaringType != m.DeclaringType);
#else
            MethodInfo method = type.GetTypeInfo()
                .GetMethod("Equals", new[] { typeof(object) });

            return method != null
                && method.GetBaseDefinition().DeclaringType != method.DeclaringType;
#endif
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
            if (IsInterface(typeToReflect))
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

        private static bool IsInterface(Type typeToReflect)
        {
            return typeToReflect.GetTypeInfo().IsInterface;
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
            MethodInfo getMethod = propertyInfo.GetGetMethod(true);
            return getMethod != null && !getMethod.IsPrivate && !getMethod.IsFamily;
        }

        /// <summary>
        /// Check if the type is declared as abstract.
        /// </summary>
        /// <param name="type">Type to be checked</param>
        /// <returns></returns>
        public static bool IsCSharpAbstract(this Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();
            return typeInfo.IsAbstract && !typeInfo.IsSealed;
        }

        /// <summary>
        /// Check if the type is declared as sealed.
        /// </summary>
        /// <param name="type">Type to be checked</param>
        /// <returns></returns>
        public static bool IsCSharpSealed(this Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();
            return typeInfo.IsSealed && !typeInfo.IsAbstract;
        }

        /// <summary>
        /// Check if the type is declared as static.
        /// </summary>
        /// <param name="type">Type to be checked</param>
        /// <returns></returns>
        public static bool IsCSharpStatic(this Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();
            return typeInfo.IsSealed && typeInfo.IsAbstract;
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

        public static MethodInfo GetImplicitConversionOperator(this Type type, Type sourceType, Type targetType)
        {
            return type
                .GetConversionOperators(sourceType, targetType, name => name == "op_Implicit")
                .SingleOrDefault();
        }

        public static MethodInfo GetExplicitConversionOperator(this Type type, Type sourceType, Type targetType)
        {
            return type
                .GetConversionOperators(sourceType, targetType, name => name == "op_Explicit")
                .SingleOrDefault();
        }

        private static IEnumerable<MethodInfo> GetConversionOperators(this Type type, Type sourceType, Type targetType,
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

        public static bool HasValueSemantics(this Type type)
        {
            return type.OverridesEquals() &&
                   !type.IsAnonymousType() && !type.IsTuple() && !IsKeyValuePair(type);
        }

        private static bool IsKeyValuePair(Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>);
        }

        private static bool IsAnonymousType(this Type type)
        {
            bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType");

            if (!nameContainsAnonymousType)
            {
                return false;
            }

            bool hasCompilerGeneratedAttribute =
                type.GetTypeInfo().IsDecoratedWith<CompilerGeneratedAttribute>();

            return hasCompilerGeneratedAttribute;
        }

        private static bool IsTuple(this Type type)
        {
            if (!type.GetTypeInfo().IsGenericType)
            {
                return false;
            }

            Type openType = type.GetGenericTypeDefinition();
            return openType == typeof(ValueTuple<>)
                   || openType == typeof(ValueTuple<,>)
                   || openType == typeof(ValueTuple<,,>)
                   || openType == typeof(ValueTuple<,,,>)
                   || openType == typeof(ValueTuple<,,,,>)
                   || openType == typeof(ValueTuple<,,,,,>)
                   || openType == typeof(ValueTuple<,,,,,,>)
                   || (openType == typeof(ValueTuple<,,,,,,,>) && IsTuple(type.GetGenericArguments()[7]))
                   || openType == typeof(Tuple<>)
                   || openType == typeof(Tuple<,>)
                   || openType == typeof(Tuple<,,>)
                   || openType == typeof(Tuple<,,,>)
                   || openType == typeof(Tuple<,,,,>)
                   || openType == typeof(Tuple<,,,,,>)
                   || openType == typeof(Tuple<,,,,,,>)
                   || (openType == typeof(Tuple<,,,,,,,>) && IsTuple(type.GetGenericArguments()[7]));
        }

        internal static bool IsAssignableToOpenGeneric(this Type type, Type definition)
        {
            // The CLR type system does not consider anything to be assignable to an open generic type.
            // For the purposes of test assertions, the user probably means that the subject type is
            // assignable to any generic type based on the given generic type definition.

            if (definition.GetTypeInfo().IsInterface)
            {
                return type.IsImplementationOfOpenGeneric(definition);
            }
            else
            {
                return type.IsSameOrEqualTo(definition) || type.IsDerivedFromOpenGeneric(definition);
            }
        }

        internal static bool IsImplementationOfOpenGeneric(this Type type, Type definition)
        {
            // check subject against definition
            TypeInfo subjectInfo = type.GetTypeInfo();
            if (subjectInfo.IsInterface && subjectInfo.IsGenericType &&
                subjectInfo.GetGenericTypeDefinition().IsSameOrEqualTo(definition))
            {
                return true;
            }

            // check subject's interfaces against definition
            return subjectInfo.ImplementedInterfaces
                .Select(i => i.GetTypeInfo())
                .Where(i => i.IsGenericType)
                .Select(i => i.GetGenericTypeDefinition())
                .Any(d => d.IsSameOrEqualTo(definition));
        }

        internal static bool IsDerivedFromOpenGeneric(this Type type, Type definition)
        {
            if (type.IsSameOrEqualTo(definition))
            {
                // do not consider a type to be derived from itself
                return false;
            }

            // check subject and its base types against definition
            for (TypeInfo baseType = type.GetTypeInfo(); baseType != null;
                    baseType = baseType.BaseType?.GetTypeInfo())
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition().IsSameOrEqualTo(definition))
                {
                    return true;
                }
            }

            return false;
        }

        internal static bool IsUnderNamespace(this Type type, string @namespace)
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
