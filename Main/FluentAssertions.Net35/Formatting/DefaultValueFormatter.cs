using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FluentAssertions.Formatting
{
    internal class DefaultValueFormatter : IValueFormatter
    {
        private static IList<object> referencedObjects;

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
        public string ToString(object value, int nestedPropertyLevel = 0)
        {
            if (value.GetType() == typeof(object))
            {
                return string.Format("System.Object (HashCode={0})", value.GetHashCode());
            }
            
            if (HasDefaultToStringImplementation(value))
            {
                if (nestedPropertyLevel == 0)
                {
                    InitializeCyclicReferenceDetection();
                }

                if (DetectCyclicReferenceFor(value))
                {
                    return string.Format("Cyclic reference detected for object of type {0}.", value.GetType());
                }

                return GetTypeAndPublicPropertyValues(value, nestedPropertyLevel);
            }

            return value.ToString();
        }

        private static void InitializeCyclicReferenceDetection()
        {
            referencedObjects = new List<object>();
        }

        private static bool DetectCyclicReferenceFor(object value)
        {
            if (referencedObjects.Contains(value))
            {
                return true;
            }

            referencedObjects.Add(value);

            return false;
        }

        private string GetTypeAndPublicPropertyValues(object obj, int nestedPropertyLevel)
        {
            Type type = obj.GetType();

            string currenLevelIndenting = GetIndentingSpacesForLevel(nestedPropertyLevel);

            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine(string.Format("{0}{1}", currenLevelIndenting, type.FullName));
            builder.AppendLine(string.Format("{0}{{", currenLevelIndenting));

            foreach (var propertyInfo in type.GetProperties().OrderBy(pi => pi.Name))
            {
                int nextPropertyNestingLevel = nestedPropertyLevel + 1;

                object propertyValue = propertyInfo.GetValue(obj, null);
                builder.AppendFormat("{0}{1} = {2}" + Environment.NewLine,
                    GetIndentingSpacesForLevel(nextPropertyNestingLevel),
                    propertyInfo.Name,
                    Formatter.ToString(propertyValue, nextPropertyNestingLevel));
            }

            builder.AppendFormat("{0}}}", currenLevelIndenting);

            return builder.ToString();
        }

        private string GetIndentingSpacesForLevel(int count)
        {
            var spaces = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                spaces.Append("   ");
            }

            return spaces.ToString();
        }

        private static bool HasDefaultToStringImplementation(object value)
        {
            return value.ToString().Equals(value.GetType().FullName);
        }
    }
}