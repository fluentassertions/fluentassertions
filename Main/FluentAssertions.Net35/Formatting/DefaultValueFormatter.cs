using System;
using System.Linq;
using System.Reflection;
using System.Text;

using FluentAssertions.Assertions;
using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    internal class DefaultValueFormatter : IValueFormatter
    {
        #region Private Definitions

        private const int RootLevel = 0;
        private const int SpacesPerIndentionLevel = 3;
        private readonly ObjectTracker objectTracker = new ObjectTracker();

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
        /// <param name="nestedPropertyLevel">
        /// The level of nesting for the supplied value. This is used for indenting the format string for objects that have
        /// no <see cref="object.ToString()"/> override.
        /// </param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(object value, int nestedPropertyLevel = RootLevel)
        {
            if (value.GetType() == typeof (object))
            {
                return string.Format("System.Object (HashCode={0})", value.GetHashCode());
            }

            if (HasDefaultToStringImplementation(value))
            {
                try
                {
                    DetectCyclicReferenceOf(value, nestedPropertyLevel);
                }
                catch (ObjectAlreadyTrackedException)
                {
                    return string.Format("Cyclic reference detected for object of type {0}.", value.GetType());
                }

                return GetTypeAndPublicPropertyValues(value, nestedPropertyLevel);
            }

            return value.ToString();
        }

        private static bool HasDefaultToStringImplementation(object value)
        {
            return value.ToString().Equals(value.GetType().FullName);
        }

        private void DetectCyclicReferenceOf(object value, int nestedPropertyLevel)
        {
            if (nestedPropertyLevel == RootLevel)
            {
                objectTracker.Reset();
            }

            objectTracker.Add(value);
        }

        private string GetTypeAndPublicPropertyValues(object obj, int nestedPropertyLevel)
        {
            var builder = new StringBuilder();
            
            if (nestedPropertyLevel == RootLevel)
            {
                builder.AppendLine();
                builder.AppendLine();
            }

            Type type = obj.GetType();
            builder.AppendLine(type.FullName);
            builder.AppendLine(CreateWhitespaceForLevel(nestedPropertyLevel) + "{");

            foreach (var propertyInfo in type.GetProperties().OrderBy(pi => pi.Name))
            {
                builder.AppendLine(GetPropertyValueTextFor(obj, propertyInfo, nestedPropertyLevel + 1));
            }

            builder.AppendFormat("{0}}}", CreateWhitespaceForLevel(nestedPropertyLevel));

            return builder.ToString();
        }

        private string GetPropertyValueTextFor(object value, PropertyInfo propertyInfo, int nextPropertyNestingLevel)
        {
            object propertyValue = propertyInfo.GetValue(value, null);

            return string.Format("{0}{1} = {2}",
                CreateWhitespaceForLevel(nextPropertyNestingLevel),
                propertyInfo.Name,
                Formatter.ToString(propertyValue, nextPropertyNestingLevel));
        }

        private string CreateWhitespaceForLevel(int level)
        {
            return new string(' ', level * SpacesPerIndentionLevel);
        }
    }
}