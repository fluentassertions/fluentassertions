---
title: JSON
permalink: /json/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

For projects targeting .NET 6 or later, there's built-in support for assertions on the types `JsonNode` and `JsonArray` (including `JsonNode` derived classes, e.g. `JsonValue` and `JsonObject`) from the `System.Text.Json` namespace.

```csharp
var jsonNode = JsonNode.Parse("{ \"name\": \"John\" }");
jsonNode.Should().HaveProperty("name").Which.ToString().Should().Be("John");
jsonNode.Should().NotHaveProperty("code");
jsonNode.BeAnArray().Which.Should().HaveCount(3);
jsonNode.NotBeAnArray();

var jsonNode = JsonNode.Parse("\"Hello World\"");
jsonNode.Should().BeString().Which.Should().Be("Hello World");
jsonNode.Should().NotBeString();

var jsonNode = JsonNode.Parse("42");
jsonNode.Should().BeNumeric().Which.Should().Be(42);
jsonNode.Should().NotBeNumeric();

var jsonNode = JsonNode.Parse("true");
jsonNode.Should().BeBool().Which.Should().BeTrue();
jsonNode.Should().NotBeBool();
```

Although there's no official date time type in JSON like there is for numbers and arrays, there still is the ISO 8601 standard. So Fluent Assertions recognizes those dates like this:

```csharp
var jsonNode = JsonNode.Parse("\"2025-09-11T21:17:00\"");
jsonNode.Should().BeLocalDate().Which.Should().Be(11.September(2025).At(21, 17).AsLocal());
jsonNode.Should().NotBeUtcDate();

var jsonNode = JsonNode.Parse("\"2025-09-11T21:17:00Z\"");
jsonNode.Should().BeUtcDate().Which.Should().Be(11.September(2025).At(21, 17).AsUtc());
jsonNode.Should().NotBeLocalDate();
```

JSON arrays get special treatment as well as the same assertions that are available to collections are available here as well.

```csharp
JsonArray array = JsonNode.Parse("[1, 2, 3]")!.AsArray();
array.Should().NotBeEmpty();
```