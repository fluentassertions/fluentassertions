using System;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Some unit test frameworks (like xUnit) have their own synchronization context
    /// that does not work well with blocking waits and can lead to deadlocks.
    /// These methods create the task in the default synchronization context
    /// and blocks until the task is completed.
    /// </summary>
    internal static class TaskExtensions
    {
        public static void ExecuteInDefaultSynchronizationContext(this Action action)
        {
            using (NoSynchronizationContextScope.Enter())
            {
                action();
            }
        }

        public static TResult ExecuteInDefaultSynchronizationContext<TResult>(this Func<TResult> action)
        {
            using (NoSynchronizationContextScope.Enter())
            {
                return action();
            }
        }

    }
}
