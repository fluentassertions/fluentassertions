namespace FluentAssertions.Assertions.Structural
{
    internal class ReferenceEqualityStep : IStructuralEqualityStep
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
            if (ReferenceEquals(context.Subject, context.Expectation))
            {
                return true;
            }

            if (ReferenceEquals(context.Expectation, null))
            {
                string propertyPath = context.PropertyPath;
                if (propertyPath.Length == 0)
                {
                    propertyPath = "subject";
                }

                FluentAssertions.Execute.Verification
                    .BecauseOf(context.Reason, context.ReasonArgs)
                    .FailWith("Expected " + propertyPath + " to be {0}{reason}, but found {1}.", context.Expectation, context.Subject);
            }

            return !ReferenceEquals(context.Subject, null) && context.Subject.Equals(context.Expectation);
        }
    }
}
