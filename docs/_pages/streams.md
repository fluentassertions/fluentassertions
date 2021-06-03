---
title: Streams
permalink: /streams/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

## Streams ##

```csharp
var stream = new MemoryStream(new byte[1024], writable: false);

stream.Should().NotBeWritable();
stream.Should().BeReadable();
stream.Should().BeReadOnly();
stream.Should().BeSeekable();

stream.Should().HaveLength(1024);
stream.Should().HavePosition(0);

```

There are also additional assertions for `BufferedStream`.

```csharp
var stream = new BufferedStream(new MemoryStream(), 1024)

subject.Should().HaveBufferSize(1024);
subject.Should().NotHaveBufferSize(2048);
```
