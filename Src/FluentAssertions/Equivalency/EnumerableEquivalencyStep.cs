using System;

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
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Type expectationType = config.GetExpectationType(context.RuntimeType, context.CompileTimeType);

            return IsCollection(expectationType);
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
            if (AssertSubjectIsCollection(context.Subject))
            {
                var validator = new EnumerableEquivalencyValidator(parent, context)
                {
                    Recursive = context.CurrentNode.IsRoot || config.IsRecursive,
                    OrderingRules = config.OrderingRules
                };

                validator.Execute(ToArray(context.Subject), ToArray(context.Expectation));
            }

            return true;
        }

        private static bool AssertSubjectIsCollection(object subject)
        {
            bool conditionMet = AssertionScope.Current
                .ForCondition(subject is not null)
                .FailWith("Expected a collection, but {context:Subject} is <null>.");

            if (conditionMet)
            {
                conditionMet = AssertionScope.Current
                .ForCondition(IsCollection(subject.GetType()))
                .FailWith("Expected a collection, but {context:Subject} is of a non-collection type.");
            }

            return conditionMet;
        }

        private static bool IsCollection(Type type)
        {
            return !typeof(string).IsAssignableFrom(type) && typeof(IEnumerable).IsAssignableFrom(type);
        }

        internal static object[] ToArray(object value)
        {
            return value is not null ? ((IEnumerable)value).Cast<object>().ToArray() : null;
        }
    }
}
