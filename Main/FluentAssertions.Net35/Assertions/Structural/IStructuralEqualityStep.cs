namespace FluentAssertions.Assertions.Structure
{
    internal interface IStructuralEqualityStep
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
        bool Execute(StructuralEqualityContext context, IStructuralEqualityValidator parent);
    }
}