using System;
using System.Collections.Generic;

namespace FluentAssertions.Events;

/// <summary>
/// Settings for the <see cref="EventMonitor{T}"/>.
/// </summary>
public class EventMonitorOptions
{
    /// <summary>
    /// Will ignore the events, if they throw an exception on any custom event accessor implementation. default: false.
    /// </summary>
    internal bool ShouldIgnoreEventAccessorExceptions { get; private set; }

    /// <summary>
    /// Func used to generate the timestamp.
    /// </summary>
    internal Func<DateTime> TimestampProvider { get; private set; } = () => DateTime.Now;

    /// <summary>
    /// When called it will ignore event accessor Exceptions.
    /// </summary>
    /// <returns>The options instance for method stacking.</returns>
    public EventMonitorOptions IgnoreEventAccessorExceptions()
    {
        ShouldIgnoreEventAccessorExceptions = true;
        return this;
    }

    /// <summary>
    /// Sets the timestamp provider. default it is <see cref="DateTime.Now"/>.
    /// </summary>
    /// <param name="timestampProvider">The timestamp provider.</param>
    /// <returns>The options instance for method stacking.</returns>
    public EventMonitorOptions ConfigureTimestampProvider(Func<DateTime> timestampProvider)
    {
        if (timestampProvider != null)
        {
            TimestampProvider = timestampProvider;
        }

        return this;
    }
}
