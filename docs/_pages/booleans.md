---
title: Booleans
permalink: /booleans/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

```csharp
bool theBoolean = false;
theBoolean.Should().BeFalse("it's set to false");

theBoolean = true;
theBoolean.Should().BeTrue();
theBoolean.Should().Be(otherBoolean);
theBoolean.Should().NotBe(false);
```

Obviously the above assertions also work for nullable booleans, but if you really want to be make sure a boolean is either `true` or `false` and not `null`, you can use these methods.

```csharp
theBoolean.Should().NotBeFalse();
theBoolean.Should().NotBeTrue();
```
