---
title: Numeric types and everything else that implements IComparable<T\>
permalink: /numerictypes/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

```csharp
int theInt = 5;
theInt.Should().BeGreaterOrEqualTo(5);
theInt.Should().BeGreaterOrEqualTo(3);
theInt.Should().BeGreaterThan(4);
theInt.Should().BeLessOrEqualTo(5);
theInt.Should().BeLessThan(6);
theInt.Should().BePositive();
theInt.Should().Be(5);
theInt.Should().NotBe(10);
theInt.Should().BeInRange(1, 10);
theInt.Should().NotBeInRange(6, 10);
theInt.Should().Match(x => x % 2 == 1);

theInt = 0;
//theInt.Should().BePositive(); => Expected positive value, but found 0
//theInt.Should().BeNegative(); => Expected negative value, but found 0

theInt = -8;
theInt.Should().BeNegative();
int? nullableInt = 3;
nullableInt.Should().Be(3);

double theDouble = 5.1;
theDouble.Should().BeGreaterThan(5);
byte theByte = 2;
theByte.Should().Be(2);
```

Notice that `Should().Be()` and `Should().NotBe()` are not available for floats and doubles. Floating point variables are inheritably inaccurate and should never be compared for equality. Instead, either use the `Should().BeInRange()` method or the following method specifically designed for floating point or `decimal` variables.

```csharp
float value = 3.1415927F;
value.Should().BeApproximately(3.14F, 0.01F);
```

This will verify that the value of the float is between 3.139 and 3.141.

Conversely, to assert that the value differs by an amount, you can do this.

```csharp
float value = 3.5F;
value.Should().NotBeApproximately(2.5F, 0.5F);
```

This will verify that the value of the float is not between 2.0 and 3.0.

To assert that a value matches one of the provided values, you can do this.

```csharp
value.Should().BeOneOf(new[] { 3, 6});
```
