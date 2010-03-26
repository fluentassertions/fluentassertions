using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions
{
    public class PropertyAssertions<T> : AssertionsBase<T>
    {
        private readonly List<PropertyInfo> properties = new List<PropertyInfo>();

        public PropertyAssertions(T subject)
        {
            Subject = subject;
        }

        public PropertyAssertions<T> AllProperties
        {
            get
            {
                properties.AddRange(typeof (T).GetProperties(BindingFlags.Public | BindingFlags.Instance));

                return this;
            }
        }

        public void EqualTo(object comparee)
        {
            EqualTo(comparee, string.Empty);
        }

        public void EqualTo(object comparee, string reason, params object[] reasonParameters)
        {
            PropertyInfo[] compareeProperties = comparee.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertyInfo in properties)
            {
                object subjectValue = propertyInfo.GetValue(Subject, null);

                PropertyInfo compareeProperty = compareeProperties.Single(pi => pi.Name == propertyInfo.Name);
                object expectedValue = compareeProperty.GetValue(comparee, null);

                if (!AreEqual(subjectValue, expectedValue))
                {
                    FailWith("Expected property " + propertyInfo.Name + " to have value {0}{2}, but found {1}.",
                        expectedValue, subjectValue, reason, reasonParameters);
                } 
            }
        }

        private static bool AreEqual(object subjectValue, object expectedValue)
        {
            if (ReferenceEquals(subjectValue, null) && ReferenceEquals(expectedValue, null))
            {
                return true;
            }

            if (!ReferenceEquals(subjectValue, null))
            {
                if (subjectValue.Equals(expectedValue))
                {
                    return true;
                }

                if (Convert.ChangeType(subjectValue, expectedValue.GetType()).Equals(expectedValue))
                {
                    return true;
                }
            }

            return false;
        }
    }
}