using System.Collections;
using System.Diagnostics.CodeAnalysis;

using FluentAssertions.Execution;

using static System.FormattableString;

namespace FluentAssertions.Equivalency
{
    public class DictionaryEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return typeof(IDictionary).IsAssignableFrom(config.GetExpectationType(context.RuntimeType, context.CompileTimeType));
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
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public virtual bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            var subject = context.Subject as IDictionary;
            var expectation = context.Expectation as IDictionary;

            if (PreconditionsAreMet(expectation, subject))
            {
                if (expectation is not null)
                {
                    foreach (object key in expectation.Keys)
                    {
                        if (config.IsRecursive)
                        {
                            context.Tracer.WriteLine(member => Invariant($"Recursing into dictionary item {key} at {member.Description}"));
                            parent.AssertEqualityUsing(context.AsDictionaryItem(key, subject[key], expectation[key]));
                        }
                        else
                        {
                            context.Tracer.WriteLine(member =>
                                Invariant($"Comparing dictionary item {key} at {member.Description} between subject and expectation"));
                            subject[key].Should().Be(expectation[key], context.Reason.FormattedMessage, context.Reason.Arguments);
                        }
                    }
                }
            }

            return true;
        }

        private static bool PreconditionsAreMet(IDictionary expectation, IDictionary subject)
        {
            return AssertIsDictionary(subject)
                   && AssertEitherIsNotNull(expectation, subject)
                   && AssertSameLength(expectation, subject);
        }

        private static bool AssertEitherIsNotNull(IDictionary expectation, IDictionary subject)
        {
            return AssertionScope.Current
                .ForCondition(((expectation is null) && (subject is null)) || (expectation is not null))
                .FailWith("Expected {context:subject} to be {0}{reason}, but found {1}.", null, subject);
        }

        private static bool AssertIsDictionary(IDictionary subject)
        {
            return AssertionScope.Current
                .ForCondition(subject is not null)
                .FailWith("Expected {context:subject} to be a dictionary, but it is not.");
        }

        private static bool AssertSameLength(IDictionary expectation, IDictionary subject)
        {
            return AssertionScope.Current
                .ForCondition((expectation is null) || (subject.Keys.Count == expectation.Keys.Count))
                .FailWith("Expected {context:subject} to be a dictionary with {0} item(s), but it only contains {1} item(s).",
                    expectation?.Keys.Count, subject?.Keys.Count);
        }
    }
}
