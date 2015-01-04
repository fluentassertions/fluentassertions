using System;
using System.Linq.Expressions;

using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// General purpose implementation of <see cref="IAssertionRule"/> that uses a predicate to determine whether
    /// this rule applies to a particular property and executes an action to assert equality.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    [Obsolete("This class will be removed in a future version.  Use `EquivalencyAssertionOptions.Using(Action<IAssertionContext<TProperty>>)` from the Fluent API instead.")]
    public class AssertionRule<TSubject> : IAssertionRule
    {
        private readonly Func<ISubjectInfo, bool> predicate;
        private readonly Action<IAssertionContext<TSubject>> action;
        private readonly string description;

        public AssertionRule(Expression<Func<ISubjectInfo, bool>> predicate, Action<IAssertionContext<TSubject>> action)
        {
            this.predicate = predicate.Compile();
            this.action = action;
            description = "Invoke Action<" + typeof(TSubject).Name + "> when " + predicate.Body;
        }

        public AssertionRule(Action<IAssertionContext<TSubject>> action)
        {
            Expression<Func<ISubjectInfo, bool>> predicate =
                info => info.RuntimeType.IsSameOrInherits(typeof (string));

            this.predicate = predicate.Compile();
            this.action = action;
            description = "Invoke Action<" + typeof(TSubject).Name + "> when " + predicate.Body;
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
                bool expectationisNull = ReferenceEquals(context.Expectation, null);

                bool succeeded =
                    AssertionScope.Current
                        .ForCondition(expectationisNull || context.Expectation.GetType().IsSameOrInherits(typeof(TSubject)))
                        .FailWith("Expected " + context.SelectedMemberDescription + " to be a {0}{reason}, but found a {1}",
                            !expectationisNull ? context.Expectation.GetType() : null, context.SelectedMemberInfo.MemberType);

                if (succeeded)
                {
                    action(AssertionContext<TSubject>.CreateFromEquivalencyValidationContext(context));
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return description;
        }
    }
}