---
title: Execution Time
permalink: /executiontime
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

## Members and Actions
Fluent Assertions also provides a method to assert that the execution time of particular method or action does not exceed a predefined value.
To verify the execution time of a method, use the following syntax:

```csharp
public class SomePotentiallyVerySlowClass
{
    public void ExpensiveMethod()
    {
        for (short i = 0; i < short.MaxValue; i++)
        {
            string tmp = " ";
            if (!string.IsNullOrEmpty(tmp))
            {
                tmp += " ";
            }
        }
    }
}
var subject = new SomePotentiallyVerySlowClass();
subject.ExecutionTimeOf(s => s.ExpensiveMethod()).Should().BeLessOrEqualTo(500.Milliseconds());
```

Alternatively, to verify the execution time of an arbitrary action, use this syntax:

```csharp
Action someAction = () => Thread.Sleep(100);
someAction.ExecutionTime().Should().BeLessOrEqualTo(200.Milliseconds());
```

The supported assertions on `ExecutionTime()` are a subset of those found for `TimeSpan`s, namely:
```
someAction.ExecutionTime().Should().BeLessOrEqualTo(200.Milliseconds());
someAction.ExecutionTime().Should().BeLessThan(200.Milliseconds());
someAction.ExecutionTime().Should().BeGreaterThan(100.Milliseconds());
someAction.ExecutionTime().Should().BeGreaterOrEqualTo(100.Milliseconds());
someAction.ExecutionTime().Should().BeCloseTo(150.Milliseconds(), 50.Milliseconds());
```

## Tasks
If you're dealing with a `Task`, you can also assert that it completed within a specified period of time:

```csharp
Func<Task> someAsyncWork = () => SomethingReturningATask();
someAsyncWork.Should().CompleteWithin(100.Milliseconds());
```

This will result in a blocking call, but going fully `async` is supported too:

```csharp
await someAsyncWork.Should().CompleteWithinAsync(100.Milliseconds());
```

If the `Task` is generic and returns a value, you can use that to write a continuing assertion:

```csharp
Func<Task<int>> someAsyncFunc;

someAsyncFunc.Should().CompleteWithin(100.Milliseconds()).Which.Should().Be(42);
```

A fully `async` version is available as well.
