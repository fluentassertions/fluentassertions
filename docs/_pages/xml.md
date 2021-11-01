---
title: XML
permalink: /xml/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

Fluent Assertions has support for assertions on several of the LINQ-to-XML classes:

```csharp
xDocument.Should().HaveRoot("configuration");
xDocument.Should().HaveElement("settings");
xDocument.Should().HaveSingleElement("settings");
xDocument.Should().HaveElementCount("settings", 1);

xElement.Should().HaveValue("36");
xElement.Should().HaveAttribute("age", "36");
xElement.Should().HaveElement("address");
xElement.Should().HaveElementWithNamespace("address", "http://www.example.com/2012/test");

xElement.Should().HaveInnerText("some textanother textmore text");
```

Those two last assertions also support `XName` parameters:

```csharp
xElement.Should().HaveAttribute(XName.Get("age", "http://www.example.com/2012/test"), "36");
xElement.Should().HaveElement(XName.Get("address", "http://www.example.com/2012/test"));

xAttribute.Should().HaveValue("Amsterdam");
```

You can also perform a deep comparison between two elements like this.

```csharp
xDocument.Should().BeEquivalentTo(XDocument.Parse("<configuration><item>value</item></configuration>"));
xElement.Should().BeEquivalentTo(XElement.Parse("<item>value</item>"));
```

Chaining additional assertions on top of a particular (root) element is possible through this syntax.

```csharp
xDocument.Should().HaveElement("child")
  .Which.Should().BeOfType<XElement>()
    .And.HaveAttribute("attr", "1");
```
