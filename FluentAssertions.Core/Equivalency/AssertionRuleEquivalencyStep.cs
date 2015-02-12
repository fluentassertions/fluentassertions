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
            bool expectationisNull = ReferenceEquals(context.Expectation, null);

            bool succeeded =
                AssertionScope.Current.ForCondition(
                    expectationisNull || context.Expectation.GetType().IsSameOrInherits(typeof(TSubject)))
                    .FailWith(
                        "Expected " + context.SelectedMemberDescription + " to be a {0}{reason}, but found a {1}",
                        !expectationisNull ? context.Expectation.GetType() : null,
                        context.SelectedMemberInfo.MemberType);

            if (succeeded)
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