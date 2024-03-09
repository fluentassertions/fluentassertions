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
    private readonly AssertionChain previousAssertionChain;
    private readonly T selector;

    internal GivenSelector(Func<T> selector, AssertionChain previousAssertionChain)
    {
        this.previousAssertionChain = previousAssertionChain;

        this.selector = previousAssertionChain.Succeeded ? selector() : default;
    }

    public bool Succeeded => previousAssertionChain.Succeeded;

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

        if (previousAssertionChain.Succeeded)
        {
            previousAssertionChain.ForCondition(predicate(selector));
        }

        return this;
    }

    public GivenSelector<TOut> Given<TOut>(Func<T, TOut> selector)
    {
        Guard.ThrowIfArgumentIsNull(selector);

        return new GivenSelector<TOut>(() => selector(this.selector), previousAssertionChain);
    }

    public ContinuationOfGiven<T> FailWith(string message)
    {
        return FailWith(message, Array.Empty<object>());
    }

    public ContinuationOfGiven<T> FailWith(string message, params Func<T, object>[] args)
    {
        if (previousAssertionChain.Succeeded)
        {
            object[] mappedArguments = args.Select(a => a(selector)).ToArray();
            return FailWith(message, mappedArguments);
        }

        return new ContinuationOfGiven<T>(this);
    }

    public ContinuationOfGiven<T> FailWith(string message, params object[] args)
    {
        if (previousAssertionChain.Succeeded)
        {
            previousAssertionChain.FailWith(message, args);
            return new ContinuationOfGiven<T>(this);
        }

        return new ContinuationOfGiven<T>(this);
    }
}
