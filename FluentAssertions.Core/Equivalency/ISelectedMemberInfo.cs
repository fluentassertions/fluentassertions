using System;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Exposes information about an object's member
    /// </summary>
    public interface ISelectedMemberInfo
    {
        /// <summary>
        /// Gets the name of the current member.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the type of this member.
        /// </summary>
        Type MemberType { get; }

        /// <summary>
        /// Gets the class that declares this member.
        /// </summary>
        Type DeclaringType { get; }

        /// <summary>
        /// Returns the member value of a specified object with optional index values for indexed properties or methods.
        /// </summary>
        object GetValue(object obj, object[] index);
    }
}