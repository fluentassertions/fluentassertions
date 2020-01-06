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
        #region Private Definitions

        private const int RootLevel = 0;
        private const int SpacesPerIndentionLevel = 3;

        #endregion

        /// <summary>
        /// Determines whether this instance can handle the specified value.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>
        /// <c>true</c> if this instance can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
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
                if (true)
                {
                    return prefix + GetTypeAndPublicPropertyValues(value, context, formatChild);
                }
            }

            return prefix + value;
        }

        private static bool HasDefaultToStringImplementation(object value)
        {
            string str = value.ToString();

            return str is null || str.Equals(value.GetType().ToString());
        }

        private static string GetTypeAndPublicPropertyValues(object obj, FormattingContext context, FormatChild formatChild)
        {
            var builder = new StringBuilder();

            if (context.Depth == RootLevel)
            {
                builder.AppendLine();
                builder.AppendLine();
            }

            Type type = obj.GetType();
            builder.AppendLine(type.FullName);
            builder.Append(CreateWhitespaceForLevel(context.Depth)).Append('{').AppendLine();

            IEnumerable<SelectedMemberInfo> properties = type.GetNonPrivateMembers();
            foreach (SelectedMemberInfo propertyInfo in properties.OrderBy(pi => pi.Name))
            {
                string propertyValueText = GetPropertyValueTextFor(obj, propertyInfo, context, formatChild);
                builder.AppendLine(propertyValueText);
            }

            builder.Append(CreateWhitespaceForLevel(context.Depth)).Append('}');

            return builder.ToString();
        }

        private static string GetPropertyValueTextFor(object value, SelectedMemberInfo selectedMemberInfo, FormattingContext context, FormatChild formatChild)
        {
            object propertyValue;

            try
            {
                propertyValue = selectedMemberInfo.GetValue(value, null);
            }
            catch (Exception ex)
            {
                propertyValue = string.Format("[Member '{0}' threw an exception: '{1}']", selectedMemberInfo.Name, ex.Message);
            }

            return string.Format("{0}{1} = {2}",
                CreateWhitespaceForLevel(context.Depth + 1),
                selectedMemberInfo.Name,
                formatChild(selectedMemberInfo.Name, propertyValue));
        }

        private static string CreateWhitespaceForLevel(int level)
        {
            return new string(' ', level * SpacesPerIndentionLevel);
        }
    }
}
