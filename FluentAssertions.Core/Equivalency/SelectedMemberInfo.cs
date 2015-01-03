using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Provides factory methods for acquring ISelectedMemberInfo objects
    /// </summary>
    public static class SelectedMemberInfo
    {
        public static ISelectedMemberInfo Create(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null || propertyInfo.IsIndexer())
            {
                return null;
            }

            return new PropertySelectedMemberInfo(propertyInfo);
        }

        public static ISelectedMemberInfo Create(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                return null;
            }

            return new FieldSelectedMemberInfo(fieldInfo);
        }
    }
}