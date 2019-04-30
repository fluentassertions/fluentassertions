using System;
using System.Threading;

namespace FluentAssertions.Common
{
    internal static class NoSynchronizationContextScope
    {
        public static DisposingAction Enter()
        {
            var context = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(null);
            return new DisposingAction(() => SynchronizationContext.SetSynchronizationContext(context));
        }

        internal class DisposingAction : IDisposable
        {
            private readonly Action action;

            public DisposingAction(Action action)
            {
                this.action = action;
            }

            public void Dispose()
            {
                action();
            }
        }
    }
}
