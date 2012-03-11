using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Provides assertion methods for asserting that two collections are structurally equal.
    /// </summary>
    public class CollectionPropertyAssertions<T> : IPropertyAssertions<T>
    {
        #region Private Definitions

        private readonly IEnumerable subject;
        private readonly PropertyEqualityValidator validator = new PropertyEqualityValidator();

        #endregion

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
            validator.PropertySelection = PropertySelection.AllRuntimePublic;
            return this;
        }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="PropertyAssertions{T}.EqualTo(object)"/>, 
        /// except those that the other object does not have.
        /// </summary>
        public IPropertyAssertions<T> SharedProperties()
        {
            validator.PropertySelection = PropertySelection.OnlyShared;
            return this;
        }

        /// <summary>
        /// Perform recursive property comparison of the child properties for objects that are of incompatible type.
        /// </summary>
        /// <param name="cyclicReferenceHandling">
        /// Indication of how cyclic references in the nested properties should be handled. By default this will result in an
        /// exception, but if <see cref="CyclicReferenceHandling.Ignore"/> is specified, cyclic references will just be ignored.
        /// </param>
        public IPropertyAssertions<T> IncludingNestedObjects(
            CyclicReferenceHandling cyclicReferenceHandling = CyclicReferenceHandling.ThrowException)
        {
            validator.RecurseOnNestedObjects = true;
            return this;
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
            throw new NotSupportedException("Property selection on a collection is not supported");
        }

        /// <summary>
        /// Excludes the properties specified by the <paramref name="propertyExpression"/> from the comparison.
        /// </summary>
        /// <param name="propertyExpression">A single property expression to exclude.</param>
        /// <param name="propertyExpressions">Optional list of additional property expressions to exclude.</param>
        public IPropertyAssertions<T> But(Expression<Func<T, object>> propertyExpression,
            params Expression<Func<T, object>>[] propertyExpressions)
        {
            throw new NotSupportedException("Property selection on a collection is not supported");
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
            throw new NotSupportedException("Property selection on a collection is not supported");
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
            AssertExpectedObjectIsACollection(expected);

            object[] subjectItems = subject.Cast<object>().ToArray();
            object[] expectedItems = ((IEnumerable)expected).Cast<object>().ToArray();

            Execute.Verification
                .ForCondition(subjectItems.Length == expectedItems.Length)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected collection {0} to contain {1} item(s){reason}, but found {2}",
                    subjectItems, expectedItems.Length, subjectItems.Length);

            validator.Reason = reason;
            validator.ReasonArgs = reasonArgs;

            AssertAllItemsAreStructurallyEqual(expectedItems, subjectItems);
        }

        private static void AssertExpectedObjectIsACollection(object expected)
        {
            if (!(expected is IEnumerable) || (expected is string))
            {
                throw new ArgumentException("expected", "Cannot compare a collection with a non-collection.");
            }
        }

        private void AssertAllItemsAreStructurallyEqual(object[] expectedItems, object[] subjectItems)
        {
            for (int i = 0; i < subjectItems.Length; i++)
            {
                validator.CompileTimeType = subjectItems[i].GetType();
                validator.AssertEquality(subjectItems[i], expectedItems[i], "item[" + i + "]");
            }
        }
    }
}