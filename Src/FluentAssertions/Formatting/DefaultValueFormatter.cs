using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;

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
                return string.Format("System.Object (HashCode={0})", value.GetHashCode());
            }

            string prefix = (context.UseLineBreaks ? Environment.NewLine : "");

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
        protected virtual IEnumerable<SelectedMemberInfo> GetMembers(Type type) => type.GetNonPrivateMembers();

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

            IEnumerable<SelectedMemberInfo> members = GetMembers(type);
            foreach (SelectedMemberInfo memberInfo in members.OrderBy(mi => mi.Name))
            {
                string memberValueText = GetMemberValueTextFor(obj, memberInfo, context, formatChild);
                builder.AppendLine(memberValueText);
            }

            builder.Append(CreateWhitespaceForLevel(context.Depth)).Append('}');

            return builder.ToString();
        }

        private string GetMemberValueTextFor(object value, SelectedMemberInfo selectedMemberInfo, FormattingContext context, FormatChild formatChild)
        {
            object memberValue;

            try
            {
                memberValue = selectedMemberInfo.GetValue(value, null);
            }
            catch (Exception ex)
            {
                memberValue = string.Format("[Member '{0}' threw an exception: '{1}']", selectedMemberInfo.Name, ex.Message);
            }

            return string.Format("{0}{1} = {2}",
                CreateWhitespaceForLevel(context.Depth + 1),
                selectedMemberInfo.Name,
                formatChild(selectedMemberInfo.Name, memberValue));
        }

        private string CreateWhitespaceForLevel(int level)
        {
            return new string(' ', level * SpacesPerIndentionLevel);
        }
    }
}
