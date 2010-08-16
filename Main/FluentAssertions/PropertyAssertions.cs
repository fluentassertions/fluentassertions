using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentAssertions
{
    public class PropertyAssertions<T> : AssertionsBase<T>
    {
        private readonly List<PropertyInfo> selectedPropertyInfos = new List<PropertyInfo>();

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
            selectedPropertyInfos.AddRange(typeof (T).GetProperties(BindingFlags.Public | BindingFlags.Instance));

            return this;
        }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="EqualTo(object)"/>, 
        /// except those specified using a property expression.
        /// </summary>
        public PropertyAssertions<T> AllPropertiesBut(Expression<Func<T, object>> propertyExpression, params Expression<Func<T, object>>[] propertyExpressions)
        {
            AllProperties();

            foreach (var expression in propertyExpressions.Concat(new[] { propertyExpression }))
            {
                selectedPropertyInfos.Remove(GetPropertyInfo(expression));
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
                selectedPropertyInfos.Add(GetPropertyInfo(expression));
            }

            return this;
        }

        private static PropertyInfo GetPropertyInfo(Expression<Func<T, object>> expression)
        {
            if (ReferenceEquals(expression, null))
            {
                throw new NullReferenceException("Expected a property expression, but found <null>.");
            }

            PropertyInfo propertyInfo = AttemptToGetPropertyInfoFromCastExpression(expression);
            if (propertyInfo == null)
            {
                propertyInfo = AttemptToGetPropertyInfoFromPropertyExpression(expression);
            }

            if (propertyInfo == null)
            {
                throw new ArgumentException("Cannot use <" + expression.Body + "> when a property expression is expected.");
            }

            return propertyInfo;
        }

        private static PropertyInfo AttemptToGetPropertyInfoFromPropertyExpression(Expression<Func<T, object>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                return (PropertyInfo) memberExpression.Member;
            }
                
            return null;
        }

        private static PropertyInfo AttemptToGetPropertyInfoFromCastExpression(Expression<Func<T, object>> expression)
        {
            UnaryExpression castExpression = expression.Body as UnaryExpression;
            if (castExpression != null)
            {
                return (PropertyInfo) ((MemberExpression) castExpression.Operand).Member;
            }

            return null;
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

            if (selectedPropertyInfos.Count == 0)
            {
                throw new InvalidOperationException("Please specify some properties to include in the comparison.");
            }

            foreach (var propertyInfo in selectedPropertyInfos)
            {
                object subjectValue = propertyInfo.GetValue(Subject, null);
                object expectedValue = GetPropertyValueFrom(comparee, propertyInfo.Name, reason, reasonParameters);

                if (!AreEqualOrConvertable(subjectValue, expectedValue))
                {
                    FailWith("Expected property " + propertyInfo.Name + " to have value {0}{2}, but found {1}.",
                        expectedValue, subjectValue, reason, reasonParameters);
                } 
            }
        }

        private object GetPropertyValueFrom(object comparee, string propertyName, string reason, object[] reasonParameters)
        {
            PropertyInfo[] compareeProperties = comparee.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            PropertyInfo compareeProperty = compareeProperties.SingleOrDefault(pi => pi.Name == propertyName);
            if (compareeProperty == null)
            {
                FailWith("Subject has property " + propertyName + " that is not available in comparee.", null, null, reason, reasonParameters);    
            }

            return compareeProperty.GetValue(comparee, null);
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