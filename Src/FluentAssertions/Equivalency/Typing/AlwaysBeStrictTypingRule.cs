namespace FluentAssertions.Equivalency.Typing;

/// <summary>
/// An implementation of <see cref="ITypingRule"/> that applies strict typing to all objects.
/// </summary>
internal class AlwaysBeStrictTypingRule : ITypingRule
{
    /// <inheritdoc />
    public bool UseStrictTyping(Comparands comparands, INode node)
    {
        return true;
    }

    /// <summary>
    /// Returns a string representation of this object.
    /// </summary>
    public override string ToString()
    {
        return "The types of the fields and properties must be the same";
    }
}
