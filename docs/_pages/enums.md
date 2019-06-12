---
title: Enums
permalink: /enums/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

## Enums ##
With the standard `Should().Be()` method, Enums are compared using .NET's `Enum.Equals()` implementation.
This means that the Enums must be of the same type, and have the same underlying value.