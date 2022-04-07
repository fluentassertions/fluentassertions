---
title: Event Monitoring
permalink: /eventmonitoring/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

Fluent Assertions has a set of extensions that allow you to verify that an object raised a particular event.
Before you can invoke the assertion extensions, you must first tell Fluent Assertions that you want to monitor the object:

```csharp
var subject = new EditCustomerViewModel();
using var monitoredSubject = subject.Monitor();

subject.Foo();
monitoredSubject.Should().Raise("NameChangedEvent");
```

Notice that Fluent Assertions will keep monitoring the `subject` for as long as the `using` block lasts.

Assuming that we’re dealing with an MVVM implementation, you might want to verify that it raised its `PropertyChanged` event for a particular property:

```csharp
monitoredSubject
  .Should().Raise("PropertyChanged")
  .WithSender(subject)
  .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == "SomeProperty");
```

`WithSender()` will verify that all occurrences of the event had their `sender` argument set to the specified object.
`WithArgs()` just verifies that at least one occurrence had a matching `EventArgs` object. Both will return an `IEventRecording` representing only the events that match the constraint.

This means that event monitoring only works for events that comply with the standard two-argument `sender`/`args` .NET pattern.

Since verifying for `PropertyChanged` events is so common, we've included a specialized shortcut to the example above:

```csharp
subject.Should().RaisePropertyChangeFor(x => x.SomeProperty);
```

You can also do the opposite; asserting that a particular event was not raised.

```csharp
subject.Should().NotRaisePropertyChangeFor(x => x.SomeProperty);
```

Or...

```csharp
subject.Should().NotRaise("SomeOtherEvent");
```

`Monitor()` is a generic method, but you will usually have the compiler infer the type. You _can_ specify an explicit type to limit which events you want to listen to:

```csharp
var subject = new ClassWithManyEvents();
using var monitor = subject.Monitor<IInterfaceWithFewEvents>();
```

This generic version of `Monitor()` is also very useful if you wish to monitor events of a dynamically generated class using `System.Reflection.Emit`. Since events are dynamically generated and are not present in parent class non-generic version of `Monitor()` will not find the events. This way you can tell the event monitor which interface was implemented in the generated class.

```csharp
POCOClass subject = EmitViewModelFromPOCOClass();

using var monitor = subject.Monitor<ISomeInterface>();

// POCO class doesn't have INotifyPropertyChanged implemented
monitor.Should().Raise("SomeEvent");

```

The `IMonitor` interface returned by `Monitor()` exposes a method named `GetRecordingFor` as well as the properties `MonitoredEvents` and `OccurredEvents` that you can use to directly interact with the monitor, e.g. to create your own extensions. For example:

```csharp
var eventSource = new ClassThatRaisesEventsItself();

using IMonitor monitor = eventSource.Monitor<IEventRaisingInterface>();

EventMetadata[] metadata = monitor.MonitoredEvents;

metadata.Should().BeEquivalentTo(new[]
{
    new
    {
        EventName = nameof(IEventRaisingInterface.InterfaceEvent),
        HandlerType = typeof(EventHandler)
    }
});
```

## Limitations

This feature is not available in .NET Standard 2.0, because [`System.Reflection.Emit.DynamicMethod`](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.dynamicmethod) is required to generate event handlers dynamically. 
