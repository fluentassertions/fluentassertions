using System;
using FluentAssertions.Common;

namespace FluentAssertions.Specs
{
    internal class TestTimer : ITimer
    {
        private readonly Func<TimeSpan> getElapsed;

        public TestTimer(Func<TimeSpan> getElapsed)
        {
            this.getElapsed = getElapsed;
        }

        public TimeSpan Elapsed => getElapsed();

        public void Dispose()
        {
        }
    }
}
