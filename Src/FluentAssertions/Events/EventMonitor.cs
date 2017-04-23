using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Events
{
    internal partial class EventMonitor : IEventMonitor
    {
        [ThreadStatic]
        private static EventRecordersMap eventRecordersMap;

        private static EventRecordersMap Map
        {
            get
            {
                eventRecordersMap = eventRecordersMap ?? new EventRecordersMap();
                return eventRecordersMap;
            }
        }

#if NET45
        public static IEventMonitor Attach(object eventSource, Type type)
#else
        public static IEventMonitor Attach(System.ComponentModel.INotifyPropertyChanged eventSource, Type type)
#endif
        {
            if (eventSource == null)
            {
                throw new ArgumentNullException(nameof(eventSource), "Cannot monitor the events of a <null> object.");
            }

            IEventMonitor eventMonitor;
            if (!Map.TryGetMonitor(eventSource, out eventMonitor))
            {
                eventMonitor = new EventMonitor(eventSource);
                Map.Add(eventSource, eventMonitor);
            }

            eventMonitor.Attach(type);
            return eventMonitor;
        }

        public static IEventMonitor Get(object eventSource)
        {
            return Map[eventSource];
        }
    }

#if NET45
    internal partial class EventMonitor
    {
        private readonly WeakReference eventSource;
        private readonly IDictionary<string, IEventRecorder> registeredRecorders = new Dictionary<string, IEventRecorder>();


        private EventMonitor(object eventSource)
        {
            if (eventSource == null)
            {
                throw new ArgumentNullException(nameof(eventSource), "Cannot monitor the events of a <null> object.");
            }

            this.eventSource = new WeakReference(eventSource);
        }

        public void Reset()
        {
            foreach (var recorder in registeredRecorders.Values)
            {
                recorder.Reset();
            }
        }

        public void Attach(Type typeDefiningEventsToMonitor)
        {
            if (eventSource.Target == null) throw new InvalidOperationException("Cannot monitor events on garbage-collected object");

            var events = typeDefiningEventsToMonitor.GetEvents();
            if (!events.Any())
            {
                throw new InvalidOperationException($"Type {typeDefiningEventsToMonitor.Name} does not expose any events.");
            }

            foreach (var eventInfo in events)
            {
                EnsureEventHandlerAttached(eventInfo);
            }
        }

        public IEventRecorder GetEventRecorder(string eventName)
        {
            IEventRecorder recorder;
            if (!registeredRecorders.TryGetValue(eventName, out recorder))
            {
                throw new InvalidOperationException($"Not monitoring any events named \"{eventName}\".");
            }
            return recorder;
        }

        private void EnsureEventHandlerAttached(EventInfo eventInfo)
        {
            IEventRecorder recorder;
            if (!registeredRecorders.TryGetValue(eventInfo.Name, out recorder))
            {
                recorder = new EventRecorder(eventSource.Target, eventInfo.Name);
                registeredRecorders.Add(eventInfo.Name, recorder);
                var handler = EventHandlerFactory.GenerateHandler(eventInfo.EventHandlerType, recorder);
                eventInfo.AddEventHandler(eventSource.Target, handler);
            }
        }
    }
#else
    internal partial class EventMonitor
    {
        private readonly EventRecorder eventRecorder;

        public EventMonitor(System.ComponentModel.INotifyPropertyChanged eventSource)
        {
            eventRecorder = new EventRecorder(eventSource, "PropertyChanged");
            eventSource.PropertyChanged += (sender, args) => eventRecorder.RecordEvent(sender, args);
        }
        
        public void Reset()
        {
            eventRecorder.Reset();
        }

        public void Attach(Type typeDefiningEventsToMonitor)
        {
            if (typeDefiningEventsToMonitor != typeof(System.ComponentModel.INotifyPropertyChanged))
            {
                throw new NotSupportedException($"Cannot monitor events of type \"{typeDefiningEventsToMonitor.Name}\".");
            }
        }

        public IEventRecorder GetEventRecorder(string eventName)
        {
            switch (eventName)
            {
            case "PropertyChanged":
                return eventRecorder;
            
            default:
                throw new InvalidOperationException($"Not monitoring any events named \"{eventName}\".");
            }
        }
    }
#endif
}
