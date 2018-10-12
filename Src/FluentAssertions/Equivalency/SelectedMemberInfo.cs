using System;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Exposes information about an object's member
    /// </summary>
    public abstract class SelectedMemberInfo
    {
        public static SelectedMemberInfo Create(PropertyInfo propertyInfo)
        {
            if (propertyInfo is null || propertyInfo.IsIndexer())
            {
                return null;
            }

            return new PropertySelectedMemberInfo(propertyInfo);
        }

        public static SelectedMemberInfo Create(FieldInfo fieldInfo)
        {
            if (fieldInfo is null)
            {
                return null;
            }

            return new FieldSelectedMemberInfo(fieldInfo);
        }

        /// <summary>
        /// Gets the name of the current member.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the type of this member.
        /// </summary>
        public abstract Type MemberType { get; }

        /// <summary>
        /// Gets the class that declares this member.
        /// </summary>
        public abstract Type DeclaringType { get; }

        /// <summary>
        /// Gets the access modifier for the getter of this member.
        /// </summary>
        internal abstract CSharpAccessModifier GetGetAccessModifier();

        /// <summary>
        /// Gets the access modifier for the setter of this member.
        /// </summary>
        internal abstract CSharpAccessModifier GetSetAccessModifier();

        /// <summary>
        /// Returns the member value of a specified object with optional index values for indexed properties or methods.
        /// </summary>
        public abstract object GetValue(object obj, object[] index);
    }
}
