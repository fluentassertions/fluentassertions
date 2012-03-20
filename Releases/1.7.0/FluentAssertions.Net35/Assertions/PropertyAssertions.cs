using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Provides methods for selecting one or more properties of an object and comparing them with another object.
    /// </summary>
    public class PropertyAssertions<T>
    {
        private const BindingFlags InstancePropertiesFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
        private readonly PropertyEqualityValidator validator;

        internal protected PropertyAssertions(T subject)
        {
            if (ReferenceEquals(subject, null))
            {
                throw new NullReferenceException("Cannot compare the properties of a <null> object.");
            }

            Subject = subject;
            validator = new PropertyEqualityValidator(subject);
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
            AddPublicProperties(typeof (T));

            return this;
        }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> including those of the run-time type when comparing the subject 
        /// with another object using <see cref="EqualTo(object)"/>.
        /// </summary>
        internal PropertyAssertions<T> AllRuntimeProperties()
        {
            AddPublicProperties(Subject.GetType());

            return this;
        }

        private void AddPublicProperties(Type typeToReflect)
        {
            foreach (var propertyInfo in typeToReflect.GetProperties(InstancePropertiesFlag))
            {
                var getter = propertyInfo.GetGetMethod(true);
                if ((getter != null) && !getter.IsPrivate)
                {
                    validator.Properties.Add(propertyInfo);
                }
            }
        }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="EqualTo(object)"/>, 
        /// except those that the other object does not have.
        /// </summary>
        public PropertyAssertions<T> SharedProperties()
        {
            validator.OnlySharedProperties = true;
            return AllProperties();
        }

        /// <summary>
        /// Perform recursive property comparison of the child properties for objects that are of incompatible type.
        /// </summary>
        /// <param name="ignore"> </param>
        public PropertyAssertions<T> IncludingNestedObjects(CyclicReferenceHandling cyclicReferenceHandling = CyclicReferenceHandling.ThrowException)
        {
            validator.RecurseOnNestedObjects = true;
            validator.CyclicReferenceHandling = cyclicReferenceHandling;
            return this;
        }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="EqualTo(object)"/>, 
        /// except those specified using a property expression.
        /// </summary>
        /// <param name="propertyExpression">A single property expression to exclude.</param>
        /// <param name="propertyExpressions">Optional list of additional property expressions to exclude.</param>
        public PropertyAssertions<T> AllPropertiesBut(Expression<Func<T, object>> propertyExpression, params Expression<Func<T, object>>[] propertyExpressions)
        {
            return AllProperties().But(propertyExpression, propertyExpressions);
        }

        /// <summary>
        /// Excludes the properties specified by the <paramref name="propertyExpression"/> from the comparison.
        /// </summary>
        /// <param name="propertyExpression">A single property expression to exclude.</param>
        /// <param name="propertyExpressions">Optional list of additional property expressions to exclude.</param>
        public PropertyAssertions<T> But(Expression<Func<T, object>> propertyExpression, params Expression<Func<T, object>>[] propertyExpressions)
        {
            foreach (var expression in propertyExpressions.Concat(new[] { propertyExpression }))
            {
                PropertyInfo propertyToRemove = expression.GetPropertyInfo();
                validator.Properties.Remove(validator.Properties.Single(p => p.Name == propertyToRemove.Name));
            }

            return this;
        }

        /// <summary>
        /// Includes only those properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="EqualTo(object)"/>
        /// that were specified using a property expression.
        /// </summary>
        /// <param name="propertyExpression">A single property expression to include.</param>
        /// <param name="propertyExpressions">Optional list of additional property expressions to include.</param>
        public PropertyAssertions<T> Properties(Expression<Func<T, object>> propertyExpression, params Expression<Func<T, object>>[] propertyExpressions)
        {
            foreach (var expression in propertyExpressions.Concat(new[] { propertyExpression }))
            {
                validator.Properties.Add(expression.GetPropertyInfo());
            }

            return this;
        }

        /// <summary>
        /// Asserts that the previously selected properties of <typeparamref name="T"/> have the same value as the equally named
        /// properties of <paramref name="otherObject"/>.
        /// </summary>
        /// <param name="otherObject">The object to compare the current object with</param>
        /// <remarks>
        /// Property values are considered equal if, after converting them to the requested type, calling <see cref="EqualTo(object)"/> 
        /// returns <c>true</c>.
        /// </remarks>
        public void EqualTo(object otherObject)
        {
            EqualTo(otherObject, string.Empty);
        }

        /// <summary>
        /// Asserts that the previously selected properties of <typeparamref name="T"/> have the same value as the equally named
        /// properties of <paramref name="otherObject"/>.
        /// </summary>
        /// <param name="otherObject">The object to compare the current object with</param>
        /// <remarks>
        /// Property values are considered equal if, after converting them to the requested type, calling <see cref="EqualTo(object)"/> 
        /// returns <c>true</c>.
        /// </remarks>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        public void EqualTo(object otherObject, string reason, params object[] reasonArgs)
        {
            validator.OtherObject = otherObject;
            validator.Reason = reason;
            validator.ReasonArgs = reasonArgs;
            validator.Validate();
        }
    }
}