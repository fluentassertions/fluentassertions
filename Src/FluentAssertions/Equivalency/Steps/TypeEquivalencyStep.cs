using System;
using System.Collections;
using System.Linq;
using FluentAssertions.Equivalency.Typing;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

/// <summary>
/// An equivalency step that asserts two objects must be of the same type to be equivalent.
/// </summary>
/// <remarks>
/// This differs from the default equivalency assertion which states that two objects are equivalent if they have the
/// same properties and values, regardless of their type.
/// </remarks>
public class TypeEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
    {
        if (context.Options is not IContainTypingRules options)
        {
            return EquivalencyResult.ContinueWithNext;
        }

        // When comparing using a collection we want to compare the children of the collection not the collection itself (the root object)
        if (comparands.Subject is IEnumerable)
        {
            return EquivalencyResult.ContinueWithNext;
        }

        // If both are null or reference the same instance, there's no need to have this step check anything
        if (ReferenceEquals(comparands.Subject, comparands.Expectation))
        {
            return EquivalencyResult.ContinueWithNext;
        }

        // The above checks if both were null, but if only one is null then there's no need to have this step check anything
        if (comparands.Subject is null || comparands.Expectation is null)
        {
            return EquivalencyResult.ContinueWithNext;
        }

        // Check if strict typing should be applied based on typing rules
        bool shouldUseStrictTyping = options.TypingRules.Any(rule => rule.UseStrictTyping(comparands, context.CurrentNode));
        if (!shouldUseStrictTyping)
        {
            return EquivalencyResult.ContinueWithNext;
        }

        Type expectedType = comparands.GetExpectedType(context.Options);

        AssertionChain.GetOrCreate().For(context)
            .ForCondition(comparands.Subject.GetType() == expectedType)
            .FailWith("Expected {context:subject} to be of type {0}, but found {1}", expectedType, comparands.Subject.GetType());

        return EquivalencyResult.ContinueWithNext;
    }
}
