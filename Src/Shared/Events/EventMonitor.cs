using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Events
{

#if !SILVERLIGHT && !WINRT && !PORTABLE && !CORE_CLR
    internal class EventMonitor : IEventMonitor
    {
        private readonly IReadOnlyCollection<EventRecorder> eventRecorders;

        public EventMonitor(object eventSource, Type typeDefiningEventsToMonitor)
        {
            if (eventSource == null)
            {
                throw new ArgumentNullException( nameof(eventSource), "Cannot monitor the events of a <null> object." );
            }
            
            this.eventRecorders = BuildRecorders( eventSource, typeDefiningEventsToMonitor );
        }

        public void Reset()
        {
            foreach (var recorder in eventRecorders)
            {
                recorder.Reset();
            }
        }

        private static EventRecorder[] BuildRecorders( object eventSource, Type eventSourceType )
        {
            EventRecorder[] recorders = eventSourceType
                .GetEvents()
                .Select( @event => CreateEventHandler( eventSource, @event ) )
                .ToArray();

            if ( !recorders.Any() )
            {
                throw new InvalidOperationException($"Type {eventSourceType.Name} does not expose any events." );
            }

            return recorders;
        }

        private static EventRecorder CreateEventHandler( object eventSource, EventInfo eventInfo )
        {
            var eventRecorder = new EventRecorder( eventSource, eventInfo.Name );

            Delegate handler = EventHandlerFactory.GenerateHandler( eventInfo.EventHandlerType, eventRecorder );
            eventInfo.AddEventHandler( eventSource, handler );

            return eventRecorder;
        }
    }
#else
    internal class EventMonitor : IEventMonitor
    {
        private readonly EventRecorder eventRecorder;

        public EventMonitor(INotifyPropertyChanged eventSource)
        {
            eventRecorder = new EventRecorder(eventSource, "PropertyChanged");
            eventSource.PropertyChanged += (sender, args) => eventRecorder.RecordEvent(sender, args);
        }
        
        public void Reset()
        {
            eventRecorder.Reset();
        }
    }
#endif
}
