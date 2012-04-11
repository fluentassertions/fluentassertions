using System;

namespace FluentAssertions.Structural
{
    internal class DateTimeEqualityStep : IStructuralEqualityStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(StructuralEqualityContext context)
        {
            return (context.Subject is DateTime);
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
        public bool Handle(StructuralEqualityContext context, IStructuralEqualityValidator parent)
        {
            var expectation = (DateTime) context.Expectation;

            if (!(context.Subject is DateTime) || !expectation.Equals(context.Subject))
            {
                ((DateTime)context.Subject).Should().Be(expectation, context.Reason, context.ReasonArgs);
            }

            return true;
        }
    }
}