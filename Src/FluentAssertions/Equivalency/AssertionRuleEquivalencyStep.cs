using System;
using System.Linq.Expressions;

using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class AssertionRuleEquivalencyStep<TSubject> : IEquivalencyStep
    {
        private readonly Expression<Func<ISubjectInfo, bool>> canHandle;

        private readonly Action<IAssertionContext<TSubject>> handle;

        public AssertionRuleEquivalencyStep(Expression<Func<ISubjectInfo, bool>> predicate, Action<IAssertionContext<TSubject>> action)
        {
            canHandle = predicate;
            handle = action;
        }

        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Func<ISubjectInfo, bool> predicate = canHandle.Compile();

            return ((context.SelectedMemberInfo != null) && predicate(context));
        }

        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            bool subjectIsNull = ReferenceEquals(context.Subject, null);

            bool subjectIsValidType =
                AssertionScope.Current
                    .ForCondition(context.Subject.GetType().IsSameOrInherits(typeof(TSubject)))
                    .FailWith("Expected " + context.SelectedMemberDescription + " from subject to be a {0}{reason}, but found a {1}.",
                        typeof(TSubject), context.Subject?.GetType());

            bool expectationIsNull = ReferenceEquals(context.Expectation, null);

            bool expectationIsValidType =
                AssertionScope.Current
                    .ForCondition(expectationIsNull || context.Expectation.GetType().IsSameOrInherits(typeof(TSubject)))
                    .FailWith("Expected " + context.SelectedMemberDescription + " from expectation to be a {0}{reason}, but found a {1}.",
                        typeof(TSubject), context.SelectedMemberInfo.MemberType);

            if (subjectIsValidType && expectationIsValidType)
            {
                handle(AssertionContext<TSubject>.CreateFromEquivalencyValidationContext(context));
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
            return "Invoke Action<" + typeof(TSubject).Name + "> when " + canHandle.Body;
        }
    }
}
