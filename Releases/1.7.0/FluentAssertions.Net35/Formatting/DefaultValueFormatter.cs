using System;
using System.Linq;
using System.Reflection;
using System.Text;

using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    internal class DefaultValueFormatter : IValueFormatter
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
        /// <param name="uniqueObjectTracker">
        /// An object that is passed through recursive calls and which should be used to detect circular references
        /// in the object graph that is being converted to a string representation.</param>
        /// <param name="nestedPropertyLevel">
        ///     The level of nesting for the supplied value. This is used for indenting the format string for objects that have
        ///     no <see cref="object.ToString()"/> override.
        /// </param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(object value, UniqueObjectTracker uniqueObjectTracker, int nestedPropertyLevel = 0)
        {
            if (value.GetType() == typeof (object))
            {
                return string.Format("System.Object (HashCode={0})", value.GetHashCode());
            }

            if (HasDefaultToStringImplementation(value))
            {
                try
                {
                    uniqueObjectTracker.Track(value);
                }
                catch (ObjectAlreadyTrackedException)
                {
                    return string.Format("{{Cyclic reference to type {0} detected}}", value.GetType());
                }

                return GetTypeAndPublicPropertyValues(value, nestedPropertyLevel, uniqueObjectTracker);
            }

            return value.ToString();
        }

        private static bool HasDefaultToStringImplementation(object value)
        {
            return value.ToString().Equals(value.GetType().FullName);
        }

        private string GetTypeAndPublicPropertyValues(object obj, int nestedPropertyLevel, UniqueObjectTracker uniqueObjectTracker)
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

            foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(pi => pi.Name))
            {
                builder.AppendLine(GetPropertyValueTextFor(obj, propertyInfo, nestedPropertyLevel + 1, uniqueObjectTracker));
            }

            builder.AppendFormat("{0}}}", CreateWhitespaceForLevel(nestedPropertyLevel));

            return builder.ToString();
        }

        private string GetPropertyValueTextFor(object value, PropertyInfo propertyInfo, int nextPropertyNestingLevel, UniqueObjectTracker uniqueObjectTracker)
        {
            object propertyValue;

            try
            {
                propertyValue = propertyInfo.GetValue(value, null);
            }
            catch(Exception ex)
            {
                propertyValue = string.Format("[Property '{0}' threw an exception: '{1}']", propertyInfo.Name, ex.Message);
            }

            return string.Format("{0}{1} = {2}",
                CreateWhitespaceForLevel(nextPropertyNestingLevel),
                propertyInfo.Name,
                Formatter.ToString(propertyValue, uniqueObjectTracker, nextPropertyNestingLevel));
        }

        private string CreateWhitespaceForLevel(int level)
        {
            return new string(' ', level * SpacesPerIndentionLevel);
        }
    }
}