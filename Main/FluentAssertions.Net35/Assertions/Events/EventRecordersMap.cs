using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FluentAssertions.EventMonitoring
{
    /// <summary>
    ///     Simple dictionary that uses a <see cref = "WeakReference" /> to the event source as the key. 
    ///     This should ensure the Garbage Collector can still clean-up the event source object.
    /// </summary>
    [DebuggerNonUserCode]
    internal sealed class EventRecordersMap
    {
        private readonly Dictionary<WeakReference, IEnumerable<EventRecorder>> map =
            new Dictionary<WeakReference, IEnumerable<EventRecorder>>();

        public void Add(object eventSource, IEnumerable<EventRecorder> recorders)
        {
            ForEach(eventSource, keyValuePair => map.Remove(keyValuePair.Key));

            map.Add(new WeakReference(eventSource), recorders);
        }

        public IEnumerable<EventRecorder> this[object eventSource]
        {
            get
            {
                IEnumerable<EventRecorder> result = null;

                ForEach(eventSource, pair => result = pair.Value);

                if (result == null)
                {
                    throw new InvalidOperationException(string.Format(
                        "Object <{0}> is not being monitored for events or has already been garbage collected. " +
                            "Use the MonitorEvents() extension method to start monitoring events.", eventSource));
                }

                return result;
            }
        }

        private void ForEach(object eventSource, Action<KeyValuePair<WeakReference, IEnumerable<EventRecorder>>> action)
        {
            foreach (var keyValuePair in map.ToArray())
            {
                if (IsObjectGarbageCollected(keyValuePair.Key))
                {
                    map.Remove(keyValuePair.Key);
                }
                else if (RefersToSameObject(keyValuePair.Key, eventSource))
                {
                    action(keyValuePair);
                }
                else
                {
                    // Ignore
                }
            }
        }

        private static bool RefersToSameObject(WeakReference weakReference, object eventSource)
        {
            return ReferenceEquals(weakReference.Target, eventSource);
        }

        private static bool IsObjectGarbageCollected(WeakReference weakReference)
        {
            return ReferenceEquals(weakReference.Target, null);
        }
    }
}