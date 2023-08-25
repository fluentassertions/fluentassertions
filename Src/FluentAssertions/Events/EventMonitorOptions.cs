using System;

namespace FluentAssertions.Events;

/// <summary>
/// Settings for the EventMonitor.
/// </summary>
public class EventMonitorOptions
{
    /// <summary>
    /// Will ignore the events, if they throw an exception on any custom event accessor implementation. default: false.
    /// </summary>
    internal bool ShouldIgnoreEventAccessorExceptions { get; private set; }

    /// <summary>
    /// This will record the event, even if the event accessor add event accessor threw an exception. To ignore exceptions in the event add accessor, call <see cref="IgnoreEventAccessorExceptions"/> property to true. default: false.
    /// </summary>
    internal bool RecordEventsWithBrokenAccessor { get; private set; }

    /// <summary>
    /// Func used to generate the timestamp.
    /// </summary>
    internal Func<DateTime> TimestampProvider { get; private set; } = () => DateTime.UtcNow;

    /// <summary>
    /// When called it will ignore event accessor Exceptions.
    /// </summary>
    /// <param name="recordEventsWithBrokenAccessor">This will record the event, even if the event add event accessor threw an exception. default: false.</param>
    /// <returns>The options instance for method stacking.</returns>
    public EventMonitorOptions IgnoreEventAccessorExceptions(bool recordEventsWithBrokenAccessor = false)
    {
        ShouldIgnoreEventAccessorExceptions = true;
        RecordEventsWithBrokenAccessor = recordEventsWithBrokenAccessor;
        return this;
    }

    /// <summary>
    /// Sets the timestamp provider. By default it is <see cref="DateTime.UtcNow"/>.
    /// </summary>
    /// <param name="timestampProvider">The timestamp provider.</param>
    /// <returns>The options instance for method stacking.</returns>
    internal EventMonitorOptions ConfigureTimestampProvider(Func<DateTime> timestampProvider)
    {
        if (timestampProvider != null)
        {
            TimestampProvider = timestampProvider;
        }

        return this;
    }
}
