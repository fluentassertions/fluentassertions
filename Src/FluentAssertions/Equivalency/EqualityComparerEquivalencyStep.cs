using System;
using System.Collections.Generic;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class EqualityComparerEquivalencyStep<T> : IEquivalencyStep
    {
        private readonly IEqualityComparer<T> comparer;

        public EqualityComparerEquivalencyStep(IEqualityComparer<T> comparer)
        {
            this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return config.GetExpectationType(context) == typeof(T);
        }

        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            Execute.Assertion
                .BecauseOf(context.Because, context.BecauseArgs)
                .ForCondition(context.Subject is T)
                .FailWith("Expected {context:object} to be of type {0}{because}, but found {1}", typeof(T), context.Subject)
                .Then
                .ForCondition(comparer.Equals((T)context.Subject, (T)context.Expectation))
                .FailWith("Expected {context:object} to be equal to {1} according to {0}{because}, but {2} was not.",
                    comparer.ToString(), context.Expectation, context.Subject);

            return true;
        }

        public override string ToString()
        {
            return $"Use {comparer} for objects of type {typeof(T)}";
        }
    }
}
