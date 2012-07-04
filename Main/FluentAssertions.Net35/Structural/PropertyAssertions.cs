using System;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAssertions.Structural
{
    /// <summary>
    /// Provides methods for selecting one or more properties of an object and comparing them with another object.
    /// </summary>
    public class PropertyAssertions<T>
    {
        private readonly StructuralEqualityContext context = new StructuralEqualityContext();
        private T subject;

        internal protected PropertyAssertions(T subject)
        {
            if (ReferenceEquals(subject, null))
            {
                throw new NullReferenceException("Cannot compare the properties of a <null> object.");
            }

            this.subject = subject;
        }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="EqualTo(object)"/>.
        /// </summary>
        public PropertyAssertions<T> AllProperties()
        {
            context.Config.IncludeAllDeclaredProperties();
            return this;
        }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> including those of the run-time type when comparing the subject 
        /// with another object using <see cref="EqualTo(object)"/>.
        /// </summary>
        public PropertyAssertions<T> AllRuntimeProperties()
        {
            context.Config.IncludeAllRuntimeProperties();
            return this;
        }

        /// <summary>
        /// Includes all properties of <typeparamref name="T"/> when comparing the subject with another object using <see cref="EqualTo(object)"/>, 
        /// except those that the other object does not have.
        /// </summary>
        public PropertyAssertions<T> SharedProperties()
        {
            context.Config.IncludeAllDeclaredProperties();
            context.Config.TryMatchByName();
            return this;
        }

        /// <summary>
        /// Perform recursive property comparison of the child properties for objects that are of incompatible type.
        /// </summary>
        /// <param name="cyclicReferenceHandling">
        /// Indication of how cyclic references in the nested properties should be handled. By default this will result in an
        /// exception, but if <see cref="CyclicReferenceHandling.Ignore"/> is specified, cyclic references will just be ignored.
        /// </param>
        public PropertyAssertions<T> IncludingNestedObjects(
            CyclicReferenceHandling cyclicReferenceHandling = CyclicReferenceHandling.ThrowException)
        {
            context.Config.Recursive();

            if (cyclicReferenceHandling == CyclicReferenceHandling.Ignore)
            {
                context.Config.IgnoreCyclicReferences();
            }

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
            context.Config.AddRule(new AllDeclaredPublicPropertiesSelectionRule());

            foreach (var expression in propertyExpressions.Concat(new[] { propertyExpression }))
            {
                context.Config.Ignore(expression);
            }

            return this;
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
                context.Config.Ignore(expression);
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
                context.Config.Include(expression);
            }

            return this;
        }

        /// <summary>
        /// Asserts that the previously selected properties of <typeparamref name="T"/> have the same value as the equally named
        /// properties of <paramref name="expectation"/>.
        /// </summary>
        /// <param name="expectation">The object to compare the current object with</param>
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
        public void EqualTo(object expectation, string reason = "", params object[] reasonArgs)
        {
            context.Subject = subject;
            context.Expectation = expectation;
            context.CompileTimeType = typeof (T);
            context.Reason = reason;
            context.ReasonArgs = reasonArgs;
            new StructuralEqualityValidator().AssertEquality(context);
        }
    }
}
