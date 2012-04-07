using System;

namespace FluentAssertions.Assertions.Structural
{
    internal class DateTimeEqualityStep : IStructuralEqualityStep
    {
        /// <summary>
        /// Applies a step as part of the task to compare two objects for structural equality.
        /// </summary>
        /// <value>
        /// Returns <c>true</c> if this step finalizes the comparison task, returns <c>false</c> otherwise.
        /// </value>
        /// <remarks>
        /// May throw when preconditions are not met, or if it detects mismatching data.
        /// </remarks>
        public bool Execute(StructuralEqualityContext context, IStructuralEqualityValidator parent)
        {
            if (context.Subject is DateTime)
            {
                DateTime expectation = (DateTime) context.Expectation;

                if ((context.Subject is DateTime) && expectation.Equals(context.Subject))
                {
                    return true;
                }

                ((DateTime)context.Subject).Should().Be(expectation, context.Reason, context.ReasonArgs);
            }

            return false;
        }
    }
}