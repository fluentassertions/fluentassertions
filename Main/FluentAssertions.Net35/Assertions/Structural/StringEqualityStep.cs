namespace FluentAssertions.Assertions.Structural
{
    internal class StringEqualityStep : IStructuralEqualityStep
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
        public bool Execute(StructuralEqualityContext context, IStructuralEqualityValidator structuralEqualityValidator)
        {
            string subject = context.Subject as string;
            if (subject != null)
            {
                if ((context.Expectation is string) && subject.Equals(context.Expectation))
                {
                    return true;
                }

                subject.Should().Be(context.Expectation.ToString(), context.Reason, context.ReasonArgs);
            }

            return false;
        }
    }
}