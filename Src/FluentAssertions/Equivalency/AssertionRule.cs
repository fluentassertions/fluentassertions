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
    internal class AssertionRule<TSubject> : IAssertionRule
    {
        private readonly Func<IMemberInfo, bool> predicate;
        private readonly Action<IAssertionContext<TSubject>> action;
        private readonly string description;

        public AssertionRule(Expression<Func<IMemberInfo, bool>> predicate, Action<IAssertionContext<TSubject>> action)
        {
            this.predicate = predicate.Compile();
            this.action = action;
            description = "Invoke Action<" + typeof(TSubject).Name + "> when " + predicate.Body;
        }

        /// <summary>
        /// Defines how a subject's property is compared for equality with the same property of the expectation.
        /// </summary>
        /// <returns>
        /// Returns <c>true</c> if the rule was applied correctly and the assertion didn't cause any exceptions.
        /// Returns <c>false</c> if this rule doesn't support the subject's type.
        /// Throws if the rule did support the data type but assertion fails.
        /// </returns>
        public bool AssertEquality(IEquivalencyValidationContext context)
        {
            if (predicate(context))
            {
                bool subjectIsNull = context.Subject is null;

                bool subjectIsValidType =
                    AssertionScope.Current
                        .ForCondition(subjectIsNull || context.Subject.GetType().IsSameOrInherits(typeof(TSubject)))
                        .FailWith("Expected " + context.SelectedMemberDescription + " from subject to be a {0}{reason}, but found a {1}.",
                            typeof(TSubject), context.Subject?.GetType());

                bool expectationIsNull = context.Expectation is null;

                bool expectationIsValidType =
                    AssertionScope.Current
                        .ForCondition(expectationIsNull || context.Expectation.GetType().IsSameOrInherits(typeof(TSubject)))
                        .FailWith("Expected " + context.SelectedMemberDescription + " from expectation to be a {0}{reason}, but found a {1}.",
                            typeof(TSubject), context.Expectation?.GetType());

                if (subjectIsValidType && expectationIsValidType)
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
