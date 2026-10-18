---
title: Uris
permalink: /uris/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

```csharp
var uri = new Uri("https://api.example.com:8443/orders/42?page=2#top");

uri.Should().BeAbsolute();
uri.Should().HaveScheme("https");
uri.Should().HaveHost("api.example.com");
uri.Should().HavePort(8443);
uri.Should().HavePath("/orders/42");
uri.Should().HaveFragment("top");
uri.Should().HaveQueryParameter("page", "2");
uri.Should().HaveQueryParameter("page").Which.Should().Be("2");
uri.Should().NotHaveQueryParameter("size");
uri.Should().Be("https://api.example.com:8443/orders/42?page=2#top");

new Uri("/orders", UriKind.Relative).Should().BeRelative();
new Uri("https://api.example.com/").Should().BeBaseOf(new Uri("https://api.example.com/orders"));
```

Scheme and host are compared case-insensitively. Query parameter names and values are compared after unescaping.
`HaveScheme`, `HaveHost`, `HavePort`, `HavePath`, `HaveFragment` and the query parameter assertions require an absolute URI.
Most of them have a `Not…` counterpart.
