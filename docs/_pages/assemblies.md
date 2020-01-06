---
title: Assembly References
permalink: /assemblies/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

If you're running .NET 4.5 or .NET Standard 2.0, you have access to methods to assert an assembly does or does not reference another assembly.
These are typically used to enforce layers within an application, such as for example, asserting the web layer does not reference the data layer.
To assert the references, use the the following syntax:

```csharp 
assembly.Should().Reference(otherAssembly);
assembly.Should().NotReference(otherAssembly);
```
