---
title: Guids
permalink: /guids/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

The assertions you can do on Guids are simple.
You can assert their equality to another Guid, or you can assert that a Guid is empty.

```csharp
Guid theGuid = Guid.NewGuid();
Guid sameGuid = theGuid;
Guid otherGuid = Guid.NewGuid();

theGuid.Should().Be(sameGuid);
theGuid.Should().NotBe(otherGuid);
theGuid.Should().NotBeEmpty();

Guid.Empty.Should().BeEmpty();
```
