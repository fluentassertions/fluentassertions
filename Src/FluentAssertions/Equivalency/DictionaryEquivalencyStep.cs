using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions.Execution;
using FluentAssertions.Localization;

namespace FluentAssertions.Equivalency
{
    public class DictionaryEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return typeof(IDictionary).IsAssignableFrom(config.GetExpectationType(context));
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
                if (expectation != null)
                {
                    foreach (object key in expectation.Keys)
                    {
                        if (config.IsRecursive)
                        {
                            context.TraceSingle(path => string.Format(Resources.Dictionary_RecursingIntoDictionaryItemXAtYFormat, key, path));
                            parent.AssertEqualityUsing(context.CreateForDictionaryItem(key, subject[key], expectation[key]));
                        }
                        else
                        {
                            context.TraceSingle(path => string.Format(Resources.Dictionary_ComparingDictionaryItemXAtYBetweenSubjectAndExpectationFormat, key, path));
                            subject[key].Should().Be(expectation[key], context.Because, context.BecauseArgs);
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
                .ForCondition(((expectation is null) && (subject is null)) || (expectation != null))
                .FailWith(Resources.Dictionary_ExpectedSubjectToBeXFormat + Resources.Common_CommaButFoundYFormat, null, subject);
        }

        private static bool AssertIsDictionary(IDictionary subject)
        {
            return AssertionScope.Current
                .ForCondition(subject != null)
                .FailWith(Resources.Dictionary_ExpectedSubjectToBeDictionaryButIsNot);
        }

        private static bool AssertSameLength(IDictionary expectation, IDictionary subject)
        {
            return AssertionScope.Current
                .ForCondition((expectation is null) || (subject.Keys.Count == expectation.Keys.Count))
                .FailWith(Resources.Dictionary_ExpectedSubjectToBeDictionaryWithXItemsButItOnlyContainsYItemsFormat,
                    expectation?.Keys.Count, subject?.Keys.Count);
        }
    }
}
