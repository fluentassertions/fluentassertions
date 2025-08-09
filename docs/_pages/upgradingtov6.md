---
title: Upgrading to version 6.0
permalink: /upgradingtov6
layout: single
toc: true
sidebar:
  nav: "sidebar"
---

## Enums

In Fluent Assertions v5 enums were handled by the `ObjectAssertions` class, which is what you have in hand when invoking `Should()` on any type not handled by a specialized overload of `Should()`.
`ObjectAssertions` derives from `ReferenceTypeAssertions`, so all these assertions were available when asserting on an enum.

* `[Not]Be(object)`
* `[Not]BeEquivalentTo<T>(T)`
* `[Not]BeNull()`
* `[Not]BeSameAs(object)`
* `[Not]BeOfType<T>()`
* `[Not]BeOfType(Type)`
* `[Not]BeAssignableTo<T>()`
* `[Not]BeAssignableTo(Type)`
* `Match(Func<object, bool>)`
* `Match<T>(Func<T, bool>)`
* `[Not]HaveFlag(Enum)`

This design had several downsides.

A lot, if not most, of the assertions listed above are irrelevant for enums.
Conversely, `HaveFlag` is only relevant for enums, but it was always available even though your object was not an enum.
We wanted to introduce new enum specific assertions, but adding those would be even more auto-complete noise for non enums.
`HaveFlag` was written long before the enum type constraint was introduced in C# 7.3, so checking whether the subject and expectation was the same of enum was performed on runtime instead of compile-time.

`Be` has some workarounds to ensure that a boxed `1` and boxed `1.0` are considered to be equal.
This approach did not work well for enums as it would consider two enums to be equal if their underlying integral values are equal.
To put it in code, in the snippet below all four assertions would pass, but only the first one should.

```csharp
public enum MyEnum { One = 1 }
public enum MyOtherEnum { One = 1 }

MyEnum.One.Should().Be(MyEnum.One);

MyEnum.One.Should().Be(MyOtherEnum.One);
MyEnum.One.Should().Be(1);
1.As<object>().Should().Be(MyOtherEnum.One);
```

In Fluent Assertions v6 enums are now handled by `EnumAssertions` which should cover the same use cases and even more, but without the old bugs and unexpected surprises.
Technically nullable enums are handled by `NullableEnumAssertions` which in addition to the assertions mentioned below have `[Not]BeNull` and `[Not]HaveValue`.
`Be` and `HaveFlag` now calls directly into `Enum.Equals` and `Enum.HasFlag`, as you would expect, and any try to compare an enum with another type of enum or integer leads to a compilation error.
If you want to compare two enums of different types, you can use `HaveSameValueAs` or `HaveSameNameAs` depending on how _you_ define equality for different enums.
Lastly, if you want to verify than an enum has a specific integral value, you can use `HaveValue`.

When comparing object graphs with enum members, we have constrained when we consider them to be equivalent.
An enum is now only considered to be equivalent to an enum of the same or another type, but you can control whether they should equal by name or by value.
The practical implications are that the following examples now fails.

```csharp
var subject = new { Value = "One" };
var expectation = new { Value = MyOtherEnum.One };
subject.Should().BeEquivalentTo(expectation,  opt => opt.ComparingEnumsByName());

var subject = new { Value = 1 };
var expectation = new { Value = MyOtherEnum.One };
subject.Should().BeEquivalentTo(expectation,  opt => opt.ComparingEnumsByValue());
```

If your assertions rely on the formatting of enums in failure messages, you'll notice that we have given it a facelift.
Previously, formatting an enum would simply be a call to `ToString()`, but to provide more detail we now format `MyEnum.One` as `"MyEnum.One(1)"` instead of `"One"`.

## IEquivalencyStep

In v6, we applied some major refactoring to the equivalency validator, of which most of it is internal and therefore won't be visible to consumers of the library. But one thing that does, is that we split off the subject and expectation from the `IEquivalencyValidationContext` and move them into their own type called `Comparands`. Since this affected the `IEquivalencyStep` and we already had some ideas to simplify that abstraction, we removed the `CanHandle` method and replaced the Boolean return value of `Handle` with a more self-describing `EquivalencyResult`. The consequence of this is that `Handle` must first check whether the comparands are applicable to the step and bail out with `EquivalencyResult.ContinueWithNext` if that isn't the case. There's a convenience base-class called `EquivalencyStep<T>` that remove some of that burden for you. Check out `DictionaryEquivalencyStep` for an example of that. Also, the [extensibility section](extensibility/#equivalency-assertion-step-by-step) has been updated to reflect the new signatures and types.

## Using

Since v2, released back in late 2012, the syntax for overriding the default comparison of properties during structural equivalency has more or less been

```csharp
orderDto.Should().BeEquivalentTo(order, opt => opt
    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Seconds()))
    .WhenTypeIs<DateTime>());
```

As there were no restrictions on the relationship between the generic parameters of `Using<TProperty>` and `WhenTypeIs<TMemberType>` you could write nonsense such as

```csharp
var subject = new { Value = "One" };
var expectation = new { Value = "Two" };

subject.Should().BeEquivalentTo(expectation, opt => opt
    .Using<int>(e => e.Subject.Should().Be(e.Expectation))
    .WhenTypeIs<string>()
);
```

This would compile, but then fail at runtime with

```text
Expected member Value from subject to be a System.Int32, but found a System.String.
Expected member Value from expectation to be a System.Int32, but found a System.String.
```

In v6 we have restricted this relationship between `WhenTypeIs` and `Using`, such that `TMemberType` must be assignable to `TProperty`.
The snippet above now gives a compile error

```text
CS0311: There is no implicit reference conversion from 'string' to 'int'.
```

This change also breaks compilation for cases that might worked before, but only due to assumptions about the runtime values.

```csharp
.Using<Derived>()
.WhenTypeIs<Base>() // assuming that all Bases are of type `Derived`
```

```csharp
.Using<int>()
.WhenTypeIs<int?>() // null is an int? but not an int
```

```csharp
.Using<int?>()
.WhenTypeIs<int>() // This would work, but there's no reason to cast int to int?
```

Besides the generic constraint, we also fixed two cases regarding non-nullable values, that we didn't handle correctly before.

In the first case, we would match both `null` and `0` as an `int?`, but then cast both to `int`, which gave a `NullReferenceException`.

```csharp
var subject = new { Value = null as int? };
var expectation = new { Value = 0 };

subject.Should().BeEquivalentTo(expectation, opt => opt
    .Using<int>(e => e.Subject.Should().Be(e.Expectation))
    .WhenTypeIs<int?>()
);
```

In the second case we would cast a `null` expectation to `default(TMember)`, which worked fine for reference types, but for e.g. `int` this meant that we considered `null` to be equal to `0`.

```csharp
var subject = new { Value = 0 };
var expectation = new { Value = null as int? };

subject.Should().BeEquivalentTo(expectation, opt => opt
    .Using<int>(e => e.Subject.Should().Be(e.Expectation))
    .WhenTypeIs<int?>()
);
```

## Value Formatters

Within Fluent Assertions, the `Formatter` class is responsible for rendering a textual representation of the objects involved in an assertion. Those objects can turn out to be entire graphs, especially when you use `BeEquivalentTo`. Rendering such a graph can be an expensive operation, so in 5.x we already had limits on how deep the `Formatter` would traverse the object graph. Because we received several performance-related issues, we decided to slightly redesign how implementations of `IValueFormatter` should work. This unfortunately required us to introduce some breaking changes in the signature of the `Format` method as well as some behavioral changes. You can read all about that in the updated [extensibility guide](/extensibility/#rendering-objects-with-beauty), but the gist of it is that instead of returning a `string`, you now need to use the `FormattedObjectGraph`, which acts like a kind of `StringBuilder`. For instance, this is what the `StringValueFormatter` now looks like:

```csharp
public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
{
    string result = "\"" + value + "\"";

    if (context.UseLineBreaks)
    {
        formattedGraph.AddFragmentOnNewLine(result);
    }
    else
    {
        formattedGraph.AddFragment(result);
    }
}
```

## Collections

As part of embracing the generic type system and improving the maintainability of the code base, we have removed support for non-generic collections.
The overload of `Should()` taking an `IEnumerable` has been removed and the new best matching overload of `Should` (from a compiler perspective) is now the one that returns an `ObjectAssertions`.

```csharp
IEnumerable subject;

subject.Should().HaveCount(42); // No longer compiles
subject.Cast<object>().Should().HaveCount(42);
```

With Fluent Assertion 5.0 we [redefined equivalency](https://www.continuousimprover.com/2018/02/fluent-assertions-50-best-unit-test.html#redefining-equivalency) to let the expectation drive the comparison.
If your expectation is an interface we will by default only compare the subset of members defined on the interface, but as always you can override it by using `RespectingRuntimeTypes`.
To live up to this design we have removed `BeEquivalentTo(params object[])` as it discarded compile time type information about the expectation.

In the example below all expectations are of type `I`, so `AA`, which is defined on `A`, should not be included in the comparison and all assertions should pass.

```csharp
interface I
{
    int II { get; set; }
}

class A : I
{
    public int II { get; set; }
    public int AA { get; set; }
}

A[] items = new[] { new A() { AA = 42 } };

items.Should().BeEquivalentTo((I)new A()); // (1)
items.Should().BeEquivalentTo(new I[] { new A() }); // (2)
items.Should().BeEquivalentTo(new List<I> { new A() }); // (3)
items.Should().BeEquivalentTo(new I[] { new A() }, opt => opt); // (4)
```

In Fluent Assertions 5.0 `(1)` and `(2)` both used the `BeEquivalentTo(params object[])` overload, which meant the expectation was seen as `object`s with runtime time `A` and now `AA` was unexpectedly included in the comparison.
Case `(3)` works as expected, as `List<T>` is not implicitly convertible to `T[]` and the compiler picks `BeEquivalentTo(IEnumerable<I>)`, which retains the compile time information about the expectation being objects of type `I`.
A case which led to some confusion among our users was that `(2)` changed behavior when adding assertion options, as shown in `(4)`, as `BeEquivalentTo(IEnumerable<T>, Func<EquivalencyAssertionOptions<I>>)` was now picked because `I[]` is implicitly convertible to `IEnumerable<I>`.

You might ask if we could not just have changed the overload signature from `params object[]` to `params T[]` to solve the type problem while keeping the convenient overload.
The answer is no (as far as we know), as the compiler prefers resolving e.g. `IEnumerable<T>` to `params IEnumerable<T>[]` over `IEnumerable<T>`.

For Fluent Assertions 6.0 the implications of this are that `(1)` no longer compiles, as `BeEquivalentTo` takes an `IEnumerable<T>`, but `(2)` now works as expected.

## Asserting exceptions asynchronously 
In the past, we would invoke asynchronous code by wrapping it in a synchronously blocking call. Unfortunately this resulted in occasional deadlocks, which we fixed by temporarily clearing the `SynchronizationContext`. This worked, but felt like an ugly workaround. So in v6, we decided to fully embrace asynchronous code and make some of the assertion APIs async itself. This means that using `Should().Throw()`, `ThrowExactly()` and `NotThrow()` will no longer magically work on `async` code and you need to use the versions with the `Async` postfix instead.

For background information on this decision, check out the following articles:

* [Asynchronous programming with async and await](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/)
* [Asynchronous Programming](https://docs.microsoft.com/en-us/dotnet/csharp/async)
* [Don't Block on Async Code](https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html)
* [Understanding Async, Avoiding Deadlocks in C#](https://medium.com/rubrikkgroup/understanding-async-avoiding-deadlocks-e41f8f2c6f5d)

## Chaining event assertions
As part of fixing [this PR](https://github.com/fluentassertions/fluentassertions/issues/337) we had to change the event assertions work. Now, event constraining extensions like `WithArgs` and `WithSender` will return only the events that match the constraints. This means that this will no longer work:

```csharp
foo
    .ShouldRaise("SomeEvent")
         .WithArgs<string>(args => args == "some event payload")
         .WithArgs<string>(args => args == "other payload");
```

In 6.0, you need to write this as:

```csharp
foo
    .ShouldRaise("SomeEvent")
    .WithArgs<string>(args => args == "some event payload");

foo
    .ShouldRaise("SomeEvent")
    .WithArgs<string>(args => args == "other payload");
```
