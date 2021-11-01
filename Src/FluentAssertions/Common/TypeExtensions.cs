using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentAssertions.Equivalency;

namespace FluentAssertions.Common
{
    internal static class TypeExtensions
    {
        private const BindingFlags PublicInstanceMembersFlag =
            BindingFlags.Public | BindingFlags.Instance;

        private const BindingFlags AllInstanceMembersFlag =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private const BindingFlags AllStaticAndInstanceMembersFlag =
            PublicInstanceMembersFlag | BindingFlags.NonPublic | BindingFlags.Static;

        private static readonly ConcurrentDictionary<Type, bool> HasValueSemanticsCache = new();
        private static readonly ConcurrentDictionary<Type, bool> TypeIsRecordCache = new();
        private static readonly ConcurrentDictionary<(Type Type, MemberVisibility Visibility), IEnumerable<PropertyInfo>> NonPrivatePropertiesCache = new();
        private static readonly ConcurrentDictionary<(Type Type, MemberVisibility Visibility), IEnumerable<FieldInfo>> NonPrivateFieldsCache = new();

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

        public static bool IsDecoratedWith<TAttribute>(this Type type,
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            return GetCustomAttributes(type, isMatchingAttributePredicate).Any();
        }

        public static bool IsDecoratedWith<TAttribute>(this MemberInfo type,
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            return GetCustomAttributes(type, isMatchingAttributePredicate).Any();
        }

        public static bool IsDecoratedWithOrInherit<TAttribute>(this Type type,
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            return GetCustomAttributes(type, isMatchingAttributePredicate, inherit: true).Any();
        }

        public static IEnumerable<TAttribute> GetMatchingAttributes<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return GetCustomAttributes<TAttribute>(type);
        }

        public static IEnumerable<TAttribute> GetMatchingAttributes<TAttribute>(this Type type,
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            return GetCustomAttributes(type, isMatchingAttributePredicate);
        }

        public static IEnumerable<TAttribute> GetMatchingOrInheritedAttributes<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return GetCustomAttributes<TAttribute>(type, inherit: true);
        }

        public static IEnumerable<TAttribute> GetMatchingOrInheritedAttributes<TAttribute>(this Type type,
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
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
        /// Determines whether two <see cref="IMember" /> objects refer to the same
        /// member.
        /// </summary>
        public static bool IsEquivalentTo(this IMember property, IMember otherProperty)
        {
            return (property.DeclaringType.IsSameOrInherits(otherProperty.DeclaringType) ||
                    otherProperty.DeclaringType.IsSameOrInherits(property.DeclaringType)) &&
                   property.Name == otherProperty.Name;
        }

        /// <summary>
        /// Returns the interfaces that the <paramref name="type"/> implements that are concrete
        /// versions of the <paramref name="openGenericType"/>.
        /// </summary>
        public static Type[] GetClosedGenericInterfaces(this Type type, Type openGenericType)
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

            return method is not null
                   && method.GetBaseDefinition().DeclaringType != method.DeclaringType;
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
                type.GetProperties(AllInstanceMembersFlag)
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
                type.GetFields(AllInstanceMembersFlag)
                    .Where(pi => pi.Name == fieldName)
                    .ToList();

            return properties.Count > 1
                ? properties.SingleOrDefault(p => p.FieldType == preferredType)
                : properties.SingleOrDefault();
        }

        public static IEnumerable<MemberInfo> GetNonPrivateMembers(this Type typeToReflect, MemberVisibility visibility)
        {
            return
                GetNonPrivateProperties(typeToReflect, visibility)
                    .Concat<MemberInfo>(GetNonPrivateFields(typeToReflect, visibility))
                    .ToArray();
        }

        public static IEnumerable<PropertyInfo> GetNonPrivateProperties(this Type typeToReflect, MemberVisibility visibility)
        {
            return NonPrivatePropertiesCache.GetOrAdd((typeToReflect, visibility), static key =>
            {
                IEnumerable<PropertyInfo> query =
                    from propertyInfo in GetPropertiesFromHierarchy(key.Type, key.Visibility)
                    where HasNonPrivateGetter(propertyInfo)
                    where !propertyInfo.IsIndexer()
                    select propertyInfo;

                return query.ToArray();
            });
        }

        private static IEnumerable<PropertyInfo> GetPropertiesFromHierarchy(Type typeToReflect, MemberVisibility memberVisibility)
        {
            bool includeInternals = memberVisibility.HasFlag(MemberVisibility.Internal);

            return GetMembersFromHierarchy(typeToReflect, type =>
            {
                return type
                    .GetProperties(AllInstanceMembersFlag | BindingFlags.DeclaredOnly)
                    .Where(property => property.GetMethod?.IsPrivate == false)
                    .Where(property => includeInternals || (property.GetMethod?.IsAssembly == false && property.GetMethod?.IsFamilyOrAssembly == false))
                    .ToArray();
            });
        }

        public static IEnumerable<FieldInfo> GetNonPrivateFields(this Type typeToReflect, MemberVisibility visibility)
        {
            return NonPrivateFieldsCache.GetOrAdd((typeToReflect, visibility), static key =>
            {
                IEnumerable<FieldInfo> query =
                    from fieldInfo in GetFieldsFromHierarchy(key.Type, key.Visibility)
                    where !fieldInfo.IsPrivate
                    where !fieldInfo.IsFamily
                    select fieldInfo;

                return query.ToArray();
            });
        }

        private static IEnumerable<FieldInfo> GetFieldsFromHierarchy(Type typeToReflect, MemberVisibility memberVisibility)
        {
            bool includeInternals = memberVisibility.HasFlag(MemberVisibility.Internal);

            return GetMembersFromHierarchy(typeToReflect, type =>
            {
                return type
                    .GetFields(AllInstanceMembersFlag)
                    .Where(field => !field.IsPrivate)
                    .Where(field => includeInternals || (!field.IsAssembly && !field.IsFamilyOrAssembly))
                    .ToArray();
            });
        }

        private static IEnumerable<TMemberInfo> GetMembersFromHierarchy<TMemberInfo>(
            Type typeToReflect,
            Func<Type, IEnumerable<TMemberInfo>> getMembers)
            where TMemberInfo : MemberInfo
        {
            if (typeToReflect.IsInterface)
            {
                return GetInterfaceMembers(typeToReflect, getMembers);
            }
            else
            {
                return GetClassMembers(typeToReflect, getMembers);
            }
        }

        private static List<TMemberInfo> GetInterfaceMembers<TMemberInfo>(Type typeToReflect, Func<Type, IEnumerable<TMemberInfo>> getMembers)
            where TMemberInfo : MemberInfo
        {
            List<TMemberInfo> members = new();

            var considered = new List<Type>();
            var queue = new Queue<Type>();
            considered.Add(typeToReflect);
            queue.Enqueue(typeToReflect);

            while (queue.Count > 0)
            {
                Type subType = queue.Dequeue();
                foreach (Type subInterface in subType.GetInterfaces())
                {
                    if (considered.Contains(subInterface))
                    {
                        continue;
                    }

                    considered.Add(subInterface);
                    queue.Enqueue(subInterface);
                }

                IEnumerable<TMemberInfo> typeMembers = getMembers(subType);

                IEnumerable<TMemberInfo> newPropertyInfos = typeMembers.Where(x => !members.Contains(x));

                members.InsertRange(0, newPropertyInfos);
            }

            return members;
        }

        private static List<TMemberInfo> GetClassMembers<TMemberInfo>(Type typeToReflect, Func<Type, IEnumerable<TMemberInfo>> getMembers)
            where TMemberInfo : MemberInfo
        {
            List<TMemberInfo> members = new();

            while (typeToReflect != null)
            {
                foreach (var memberInfo in getMembers(typeToReflect))
                {
                    if (members.All(mi => mi.Name != memberInfo.Name))
                    {
                        members.Add(memberInfo);
                    }
                }

                typeToReflect = typeToReflect.BaseType;
            }

            return members;
        }

        private static bool HasNonPrivateGetter(PropertyInfo propertyInfo)
        {
            MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: true);
            return getMethod is not null && !getMethod.IsPrivate && !getMethod.IsFamily;
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
            return type.GetMethods(AllStaticAndInstanceMembersFlag)
                .SingleOrDefault(m =>
                    m.Name == methodName && m.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes));
        }

        public static bool HasMethod(this Type type, string methodName, IEnumerable<Type> parameterTypes)
        {
            return type.GetMethod(methodName, parameterTypes) is not null;
        }

        public static MethodInfo GetParameterlessMethod(this Type type, string methodName)
        {
            return type.GetMethod(methodName, Enumerable.Empty<Type>());
        }

        public static bool HasParameterlessMethod(this Type type, string methodName)
        {
            return type.GetParameterlessMethod(methodName) is not null;
        }

        public static PropertyInfo FindPropertyByName(this Type type, string propertyName)
        {
            return type.GetProperty(propertyName, AllStaticAndInstanceMembersFlag);
        }

        public static bool HasExplicitlyImplementedProperty(this Type type, Type interfaceType, string propertyName)
        {
            bool hasGetter = type.HasParameterlessMethod($"{interfaceType.FullName}.get_{propertyName}");
            bool hasSetter = type.GetMethods(AllStaticAndInstanceMembersFlag)
                .SingleOrDefault(m =>
                    m.Name == $"{interfaceType.FullName}.set_{propertyName}" &&
                    m.GetParameters().Length == 1) is not null;

            return hasGetter || hasSetter;
        }

        public static PropertyInfo GetIndexerByParameterTypes(this Type type, IEnumerable<Type> parameterTypes)
        {
            return type.GetProperties(AllStaticAndInstanceMembersFlag)
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
                .GetConstructors(AllInstanceMembersFlag)
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
            for (Type baseType = type;
                baseType is not null;
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

        public static bool IsSameOrInherits(this Type actualType, Type expectedType)
        {
            return actualType == expectedType ||
                   expectedType.IsAssignableFrom(actualType);
        }

        public static MethodInfo GetExplicitConversionOperator(this Type type, Type sourceType, Type targetType)
        {
            return type
                .GetConversionOperators(sourceType, targetType, name => name == "op_Explicit")
                .SingleOrDefault();
        }

        public static MethodInfo GetImplicitConversionOperator(this Type type, Type sourceType, Type targetType)
        {
            return type
                .GetConversionOperators(sourceType, targetType, name => name == "op_Implicit")
                .SingleOrDefault();
        }

        public static bool HasValueSemantics(this Type type)
        {
            return HasValueSemanticsCache.GetOrAdd(type, static t =>
                t.OverridesEquals() &&
                !t.IsAnonymousType() &&
                !t.IsTuple() &&
                !IsKeyValuePair(t));
        }

        private static bool IsTuple(this Type type)
        {
            if (!type.IsGenericType)
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

        private static bool IsAnonymousType(this Type type)
        {
            bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType", StringComparison.Ordinal);

            if (!nameContainsAnonymousType)
            {
                return false;
            }

            bool hasCompilerGeneratedAttribute =
                type.IsDecoratedWith<CompilerGeneratedAttribute>();

            return hasCompilerGeneratedAttribute;
        }

        public static bool IsRecord(this Type type)
        {
            return TypeIsRecordCache.GetOrAdd(type, static t =>
                t.GetMethod("<Clone>$") is not null &&
                t.GetTypeInfo()
                     .DeclaredProperties
                     .FirstOrDefault(p => p.Name == "EqualityContract")?
                     .GetMethod?
                     .GetCustomAttribute(typeof(CompilerGeneratedAttribute)) is not null);
        }

        private static bool IsKeyValuePair(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>);
        }

        /// <summary>
        /// If the type provided is a nullable type, gets the underlying type. Returns the type itself otherwise.
        /// </summary>
        public static Type NullableOrActualType(this Type type)
        {
            if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments().First();
            }

            return type;
        }
    }
}
