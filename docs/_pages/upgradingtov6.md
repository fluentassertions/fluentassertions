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

If your assertions rely on the formatting of enums in failure messages, you'll notice that we have given it a facelift.
Previously, formatting an enum would simply be a call to `ToString()`, but to provide more detail we now format `MyEnum.One` as `"MyEnum.One(1)"` instead of `"One"`.