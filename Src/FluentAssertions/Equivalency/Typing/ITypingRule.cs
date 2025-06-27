namespace FluentAssertions.Equivalency.Typing;

/// <summary>
/// Represents a rule that determines whether the types of the fields and properties in an object graph should be the same between
/// subject and expectation.
/// </summary>
internal interface ITypingRule
{
    /// <summary>
    /// Determines whether strict typing should be applied to the given node in the object graph.
    /// </summary>
    bool UseStrictTyping(Comparands comparands, INode node);
}
