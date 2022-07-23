using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Specialized;

#pragma warning disable CS0659 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals

#if NET6_0_OR_GREATER
public class TaskCompletionSourceAssertions : TaskCompletionSourceAssertionsBase
{
    private readonly TaskCompletionSource subject;

    public TaskCompletionSourceAssertions(TaskCompletionSource tcs)
        : this(tcs, new Clock())
    {
    }

    public TaskCompletionSourceAssertions(TaskCompletionSource tcs, IClock clock)
        : base(clock)
    {
        subject = tcs;
    }

    /// <summary>
    /// Asserts that the <see cref="Task"/> of the current <see cref="TaskCompletionSource"/> will complete within the specified time.
    /// </summary>
    /// <param name="timeSpan">The allowed time span for the operation.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public async Task<AndConstraint<TaskCompletionSourceAssertions>> CompleteWithinAsync(
        TimeSpan timeSpan, string because = "", params object[] becauseArgs)
    {
        var success = Execute.Assertion
            .ForCondition(subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} to complete within {0}{reason}, but found <null>.", timeSpan);

        if (success)
        {
            bool completesWithinTimeout = await CompletesWithinTimeoutAsync(subject.Task, timeSpan);
            Execute.Assertion
                .ForCondition(completesWithinTimeout)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);
        }

        return new AndConstraint<TaskCompletionSourceAssertions>(this);
    }

    /// <summary>
    /// Asserts that the <see cref="Task"/> of the current <see cref="TaskCompletionSource"/> will not complete within the specified time.
    /// </summary>
    /// <param name="timeSpan">The time span to wait for the operation.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public async Task<AndConstraint<TaskCompletionSourceAssertions>> NotCompleteWithinAsync(
        TimeSpan timeSpan, string because = "", params object[] becauseArgs)
    {
        var success = Execute.Assertion
            .ForCondition(subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} to not complete within {0}{reason}, but found <null>.", timeSpan);

        if (success)
        {
            bool completesWithinTimeout = await CompletesWithinTimeoutAsync(subject.Task, timeSpan);
            Execute.Assertion
                .ForCondition(!completesWithinTimeout)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to not complete within {0}{reason}.", timeSpan);
        }

        return new AndConstraint<TaskCompletionSourceAssertions>(this);
    }
}

#endif

public class TaskCompletionSourceAssertions<T> : TaskCompletionSourceAssertionsBase
{
    private readonly TaskCompletionSource<T> subject;

    public TaskCompletionSourceAssertions(TaskCompletionSource<T> tcs)
        : this(tcs, new Clock())
    {
    }

    public TaskCompletionSourceAssertions(TaskCompletionSource<T> tcs, IClock clock)
        : base(clock)
    {
        subject = tcs;
    }

    /// <summary>
    /// Asserts that the <see cref="Task"/> of the current <see cref="TaskCompletionSource{T}"/> will complete within the specified time.
    /// </summary>
    /// <param name="timeSpan">The allowed time span for the operation.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public async Task<AndWhichConstraint<TaskCompletionSourceAssertions<T>, T>> CompleteWithinAsync(
        TimeSpan timeSpan, string because = "", params object[] becauseArgs)
    {
        var success = Execute.Assertion
            .ForCondition(subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} to complete within {0}{reason}, but found <null>.", timeSpan);

        if (!success)
        {
            return new AndWhichConstraint<TaskCompletionSourceAssertions<T>, T>(this, default(T));
        }

        bool completesWithinTimeout = await CompletesWithinTimeoutAsync(subject.Task, timeSpan);
        Execute.Assertion
            .ForCondition(completesWithinTimeout)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);
        T result = subject.Task.IsCompleted ? subject.Task.Result : default;
        return new AndWhichConstraint<TaskCompletionSourceAssertions<T>, T>(this, result);
    }

    /// <summary>
    /// Asserts that the <see cref="Task"/> of the current <see cref="TaskCompletionSource{T}"/> will not complete within the specified time.
    /// </summary>
    /// <param name="timeSpan">The time span to wait for the operation.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public async Task<AndConstraint<TaskCompletionSourceAssertions<T>>> NotCompleteWithinAsync(
        TimeSpan timeSpan, string because = "", params object[] becauseArgs)
    {
        var success = Execute.Assertion
            .ForCondition(subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context} to complete within {0}{reason}, but found <null>.", timeSpan);

        if (success)
        {
            bool completesWithinTimeout = await CompletesWithinTimeoutAsync(subject.Task, timeSpan);
            Execute.Assertion
                .ForCondition(!completesWithinTimeout)
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect {context:task} to complete within {0}{reason}.", timeSpan);
        }

        return new AndConstraint<TaskCompletionSourceAssertions<T>>(this);
    }
}

/// <summary>
/// Implements base functionality for assertions on TaskCompletionSource.
/// </summary>
public class TaskCompletionSourceAssertionsBase
{
    protected TaskCompletionSourceAssertionsBase(IClock clock)
    {
        Clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    private protected IClock Clock { get; }

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        throw new NotSupportedException("Calling Equals on Assertion classes is not supported.");

    /// <summary>
    ///     Monitors the specified task whether it completes withing the remaining time span.
    /// </summary>
    private protected async Task<bool> CompletesWithinTimeoutAsync(Task target, TimeSpan remainingTime)
    {
        using var timeoutCancellationTokenSource = new CancellationTokenSource();

        Task completedTask =
            await Task.WhenAny(target, Clock.DelayAsync(remainingTime, timeoutCancellationTokenSource.Token));

        if (completedTask != target)
        {
            return false;
        }

        // cancel the clock
        timeoutCancellationTokenSource.Cancel();
        return true;
    }
}
