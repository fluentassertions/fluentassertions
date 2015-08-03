using System;

using System.Collections;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class EnumerableEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the verificationScope subject and/or expectation.
        /// </summary>
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Type subjectType = config.GetSubjectType(context);

            return IsCollection(subjectType);
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
        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
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
            bool conditionMet = AssertionScope.Current
                .ForCondition(!ReferenceEquals(expectation, null))
                .FailWith("{context:Subject} is a collection and cannot be compared to <null>.");

            if (conditionMet)
            {
                conditionMet = AssertionScope.Current
                .ForCondition(IsCollection(expectation.GetType()))
                .FailWith("{context:Subject} is a collection and cannot be compared with a non-collection type.");
            }

            return conditionMet;
        }

        private static bool IsCollection(Type type)
        {
            return !typeof(string).IsAssignableFrom(type) && typeof(IEnumerable).IsAssignableFrom(type);
        }

        internal static object[] ToArray(object value)
        {
            return !ReferenceEquals(value, null) ? ((IEnumerable)value).Cast<object>().ToArray() : null;
        }
    }
}