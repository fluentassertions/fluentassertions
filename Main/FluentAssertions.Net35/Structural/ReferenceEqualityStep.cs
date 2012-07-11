namespace FluentAssertions.Structural
{
    internal class ReferenceEqualityStep : IStructuralEqualityStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(StructuralEqualityContext context)
        {
            return true;
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
        public bool Handle(StructuralEqualityContext context, IStructuralEqualityValidator structuralEqualityValidator)
        {
            if (ReferenceEquals(context.Subject, context.Expectation))
            {
                return true;
            }

            if (ReferenceEquals(context.Expectation, null))
            {
                string propertyPath = context.PropertyDescription;
                if (propertyPath.Length == 0)
                {
                    propertyPath = "subject";
                }

                context.Verification
                    .FailWith("Expected " + propertyPath + " to be {0}{reason}, but found {1}.", context.Expectation, context.Subject);
            }

            return !ReferenceEquals(context.Subject, null) && context.Subject.Equals(context.Expectation);
        }
    }
}
