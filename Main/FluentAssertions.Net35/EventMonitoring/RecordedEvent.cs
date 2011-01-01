using System.Diagnostics;

namespace FluentAssertions.EventMonitoring
{
    /// <summary>
    ///   This class is used to store data about an intercepted event
    /// </summary>
    [DebuggerNonUserCode]
    public class RecordedEvent
    {
        /// <summary>
        ///   Default constructor stores the parameters the event was raised with
        /// </summary>
        public RecordedEvent(params object[] parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        ///   Parameters for the event
        /// </summary>
        public object[] Parameters { get; private set; }
    }
}