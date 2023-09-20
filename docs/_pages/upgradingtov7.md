---
title: Upgrading to version 7.0
permalink: /upgradingtov7
layout: single
toc: true
sidebar:
  nav: "sidebar"
---

## Dropping support for .NET Core 2.x and .NET Core 3.x.

As of v7, we've decided to no longer directly target .NET Core. All versions of .NET Core, including .NET 5 are [already out of support by Microsoft](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core). But we still support .NET Standard 2.0 and 2.1, so Fluent Assertions will still work with those deprecated frameworks.
