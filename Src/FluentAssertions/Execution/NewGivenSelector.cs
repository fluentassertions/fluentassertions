using System;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Execution;

/// <summary>
/// Represents a chaining object returned from <see cref="AssertionScope.Given{T}"/> to continue the assertion using
/// an object returned by a selector.
/// </summary>
public class NewGivenSelector<T>
{
    private readonly IAssertion previousAssertion;
    private readonly T selector;

    internal NewGivenSelector(Func<T> selector, IAssertion previousAssertion)
    {
        this.previousAssertion = previousAssertion;

        this.selector = previousAssertion.Succeeded ? selector() : default;
    }

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
    public NewGivenSelector<T> ForCondition(Func<T, bool> predicate)
    {
        Guard.ThrowIfArgumentIsNull(predicate);

        if (previousAssertion.Succeeded)
        {
            previousAssertion.ForCondition(predicate(selector));
        }

        return this;
    }

    /// <remarks>
    /// The <paramref name="selector"/> will not be invoked if the prior assertion failed,
    /// nor will <see cref="FailWith(string,System.Func{T,object}[])"/> throw any exceptions.
    /// </remarks>
    /// <inheritdoc cref="IAssertionScope.Given{T}"/>
    /// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
    public NewGivenSelector<TOut> Given<TOut>(Func<T, TOut> selector)
    {
        Guard.ThrowIfArgumentIsNull(selector);

        return new NewGivenSelector<TOut>(() => selector(this.selector), previousAssertion);
    }

    /// <inheritdoc cref="IAssertionScope.FailWith(string)"/>
    public NewContinuationOfGiven<T> FailWith(string message)
    {
        return FailWith(message, Array.Empty<object>());
    }

    /// <remarks>
    /// <inheritdoc cref="IAssertionScope.FailWith(string, object[])"/>
    /// The <paramref name="args"/> will not be invoked if the prior assertion failed,
    /// nor will <see cref="FailWith(string, Func{T,object}[])"/> throw any exceptions.
    /// </remarks>
    /// <inheritdoc cref="IAssertionScope.FailWith(string, object[])"/>
    public NewContinuationOfGiven<T> FailWith(string message, params Func<T, object>[] args)
    {
        if (previousAssertion.Succeeded)
        {
            object[] mappedArguments = args.Select(a => a(selector)).ToArray();
            return FailWith(message, mappedArguments);
        }

        return new NewContinuationOfGiven<T>(this);
    }

    /// <remarks>
    /// <inheritdoc cref="IAssertionScope.FailWith(string, object[])"/>
    /// The <paramref name="args"/> will not be invoked if the prior assertion failed,
    /// nor will <see cref="FailWith(string, object[])"/> throw any exceptions.
    /// </remarks>
    /// <inheritdoc cref="IAssertionScope.FailWith(string, object[])"/>
    public NewContinuationOfGiven<T> FailWith(string message, params object[] args)
    {
        if (previousAssertion.Succeeded)
        {
            previousAssertion.FailWith(message, args);
            return new NewContinuationOfGiven<T>(this);
        }

        return new NewContinuationOfGiven<T>(this);
    }
}
