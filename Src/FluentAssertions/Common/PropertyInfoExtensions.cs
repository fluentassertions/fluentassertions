using System.Reflection;

namespace FluentAssertionsAsync.Common;

internal static class PropertyInfoExtensions
{
    internal static bool IsVirtual(this PropertyInfo property)
    {
        MethodInfo methodInfo = property.GetGetMethod(nonPublic: true) ?? property.GetSetMethod(nonPublic: true);
        return !methodInfo.IsNonVirtual();
    }

    internal static bool IsStatic(this PropertyInfo property)
    {
        MethodInfo methodInfo = property.GetGetMethod(nonPublic: true) ?? property.GetSetMethod(nonPublic: true);
        return methodInfo.IsStatic;
    }

    internal static bool IsAbstract(this PropertyInfo property)
    {
        MethodInfo methodInfo = property.GetGetMethod(nonPublic: true) ?? property.GetSetMethod(nonPublic: true);
        return methodInfo.IsAbstract;
    }
}
