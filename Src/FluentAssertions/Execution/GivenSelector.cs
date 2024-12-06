using System;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Execution;

/// <summary>
/// Represents a chaining object returned from <see cref="AssertionChain"/> to continue the assertion using
/// an object returned by a selector.
/// </summary>
public class GivenSelector<T>
{
    private readonly AssertionChain assertionChain;
    private readonly T selector;

    internal GivenSelector(Func<T> selector, AssertionChain assertionChain)
    {
        this.assertionChain = assertionChain;

        this.selector = assertionChain.Succeeded ? selector() : default;
    }

    public bool Succeeded => assertionChain.Succeeded;

    /// <summary>
    /// Specify the condition that must be satisfied upon the subject selected through a prior selector.
    /// </summary>
    /// <param name="predicate">
    /// If <see langword="true"/> the assertion will be treated as successful and no exceptions will be thrown.
    /// </param>
    /// <remarks>
    /// The condition will not be evaluated if the prior assertion failed,
    /// nor will <see cref="FailWith(string,System.Func{T,object}[])"/> throw any exceptions.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    public GivenSelector<T> ForCondition(Func<T, bool> predicate)
    {
        Guard.ThrowIfArgumentIsNull(predicate);

        if (assertionChain.Succeeded)
        {
            assertionChain.ForCondition(predicate(selector));
        }

        return this;
    }

    public GivenSelector<T> ForConstraint(OccurrenceConstraint constraint, Func<T, int> func)
    {
        Guard.ThrowIfArgumentIsNull(func);

        if (assertionChain.Succeeded)
        {
            assertionChain.ForConstraint(constraint, func(selector));
        }

        return this;
    }

    public GivenSelector<TOut> Given<TOut>(Func<T, TOut> selector)
    {
        Guard.ThrowIfArgumentIsNull(selector);

        return new GivenSelector<TOut>(() => selector(this.selector), assertionChain);
    }

    public ContinuationOfGiven<T> FailWith(string message)
    {
        return FailWith(message, Array.Empty<object>());
    }

    public ContinuationOfGiven<T> FailWith(string message, params Func<T, object>[] args)
    {
        if (assertionChain.PreviousAssertionSucceeded)
        {
            object[] mappedArguments = args.Select(a => a(selector)).ToArray();
            return FailWith(message, mappedArguments);
        }

        return new ContinuationOfGiven<T>(this);
    }

    public ContinuationOfGiven<T> FailWith(string message, params object[] args)
    {
        assertionChain.FailWith(message, args);
        return new ContinuationOfGiven<T>(this);
    }

    public ContinuationOfGiven<T> FailWith(Func<T, string> message)
    {
        assertionChain.FailWith(message(selector));
        return new ContinuationOfGiven<T>(this);
    }
}
