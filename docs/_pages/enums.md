---
title: Enums
permalink: /enums/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

## Enums

Fluent Assertions have several ways to compare enums.

The basic ones, `Be` and `HaveFlag`, just calls directly into `Enum.Equals` and `Enum.HasFlag`.

```csharp
enum MyEnum { One = 1, Two = 2, Three = 3}

myEnum = MyEnum.One;

myEnum.Should().Be(MyEnum.One);
myEnum.Should().NotBe(MyEnum.Two);
myEnum.Should().BeOneOf(MyEnum.One, MyEnum.Two);

regexOptions.Should().HaveFlag(RegexOptions.Global);
regexOptions.Should().NotHaveFlag(RegexOptions.CaseInsensitive);
```

If you want to compare enums of different types, you can use `HaveSameValueAs` or `HaveSameNameAs` depending on how _you_ define equality for different enums.

```csharp
enum SameNameEnum { One = 11 }
enum SameValueEnum { OneOne = 1 }

MyEnum.One.Should().HaveSameNameAs(SameNameEnum.One);
MyEnum.One.Should().HaveSameValueAs(SameValueEnum.OneOne);

MyEnum.One.Should().NotHaveSameNameAs(SameValueEnum.OneOne);
MyEnum.One.Should().NotHaveSameValueAs(SameNameEnum.One);
```

Lastly, if you want to verify that an enum has a specific integral value, you can use `HaveValue`.

```csharp
MyEnum.One.Should().HaveValue(1);
MyEnum.One.Should().NotHaveValue(2);
```

```csharp
var myEnum = (MyEnum)1;
myEnum.Should().BeDefined();

myEnum = (MyEnum)99;
myEnum.Should().NotBeDefined();
```
