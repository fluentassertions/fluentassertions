using System;
using System.Text;
using System.Linq;

namespace FluentAssertions.Formatting
{
    internal class DefaultValueFormatter : IValueFormatter
    {
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
        ///   Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>
        ///   A <see cref = "System.String" /> that represents this instance.
        /// </returns>
        public string ToString(object value)
        {
            if (value.GetType() == typeof(object))
            {
                return string.Format("System.Object (HashCode={0})", value.GetHashCode());
            }
            else if (HasDefaultToStringImplementation(value))
            {
                return GetTypeAndPublicPropertyValues(value);
            }
            else
            {
                return value.ToString();
            }
        }

        private string GetTypeAndPublicPropertyValues(object obj)
        {
            Type type = obj.GetType();

            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine(type.FullName);
            builder.AppendLine("{");

            foreach (var propertyInfo in type.GetProperties().OrderBy(pi => pi.Name))
            {
                object propertyValue = propertyInfo.GetValue(obj, null);
                builder.AppendFormat("    {0} = {1}" + Environment.NewLine, propertyInfo.Name, Formatter.ToString(propertyValue));
            }

            builder.Append("}");

            return builder.ToString();
        }

        private static bool HasDefaultToStringImplementation(object value)
        {
            return value.ToString().Equals(value.GetType().FullName);
        }
    }
}