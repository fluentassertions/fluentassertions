using System;

namespace FluentAssertions.Events
{
    /// <summary>
    /// Provides the metadata of a monitored event.
    /// </summary>
    public class EventMetadata
    {
        /// <summary>
        /// The name of the event member on the monitored object
        /// </summary>
        public string EventName { get; }

        /// <summary>
        /// The type of the event handler and event args.
        /// </summary>
        public Type HandlerType { get; }

        public EventMetadata(string eventName, Type handlerType)
        {
            EventName = eventName;
            HandlerType = handlerType;
        }
    }
}
