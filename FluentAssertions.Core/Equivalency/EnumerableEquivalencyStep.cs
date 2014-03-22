using System.Collections;
using System.Linq;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class EnumerableEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the verificationScope subject and/or expectation.
        /// </summary>
        public bool CanHandle(EquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return IsCollection(context.Subject);
        }

        /// <summary>
        /// Applies a step as part of the task to compare two objects for structural equality.
        /// </summary>
        /// <value>
        /// Should return <c>true</c> if the subject matches the expectation or if no additional assertions
        /// have to be executed. Should return <c>false</c> otherwise.
        /// </value>
        /// <remarks>
        /// May throw when preconditions are not met or if it detects mismatching data.
        /// </remarks>
        public bool Handle(EquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            if (AssertExpectationIsCollection(context.Expectation))
            {
                var validator = new EnumerableEquivalencyValidator(parent, context)
                {
                    Recursive = context.IsRoot || config.IsRecursive,
                    OrderingRules = config.OrderingRules
                };

                validator.Execute(ToArray(context.Subject), ToArray(context.Expectation));
            }

            return true;
        }

        private static bool AssertExpectationIsCollection(object expectation)
        {
            return AssertionScope.Current
                .ForCondition(IsCollection(expectation))
                .FailWith("{context:Subject} is a collection and cannot be compared with a non-collection type.");
        }

        private static bool IsCollection(object value)
        {
            return (!(value is string) && (value is IEnumerable));
        }

        private object[] ToArray(object value)
        {
            return ((IEnumerable)value).Cast<object>().ToArray();
        }
    }
}