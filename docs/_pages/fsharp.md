---
title: F# Usage
permalink: /fsharp/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

### F# usage

FluentAssertions was made with C# in mind. While it can be used from F#, the experience will not always be as intended.

For a better F# experience we encourage F# developers to try out [Faqt](https://github.com/cmeeren/Faqt/), an assertion library inspired by FluentAssertions and made specifically for F#.

Some of the awkwardness you might hit with FluentAssertions when using F# is:

* To aid F#'s overload resolution, you often have to cast subject values. This does not work in all cases and will only give you a subset of assertions. For example:

  ```fsharp
  let x = [1; 2]
  x.Should().Contain(1, "") // Overload resolution error for "Should"
  (x :> seq<_>).Should().Contain(1, "") // OK, but only gives access to GenericCollectionAssertions<_>
  ```

  The need to cast will break your fluent chains, defeating some of the purpose of FluentAssertions.

* Sometimes not even casting will solve the issue, and you will have to forgo `Should` entirely and instead directly construct the correct assertion type. For example:

  ```fsharp
  let x = dict ["A", 1; "B", 2]
 
  // All of these give overload resolution errors for "Should"
  x.Should().ContainKey(1, "")
  (x :> IDictionary<_,_>).Should().ContainKey(1, "") 
  (x :> seq<KeyValuePair<_,_>>).Should().Contain(KeyValuePair("A", 1), "")
 
  // You have to construct the desired assertion directly
  GenericDictionaryAssertions<_, _, _>(x).ContainKey("A", "")
  GenericCollectionAssertions<_, _, _>(x).Contain(KeyValuePair("A", 1), "")
  ```

  As with casting, the need to construct the assertion directly will break your fluent chains, defeating one of the key points of FluentAssertions.

* As can be seen above, the `because` parameter cannot be omitted when used from
  F# ([#2225](https://github.com/fluentassertions/fluentassertions/issues/2225)). Adding overloads with a fixed `because` is [deemed out of scope](https://github.com/fluentassertions/fluentassertions/issues/2225#issuecomment-1636733116) for the main FluentAssertions package since it would double the API surface.
* Several assertions (specifically, those that accept an `Action<_>`) require an explicit `ignore` when used from F# ([#2226](https://github.com/fluentassertions/fluentassertions/issues/2226)).
