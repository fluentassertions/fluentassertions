using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Extension methods for getting method and property selectors for a type.
    /// </summary>
    [DebuggerNonUserCode]
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns a method selector for the current <see cref="Type"/>.
        /// </summary>
        public static MethodSelector Methods(this Type type)
        {
            return new MethodSelector(type);
        }
    }
}