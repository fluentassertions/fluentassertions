---
title: Upgrading to version 6.0
permalink: /upgradingtov6
layout: single
toc: true
sidebar:
  nav: "sidebar"
---

## Enums ##

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
`HaveFlag` was written long before the enum type constraint was introduced i C# 7.3, so checking whether the subject and expectation was the same of enum was performed on runtime instead of compile-time.

`Be` has some workarounds to ensure that a boxed `1` and boxed `1.0` are considered to be equal.
This approach did not work well for enums as it would consider two enums to be equal if their underlying integral values are equal.
To put it in code, in the snippet below all four assertions would pass, but only the first one should.
```c#
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
An enum is now only considered to be equivalent to an enum of the same of another type, but you can control whether they should equal by name or by value.
The practical implications are that the following examples now fails.
```cs
var subject = new { Value = "One" };
var expectation = new { Value = MyOtherEnum.One };
subject.Should().BeEquivalentTo(expectation,  opt => opt.ComparingEnumsByName());

var subject = new { Value = 1 };
var expectation = new { Value = MyOtherEnum.One };
subject.Should().BeEquivalentTo(expectation,  opt => opt.ComparingEnumsByValue());
```

If your assertions rely on the formatting of enums in failure messages, you'll notice that we have given it a facelift.
Previously, formatting an enum would simply be a call to `ToString()`, but to provide more detail we now format `MyEnum.One` as `"MyEnum.One(1)"` instead of `"One"`.


## Using ##

Since v2, released back in late 2012, the syntax for overriding the default comparison of properties during structural equivalency has more or less been
```cs
orderDto.Should().BeEquivalentTo(order, opt => opt
    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Seconds()))
    .WhenTypeIs<DateTime>());
```

As there were no restrictions on the relationship between the generic parameters of `Using<TProperty>` and `WhenTypeIs<TMemberType>` you could write nonsense such as
```
var subject = new { Value = "One" };
var expectation = new { Value = "Two" };

subject.Should().BeEquivalentTo(expectation, opt => opt
    .Using<int>(e => e.Subject.Should().Be(e.Expectation))
    .WhenTypeIs<string>()
);
```

This would compile, but then fail at runtime with

```
Expected member Value from subject to be a System.Int32, but found a System.String.
Expected member Value from expectation to be a System.Int32, but found a System.String.
```

In v6 we have restricted this relationship between `WhenTypeIs` and `Using`, such that `TMemberType` must be assignable to `TProperty`.
The snippet above now gives a compile error
```
CS0311: There is no implicit reference conversion from 'string' to 'int'.
```

This change also breaks compilation for cases that might worked before, but only due to assumptions about the runtime values.

```
.Using<Derived>()
.WhenTypeIs<Base>() // assuming that all Bases are of type `Derived`
```

```cs
.Using<int>()
.WhenTypeIs<int?>() // null is an int? but not an int
```

```cs
.Using<int?>()
.WhenTypeIs<int>() // This would work, but there's no reason to cast int to int?
```

Besides the generic constraint, we also fixed two cases regarding non-nullable values, that we didn't handle correctly before.

In the first case, we would match both `null` and `0` as an `int?`, but then cast both to `int`, which gave a `NullReferenceException`.
```cs
var subject = new { Value = null as int? };
var expectation = new { Value = 0 };

subject.Should().BeEquivalentTo(expectation, opt => opt
    .Using<int>(e => e.Subject.Should().Be(e.Expectation))
    .WhenTypeIs<int?>()
);
```

In the second case we would cast a `null` expectation to `default(TMember)`, which worked fine for reference types, but for e.g. `int` this meant that we considered `null` to be equal to `0`.

```cs
var subject = new { Value = 0 };
var expectation = new { Value = null as int? };

subject.Should().BeEquivalentTo(expectation, opt => opt
    .Using<int>(e => e.Subject.Should().Be(e.Expectation))
    .WhenTypeIs<int?>()
);
```