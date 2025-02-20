using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Specialized;

/// <summary>
/// Contains a number of methods to assert that an asynchronous method yields the expected result.
/// </summary>
/// <typeparam name="TTask">The type of <see cref="Task{T}"/> to be handled.</typeparam>
/// <typeparam name="TAssertions">The type of assertion to be returned.</typeparam>
[DebuggerNonUserCode]
public class AsyncFunctionAssertions<TTask, TAssertions> : DelegateAssertionsBase<Func<TTask>, TAssertions>
    where TTask : Task
    where TAssertions : AsyncFunctionAssertions<TTask, TAssertions>
{
    private readonly AssertionChain assertionChain;

    protected AsyncFunctionAssertions(Func<TTask> subject, IExtractExceptions extractor, AssertionChain assertionChain,
        IClock clock)
        : base(subject, extractor, assertionChain, clock)
    {
        this.assertionChain = assertionChain;
    }

    protected override string Identifier => "async function";

    /// <summary>
    /// Asserts that the current <typeparamref name="TTask"/> will not complete within the specified time.
    /// </summary>
    /// <param name="timeSpan">The allowed time span for the operation.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public async Task<AndConstraint<TAssertions>> NotCompleteWithinAsync(TimeSpan timeSpan,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:task} to complete within {0}{reason}, but found <null>.", timeSpan);

        if (assertionChain.Succeeded)
        {
            (Task task, TimeSpan remainingTime) = InvokeWithTimer(timeSpan);

            if (remainingTime >= TimeSpan.Zero)
            {
                bool completesWithinTimeout = await CompletesWithinTimeoutAsync(task, remainingTime, _ => Task.CompletedTask);

                assertionChain
                    .ForCondition(!completesWithinTimeout)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {context:task} to complete within {0}{reason}.", timeSpan);
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Func{Task}"/> throws an exception of the exact type <typeparamref name="TException"/> (and not a derived exception type).
    /// </summary>
    /// <typeparam name="TException">The type of exception expected to be thrown.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <returns>
    /// Returns an object that allows asserting additional members of the thrown exception.
    /// </returns>
    public async Task<ExceptionAssertions<TException>> ThrowExactlyAsync<TException>(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TException : Exception
    {
        Type expectedType = typeof(TException);

        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} to throw exactly {0}{reason}, but found <null>.", expectedType);

        if (assertionChain.Succeeded)
        {
            Exception exception = await InvokeWithInterceptionAsync(Subject);

            assertionChain
                .ForCondition(exception is not null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0}{reason}, but no exception was thrown.", expectedType);

            if (assertionChain.Succeeded)
            {
                exception.Should().BeOfType(expectedType, because, becauseArgs);
            }

            return new ExceptionAssertions<TException>([exception as TException], assertionChain);
        }

        return new ExceptionAssertions<TException>([], assertionChain);
    }

    /// <summary>
    /// Asserts that the current <see cref="Func{Task}"/> throws an exception of type <typeparamref name="TException"/>.
    /// </summary>
    /// <typeparam name="TException">The type of exception expected to be thrown.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public async Task<ExceptionAssertions<TException>> ThrowAsync<TException>(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TException : Exception
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} to throw {0}{reason}, but found <null>.", typeof(TException));

        if (assertionChain.Succeeded)
        {
            Exception exception = await InvokeWithInterceptionAsync(Subject);
            return ThrowInternal<TException>(exception, because, becauseArgs);
        }

        return new ExceptionAssertions<TException>([], assertionChain);
    }

    /// <summary>
    /// Asserts that the current <see cref="Func{Task}"/> throws an exception of type <typeparamref name="TException"/>
    /// within a specific timeout.
    /// </summary>
    /// <typeparam name="TException">The type of exception expected to be thrown.</typeparam>
    /// <param name="timeSpan">The allowed time span for the operation.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public async Task<ExceptionAssertions<TException>> ThrowWithinAsync<TException>(TimeSpan timeSpan,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TException : Exception
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} to throw {0} within {1}{reason}, but found <null>.",
                typeof(TException), timeSpan);

        if (assertionChain.Succeeded)
        {
            Exception caughtException = await InvokeWithInterceptionAsync(timeSpan);
            return AssertThrows<TException>(caughtException, timeSpan, because, becauseArgs);
        }

        return new ExceptionAssertions<TException>([], assertionChain);
    }

    private ExceptionAssertions<TException> AssertThrows<TException>(
        Exception exception, TimeSpan timeSpan,
        [StringSyntax("CompositeFormat")] string because, object[] becauseArgs)
        where TException : Exception
    {
        TException[] expectedExceptions = Extractor.OfType<TException>(exception).ToArray();

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected a <{0}> to be thrown within {1}{reason}, ", typeof(TException), timeSpan, chain => chain
                .ForCondition(exception is not null)
                .FailWith("but no exception was thrown.")
                .Then
                .ForCondition(expectedExceptions.Length > 0)
                .FailWith("but found <{0}>:" + Environment.NewLine + "{1}.",
                    exception?.GetType(),
                    exception));

        return new ExceptionAssertions<TException>(expectedExceptions, assertionChain);
    }

    private async Task<Exception> InvokeWithInterceptionAsync(TimeSpan timeout)
    {
        try
        {
            // For the duration of this nested invocation, configure CallerIdentifier
            // to match the contents of the subject rather than our own call site.
            //
            //   Func<Task> action = async () => await subject.Should().BeSomething();
            //   await action.Should().ThrowAsync<Exception>();
            //
            // If an assertion failure occurs, we want the message to talk about "subject"
            // not "await action".
            using (CallerIdentifier.OnlyOneFluentAssertionScopeOnCallStack()
                       ? CallerIdentifier.OverrideStackSearchUsingCurrentScope()
                       : default)
            {
                (TTask task, TimeSpan remainingTime) = InvokeWithTimer(timeout);

                if (remainingTime < TimeSpan.Zero)
                {
                    // timeout reached without exception
                    return null;
                }

                if (task.IsFaulted)
                {
                    // exception in synchronous portion
                    return task.Exception!.GetBaseException();
                }

                // Start monitoring the task regarding timeout.
                // Here we do not need to know whether the task completes (successfully) in timeout
                // or does not complete. We are only interested in the exception which is thrown, not returned.
                // So, we can ignore the result.
                _ = await CompletesWithinTimeoutAsync(task, remainingTime, cancelledTask => cancelledTask);
            }

            return null;
        }
        catch (Exception exception)
        {
            return exception;
        }
    }

    /// <summary>
    /// Asserts that the current <see cref="Func{Task}"/> does not throw an exception of type <typeparamref name="TException"/>.
    /// </summary>
    /// <typeparam name="TException">The type of exception expected to not be thrown.</typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public async Task<AndConstraint<TAssertions>> NotThrowAsync<TException>([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
        where TException : Exception
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} not to throw{reason}, but found <null>.");

        if (assertionChain.Succeeded)
        {
            try
            {
                await Subject!.Invoke();
            }
            catch (Exception exception)
            {
                return NotThrowInternal<TException>(exception, because, becauseArgs);
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    ///     Invokes the subject and measures the sync execution time.
    /// </summary>
    private protected (TTask result, TimeSpan remainingTime) InvokeWithTimer(TimeSpan timeSpan)
    {
        ITimer timer = Clock.StartTimer();
        TTask result = Subject.Invoke();
        TimeSpan remainingTime = timeSpan - timer.Elapsed;

        return (result, remainingTime);
    }

    /// <summary>
    ///     Monitors the specified task whether it completes withing the remaining time span.
    /// </summary>
    private protected async Task<bool> CompletesWithinTimeoutAsync(Task target, TimeSpan remainingTime, Func<Task, Task> onTaskCanceled)
    {
        using var delayCancellationTokenSource = new CancellationTokenSource();

        Task delayTask = Clock.DelayAsync(remainingTime, delayCancellationTokenSource.Token);
        Task completedTask = await Task.WhenAny(target, delayTask);

        if (completedTask.IsFaulted)
        {
            // Throw the inner exception.
            await completedTask;
        }

        if (completedTask != target)
        {
            // The monitored task did not complete.
            return false;
        }

        if (target.IsCanceled)
        {
            await onTaskCanceled(target);
        }

        // The monitored task is completed, we shall cancel the clock.
        delayCancellationTokenSource.Cancel();
        return true;
    }

    private protected static async Task<Exception> InvokeWithInterceptionAsync(Func<Task> action)
    {
        try
        {
            // For the duration of this nested invocation, configure CallerIdentifier
            // to match the contents of the subject rather than our own call site.
            //
            //   Func<Task> action = async () => await subject.Should().BeSomething();
            //   await action.Should().ThrowAsync<Exception>();
            //
            // If an assertion failure occurs, we want the message to talk about "subject"
            // not "await action".
            using (CallerIdentifier.OnlyOneFluentAssertionScopeOnCallStack()
                       ? CallerIdentifier.OverrideStackSearchUsingCurrentScope()
                       : default)
            {
                await action();
            }

            return null;
        }
        catch (Exception exception)
        {
            return exception;
        }
    }
}
