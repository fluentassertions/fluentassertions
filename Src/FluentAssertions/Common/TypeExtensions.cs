using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using FluentAssertionsAsync.Equivalency;

namespace FluentAssertionsAsync.Common;

internal static class TypeExtensions
{
    private const BindingFlags PublicInstanceMembersFlag =
        BindingFlags.Public | BindingFlags.Instance;

    private const BindingFlags AllStaticAndInstanceMembersFlag =
        PublicInstanceMembersFlag | BindingFlags.NonPublic | BindingFlags.Static;

    private static readonly ConcurrentDictionary<Type, bool> HasValueSemanticsCache = new();
    private static readonly ConcurrentDictionary<Type, bool> TypeIsRecordCache = new();
    private static readonly ConcurrentDictionary<Type, bool> TypeIsCompilerGeneratedCache = new();

    private static readonly ConcurrentDictionary<(Type Type, MemberVisibility Visibility), TypeMemberReflector>
        TypeMemberReflectorsCache = new();

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

    private static TAttribute[] GetCustomAttributes<TAttribute>(this Type type, bool inherit = false)
        where TAttribute : Attribute
    {
        return (TAttribute[])type.GetCustomAttributes(typeof(TAttribute), inherit);
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
    /// Finds the property by a case-sensitive name and with a certain visibility.
    /// </summary>
    /// <remarks>
    /// If both a normal property and one that was implemented through an explicit interface implementation with the same name exist,
    /// then the normal property will be returned.
    /// </remarks>
    /// <returns>
    /// Returns <see langword="null"/> if no such property exists.
    /// </returns>
    public static PropertyInfo FindProperty(this Type type, string propertyName, MemberVisibility memberVisibility)
    {
        var properties = type.GetProperties(memberVisibility);

        return Array.Find(properties, p =>
            p.Name == propertyName || p.Name.EndsWith("." + propertyName, StringComparison.Ordinal));
    }

    /// <summary>
    /// Finds the field by a case-sensitive name.
    /// </summary>
    /// <returns>
    /// Returns <see langword="null"/> if no such field exists.
    /// </returns>
    public static FieldInfo FindField(this Type type, string fieldName, MemberVisibility memberVisibility)
    {
        var fields = type.GetFields(memberVisibility);

        return Array.Find(fields, p => p.Name == fieldName);
    }

    public static MemberInfo[] GetMembers(this Type typeToReflect, MemberVisibility visibility)
    {
        return GetTypeReflectorFor(typeToReflect, visibility).Members;
    }

    public static PropertyInfo[] GetProperties(this Type typeToReflect, MemberVisibility visibility)
    {
        return GetTypeReflectorFor(typeToReflect, visibility).Properties;
    }

    public static FieldInfo[] GetFields(this Type typeToReflect, MemberVisibility visibility)
    {
        return GetTypeReflectorFor(typeToReflect, visibility).Fields;
    }

    private static TypeMemberReflector GetTypeReflectorFor(Type typeToReflect, MemberVisibility visibility)
    {
        return TypeMemberReflectorsCache.GetOrAdd((typeToReflect, visibility),
            static key => new TypeMemberReflector(key.Type, key.Visibility));
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

    private static bool HasParameterlessMethod(this Type type, string methodName)
    {
        return type.GetParameterlessMethod(methodName) is not null;
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
        const BindingFlags allInstanceMembersFlag =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        return type
            .GetConstructors(allInstanceMembersFlag)
            .SingleOrDefault(m => m.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes));
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
                && m.GetParameters() is { Length: 1 } parameters
                && parameters[0].ParameterType == sourceType);
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

        return type == definition || type.IsDerivedFromOpenGeneric(definition);
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
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == definition);
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
        bool IsParentNamespace() => IsNamespacePrefix() && type.Namespace[@namespace.Length] is '.';
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
            .GetConversionOperators(sourceType, targetType, name => name is "op_Explicit")
            .SingleOrDefault();
    }

    public static MethodInfo GetImplicitConversionOperator(this Type type, Type sourceType, Type targetType)
    {
        return type
            .GetConversionOperators(sourceType, targetType, name => name is "op_Implicit")
            .SingleOrDefault();
    }

    public static bool HasValueSemantics(this Type type)
    {
        return HasValueSemanticsCache.GetOrAdd(type, static t =>
            t.OverridesEquals() &&
            !t.IsAnonymous() &&
            !t.IsTuple() &&
            !IsKeyValuePair(t));
    }

    public static bool IsCompilerGenerated(this Type type)
    {
        return TypeIsCompilerGeneratedCache.GetOrAdd(type, static t =>
            t.IsRecord() ||
            t.IsAnonymous() ||
            t.IsTuple());
    }

    /// <summary>
    /// Check if the type has a human-readable name.
    /// </summary>
    /// <returns>false for compiler generated type names, otherwise true.</returns>
    public static bool HasFriendlyName(this Type type)
    {
        return !type.IsAnonymous() && !type.IsTuple();
    }

    private static bool IsTuple(this Type type)
    {
        if (!type.IsGenericType)
        {
            return false;
        }

#if !(NET47 || NETSTANDARD2_0)
        return typeof(ITuple).IsAssignableFrom(type);
#else
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
#endif
    }

    private static bool IsAnonymous(this Type type)
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
        return TypeIsRecordCache.GetOrAdd(type, static t => t.IsRecordClass() || t.IsRecordStruct());
    }

    private static bool IsRecordClass(this Type type)
    {
        return type.GetMethod("<Clone>$", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly) is { } &&
            type.GetProperty("EqualityContract", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)?
                .GetMethod?.IsDecoratedWith<CompilerGeneratedAttribute>() == true;
    }

    private static bool IsRecordStruct(this Type type)
    {
        // As noted here: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-10.0/record-structs#open-questions
        // recognizing record structs from metadata is an open point. The following check is based on common sense
        // and heuristic testing, apparently giving good results but not supported by official documentation.
        return type.BaseType == typeof(ValueType) &&
            type.GetMethod("PrintMembers", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly, null,
                new[] { typeof(StringBuilder) }, null) is { } &&
            type.GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly, null,
                    new[] { type, type }, null)?
                .IsDecoratedWith<CompilerGeneratedAttribute>() == true;
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
            type = type.GetGenericArguments()[0];
        }

        return type;
    }
}
