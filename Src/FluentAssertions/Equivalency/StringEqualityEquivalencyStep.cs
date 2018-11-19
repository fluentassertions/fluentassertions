using System;

using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class StringEqualityEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Type expectationType = config.GetExpectationType(context);

            return (expectationType != null) && (expectationType == typeof(string));
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
            if (!ValidateAgainstNulls(context))
            {
                return true;
            }

            bool subjectIsString = ValidateAgainstType<string>(context);
            if (subjectIsString)
            {
                string subject = (string)context.Subject;
                string expectation = (string)context.Expectation;

                subject.Should()
                    .Be(expectation, context.Because, context.BecauseArgs);
            }

            return true;
        }

        private static bool ValidateAgainstNulls(IEquivalencyValidationContext context)
        {
            object expected = context.Expectation;
            object subject = context.Subject;

            bool onlyOneNull = (expected is null) ^ (subject is null);

            if (onlyOneNull)
            {
                string subjectDescription = GetSubjectDescription(context);

                AssertionScope.Current.FailWith(
                    $"Expected {subjectDescription} to be {{0}}{{reason}}, but found {{1}}.", expected, subject);

                return false;
            }

            return true;
        }

        private static bool ValidateAgainstType<T>(IEquivalencyValidationContext context)
        {
            bool subjectIsNull = context.Subject is null;
            if (subjectIsNull)
            {
                // Do not know the declared type of the expectation.
                return true;
            }

            return
                AssertionScope.Current
                    .ForCondition(context.Subject.GetType().IsSameOrInherits(typeof(T)))
                    .FailWith($"Expected {GetSubjectDescription(context)} to be {{0}}, but found {{1}}.",
                        context.RuntimeType, context.Subject.GetType());
        }

        private static string GetSubjectDescription(IEquivalencyValidationContext context)
        {
            return (context.SelectedMemberDescription.Length == 0) ? "subject" : context.SelectedMemberDescription;
        }
    }
}
