---
title: Exceptions
permalink: /exceptions/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

The following example verifies that the `Foo()` method throws an `InvalidOperationException` whose `Message` property has a specific value.

```csharp
subject.Invoking(y => y.Foo("Hello"))
    .Should().Throw<InvalidOperationException>()
    .WithMessage("Hello is not allowed at this moment");
```

But if you prefer the arrange-act-assert syntax, you can also use an action in your act part.

```csharp
Action act = () => subject.Foo2("Hello");

act.Should().Throw<InvalidOperationException>()
    .WithInnerException<ArgumentException>()
    .WithMessage("whatever");
```

Notice that the example also verifies that the exception has a particular inner exception with a specific message. In fact, you can even check the individual properties of the exception instance using the And property.

```csharp
Action act = () => subject.Foo(null);

act.Should().Throw<ArgumentNullException>()
    .WithParameterName("message");
```

An alternative syntax for doing the same is by chaining one or more calls to the `Where()` method:

```csharp
Action act = () => subject.Foo(null);
act.Should().Throw<ArgumentNullException>().Where(e => e.Message.StartsWith("did"));
```

However, we discovered that testing the exception message for a substring is so common, that we changed the default behavior of `WithMessage` to support wildcard expressions and match in a case-insensitive way.

The pattern can be a combination of literal and wildcard characters, but it doesn't support regular expressions.

The following wildcard specifiers are permitted in the pattern:

| Wilcard specifier | Matches                                   |
| ----------------- | ----------------------------------------- |
| * (asterisk)      | Zero or more characters in that position. |
| ? (question mark) | Exactly one character in that position.   |

```csharp
Action act = () => subject.Foo(null);
act
  .Should().Throw<ArgumentNullException>()
  .WithMessage("?did*");
```

On the other hand, you may want to verify that no exceptions were thrown.

```csharp
Action act = () => subject.Foo("Hello");
act.Should().NotThrow();
```

We know that a unit test will fail anyhow if an exception was thrown, but this syntax returns a clearer description of the exception that was thrown and fits better to the AAA syntax.

If you want to verify that a specific exception is not thrown, and want to ignore others, you can do that using an overload:

```csharp
Action act = () => subject.Foo("Hello");
act.Should().NotThrow<InvalidOperationException>();
```

Sometimes you may want to retry an assertion until it either succeeds or a given time elapses. For instance, you could be testing a network service which should become available after a certain time, say, 10 seconds:

```csharp
Action act = () => service.IsReady().Should().BeTrue();
act.Should().NotThrowAfter(10.Seconds(), 100.Milliseconds());
```

The second argument of `NotThrowAfter` specifies the time that should pass before `act` is executed again after an execution which threw an exception.

If the method you are testing returns an `IEnumerable` or `IEnumerable<T>` and it uses the `yield` keyword to construct that collection, just calling the method will not cause the effect you expected because the real work is not done until you actually iterate over that collection. You can use the `Enumerating()` extension method to force enumerating the collection like this.

```csharp
Func<IEnumerable<char>> func = () => obj.SomeMethodThatUsesYield("blah");
func.Enumerating().Should().Throw<ArgumentException>();
```

You do have to use the `Func<T>` type instead of `Action<T>` then.

Or you can do it like this:

```csharp
obj.Enumerating(x => x.SomeMethodThatUsesYield("blah")).Should().Throw<ArgumentException>();
```

The exception throwing API follows the same rules as the `try`...`catch`...construction does.
In other words, if you're expecting a certain exception to be (not) thrown, and a more specific exception is thrown instead, it would still satisfy the assertion.
So throwing an `ApplicationException` when an `Exception` was expected will not fail the assertion.
However, if you really want to be explicit about the exact type of exception, you can use `ThrowExactly` and `WithInnerExceptionExactly`.

Talking about the `async` keyword, you can also verify that an asynchronously executed method throws or doesn't throw an exception:

```csharp
Func<Task> act = () => asyncObject.ThrowAsync<ArgumentException>();
await act.Should().ThrowAsync<InvalidOperationException>();
await act.Should().NotThrowAsync();
```

Alternatively, you can use the `Awaiting` method like this:

```csharp
Func<Task> act = () => asyncObject.Awaiting(x => x.ThrowAsync<ArgumentException>());
await act.Should().ThrowAsync<ArgumentException>();
```

Both give you the same results, so it's just a matter of personal preference.

As for synchronous methods, you can also check that an asynchronously executed method executes successfully after a given wait time using `NotThrowAfter`:

```csharp
Stopwatch watch = Stopwatch.StartNew();
Func<Task> act = async () =>
{
    if (watch.ElapsedMilliseconds <= 1000)
    {
        throw new ArgumentException("The wait time has not yet elapsed.");
    }

    await Task.CompletedTask;
};

await act.Should().ThrowAsync<ArgumentException>();
await act.Should().NotThrowAfterAsync(2.Seconds(), 100.Milliseconds());
```

If you prefer single-statement assertions, consider using the `FluentActions` static class, which has `Invoking`, `Awaiting`, and `Enumerating` methods:

```csharp
FluentActions.Invoking(() => MyClass.Create(null)).Should().Throw<ArgumentNullException>();
```

To make it even more concise:

```csharp
using static FluentAssertions.FluentActions;

...

Invoking(() => MyClass.Create(null)).Should().Throw<ArgumentNullException>();
```

### Automatic AggregateException unwrapping ###

.NET 4.0 and later includes the `AggregateException` type. This exception type is typically thrown by methods which return either `Task` or `Task<TResult>` and are executed synchronously, instead of using `async` and `await`. This type contains a collection of inner exceptions which are aggregated.

Methods such as `Throw<TException>`, `ThrowAsync<TException>`, `NotThrow<TException>` and `NotThrowAsync<TException>` described above will also work for exceptions that are aggregated, whether or not you are asserting on the actual `AggregateException` or any of its (nested) aggregated exceptions.

However, the `ThrowExactly<TException>` and `ThrowExactlyAsync<TException>` methods will only work for exceptions that aren't aggregated. If you are asserting that an exception type other than `AggregateException` is thrown, an `AggregateException` must not be thrown, even if it contains an inner exception of the asserted type.
