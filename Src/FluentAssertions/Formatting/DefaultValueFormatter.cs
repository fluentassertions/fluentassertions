using System;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;

namespace FluentAssertions.Formatting
{
    public class DefaultValueFormatter : IValueFormatter
    {
        /// <summary>
        /// The number of spaces to indent the members of this object by.
        /// </summary>
        /// <remarks>The default value is 3.</remarks>
        protected virtual int SpacesPerIndentionLevel { get; } = 3;

        /// <summary>
        /// Determines whether this instance can handle the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if this instance can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanHandle(object value)
        {
            return true;
        }

        public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            if (value.GetType() == typeof(object))
            {
                formattedGraph.AddFragment($"System.Object (HashCode={value.GetHashCode()})");
                return;
            }

            if (HasDefaultToStringImplementation(value))
            {
                WriteTypeAndMemberValues(value, formattedGraph, formatChild);
            }
            else
            {
                if (context.UseLineBreaks)
                {
                    formattedGraph.AddLine(value.ToString());
                }
                else
                {
                    formattedGraph.AddFragment(value.ToString());
                }
            }
        }

        /// <summary>
        /// Selects which members of <paramref name="type"/> to format.
        /// </summary>
        /// <param name="type">The <see cref="System.Type"/> of the object being formatted.</param>
        /// <returns>The members of <paramref name="type"/> that will be included when formatting this object.</returns>
        /// <remarks>The default is all non-private members.</remarks>
        protected virtual MemberInfo[] GetMembers(Type type)
        {
            return type.GetNonPrivateMembers(MemberVisibility.Public).ToArray();
        }

        private static bool HasDefaultToStringImplementation(object value)
        {
            string str = value.ToString();

            return str is null || str == value.GetType().ToString();
        }

        private void WriteTypeAndMemberValues(object obj, FormattedObjectGraph formattedGraph, FormatChild formatChild)
        {
            Type type = obj.GetType();
            formattedGraph.AddLine(TypeDisplayName(type));
            formattedGraph.AddLine("{");

            MemberInfo[] members = GetMembers(type);
            using var iterator = new Iterator<MemberInfo>(members.OrderBy(mi => mi.Name));
            while (iterator.MoveNext())
            {
                WriteMemberValueTextFor(obj, iterator.Current, formattedGraph, formatChild);

                if (!iterator.IsLast)
                {
                    formattedGraph.AddFragment(", ");
                }
            }

            formattedGraph.AddFragmentOnNewLine("}");
        }

        /// <summary>
        /// Selects the name to display for <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="System.Type"/> of the object being formatted.</param>
        /// <returns>The name to be displayed for <paramref name="type"/>.</returns>
        /// <remarks>The default is <see cref="System.Type.FullName"/>.</remarks>
        protected virtual string TypeDisplayName(Type type) => type.FullName;

        private static void WriteMemberValueTextFor(object value, MemberInfo member, FormattedObjectGraph formattedGraph, FormatChild formatChild)
        {
            object memberValue;

            try
            {
                memberValue = member switch
                {
                    FieldInfo fi => fi.GetValue(value),
                    PropertyInfo pi => pi.GetValue(value),
                    _ => throw new InvalidOperationException()
                };
            }
            catch (Exception ex)
            {
                ex = (ex as TargetInvocationException)?.InnerException ?? ex;
                memberValue = $"[Member '{member.Name}' threw an exception: '{ex.Message}']";
            }

            formattedGraph.AddFragmentOnNewLine($"{new string(' ', FormattedObjectGraph.SpacesPerIndentation)}{member.Name} = ");
            formatChild(member.Name, memberValue, formattedGraph);
        }
    }
}
