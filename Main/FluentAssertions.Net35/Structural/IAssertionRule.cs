using System;
using System.Reflection;

namespace FluentAssertions.Structural
{
    public interface IAssertionRule
    {
        /// <summary>
        /// Defines how a subject's property is compared for equality with the same property of the expectation.
        /// </summary>
        /// <param name="subjectProperty">
        /// Provides details about the subject's property.
        /// </param>
        /// <param name="subject">
        /// The value of the subject's property.
        /// </param>
        /// <param name="expectation">
        /// The value of a property on expectation object that was identified 
        /// </param>
        /// <returns>
        /// Returns <c>true</c> if the rule was applied correctly and the assertion didn't cause any exceptions. 
        /// Returns <c>false</c> if this rule doesn't support the subject's type.
        /// Throws if the rule did support the data type but assertion fails.
        /// </returns>
        bool AssertEquality(AssertionContext context);
    }

    public class AssertionContext
    {
        public AssertionContext(PropertyInfo subjectProperty, object subject, object expectation, 
            string reason, object[] reasonArgs)
        {
            SubjectProperty = subjectProperty;
            Subject = subject;
            Expectation = expectation;
            Reason = reason;
            ReasonArgs = reasonArgs;
        }

        public PropertyInfo SubjectProperty { get; private set; }

        public object Subject { get; private set; }

        public object Expectation { get; private set; }
        public string Reason { get; set; }
        public object[] ReasonArgs { get; set; }
    }

    public class AssertionRule : IAssertionRule
    {
        private readonly Func<PropertyInfo, bool> predicate;
        private readonly Action<AssertionContext> action;

        public AssertionRule(Func<PropertyInfo, bool> predicate, Action<AssertionContext> action)
        {
            this.predicate = predicate;
            this.action = action;
        }

        /// <summary>
        /// Defines how a subject's property is compared for equality with the same property of the expectation.
        /// </summary>
        /// <param name="subjectProperty">
        /// Provides details about the subject's property.
        /// </param>
        /// <param name="subject">
        /// The value of the subject's property.
        /// </param>
        /// <param name="expectation">
        /// The value of a property on expectation object that was identified 
        /// </param>
        /// <returns>
        /// Returns <c>true</c> if the rule was applied correctly and the assertion didn't cause any exceptions. 
        /// Returns <c>false</c> if this rule doesn't support the subject's type.
        /// Throws if the rule did support the data type but assertion fails.
        /// </returns>
        public bool AssertEquality(AssertionContext context)
        {
            if (predicate(context.SubjectProperty))
            {
                action(context);

                return true;
            }

            return false;
        }
    }
}