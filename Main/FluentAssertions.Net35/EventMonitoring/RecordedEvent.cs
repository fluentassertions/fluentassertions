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
        private object [] parameters;

        /// <summary>
        /// Default constructor stores the parameters the event was raised with
        /// </summary>
        public RecordedEvent(object monitoredObject, params object [] parameters)
        {
            var eventArguments = parameters.Select(p => p == monitoredObject ? new ParameterWeakReference(p) : p);
            Parameters = eventArguments;
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
                    var weakReference = parameter as ParameterWeakReference;
                    return (weakReference != null) ? weakReference.Target : parameter;
                });
            }

            private set { parameters = value.ToArray(); }
        }

        private class ParameterWeakReference : WeakReference
        {
            public ParameterWeakReference(object target)
                : base(target)
            {
            }
        }
    }
}