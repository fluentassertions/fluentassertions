using System.Collections.Generic;

namespace FluentAssertions.Structural
{
    public interface IStructuralEqualityConfiguration
    {
        IEnumerable<ISelectionRule> SelectionRules { get; }
        IEnumerable<IMatchingRule> MatchingRules { get; }
        IEnumerable<IAssertionRule> AssertionRules { get; }
        bool Recurse { get; set; }
        CyclicReferenceHandling CyclicReferenceHandling { get; }
    }
}