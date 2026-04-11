namespace FluentAssertions.Equivalency.Selection;

/// <summary>
/// Represents a selection rule whose effect is determined by a configured member path.
/// This allows callers to ask whether the rule targets any members within the subtree rooted at
/// a given <see cref="INode"/>, even when the rule is wrapped by decorators such as the
/// collection-member options decorator.
/// </summary>
internal interface IPathBasedSelectionRule : IMemberSelectionRule
{
    /// <summary>
    /// Returns <c>true</c> when this rule targets at least one member of <paramref name="currentNode"/>
    /// or one of its descendants.
    /// </summary>
    bool SelectsMembersOf(INode currentNode);
}
