using System;
using System.Linq.Expressions;

namespace FluentAssertions.Assertions
{
    public interface IPropertyAssertions<T>
    {
        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="PropertyAssertions{T}.EqualTo(object)"/>.
        /// </summary>
        IPropertyAssertions<T> AllProperties();

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="PropertyAssertions{T}.EqualTo(object)"/>, 
        /// except those that the other object does not have.
        /// </summary>
        IPropertyAssertions<T> SharedProperties();

        /// <summary>
        /// Perform recursive property comparison of the child properties for objects that are of incompatible type.
        /// </summary>
        IPropertyAssertions<T> IncludingNestedObjects();

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="PropertyAssertions{T}.EqualTo(object)"/>, 
        /// except those specified using a property expression.
        /// </summary>
        /// <param name="propertyExpression">A single property expression to exclude.</param>
        /// <param name="propertyExpressions">Optional list of additional property expressions to exclude.</param>
        IPropertyAssertions<T> AllPropertiesBut(Expression<Func<T, object>> propertyExpression, params Expression<Func<T, object>>[] propertyExpressions);

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> including those of the run-time type when comparing the subject 
        /// with another object using <see cref="PropertyAssertions{T}.EqualTo(object)"/>.
        /// </summary>
        IPropertyAssertions<T> AllRuntimeProperties();

        /// <summary>
        /// Excludes the properties specified by the <paramref name="propertyExpression"/> from the comparison.
        /// </summary>
        /// <param name="propertyExpression">A single property expression to exclude.</param>
        /// <param name="propertyExpressions">Optional list of additional property expressions to exclude.</param>
        IPropertyAssertions<T> But(Expression<Func<T, object>> propertyExpression, params Expression<Func<T, object>>[] propertyExpressions);

        /// <summary>
        /// Includes only those properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="PropertyAssertions{T}.EqualTo(object)"/>
        /// that were specified using a property expression.
        /// </summary>
        /// <param name="propertyExpression">A single property expression to include.</param>
        /// <param name="propertyExpressions">Optional list of additional property expressions to include.</param>
        IPropertyAssertions<T> Properties(Expression<Func<T, object>> propertyExpression, params Expression<Func<T, object>>[] propertyExpressions);

        /// <summary>
        /// Asserts that the previously selected properties of <typeparamref name="T"/> have the same value as the equally named
        /// properties of <paramref name="expected"/>.
        /// </summary>
        /// <param name="expected">The object to compare the current object with</param>
        /// <remarks>
        /// Property values are considered equal if, after converting them to the requested type, calling <see cref="PropertyAssertions{T}.EqualTo(object)"/> 
        /// returns <c>true</c>.
        /// </remarks>
        void EqualTo(object expected);

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
        void EqualTo(object expected, string reason, params object[] reasonArgs);
    }
}