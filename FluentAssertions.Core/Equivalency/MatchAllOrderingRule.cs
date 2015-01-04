namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// An ordering rule that basically states that the order of items in all collections is important.
    /// </summary>
    internal class MatchAllOrderingRule : IOrderingRule
    {
        public bool AppliesTo(ISubjectInfo subjectInfo)
        {
            return true;
        }
    }
}