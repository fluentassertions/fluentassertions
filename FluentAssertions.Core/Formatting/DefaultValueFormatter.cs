using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        ///   Determines whether this instance can handle the specified value.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
        {
            return true;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <param name="useLineBreaks"> </param>
        /// <param name="processedObjects">
        /// A collection of objects that 
        /// </param>
        /// <param name="nestedPropertyLevel">
        /// The level of nesting for the supplied value. This is used for indenting the format string for objects that have
        /// no <see cref="object.ToString()"/> override.
        /// </param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(object value, bool useLineBreaks, IList<object> processedObjects = null,
            int nestedPropertyLevel = 0)
        {
            if (value.GetType() == typeof (object))
            {
                return string.Format("System.Object (HashCode={0})", value.GetHashCode());
            }

            string prefix = (useLineBreaks ? Environment.NewLine : "");

            if (HasDefaultToStringImplementation(value))
            {
                if (!processedObjects.Contains(value))
                {
                    processedObjects.Add(value);
                    return prefix + GetTypeAndPublicPropertyValues(value, nestedPropertyLevel, processedObjects);
                }
                else
                {
                    return string.Format("{{Cyclic reference to type {0} detected}}", value.GetType());
                }
            }

            return prefix + value;
        }

        private static bool HasDefaultToStringImplementation(object value)
        {
            return ReferenceEquals(value.ToString(), null) || value.ToString().Equals(value.GetType().ToString());
        }

        private static string GetTypeAndPublicPropertyValues(object obj, int nestedPropertyLevel, IList<object> processedObjects)
        {
            var builder = new StringBuilder();

            if (nestedPropertyLevel == RootLevel)
            {
                builder.AppendLine();
                builder.AppendLine();
            }

            var type = obj.GetType();
            builder.AppendLine(type.FullName);
            builder.AppendLine(CreateWhitespaceForLevel(nestedPropertyLevel) + "{");

            IEnumerable<SelectedMemberInfo> properties = type.GetNonPrivateMembers();
            foreach (var propertyInfo in properties.OrderBy(pi => pi.Name))
            {
                builder.AppendLine(GetPropertyValueTextFor(obj, propertyInfo, nestedPropertyLevel + 1, processedObjects));
            }

            builder.AppendFormat("{0}}}", CreateWhitespaceForLevel(nestedPropertyLevel));

            return builder.ToString();
        }

        private static string GetPropertyValueTextFor(object value, SelectedMemberInfo selectedMemberInfo, int nextMemberNestingLevel,
            IList<object> processedObjects)
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
                CreateWhitespaceForLevel(nextMemberNestingLevel),
                selectedMemberInfo.Name,
                Formatter.ToString(propertyValue, false, processedObjects, nextMemberNestingLevel));
        }

        private static string CreateWhitespaceForLevel(int level)
        {
            return new string(' ', level*SpacesPerIndentionLevel);
        }
    }
}