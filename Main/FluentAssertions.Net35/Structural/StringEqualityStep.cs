namespace FluentAssertions.Structural
{
    internal class StringEqualityStep : IStructuralEqualityStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(StructuralEqualityContext context)
        {
            return (context.Subject is string);
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
            string subject = (string) context.Subject;
            if ((!(context.Expectation is string)) || !subject.Equals(context.Expectation))
            {
                subject.Should().Be(context.Expectation.ToString(), context.Reason, context.ReasonArgs);
            }

            return true;
        }
    }
}