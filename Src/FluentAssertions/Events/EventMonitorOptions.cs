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
    /// This will record the event, even if the event accessor add event threw an exception. To ignore exceptions in the event add accessor, call <see cref="IgnoringEventAccessorExceptions"/> property to set it to true. default: false.
    /// </summary>
    internal bool ShouldRecordEventsWithBrokenAccessor { get; private set; }

    /// <summary>
    /// Func used to generate the timestamp.
    /// </summary>
    internal Func<DateTime> TimestampProvider { get; private set; } = () => DateTime.UtcNow;

    /// <summary>
    /// When called it will ignore event accessor Exceptions.
    /// </summary>
    public EventMonitorOptions IgnoringEventAccessorExceptions()
    {
        ShouldIgnoreEventAccessorExceptions = true;
        return this;
    }

    /// <summary>
    /// When called it will record the event even when the accessor threw an exception.
    /// </summary>
    public EventMonitorOptions RecordingEventsWithBrokenAccessor()
    {
        ShouldRecordEventsWithBrokenAccessor = true;
        return this;
    }

    /// <summary>
    /// Sets the timestamp provider. By default it is <see cref="DateTime.UtcNow"/>.
    /// </summary>
    /// <param name="timestampProvider">The timestamp provider.</param>
    internal EventMonitorOptions ConfigureTimestampProvider(Func<DateTime> timestampProvider)
    {
        if (timestampProvider != null)
        {
            TimestampProvider = timestampProvider;
        }

        return this;
    }
}
