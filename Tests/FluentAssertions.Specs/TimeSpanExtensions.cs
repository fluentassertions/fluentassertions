
using System;

namespace FluentAssertions.Specs
{
    /// <summary>
    /// Implements extensions to <see cref="TimeSpan"/> available in .NET Core 2, but not in .NET Framework.
    /// </summary>
    public static class TimeSpanExtensions
    {
        public static TimeSpan Multiply(this TimeSpan timeSpan, double factor)
        {
            if (double.IsNaN(factor))
            {
                throw new ArgumentException("Argument cannot be NaN", nameof(factor));
            }

            // Rounding to the nearest tick is as close to the result we would have with unlimited
            // precision as possible, and so likely to have the least potential to surprise.
            double ticks = Math.Round(timeSpan.Ticks * factor);
            if (ticks > long.MaxValue || ticks < long.MinValue)
            {
                throw new OverflowException("TimeSpan overflowed because the duration is too long.");
            }

            return TimeSpan.FromTicks((long)ticks);
        }

        public static TimeSpan Divide(this TimeSpan timeSpan, double divisor)
        {
            if (double.IsNaN(divisor))
            {
                throw new ArgumentException("Argument cannot be NaN", nameof(divisor));
            }

            double ticks = Math.Round(timeSpan.Ticks / divisor);
            if (ticks > long.MaxValue || ticks < long.MinValue || double.IsNaN(ticks))
            {
                throw new OverflowException("TimeSpan overflowed because the duration is too long.");
            }

            return TimeSpan.FromTicks((long)ticks);
        }
    }
}
