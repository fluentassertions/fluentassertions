using System;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    public class DefaultValueFormatter : IValueFormatter
    {
        private const int RootLevel = 0;

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

        /// <inheritdoc />
        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            if (value.GetType() == typeof(object))
            {
                return $"System.Object (HashCode={value.GetHashCode()})";
            }

            string prefix = context.UseLineBreaks ? Environment.NewLine : string.Empty;

            if (HasDefaultToStringImplementation(value))
            {
                return prefix + GetTypeAndMemberValues(value, context, formatChild);
            }

            return prefix + value;
        }

        /// <summary>
        /// Selects which members of <paramref name="type"/> to format.
        /// </summary>
        /// <param name="type">The <see cref="System.Type"/> of the object being formatted.</param>
        /// <returns>The members of <paramref name="type"/> that will be included when formatting this object.</returns>
        /// <remarks>The default is all non-private members.</remarks>
        protected virtual MemberInfo[] GetMembers(Type type)
        {
            return type.GetNonPrivateMembers().ToArray();
        }

        /// <summary>
        /// Selects the name to display for <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="System.Type"/> of the object being formatted.</param>
        /// <returns>The name to be displayed for <paramref name="type"/>.</returns>
        /// <remarks>The default is <see cref="System.Type.FullName"/>.</remarks>
        protected virtual string TypeDisplayName(Type type) => type.FullName;

        private static bool HasDefaultToStringImplementation(object value)
        {
            string str = value.ToString();

            return str is null || str == value.GetType().ToString();
        }

        private string GetTypeAndMemberValues(object obj, FormattingContext context, FormatChild formatChild)
        {
            var builder = new StringBuilder();

            if (context.Depth == RootLevel)
            {
                builder.AppendLine();
                builder.AppendLine();
            }

            Type type = obj.GetType();
            builder.AppendLine(TypeDisplayName(type));
            builder.Append(CreateWhitespaceForLevel(context.Depth)).Append('{').AppendLine();

            MemberInfo[] members = GetMembers(type);
            foreach (var memberInfo in members.OrderBy(mi => mi.Name))
            {
                string memberValueText = GetMemberValueTextFor(obj, memberInfo, context, formatChild);
                builder.AppendLine(memberValueText);
            }

            builder.Append(CreateWhitespaceForLevel(context.Depth)).Append('}');

            return builder.ToString();
        }

        private string GetMemberValueTextFor(object value, MemberInfo member, FormattingContext context, FormatChild formatChild)
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
                memberValue = $"[Member '{member.Name}' threw an exception: '{ex.Message}']";
            }

            return
                $"{CreateWhitespaceForLevel(context.Depth + 1)}{member.Name} = {formatChild(member.Name, memberValue)}";
        }

        private string CreateWhitespaceForLevel(int level)
        {
            return new string(' ', level * SpacesPerIndentionLevel);
        }
    }
}
