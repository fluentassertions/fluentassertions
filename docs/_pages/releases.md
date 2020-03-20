---
title: Releases
permalink: /releases/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

## 5.10.3
**Fixes**
* Fixed XPath index calculation in XML comparison when child node names are repeated in repeated parent nodes - [#1273](https://github.com/fluentassertions/fluentassertions/pull/1273)

## 5.10.2
**Fixes**
* Added missing dependency on System.Xml - [#79](https://github.com/fluentassertions/fluentassertions/issues/79)

## 5.10.1
This version was skipped.

## 5.10.0

**What's New**
* Added `string.Should().NotBeEquivalentTo` - [#1134](https://github.com/fluentassertions/fluentassertions/pull/1134)
* Added `collectionOfStrings.Should().ContainMatch()` to assert that the collection contains at least one string that matches a wildcard pattern - [#1138](https://github.com/fluentassertions/fluentassertions/pull/1138)
* Added overloads of `NotBeInAscendingOrder` and `NotBeInDescendingOrder` that take a property expression - [#1140](https://github.com/fluentassertions/fluentassertions/pull/1140)
* Include the index in the XPath information reported while comparing `XDocument`s for equivalence - [#1181](https://github.com/fluentassertions/fluentassertions/pull/1181)
* Added `NotHaveSameCount` and `HaveSameCount` for dictionary assertions - [#1178](https://github.com/fluentassertions/fluentassertions/pull/1178)
* Extended `string.Should().Contain()` and `string.Should().ContainEquivalentOf()` to test the number of times a phrase exists in a string using `theString.Should().Contain("is a", MoreThan.Thrice())` - [#1145](https://github.com/fluentassertions/fluentassertions/pull/1145)
* Added support for methods returning `ValueTask` and `ValueTask<T>` - [#1158](https://github.com/fluentassertions/fluentassertions/pull/1158)
* Added support for using `SatisfyRespectively` on all collections, including collections of `string` - [#1201](https://github.com/fluentassertions/fluentassertions/pull/1201)

**Fixes**
* Updated the docs to clarify that `CompleteWithin` works on `Func<Task>` and not on `Task` directly - [#1127](https://github.com/fluentassertions/fluentassertions/pull/1127).
* Renamed collection assertion `NotBeAscendingInOrder` and `NotBeDescendingInOrder` to `NotBeInAscendingOrder` and `NotBeInDescendingOrder` (in a non-breaking way) - [#1140](https://github.com/fluentassertions/fluentassertions/pull/1140)
* Collections containing `null`s where not properly treated as equivalent by `BeEquivalentTo` - [#1143](https://github.com/fluentassertions/fluentassertions/pull/1143)
* `BeEquivalentTo` now allows selecting an explicit interface member over a regular instance member when using `RespectingDeclaredTypes` and the expectation type is an interface - [#1144](https://github.com/fluentassertions/fluentassertions/pull/1144)
* Fixed a crash that can occur when trying to determine the caller identity under .NET Native - [#1149](https://github.com/fluentassertions/fluentassertions/issues/1149)
* Ensured that determining the caller identity works for namespaces that contain the phrase `System` and don't match on partial namespace segments - [#1193](https://github.com/fluentassertions/fluentassertions/pull/1193)
* `BeEquivalentTo` on an `XDocument` could report the incorrect path to a self-closing XML tag - [#1170](https://github.com/fluentassertions/fluentassertions/pull/1170)
* `BeEquivalentTo` on multi-dimensional arrays with empty elements could cause an internal error - [#1167](https://github.com/fluentassertions/fluentassertions/issues/1167)
* Several assertion APIs did not include the _reason_ in the failure messages - [#1172](https://github.com/fluentassertions/fluentassertions/pull/1172)
* A self-closing XML element was not treated as equivalent to an empty element with the same name - [#1174](https://github.com/fluentassertions/fluentassertions/pull/1174)
* Ensure that type assertion `ThatAreUnderNamespace` handles types in the global namespaces correctly - [#1197](https://github.com/fluentassertions/fluentassertions/pull/1197)
* When comparing whether two objects are equal, a conversion should be precision preserving - [#1202](https://github.com/fluentassertions/fluentassertions/pull/1202)
* `BeEquivalentTo` on normal `Tuple`s should use structural equivalency instead of treating them as value types - [#1206](https://github.com/fluentassertions/fluentassertions/pull/1206)
* Some platforms throw reflection exceptions when trying to use `ConfigurationManager`. This is now handled more gracefully - [#1210](https://github.com/fluentassertions/fluentassertions/pull/1210)
* Reintroduced the package dependency on `System.Xml.Linq` for .NET 4.5/4.7 - [#79](https://github.com/fluentassertions/fluentassertions/issues/79)
* Treat enums and their numeric representations in structural object graph comparisons the same - [#1208](https://github.com/fluentassertions/fluentassertions/pull/1208)

Thanks to contributors [Ronald Kroon](https://github.com/ronaldkroon), [Daniel Petrov](https://github.com/danielmpetrov), [@david-a-jetter](https://github.com/david-a-jetter), [Lukas Grützmacher](https://github.com/lg2de) and [Ben Randall](https://github.com/veleek).

And special thanks to [Matthias Koch](https://github.com/matkoch) to switch us over to the awesome [Nuke build system](https://nuke.build/). 

## 5.9.0

**What's New**
* Added `Match` method to (nullable) numeric assertions - [#1112](https://github.com/fluentassertions/fluentassertions/pull/1112) 

**Fixes**
* Using a custom `IAssertionStrategy` with an `AssertionScope` did not always capture all assertion failures - [#1118](https://github.com/fluentassertions/fluentassertions/issues/1118)
* Ensured `null` arguments are handled with clearer exception messages - [#1117](https://github.com/fluentassertions/fluentassertions/pull/1117)

Special thanks to contributors [@liklainy](https://github.com/Liklainy) and [Amaury Levé](https://github.com/Evangelink).

## 5.8.0

**What's New**
* Added thread-safety to tests using `AssertionScope` while running many `async` tests concurrently - [#1091](https://github.com/fluentassertions/fluentassertions/pull/1091)
* Allow users to specify a custom `IAssertionStrategy` for the assertion scope, for instance, to create screenshots when a test fails - [#1094](https://github.com/fluentassertions/fluentassertions/pull/1094) & [#906](https://github.com/fluentassertions/fluentassertions/issues/906). 
* Supports .NET Core 3.0 Preview 7 - [#1107](https://github.com/fluentassertions/fluentassertions/pull/1107)

**Fixes**
* `Excluding` with `BeEquivalentTo` did not always work well with overridden properties - [#1087](https://github.com/fluentassertions/fluentassertions/pull/1087) & [#1077](https://github.com/fluentassertions/fluentassertions/issues/1077)
* Failure message formatting threw a `FormatException` when a nested message contains braces - [#1092](https://github.com/fluentassertions/fluentassertions/pull/1092)
* Fixed confusing `(Not)BeAssignableTo` failure messages - [#1104](https://github.com/fluentassertions/fluentassertions/pull/1104) & [#1103](https://github.com/fluentassertions/fluentassertions/issues/1103)
* Fixed a potential leakage of failure messages between multiple assertions - [#1105](https://github.com/fluentassertions/fluentassertions/pull/1105)

Kudos to [conklinb](https://github.com/conklinb) and [Amaury Levé](https://github.com/Evangelink) for notable contributions and my partner-in-crime [Jonas Nyrup](https://github.com/jnyrup) for some of the fixes, a lot of (internal) quality improvements and his critical eye. 

## 5.7.0

**What's New**
* Added official support for .NET Core 3 (Preview 5 or later) - [#1057](https://github.com/fluentassertions/fluentassertions/pull/1057)
* Introduced `SatisfyRespectively` on collections - [#1043](https://github.com/fluentassertions/fluentassertions/pull/1043)
* Added an alternative fluent syntax for evaluating/invoking actions - [#1017](https://github.com/fluentassertions/fluentassertions/pull/1017)
* Added overloads of `Invoking` and `Awaiting` for different sets of generic parameters - [#1051](https://github.com/fluentassertions/fluentassertions/pull/1051)
* `NotThrowAfter` is now also available for .NET Standard 1.3 and 1.6 - [#1050](https://github.com/fluentassertions/fluentassertions/pull/1050)
* Added `CompleteWithin` to assert asynchronous operations complete within a time span - [#1013](https://github.com/fluentassertions/fluentassertions/pull/1013)/[#1048](https://github.com/fluentassertions/fluentassertions/pull/1048)
* Added `NotBeEquivalent` for objects - [#1071](https://github.com/fluentassertions/fluentassertions/pull/1071)
* Added `WithMessage()` for `async` exception assertions - [#1052](https://github.com/fluentassertions/fluentassertions/pull/1052)
* Added extension methods like `100.Nanosecond()` and `20.Microsecond()` to represent time spans - [#1069](https://github.com/fluentassertions/fluentassertions/pull/1069)

**Fixes**
* `AllBeAssignableTo` and `AllBeOfType` did not work for list of types - [#1007](https://github.com/fluentassertions/fluentassertions/pull/1007)
* Backslashes in subject or expected result were not correctly shown in the message - [#986](https://github.com/fluentassertions/fluentassertions/pull/986)
* `BeOfType` does not attach to the `AssertionScope` correctly - [#1002](https://github.com/fluentassertions/fluentassertions/pull/1002)
* Event monitoring did not detect events on interfaces - [#821](https://github.com/fluentassertions/fluentassertions/pull/821)
* Fix continuation on `NotThrow(After)` in chained `AssertionScope` invocations - [#1031](https://github.com/fluentassertions/fluentassertions/pull/1031)
* Allow nesting equivalency checks in custom assertion rules when using `BeEquivalentTo` - [#1031](https://github.com/fluentassertions/fluentassertions/pull/1031)
* Removed redundant use of the thread pool in async assertions - [#1020](https://github.com/fluentassertions/fluentassertions/pull/1020)
* Improved formatting of multidimensional arrays - [#1044](https://github.com/fluentassertions/fluentassertions/pull/1044)
* Better handling of exceptions wrapped in `AggregateException`s - [#1041](https://github.com/fluentassertions/fluentassertions/pull/1041)
* Fix `BadImageFormatException` under the .NET Core 3.0 Preview - [#1057](https://github.com/fluentassertions/fluentassertions/pull/1057)
* `ThrowExactly` and `ThrowExactlyAsync` now support expecting an `AggregateException` - [#1046](https://github.com/fluentassertions/fluentassertions/pull/1057)

Kudos to [Lukas Grützmacher](https://github.com/lg2de), [Matthias Lischka](https://github.com/matthiaslischka), [Christoffer Lette](https://github.com/Lette), [Ed Ball](https://github.com/ejball), [David Omid](https://github.com/davidomid), [mu88](https://github.com/mu88), [Dmitriy Maksimov](https://github.com/DmitriyMaksimov) and [Ivan Shimko](https://github.com/vanashimko) for the contributions and [Jonas Nyrup](https://github.com/jnyrup) to make this release possible again. 

## 5.6.0

**Fixes**
* Provide opt-out to `AssertionOptions(o => o.WithStrictOrdering())` -[#974](https://github.com/fluentassertions/fluentassertions/pull/974) 
* Add collection assertion `ContainEquivalentOf` - [#950](https://github.com/fluentassertions/fluentassertions/pull/950)
* Add `Should().NotThrowAfter` assertion for actions - [#942](https://github.com/fluentassertions/fluentassertions/pull/942)

Kudos to @BrunoJuchli, @matthiaslischka and @frederik-h for these amazing additions. 

## 5.5.3

**Fixes**
* Performance fixes in `BeEquivalenTo` - [#935](https://github.com/fluentassertions/fluentassertions/pull/935) 
* Reverted 5.5.0 changes to `AssertionScope` to ensure binary compatibility - [#977](https://github.com/fluentassertions/fluentassertions/pull/977)

## 5.5.2

**Fixes**
* Allows `BeEquivalentTo` to handle a non-generic collection as the SUT - [#975](https://github.com/fluentassertions/fluentassertions/pull/975), [#973](https://github.com/fluentassertions/fluentassertions/issues/973)
* Optimized performance of `IncludeMemberByPathSelectionRule` - [#969](https://github.com/fluentassertions/fluentassertions/pull/969)

## 5.5.1

**What's New**
* Now provides a hint when strings differ in length and contain differences - [#915](https://github.com/fluentassertions/fluentassertions/pull/915), [#907](https://github.com/fluentassertions/fluentassertions/issues/907)
* Added `ThrowAsync`, `ThrowExactlyAsync` and `NotThrowAsync` - [#931](https://github.com/fluentassertions/fluentassertions/pull/931)
* Added support for `Should().Throw` and `Should().NotThrow` for `Func<T>` - [#951](https://github.com/fluentassertions/fluentassertions/pull/951)
* Added support for `private protected` access modifier - [#932](https://github.com/fluentassertions/fluentassertions/pull/932)
* Updated `BeApproximately` to support nullable types for both the subject and the expectation nullable - [#934](https://github.com/fluentassertions/fluentassertions/pull/934)
* Added `async` version of `ExecutionTime` to - [#938](https://github.com/fluentassertions/fluentassertions/pull/938)
* Updated `NotBeApproximately` to accepting nullable subject and expectation - [#939](https://github.com/fluentassertions/fluentassertions/pull/939)
* `type.Should().Be(type)` now support open generics - [#954](https://github.com/fluentassertions/fluentassertions/issues/954), [#955](https://github.com/fluentassertions/fluentassertions/pull/955)

**Fixes**
* Minor performance improvements to prevent rendering messages if a test did not fail - [#921](https://github.com/fluentassertions/fluentassertions/pull/921), [#915](https://github.com/fluentassertions/fluentassertions/pull/915)
* Improve performance of `Should().AllBeEquivalentTo()` - [#920](https://github.com/fluentassertions/fluentassertions/pull/920), [#914](https://github.com/fluentassertions/fluentassertions/issues/914)
* Improve the presentation of enums to include the value and the number - [#923](https://github.com/fluentassertions/fluentassertions/pull/923), [#897](https://github.com/fluentassertions/fluentassertions/issues/897)
* `BeEquivalentTo` with `WithStrictOrdering` produced messy failure message - [#918](https://github.com/fluentassertions/fluentassertions/issues/918)
* Fixes detecting checking equivalency of a `null` subject to a dictionary - [#933](https://github.com/fluentassertions/fluentassertions/pull/933)
* Fixes duplicate conversions being mentioned in the output of the equivalency API - [#941](https://github.com/fluentassertions/fluentassertions/pull/941)
* Comparing an object graph against `IEnumerable` now works now as expected - [#911](https://github.com/fluentassertions/fluentassertions/pull/911)
* Selecting members during object graph assertions now better handles `new` overrides - [#960](https://github.com/fluentassertions/fluentassertions/pull/960), [#956](https://github.com/fluentassertions/fluentassertions/issues/956) 

**Note** In versions prior to 5.5, FA may have skipped certain properties in the equivalency comparison. [#960](https://github.com/fluentassertions/fluentassertions/pull/960) fixes this, so this may cause some breaking changes. 

Lots of kudos @jnyrup and @krajek for a majority for the work in this release.
