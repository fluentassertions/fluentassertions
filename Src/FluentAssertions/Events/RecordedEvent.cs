using System;
using System.Diagnostics;

namespace FluentAssertions.Events
{
    /// <summary>
    /// This class is used to store data about an intercepted event
    /// </summary>
    [DebuggerNonUserCode]
    internal class RecordedEvent
    {
        /// <summary>
        /// Default constructor stores the parameters the event was raised with
        /// </summary>
        public RecordedEvent(DateTime utcNow, int raisedOrderIndex, params object[] parameters)
        {
            Parameters = parameters;
            TimestampUtc = utcNow;
            RaisedOrderIndex = raisedOrderIndex;
        }

        /// <summary>
        /// The exact data and time in UTC format at which the event occurred.
        /// </summary>
        public DateTime TimestampUtc { get; }

        /// <summary>
        /// Parameters for the event
        /// </summary>
        public object[] Parameters { get; }

        /// <summary>
        /// The order in which this event was invoked on the monitored object.
        /// </summary>
        public int RaisedOrderIndex { get; }
    }
}
