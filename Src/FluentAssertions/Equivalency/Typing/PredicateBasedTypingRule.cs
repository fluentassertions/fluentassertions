using System;
using System.Linq.Expressions;
using FluentAssertions.Equivalency.Execution;

namespace FluentAssertions.Equivalency.Typing;

/// <summary>
/// An implementation of <see cref="ITypingRule"/> that uses a predicate to determine
/// whether strict typing should be applied during equivalency comparison.
/// </summary>
internal class PredicateBasedTypingRule : ITypingRule
{
    private readonly Func<IObjectInfo, bool> predicate;
    private readonly string description;

    /// <summary>
    /// Initializes a new instance of the <see cref="PredicateBasedTypingRule"/> class with a predicate.
    /// </summary>
    /// <param name="predicate">A predicate that determines whether strict typing should be applied.</param>
    public PredicateBasedTypingRule(Expression<Func<IObjectInfo, bool>> predicate)
    {
        description = predicate.Body.ToString();
        this.predicate = predicate.Compile();
    }

    /// <inheritdoc />
    public bool UseStrictTyping(Comparands comparands, INode node)
    {
        return predicate(new ObjectInfo(comparands, node));
    }

    /// <summary>
    /// Returns a string representation of this object.
    /// </summary>
    public override string ToString()
    {
        return $"Use strict typing when {description}";
    }
}
