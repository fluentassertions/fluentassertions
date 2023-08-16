---
title: Assembly References
permalink: /assemblies/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

You have access to methods to assert an assembly does or does not reference another assembly.
These are typically used to enforce layers within an application, such as for example, asserting the web layer does not reference the data layer.
To assert the references, use the following syntax:

```csharp
assembly.Should().Reference(otherAssembly);
assembly.Should().NotReference(otherAssembly);
```

Furthermore, you can assert if an assembly is assigned with a specific public key, or that the assembly is not signed at all.
The first can be useful, to ensure that the public key of your package is not changed unintentionally, the latter to prevent unintended signing.
To assert this, use the following syntax:

```csharp
assembly.Should().HavePublicKey("e0851575614491c6d25018fadb75");
assembly.Should().BeUnsigned();
```
