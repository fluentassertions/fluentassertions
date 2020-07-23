using System;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Specialized
{
    internal class FunctionAssertionHelpers
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
                .ForCondition(exception is null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect any exception{reason}, but found {0}.", exception);

                return default;
            }
        }

        internal static TResult NotThrowAfter<TResult>(Func<TResult> subject, IClock clock, TimeSpan waitTime, TimeSpan pollInterval, string because, object[] becauseArgs)
        {
            if (waitTime < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(waitTime), $"The value of {nameof(waitTime)} must be non-negative.");
            }

            if (pollInterval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(pollInterval), $"The value of {nameof(pollInterval)} must be non-negative.");
            }

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
}
