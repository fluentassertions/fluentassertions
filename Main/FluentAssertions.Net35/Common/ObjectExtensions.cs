using System.Linq;
using System.Reflection;

namespace FluentAssertions.Common
{
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
}