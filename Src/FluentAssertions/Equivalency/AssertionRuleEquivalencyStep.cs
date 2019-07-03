using System;
using System.Linq.Expressions;

using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Localization;

namespace FluentAssertions.Equivalency
{
    public class AssertionRuleEquivalencyStep<TSubject> : IEquivalencyStep
    {
        private readonly Expression<Func<IMemberInfo, bool>> canHandle;

        private readonly Action<IAssertionContext<TSubject>> handle;

        public AssertionRuleEquivalencyStep(Expression<Func<IMemberInfo, bool>> predicate, Action<IAssertionContext<TSubject>> action)
        {
            canHandle = predicate;
            handle = action;
        }

        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Func<IMemberInfo, bool> predicate = canHandle.Compile();

            return ((context.SelectedMemberInfo != null) && predicate(context));
        }

        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            bool subjectIsNull = context.Subject is null;

            bool subjectIsValidType =
                AssertionScope.Current
                    .ForCondition(context.Subject.GetType().IsSameOrInherits(typeof(TSubject)))
                    .FailWith(Resources.Assertion_ExpectedXFromSubjectToBeAYButFoundAZFormat, context.SelectedMemberDescription,
                        typeof(TSubject), context.Subject?.GetType());

            bool expectationIsNull = context.Expectation is null;

            bool expectationIsValidType =
                AssertionScope.Current
                    .ForCondition(expectationIsNull || context.Expectation.GetType().IsSameOrInherits(typeof(TSubject)))
                    .FailWith(Resources.Assertion_ExpectedXFromExpectationToBeAYButFoundAZFormat, context.SelectedMemberDescription,
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
            return string.Format(Resources.Assertion_InvokeActionXWhenYFormat, typeof(TSubject).Name, canHandle.Body);
        }
    }
}
