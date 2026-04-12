using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Selection;

internal abstract class SelectMemberByPathSelectionRule : IPathBasedSelectionRule
{
    private static readonly Regex LeadingCollectionIndexRegex = new(@"^\[[0-9]+]\.?");

    public virtual bool IncludesMembers => false;

    protected abstract MemberPath MemberPath { get; }

    public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers,
        MemberSelectionContext context)
    {
        var members = selectedMembers.ToList();
        AddOrRemoveMembersFrom(members, currentNode, GetPathRelativeToSelectionRoot(currentNode), context);

        return members;
    }

    /// <summary>
    /// Returns <c>true</c> if this rule would select members within the subtree rooted at
    /// <paramref name="currentNode"/>, meaning the rule path targets any member at or below the current node.
    /// </summary>
    public bool SelectsMembersOf(INode currentNode)
    {
        if (currentNode.IsRoot)
        {
            return !MemberPath.ToString().IsNullOrEmpty();
        }

        string currentPath = GetPathRelativeToSelectionRoot(currentNode);

        if (string.IsNullOrEmpty(currentPath))
        {
            return !MemberPath.ToString().IsNullOrEmpty();
        }

        // Compare normalized path segments rather than raw strings so collection rules like "Items[].Name"
        // still match the concrete node path "Items[0].Name", and so paths built through
        // MemberPath.AsParentCollectionOf are interpreted consistently.
        return new MemberPath(currentPath).IsParentOf(MemberPath);
    }

    protected abstract void AddOrRemoveMembersFrom(List<IMember> selectedMembers,
        INode parent, string parentPath,
        MemberSelectionContext context);

    /// <summary>
    /// Returns the path used for member-selection matching at <paramref name="currentNode"/>.
    /// For items inside a root collection, Fluent Assertions exposes paths such as <c>[0].Name</c>, but
    /// selection rules are expressed relative to the collection item type (for example <c>Name</c>).
    /// This method removes that root-item index so both sides use the same coordinate system.
    /// </summary>
    private static string GetPathRelativeToSelectionRoot(INode currentNode)
    {
        if (currentNode.IsRoot)
        {
            return string.Empty;
        }

        string expectationPath = currentNode.Expectation.PathAndName;
        return RemoveLeadingCollectionIndex(expectationPath);
    }

    /// <summary>
    /// Removes only the leading root-collection index from <paramref name="path"/>, such as turning
    /// <c>[0]</c> into an empty path and <c>[0].Name</c> into <c>Name</c>.
    /// Nested collection indices are intentionally left in place; they are handled later by
    /// <see cref="MemberPath"/> comparison, which treats <c>[]</c> and concrete indices as equivalent.
    /// </summary>
    private static string RemoveLeadingCollectionIndex(string path)
    {
        Match match = LeadingCollectionIndexRegex.Match(path);
        return match.Success ? path.Substring(match.Length) : path;
    }
}
