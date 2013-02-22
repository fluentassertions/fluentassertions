using System;
using System.Reflection;

using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// General purpose implementation of <see cref="IAssertionRule"/> that uses a predicate to determine whether
    /// this rule applies to a particular property and executes an action to assert equality.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    public class AssertionRule<TSubject> : IAssertionRule
    {
        private readonly Func<ISubjectInfo, bool> predicate;
        private readonly Action<IAssertionContext<TSubject>> action;

        public AssertionRule(Func<ISubjectInfo, bool> predicate, Action<IAssertionContext<TSubject>> action)
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
        public bool AssertEquality(IEquivalencyValidationContext context)
        {
            if (predicate(context))
            {
                bool succeeded = context.Verification
                    .ForCondition(context.MatchingExpectationProperty.PropertyType.IsSameOrInherits(typeof(TSubject)))
                    .FailWith("Expected " + context.PropertyDescription + " to be a {0}{reason}, but found a {1}",
                        context.MatchingExpectationProperty.PropertyType, context.PropertyInfo.PropertyType);

                if (succeeded)
                {
                    action(new AssertionContext(context.PropertyInfo,
                        (TSubject)context.Subject, (TSubject)context.Expectation, context.Reason, context.ReasonArgs));
                }

                return true;
            }

            return false;
        }

        internal class AssertionContext : IAssertionContext<TSubject>
        {
            public AssertionContext(PropertyInfo subjectProperty, TSubject subject, TSubject expectation, string reason,
                object[] reasonArgs)
            {
                SubjectProperty = subjectProperty;
                Subject = subject;
                Expectation = expectation;
                Reason = reason;
                ReasonArgs = reasonArgs;
            }

            public PropertyInfo SubjectProperty { get; private set; }
            public TSubject Subject { get; private set; }
            public TSubject Expectation { get; private set; }
            public string Reason { get; set; }
            public object[] ReasonArgs { get; set; }
        }
    }
}