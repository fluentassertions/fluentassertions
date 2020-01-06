---
title: Nullable Types
permalink: /nullabletypes/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

## Nullable types ##

```csharp
short? theShort = null;
theShort.Should().NotHaveValue();
theShort.Should().BeNull();
theShort.Should().Match(x => !x.HasValue || x > 0);

int? theInt = 3;
theInt.Should().HaveValue();
theInt.Should().NotBeNull();

DateTime? theDate = null;
theDate.Should().NotHaveValue();
theDate.Should().BeNull();
```
