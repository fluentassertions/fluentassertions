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

The assertion returns the result for subsequent value assertions.

```csharp
var tcs = new TaskCompletionSource<bool>();
(await tcs.Should().CompleteWithinAsync(1.Seconds())).Which.Should().BeTrue();
```
