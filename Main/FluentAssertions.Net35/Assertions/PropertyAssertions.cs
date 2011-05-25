using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    public class PropertyAssertions<T>
    {
        private const BindingFlags InstancePropertiesFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
        private readonly List<PropertyInfo> properties = new List<PropertyInfo>();
        private bool onlyShared = false;

        internal protected PropertyAssertions(T subject)
        {
            if (ReferenceEquals(subject, null))
            {
                throw new NullReferenceException("Cannot compare the properties of a <null> object.");
            }

            Subject = subject;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public T Subject { get; private set; }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="EqualTo(object)"/>.
        /// </summary>
        public PropertyAssertions<T> AllProperties()
        {
            foreach (var propertyInfo in typeof(T).GetProperties(InstancePropertiesFlag))
            {
                if (!propertyInfo.GetGetMethod(true).IsPrivate)
                {
                    properties.Add(propertyInfo);
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
                properties.Remove(properties.Single(p => p.Name == propertyToRemove.Name));
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
                properties.Add(expression.GetPropertyInfo());
            }

            return this;
        }

        /// <summary>
        /// Asserts that the previously selected properties of <typeparamref name="T"/> have the same value as the equally named
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
        /// Asserts that the previously selected properties of <typeparamref name="T"/> have the same value as the equally named
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
            
            CompareProperties(comparee, reason, reasonParameters);
        }

        private void CompareProperties(object comparee, string reason, object[] reasonArgs)
        {
            if (properties.Count == 0)
            {
                throw new InvalidOperationException("Please specify some properties to include in the comparison.");
            }

            foreach (var propertyInfo in properties)
            {
                CompareProperty(comparee, propertyInfo, reason, reasonArgs);
            }
        }

        private void CompareProperty(object comparee, PropertyInfo propertyInfo, string reason, object[] reasonArgs)
        {
            object actualValue = propertyInfo.GetValue(Subject, null);
            
            PropertyInfo compareeProperty = FindPropertyFrom(comparee, propertyInfo.Name);
            if (compareeProperty != null)
            {
                object expectedValue = compareeProperty.GetValue(comparee, null);

                actualValue = HarmonizeTypeDifferences(actualValue, expectedValue);

                if (!ReferenceEquals(actualValue, expectedValue))
                {
                    Verification.SubjectName = "property " + propertyInfo.Name;
                    try
                    {
                        VerifySemanticEquality(actualValue, expectedValue, reason, reasonArgs);
                    }
                    finally
                    {
                        Verification.SubjectName = null;
                    }
                }
            }
        }

        private static void VerifySemanticEquality(object subjectValue, object expectedValue, string reason, object[] reasonArgs)
        {

            if (subjectValue is string)
            {
                ((string)subjectValue).Should().Be((string) expectedValue, reason, reasonArgs);
            }
            else if (subjectValue is IEnumerable)
            {
                ((IEnumerable)subjectValue).Should().Equal(((IEnumerable)expectedValue), reason, reasonArgs);
            }
            else
            {
                subjectValue.Should().Be(expectedValue, reason, reasonArgs);
            }
        }

        private static object HarmonizeTypeDifferences(object subjectValue, object expectedValue)
        {
            if (!ReferenceEquals(subjectValue, null) && !ReferenceEquals(expectedValue, null) && (subjectValue.GetType() != expectedValue.GetType()))
            {
                subjectValue = Convert.ChangeType(subjectValue, expectedValue.GetType(), CultureInfo.CurrentCulture);
            }

            return subjectValue;
        }

        private PropertyInfo FindPropertyFrom(object comparee, string propertyName)
        {
            PropertyInfo compareeProperty = 
                comparee.GetType().GetProperties(InstancePropertiesFlag).SingleOrDefault(pi => pi.Name == propertyName);

            if (!onlyShared && (compareeProperty == null))
            {
                Execute.Verification.FailWith(
                    "Subject has property " + propertyName + " that the other object does not have.");    
            }

            return compareeProperty;
        }
    }
}