using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FluentAssertions.EventMonitoring
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
        public RecordedEvent(object monitoredObject, params object[] parameters)
        {
            Parameters = parameters.Select(p => (p == monitoredObject) ? new WeakReference(p) : p);
        }

        /// <summary>
        /// Parameters for the event
        /// </summary>
        public IEnumerable<object> Parameters
        {
            get
            {
                return parameters.Select(parameter =>
                {
                    var weakReference = parameter as WeakReference;
                    return (weakReference != null) ? weakReference.Target : parameter;
                }).ToArray();
            }

            private set { parameters = value.ToArray(); }
        }
    }
}