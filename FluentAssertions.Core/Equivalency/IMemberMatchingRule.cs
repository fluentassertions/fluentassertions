namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents a rule that defines how to map the members from the subject-under-test with the members 
    /// on the expectation object. 
    /// </summary>
    public interface IMemberMatchingRule
    {
        /// <summary>
        /// Attempts to find a member on the expectation that should be compared with the 
        /// <paramref name="subjectMember"/> during a structural equality.
        /// </summary>
        /// <remarks>
        /// Whether or not a match is required or optional is up to the specific rule. If no match is found and this is not an issue,
        /// simply return <c>null</c>.
        /// </remarks>
        /// <param name="subjectMember">
        /// The <see cref="FluentAssertions.Equivalency.SelectedMemberInfo"/> of the subject's member for which a match must be found. Can never
        /// be <c>null</c>.
        /// </param>
        /// <param name="expectation">
        /// The expectation object for which a matching member must be returned. Can never be <c>null</c>.
        /// </param>
        /// <param name="memberPath">
        /// The dotted path from the root object to the current member. Will never  be <c>null</c>.
        /// </param>
        /// <param name="config"></param>
        /// <returns>
        /// Returns the <see cref="FluentAssertions.Equivalency.SelectedMemberInfo"/> of the property with which to compare the subject with, or <c>null</c>
        /// if no match was found.
        /// </returns>
        SelectedMemberInfo Match(SelectedMemberInfo subjectMember, object expectation, string memberPath, IEquivalencyAssertionOptions config);
    }
}
