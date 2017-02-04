using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class DictionaryEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Type subjectType = config.GetSubjectType(context);

            return typeof(IDictionary).IsAssignableFrom(subjectType);
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
            var subject = (IDictionary)context.Subject;
            var expectation = context.Expectation as IDictionary;

            if (PreconditionsAreMet(context, expectation, subject))
            {
                foreach (object key in subject.Keys)
                {
                    if (config.IsRecursive)
                    {
                        context.TraceSingle(path => $"Recursing into dictionary item {key} at {path}");
                        parent.AssertEqualityUsing(context.CreateForDictionaryItem(key, subject[key], expectation[key]));
                    }
                    else
                    {
                        context.TraceSingle(path => $"Comparing dictionary item {key} at {path} between subject and expectation");
                        subject[key].Should().Be(expectation[key], context.Because, context.BecauseArgs);
                    }
                }
            }

            return true;
        }

        private static bool PreconditionsAreMet(IEquivalencyValidationContext context, IDictionary expectation, IDictionary subject)
        {
            return (AssertIsDictionary(expectation) && AssertSameLength(expectation, subject));
        }

        private static bool AssertIsDictionary(IDictionary expectation)
        {
            return AssertionScope.Current
                .ForCondition(expectation != null)
                .FailWith("{context:subject} is a dictionary and cannot be compared with a non-dictionary type.");
        }

        private static bool AssertSameLength(IDictionary expectation, IDictionary subject)
        {
            return AssertionScope.Current
                .ForCondition(subject.Keys.Count == expectation.Keys.Count)
                .FailWith("Expected {context:subject} to be a dictionary with {0} item(s), but it only contains {1} item(s).",
                    expectation.Keys.Count, subject.Keys.Count);
        }
    }
}