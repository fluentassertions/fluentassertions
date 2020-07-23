using System.Reflection;

namespace FluentAssertions.Common
{
    public static class PropertyInfoExtensions
    {
        internal static bool IsVirtual(this PropertyInfo property)
        {
            MethodInfo methodInfo = property.GetGetMethod(nonPublic: true) ?? property.GetSetMethod(nonPublic: true);
            return !methodInfo.IsNonVirtual();
        }
    }
}
