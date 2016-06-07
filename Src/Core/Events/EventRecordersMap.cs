using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FluentAssertions.Events
{
    /// <summary>
    ///     Simple dictionary that uses a <see cref = "WeakReference" /> to the event source as the key. 
    ///     This should ensure the Garbage Collector can still clean-up the event source object.
    /// </summary>
    [DebuggerNonUserCode]
    public sealed class EventRecordersMap
    {
        private readonly Dictionary<WeakReference, IEventMonitor> map = new Dictionary<WeakReference, IEventMonitor>();

        public void Add(object eventSource, IEventMonitor recorder )
        {
            ForEach(eventSource, keyValuePair => map.Remove(keyValuePair.Key));

            map.Add(new WeakReference(eventSource), recorder );
        }

        public IEventMonitor this[object eventSource]
        {
            get
            {
                IEventMonitor result = null;
                TryGetMonitor( eventSource, out result );

                if (result == null)
                {
                    throw new InvalidOperationException(string.Format(
                        "Object <{0}> is not being monitored for events or has already been garbage collected. " +
                            "Use the MonitorEvents() extension method to start monitoring events.", eventSource));
                }

                return result;
            }
        }

        public bool TryGetMonitor(object eventSource, out IEventMonitor eventMonitor)
        {
            IEventMonitor result = null;
            ForEach( eventSource, pair => result = pair.Value );
            eventMonitor = result;
            return eventMonitor != null;
        }

        private void ForEach(object eventSource, Action<KeyValuePair<WeakReference, IEventMonitor>> action)
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