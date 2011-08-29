using System;
using System.Linq;
using System.Reflection;
using System.Text;

using FluentAssertions.Assertions;

namespace FluentAssertions.Formatting
{
    internal class DefaultValueFormatter : IValueFormatter
    {
        private const int RootLevel = 0;
        private readonly CyclicReferenceTracker cyclicReferenceTracker = new CyclicReferenceTracker();

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
                    AssertNoCyclicReferenceFor(value, nestedPropertyLevel);
                }
                catch (CyclicReferenceInRecursionException)
                {
                    return string.Format("Cyclic reference detected for object of type {0}.", value.GetType());
                }

                return GetTypeAndPublicPropertyValues(value, nestedPropertyLevel);
            }

            return value.ToString();
        }

        private void AssertNoCyclicReferenceFor(object value, int nestedPropertyLevel)
        {
            if (nestedPropertyLevel == RootLevel)
            {
                cyclicReferenceTracker.Initialize();
            }

            cyclicReferenceTracker.AssertNoCyclicReferenceFor(value);
        }

        private string GetTypeAndPublicPropertyValues(object obj, int nestedPropertyLevel)
        {
            Type type = obj.GetType();

            string currenLevelIndenting = GetIndentingSpacesForLevel(nestedPropertyLevel);

            var builder = new StringBuilder();
            if (nestedPropertyLevel == RootLevel)
            {
                builder.AppendLine();
                builder.AppendLine();
            }
            builder.AppendLine(string.Format("{0}", type.FullName));
            builder.AppendLine(string.Format("{0}{{", currenLevelIndenting));

            foreach (var propertyInfo in type.GetProperties().OrderBy(pi => pi.Name))
            {
                var propertyValueText = GetPropertyValueTextFor(obj, propertyInfo, nestedPropertyLevel + 1);
                builder.AppendLine(propertyValueText);
            }

            builder.AppendFormat("{0}}}", currenLevelIndenting);

            return builder.ToString();
        }

        private string GetPropertyValueTextFor(object value, PropertyInfo propertyInfo, int nextPropertyNestingLevel)
        {
            object propertyValue = propertyInfo.GetValue(value, null);

            return string.Format("{0}{1} = {2}",
                GetIndentingSpacesForLevel(nextPropertyNestingLevel),
                propertyInfo.Name,
                Formatter.ToString(propertyValue, nextPropertyNestingLevel));
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