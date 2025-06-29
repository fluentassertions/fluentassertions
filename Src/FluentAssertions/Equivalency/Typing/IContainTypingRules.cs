using System.Collections.Generic;

namespace FluentAssertions.Equivalency.Typing;

/// <summary>
/// Marks a type as containing typing rules that influence how types are compared during equivalency assertions.
/// </summary>
internal interface IContainTypingRules
{
    /// <summary>
    /// Gets a collection of typing rules that determine how types are handled during equivalency assertions.
    /// </summary>
    IEnumerable<ITypingRule> TypingRules { get; }
}
