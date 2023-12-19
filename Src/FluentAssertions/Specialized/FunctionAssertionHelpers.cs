using System;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Specialized;

internal static class FunctionAssertionHelpers
{
    internal static T NotThrow<T>(Func<T> subject, string because, object[] becauseArgs)
    {
        try
        {
            return subject();
        }
        catch (Exception exception)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect any exception{reason}, but found {0}.", exception);

            return default;
        }
    }

    internal static TResult NotThrowAfter<TResult>(Func<TResult> subject, IClock clock, TimeSpan waitTime, TimeSpan pollInterval,
        string because, object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNegative(waitTime);
        Guard.ThrowIfArgumentIsNegative(pollInterval);

        TimeSpan? invocationEndTime = null;
        Exception exception = null;
        ITimer timer = clock.StartTimer();

        while (invocationEndTime is null || invocationEndTime < waitTime)
        {
            try
            {
                return subject();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            clock.Delay(pollInterval);
            invocationEndTime = timer.Elapsed;
        }

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect any exceptions after {0}{reason}, but found {1}.", waitTime, exception);

        return default;
    }
}
