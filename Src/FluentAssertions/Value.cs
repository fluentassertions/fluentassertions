using System;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Equivalency.Inlining;

namespace FluentAssertions;

/// <summary>
/// Provides a fluent API for defining inline assertions.
/// </summary>
public static class Value
{
    /// <summary>
    /// Builds an inline assertion that expects the subject-under-test to match the specified condition.
    /// </summary>
    /// <typeparam name="T">The type of the subject-under-test.</typeparam>
    /// <param name="condition">A Boolean condition to match.</param>
    public static IInlineEquivalencyAssertion ThatMatches<T>(Expression<Func<T, bool>> condition)
    {
        Guard.ThrowIfArgumentIsNull(condition);
        return new ConditionBasedInlineAssertion<T>(condition);
    }

    /// <summary>
    /// Builds an inline assertion that executes the specific assertion operation on the subject-under-test.
    /// </summary>
    /// <typeparam name="T">The type of the subject-under-test.</typeparam>
    /// <param name="assertion">
    /// The assertion operation to execute, typically using one of the assertion APIs Fluent Assertions provides.
    /// </param>
    public static IInlineEquivalencyAssertion ThatSatisfies<T>(Action<T> assertion)
    {
        Guard.ThrowIfArgumentIsNull(assertion);
        return new ActionBasedInlineAssertion<T>(assertion);
    }
}
