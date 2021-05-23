using System;

using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class StringEqualityEquivalencyStep : IEquivalencyStep
    {
        public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
        {
            Type expectationType = comparands.GetExpectedType(context.Options);

            if (expectationType is null || expectationType != typeof(string))
            {
                return EquivalencyResult.ContinueWithNext;
            }

            if (!ValidateAgainstNulls(comparands, context.CurrentNode))
            {
                return EquivalencyResult.AssertionCompleted;
            }

            bool subjectIsString = ValidateAgainstType<string>(comparands, context.CurrentNode);
            if (subjectIsString)
            {
                string subject = (string)comparands.Subject;
                string expectation = (string)comparands.Expectation;

                subject.Should()
                    .Be(expectation, context.Reason.FormattedMessage, context.Reason.Arguments);
            }

            return EquivalencyResult.AssertionCompleted;
        }

        private static bool ValidateAgainstNulls(Comparands comparands, INode currentNode)
        {
            object expected = comparands.Expectation;
            object subject = comparands.Subject;

            bool onlyOneNull = (expected is null) != (subject is null);

            if (onlyOneNull)
            {
                AssertionScope.Current.FailWith(
                    $"Expected {currentNode.Description} to be {{0}}{{reason}}, but found {{1}}.", expected, subject);

                return false;
            }

            return true;
        }

        private static bool ValidateAgainstType<T>(Comparands comparands, INode currentNode)
        {
            bool subjectIsNull = comparands.Subject is null;
            if (subjectIsNull)
            {
                // Do not know the declared type of the expectation.
                return true;
            }

            return
                AssertionScope.Current
                    .ForCondition(comparands.Subject.GetType().IsSameOrInherits(typeof(T)))
                    .FailWith($"Expected {currentNode} to be {{0}}, but found {{1}}.",
                        comparands.RuntimeType, comparands.Subject.GetType());
        }
    }
}
