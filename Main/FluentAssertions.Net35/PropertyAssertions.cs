using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using FluentAssertions.Common;

namespace FluentAssertions
{
    public class PropertyAssertions<T> : Assertions<T, PropertyAssertions<T>>
    {
        private const BindingFlags InstancePropertiesFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
        private readonly List<PropertyInfo> selectedSubjectProperties = new List<PropertyInfo>();
        private bool onlyShared = false;

        internal protected PropertyAssertions(T subject)
        {
            if (ReferenceEquals(subject, null))
            {
                throw new NullReferenceException("Cannot compare the properties of a <null> object");
            }

            Subject = subject;
        }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="EqualTo(object)"/>.
        /// </summary>
        public PropertyAssertions<T> AllProperties()
        {
            foreach (var propertyInfo in typeof(T).GetProperties(InstancePropertiesFlag))
            {
                if (!propertyInfo.GetGetMethod(true).IsPrivate)
                {
                    selectedSubjectProperties.Add(propertyInfo);
                }
            }

            return this;
        }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="EqualTo(object)"/>, 
        /// except those that the other object does not have.
        /// </summary>
        public PropertyAssertions<T> SharedProperties()
        {
            onlyShared = true;
            return AllProperties();
        }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="EqualTo(object)"/>, 
        /// except those specified using a property expression.
        /// </summary>
        public PropertyAssertions<T> AllPropertiesBut(Expression<Func<T, object>> propertyExpression, params Expression<Func<T, object>>[] propertyExpressions)
        {
            return AllProperties().But(propertyExpression, propertyExpressions); ;
        }

        public PropertyAssertions<T> But(Expression<Func<T, object>> propertyExpression, params Expression<Func<T, object>>[] propertyExpressions)
        {
            foreach (var expression in propertyExpressions.Concat(new[] { propertyExpression }))
            {
                PropertyInfo propertyToRemove = expression.GetPropertyInfo();
                selectedSubjectProperties.Remove(selectedSubjectProperties.Single(p => p.Name == propertyToRemove.Name));
            }

            return this;
        }

        /// <summary>
        /// Includes only those properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="EqualTo(object)"/>
        /// that were specified using a property expression.
        /// </summary>
        public PropertyAssertions<T> Properties(Expression<Func<T, object>> propertyExpression, params Expression<Func<T, object>>[] propertyExpressions)
        {
            foreach (var expression in propertyExpressions.Concat(new[] { propertyExpression }))
            {
                selectedSubjectProperties.Add(expression.GetPropertyInfo());
            }

            return this;
        }

        /// <summary>
        /// Verifies that the previously selected properties of <typeparamref name="T"/> have the same value as the equally named
        /// properties of <paramref name="comparee"/>.
        /// </summary>
        /// <remarks>
        /// Property values are considered equal if, after converting them to the requested type, calling <see cref="T.Equals(object)"/> 
        /// returns <c>true</c>.
        /// </remarks>
        public void EqualTo(object comparee)
        {
            EqualTo(comparee, string.Empty);
        }

        /// <summary>
        /// Verifies that the previously selected properties of <typeparamref name="T"/> have the same value as the equally named
        /// properties of <paramref name="comparee"/>.
        /// </summary>
        /// <remarks>
        /// Property values are considered equal if, after converting them to the requested type, calling <see cref="T.Equals(object)"/> 
        /// returns <c>true</c>.
        /// </remarks>
        public void EqualTo(object comparee, string reason, params object[] reasonParameters)
        {
            if (ReferenceEquals(comparee, null))
            {
                throw new NullReferenceException("Cannot compare subject's properties with a <null> object.");
            }

            if (selectedSubjectProperties.Count == 0)
            {
                throw new InvalidOperationException("Please specify some properties to include in the comparison.");
            }

            CompareProperties(comparee, reason, reasonParameters);
        }

        private void CompareProperties(object comparee, string reason, object[] reasonParameters)
        {
            foreach (var propertyInfo in selectedSubjectProperties)
            {
                object subjectValue = propertyInfo.GetValue(Subject, null);

                PropertyInfo compareeProperty = FindPropertyFrom(comparee, propertyInfo.Name, reason, reasonParameters);
                if (compareeProperty != null)
                {
                    object expectedValue = compareeProperty.GetValue(comparee, null);

                    if (!AreEqualOrConvertable(subjectValue, expectedValue))
                    {
                        Verification.Fail(
                            "Expected property " + propertyInfo.Name + " to have value {0}{2}, but found {1}.",
                            expectedValue, subjectValue, reason, reasonParameters);
                    }
                }
            }
        }

        private PropertyInfo FindPropertyFrom(object comparee, string propertyName, string reason, object[] reasonParameters)
        {
            PropertyInfo compareeProperty = 
                comparee.GetType().GetProperties(InstancePropertiesFlag).SingleOrDefault(pi => pi.Name == propertyName);

            if (!onlyShared && (compareeProperty == null))
            {
                Verification.Fail("Subject has property " + propertyName + " that is not available in comparee.", null, null, reason, reasonParameters);    
            }

            return compareeProperty;
        }

        private static bool AreEqualOrConvertable(object subjectValue, object expectedValue)
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

                if (ReferenceEquals(expectedValue, null))
                {
                    return false;
                }

                if (Convert.ChangeType(subjectValue, expectedValue.GetType(), CultureInfo.CurrentCulture).Equals(expectedValue))
                {
                    return true;
                }
            }

            return false;
        }
    }
}