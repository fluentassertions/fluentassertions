---
title: Specialized
permalink: /specialized/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

The class `TaskCompletionSource<T>` is quite useful in unit tests on asynchronous calls.
The following assertions helps to check that the result is available within specific time.

```csharp
var tcs = new TaskCompletionSource<bool>();
await tcs.Should().CompleteWithinAsync(1.Seconds());
```

In case the time range is not part of the behavior to be defined,
you can use a specific overload defaulting to timeout defined at `AssertionOptions.TaskTimeout`.

```csharp
var tcs = new TaskCompletionSource<bool>();
await tcs.Should().CompleteAsync();
```

These assertions returns the result for subsequent value assertions.

```csharp
var tcs = new TaskCompletionSource<bool>();
(await tcs.Should().CompleteAsync()).Which.Should().BeTrue();
```
