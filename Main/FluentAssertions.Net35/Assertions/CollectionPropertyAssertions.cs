using System;
using System.Collections;
using System.Linq.Expressions;

using System.Linq;

namespace FluentAssertions.Assertions
{
    public class CollectionPropertyAssertions<T> : IPropertyAssertions<T>
    {
        private readonly IEnumerable subject;
        PropertyEqualityValidator validator = new PropertyEqualityValidator();

        public CollectionPropertyAssertions(IEnumerable subject)
        {
            this.subject = subject;
        }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="PropertyAssertions{T}.EqualTo(object)"/>.
        /// </summary>
        public IPropertyAssertions<T> AllProperties()
        {
            validator.PropertySelection = PropertySelection.AllRuntimePublic;
            return this;
        }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> including those of the run-time type when comparing the subject 
        /// with another object using <see cref="PropertyAssertions{T}.EqualTo(object)"/>.
        /// </summary>
        public IPropertyAssertions<T> AllRuntimeProperties()
        {
            return null;
        }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="PropertyAssertions{T}.EqualTo(object)"/>, 
        /// except those that the other object does not have.
        /// </summary>
        public IPropertyAssertions<T> SharedProperties()
        {
            validator.PropertySelection = PropertySelection.OnlyShared;
            return null;
        }

        /// <summary>
        /// Perform recursive property comparison of the child properties for objects that are of incompatible type.
        /// </summary>
        public IPropertyAssertions<T> IncludingNestedObjects()
        {
            validator.RecurseOnNestedObjects = true;
            return null;
        }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="PropertyAssertions{T}.EqualTo(object)"/>, 
        /// except those specified using a property expression.
        /// </summary>
        /// <param name="propertyExpression">A single property expression to exclude.</param>
        /// <param name="propertyExpressions">Optional list of additional property expressions to exclude.</param>
        public IPropertyAssertions<T> AllPropertiesBut(Expression<Func<T, object>> propertyExpression,
            params Expression<Func<T, object>>[] propertyExpressions)
        {
            return null;
        }

        /// <summary>
        /// Excludes the properties specified by the <paramref name="propertyExpression"/> from the comparison.
        /// </summary>
        /// <param name="propertyExpression">A single property expression to exclude.</param>
        /// <param name="propertyExpressions">Optional list of additional property expressions to exclude.</param>
        public IPropertyAssertions<T> But(Expression<Func<T, object>> propertyExpression,
            params Expression<Func<T, object>>[] propertyExpressions)
        {
            return null;
        }

        /// <summary>
        /// Includes only those properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="PropertyAssertions{T}.EqualTo(object)"/>
        /// that were specified using a property expression.
        /// </summary>
        /// <param name="propertyExpression">A single property expression to include.</param>
        /// <param name="propertyExpressions">Optional list of additional property expressions to include.</param>
        public IPropertyAssertions<T> Properties(Expression<Func<T, object>> propertyExpression,
            params Expression<Func<T, object>>[] propertyExpressions)
        {
            return null;
        }

        /// <summary>
        /// Asserts that the previously selected properties of <typeparamref name="T"/> have the same value as the equally named
        /// properties of <paramref name="expected"/>.
        /// </summary>
        /// <param name="expected">The object to compare the current object with</param>
        /// <remarks>
        /// Property values are considered equal if, after converting them to the requested type, calling <see cref="PropertyAssertions{T}.EqualTo(object)"/> 
        /// returns <c>true</c>.
        /// </remarks>
        public void EqualTo(object expected)
        {
            EqualTo(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the previously selected properties of <typeparamref name="T"/> have the same value as the equally named
        /// properties of <paramref name="expected"/>.
        /// </summary>
        /// <param name="expected">The object to compare the current object with</param>
        /// <remarks>
        /// Property values are considered equal if, after converting them to the requested type, calling <see cref="PropertyAssertions{T}.EqualTo(object)"/> 
        /// returns <c>true</c>.
        /// </remarks>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        public void EqualTo(object expected, string reason, params object[] reasonArgs)
        {
            var subjectItems = subject.Cast<object>().ToArray();
            var expectedItems = ((IEnumerable)expected).Cast<object>().ToArray();

            for (int i = 0; i < subjectItems.Length; i++)
            {
                validator.CompileTimeType = subjectItems[i].GetType();
                validator.Reason = reason;
                validator.ReasonArgs = reasonArgs;
                validator.AssertEquality(subjectItems[i], expectedItems[i], "item[" + i + "]");
            }
        }
    }
}