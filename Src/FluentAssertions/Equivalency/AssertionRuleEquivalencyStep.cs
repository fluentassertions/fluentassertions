using System;
using System.Linq.Expressions;

using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class AssertionRuleEquivalencyStep<TSubject> : IEquivalencyStep
    {
        private readonly Func<IMemberInfo, bool> predicate;
        private readonly string description;
        private readonly Action<IAssertionContext<TSubject>> assertion;
        private readonly AutoConversionStep converter = new AutoConversionStep();

        public AssertionRuleEquivalencyStep(
            Expression<Func<IMemberInfo, bool>> predicate,
            Action<IAssertionContext<TSubject>> assertion)
        {
            this.predicate = predicate.Compile();
            this.assertion = assertion;
            description = predicate.ToString();
        }

        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config) => true;

        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config)
        {
            bool success = false;
            using (var scope = new AssertionScope())
            {
                // Try without conversion
                if (AppliesTo(context))
                {
                    success = ExecuteAssertion(context);
                }

                bool converted = false;
                if (!success && converter.CanHandle(context, config))
                {
                    // Convert into a child context
                    context = context.Clone();
                    converter.Handle(context, parent, config);
                    converted = true;
                }

                if (converted && AppliesTo(context))
                {
                    // Try again after conversion
                    success = ExecuteAssertion(context);
                    if (success)
                    {
                        // If the assertion succeeded after conversion, discard the failures from
                        // the previous attempt. If it didn't, let the scope throw with those failures.
                        scope.Discard();
                    }
                }
            }

            return success;
        }

        private bool AppliesTo(IEquivalencyValidationContext context) => predicate(context);

        private bool ExecuteAssertion(IEquivalencyValidationContext context)
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
                assertion(AssertionContext<TSubject>.CreateFromEquivalencyValidationContext(context));
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
            return "Invoke Action<" + typeof(TSubject).Name + "> when " + description;
        }
    }
}
