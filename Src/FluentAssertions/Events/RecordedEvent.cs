using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FluentAssertions.Events
{
    /// <summary>
    /// This class is used to store data about an intercepted event
    /// </summary>
    [DebuggerNonUserCode]
    public class RecordedEvent
    {
        private object[] parameters;

        /// <summary>
        /// Default constructor stores the parameters the event was raised with
        /// </summary>
        public RecordedEvent(DateTime utcNow, object monitoredObject, params object[] parameters)
        {
            Parameters = parameters.Select(p => (p == monitoredObject) ? new WeakReference(p) : p);
            TimestampUtc = utcNow;
        }

        /// <summary>
        /// The exact data and time in UTC format at which the event occurred.
        /// </summary>
        public DateTime TimestampUtc { get; set; }

        /// <summary>
        /// Parameters for the event
        /// </summary>
        public IEnumerable<object> Parameters
        {
            get
            {
                return parameters.Select(parameter =>
                    (parameter is WeakReference weakReference) ? weakReference.Target : parameter).ToArray();
            }

            private set { parameters = value.ToArray(); }
        }
    }
}
