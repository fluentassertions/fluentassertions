using System.Reflection;

namespace FluentAssertions.Common;

internal static class PropertyInfoExtensions
{
    /// <summary>
    /// Checks if the property is virtual
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    internal static bool IsVirtual(this PropertyInfo property)
    {
        MethodInfo methodInfo = property.GetGetMethod(nonPublic: true) ?? property.GetSetMethod(nonPublic: true);
        return !methodInfo.IsNonVirtual();
    }

    /// <summary>
    /// Checks if the property is static 
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    internal static bool IsStatic(this PropertyInfo property)
    {
        MethodInfo methodInfo = property.GetGetMethod(nonPublic: true);
        return methodInfo.IsStatic;
    }

    /// <summary>
    /// Checks if the property is Abstract
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    internal static bool IsAbstract(this PropertyInfo property)
    {
        MethodInfo methodInfo = property.GetGetMethod(nonPublic: true);
        return methodInfo.IsAbstract;
    }
}
