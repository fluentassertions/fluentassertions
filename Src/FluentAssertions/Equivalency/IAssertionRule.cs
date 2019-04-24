namespace FluentAssertions.Equivalency
{
    // REFACTOR: Should be removed in a future breaking change since it is replaced by IEquivalencyStep
    public interface IAssertionRule
    {
        /// <summary>
        /// Defines how a subject's property is compared for equality with the same property of the expectation.
        /// </summary>
        /// <returns>
        /// Returns <c>true</c> if the rule was applied correctly and the assertion didn't cause any exceptions.
        /// Returns <c>false</c> if this rule doesn't support the subject's type.
        /// Throws if the rule did support the data type but assertion fails.
        /// </returns>
        bool AssertEquality(IEquivalencyValidationContext context);
    }
}
