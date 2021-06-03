namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents a rule that defines how to map the selected members of the expectation object to the properties
    /// of the subject.
    /// </summary>
    public interface IMemberMatchingRule
    {
        /// <summary>
        /// Attempts to find a member on the subject that should be compared with the
        /// <paramref name="expectedMember"/> during a structural equality.
        /// </summary>
        /// <remarks>
        /// Whether or not a match is required or optional is up to the specific rule. If no match is found and this is not an issue,
        /// simply return <c>null</c>.
        /// </remarks>
        /// <param name="expectedMember">
        /// The <see cref="IMember"/> of the subject's member for which a match must be found. Can never
        /// be <c>null</c>.
        /// </param>
        /// <param name="subject">
        /// The subject object for which a matching member must be returned. Can never be <c>null</c>.
        /// </param>
        /// <param name="parent"></param>
        /// <param name="options"></param>
        /// <returns>
        /// Returns the <see cref="IMember"/> of the property with which to compare the subject with, or <c>null</c>
        /// if no match was found.
        /// </returns>
        IMember Match(IMember expectedMember, object subject, INode parent, IEquivalencyAssertionOptions options);
    }
}
