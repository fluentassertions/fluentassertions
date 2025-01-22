---
title: Releases
permalink: /releases/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

## 7.2.0

### Improvements
* All `Should()` methods on reference types are now annotated with the `[NotNull]` attribute for a better Fluent Assertions experience when nullable reference types are enabled - [#2987](https://github.com/fluentassertions/fluentassertions/pull/2987)

## 7.1.0

### Improvements
* Improve failure message for string assertions when checking for equality - [#2307](https://github.com/fluentassertions/fluentassertions/pull/2307)

* Added compatibility with xUnit.net v3 - [#2970](https://github.com/fluentassertions/fluentassertions/issues/2970)
* Added support for throwing TUnit exceptions when using TUnit as your testing framework - [#2971](https://github.com/fluentassertions/fluentassertions/pull/2971)

## 7.0.0

### Breaking Changes

* Dropped direct support for .NET Core 2.x and .NET Core 3.x - [#2302](https://github.com/fluentassertions/fluentassertions/pull/2302)
* Dropped support for `NSpec3` test framework - [#2356](https://github.com/fluentassertions/fluentassertions/pull/2356)
* Raised dependencies on `System.Configuration.ConfigurationManager` to 6.0.0 and `System.Threading.Tasks.Extensions` to 4.5.4 - [#2673](https://github.com/fluentassertions/fluentassertions/pull/2673) and [#2855](https://github.com/fluentassertions/fluentassertions/pull/2855)

### Fixes

* Fixed a problem in `BeEquivalentTo` where write-only properties would cause a `NullReferenceException` - [#2836](https://github.com/fluentassertions/fluentassertions/pull/2836)

## 6.12.3

### Fixes

* The expectation node identified as a cyclic reference is still compared to the subject node using simple equality - [2819](https://github.com/fluentassertions/fluentassertions/pull/2819)

## 6.12.2

### Fixes
* Better handling of normal vs explicitly implemented vs default interface properties - [2794](https://github.com/fluentassertions/fluentassertions/pull/2794)

## 6.12.1

### Improvements
* Improve `BeEmpty()` and `BeNullOrEmpty()` performance for `IEnumerable<T>`, by materializing only the first item - [#2530](https://github.com/fluentassertions/fluentassertions/pull/2530)

### Fixes
* Fixed formatting error when checking nullable `DateTimeOffset` with
`BeWithin(...).Before(...)` - [#2312](https://github.com/fluentassertions/fluentassertions/pull/2312)
* `BeEquivalentTo` will now find and can map subject properties that are implemented through an explicitly-implemented interface - [#2152](https://github.com/fluentassertions/fluentassertions/pull/2152)
* Fixed that the `because` and `becauseArgs` were not passed down the equivalency tree - [#2318](https://github.com/fluentassertions/fluentassertions/pull/2318)
* `BeEquivalentTo` can again compare a non-generic `IDictionary` with a generic one - [#2358](https://github.com/fluentassertions/fluentassertions/pull/2358)
* Fixed that the `FormattingOptions` were not respected in inner `AssertionScope` - [#2329](https://github.com/fluentassertions/fluentassertions/pull/2329)
* Capitalize `true` and `false` in failure messages and make them formattable to a custom `BooleanFormatter` - [#2390](https://github.com/fluentassertions/fluentassertions/pull/2390), [#2393](https://github.com/fluentassertions/fluentassertions/pull/2393)
* Improved the failure message for `NotBeOfType` when wrapped in an `AssertionScope` and the subject is null  - [#2399](https://github.com/fluentassertions/fluentassertions/pull/2399)
* Improved the failure message for `BeWritable`/`BeReadable` when wrapped in an `AssertionScope` and the subject is read-only/write-only - [#2399](https://github.com/fluentassertions/fluentassertions/pull/2399)
* Improved the failure message for `ThrowExactly[Async]` when wrapped in an `AssertionScope` and no exception is thrown - [#2398](https://github.com/fluentassertions/fluentassertions/pull/2398)
* Improved the failure message for `[Not]HaveExplicitProperty` when wrapped in an `AssertionScope` and not implementing the interface - [#2403](https://github.com/fluentassertions/fluentassertions/pull/2403)
* Improved the failure message for `[Not]HaveExplicitMethod` when wrapped in an `AssertionScope` and not implementing the interface - [#2403](https://github.com/fluentassertions/fluentassertions/pull/2403)
* Changed `BeEquivalentTo` to exclude `private protected` members from the comparison - [#2417](https://github.com/fluentassertions/fluentassertions/pull/2417)
* Fixed using `BeEquivalentTo` on an empty `ArraySegment` - [#2445](https://github.com/fluentassertions/fluentassertions/pull/2445), [#2511](https://github.com/fluentassertions/fluentassertions/pull/2511)
* `BeEquivalentTo` with a custom comparer can now handle null values - [#2489](https://github.com/fluentassertions/fluentassertions/pull/2489)
* Ensured that nested calls to `AssertionScope(context)` create a chained context - [#2607](https://github.com/fluentassertions/fluentassertions/pull/2607)
* One overload of the `AssertionScope` constructor would not create an actual scope associated with the thread - [#2607](https://github.com/fluentassertions/fluentassertions/pull/2607)
* Fixed `ThrowWithinAsync` not respecting `OperationCanceledException` - [#2614](https://github.com/fluentassertions/fluentassertions/pull/2614)
* Fixed using `BeEquivalentTo` with an `IEqualityComparer` targeting nullable types - [#2648](https://github.com/fluentassertions/fluentassertions/pull/2648)

## 6.12.0

### What's new
* Added `Be`, `NotBe` and `BeOneOf` for object comparisons with custom comparer - [#2111](https://github.com/fluentassertions/fluentassertions/pull/2111)
* Added `BeSignedWithPublicKey()` and `BeUnsigned()` for assertions on `Assembly` - [#2207](https://github.com/fluentassertions/fluentassertions/pull/2207)
* Added `NotContainItemsAssignableTo` for asserting that a collection does not contain any items assignable to a specific type - [#2266](https://github.com/fluentassertions/fluentassertions/pull/2266)

### Fixes
* `because` and `becauseArgs` were not included in the error message when collections of enums were not equivalent - [#2214](https://github.com/fluentassertions/fluentassertions/pull/2214)
* Improve caller identification for tests written in Visual Basic - [#2254](https://github.com/fluentassertions/fluentassertions/pull/2254)
* Improved auto conversion to enums for objects of different integral type - [#2261](https://github.com/fluentassertions/fluentassertions/pull/2261)
* Fixed exceptions when trying to auto convert strings or enums of different type to enums- [#2261](https://github.com/fluentassertions/fluentassertions/pull/2261)
* Format records and anonymous objects with their member values instead of the generated `ToString` - [#2144](https://github.com/fluentassertions/fluentassertions/pull/2144)

## 6.11.0

### What's new
* Added `ThrowWithinAsync` for assertions on `Task` - [#1974](https://github.com/fluentassertions/fluentassertions/pull/1974)
* Added support for converting integers to enums using `AutoConversion` - [#2147](https://github.com/fluentassertions/fluentassertions/pull/2147)
* Changed exception formatting to include any inner exception - [#2150](https://github.com/fluentassertions/fluentassertions/pull/2150)
* Added an expression overload for `WithoutStrictOrderingFor` - [#2151](https://github.com/fluentassertions/fluentassertions/pull/2151)

### Fixes
* Improved robustness of several assertions when they're wrapped in an `AssertionScope` - [#2133](https://github.com/fluentassertions/fluentassertions/pull/2133)
* The maximum depth `BeEquivalentTo` uses for recursive comparisons was 9 instead of the expected 10 - [#2145](https://github.com/fluentassertions/fluentassertions/pull/2145)
* Fixed `.Excluding()` and `.For().Exclude()` not working if root is a collection - [#2135](https://github.com/fluentassertions/fluentassertions/pull/2135)
* Prevent `InvalidOperationException` when formatting a lambda expression calling a constructor - [#2176](https://github.com/fluentassertions/fluentassertions/pull/2176)

## 6.10.0

### Fixes
* Fixed hanging of `CompleteWithinAsync` when used with `WithResult` and `AssertionScope` - [#2101](https://github.com/fluentassertions/fluentassertions/pull/2101)
* `BeEquivalentTo` no longer crashes on fields hiding base-class fields - [#1990](https://github.com/fluentassertions/fluentassertions/pull/1990)
* Fixed System.Net.Http dependency declaration for net47 target framework to be a framework dependency instead of a nuget dependency - [#2122](https://github.com/fluentassertions/fluentassertions/pull/2122)

## 6.9.0

### What's new
* Added `ThatAre[Not]ValueTypes` method for filtering the types - [#2083](https://github.com/fluentassertions/fluentassertions/pull/2083)
* Added `Imply` method to `BooleanAssertions` - [#2074](https://github.com/fluentassertions/fluentassertions/pull/2074)
* Added `ThatAre[Not]Interfaces` method for filtering the types - [#2057](https://github.com/fluentassertions/fluentassertions/pull/2057)
* Added `ThatAre[Not]Abstract` method for filtering the types - [#2058](https://github.com/fluentassertions/fluentassertions/pull/2058)
* Added `ThatAre[Not]Sealed` method for filtering the types - [#2059](https://github.com/fluentassertions/fluentassertions/pull/2059)
* Added `ThatAre[Not]Abstract` methods to `MethodInfoSelector.cs` for filtering the methods - [#2060](https://github.com/fluentassertions/fluentassertions/pull/2060)
* Added `ThatAre[Not]Abstract`, `ThatAre[Not]Static` and `ThatAre[Not]Virtual` properties for filtering in `PropertyInfoSelector.cs` - [#2054](https://github.com/fluentassertions/fluentassertions/pull/2054)
* Added `BeOneOf` methods for object comparisons and `IComparable`s - [#2028](https://github.com/fluentassertions/fluentassertions/pull/2028)
* Added `BeCloseTo` and `NotBeCloseTo` to `TimeOnly` - [#2030](https://github.com/fluentassertions/fluentassertions/pull/2030)
* Added new extension methods to be able to write `Exactly.Times(n)`, `AtLeast.Times(n)` and `AtMost.Times(n)` in a more fluent way - [#2047](https://github.com/fluentassertions/fluentassertions/pull/2047)
* Changed `BeEquivalentTo` to treat record structs like records, thus comparing them by member by default - [#2009](https://github.com/fluentassertions/fluentassertions/pull/2009)

### Fixes
* `PropertyInfoSelector.ThatArePublicOrInternal` now takes the setter into account when determining if a property is `public` or `internal` - [#2082](https://github.com/fluentassertions/fluentassertions/pull/2082)
* Quering properties on classes, e.g. `typeof(MyClass).Properties()`, now also includes static properties - [#2054](https://github.com/fluentassertions/fluentassertions/pull/2054)
* Nested AssertionScopes now print the inner scope reportables - [#2044](https://github.com/fluentassertions/fluentassertions/pull/2044)
* Throw `ArgumentException` instead of `ArgumentNullException` when a required `string` argument is empty - [#2023](https://github.com/fluentassertions/fluentassertions/pull/2023)
* Assertions on the ordering of a collection of `string`s now uses ordinal comparison when an `IComparer<T>` is not provided - [#2075](https://github.com/fluentassertions/fluentassertions/pull/2075)

## 6.8.0

### What's new
* Added `ContainInConsecutiveOrder` and `NotContainInConsecutiveOrder` assertions to check if a collection contains items in a specific order and to be consecutive - [#1963](https://github.com/fluentassertions/fluentassertions/pull/1963)
* Added `NotCompleteWithinAsync` for assertions on `Task` - [#1967](https://github.com/fluentassertions/fluentassertions/pull/1967)
* Added `CompleteWithinAsync` and `NotCompleteWithinAsync` for non-generic `TaskCompletionSource` (.NET 6 and above) - [#1961](https://github.com/fluentassertions/fluentassertions/pull/1961)
* Added a `ParentType` to `IObjectInfo` to help determining the parent in a call to `Using`/`When` constructs - [#1950](https://github.com/fluentassertions/fluentassertions/pull/1950)
* Added a `Monitor` to `EventAssertions` to enable writing extension methods for event assertions. - [#2008](https://github.com/fluentassertions/fluentassertions/pull/2008)

### Improvements
* Updated exception messages to provide suggestions when incorrectly using `Equals()` - [#2006](https://github.com/fluentassertions/fluentassertions/pull/2006)
* Included the time difference in the error message of `BeCloseTo` - [#2013](https://github.com/fluentassertions/fluentassertions/pull/2013)

### Fixes
* Fixed `For`/`Exclude` not excluding properties in objects in a collection - [#1953](https://github.com/fluentassertions/fluentassertions/pull/1953)
* Changed `MatchEquivalentOf` to use `CultureInfo.InvariantCulture` instead of `CultureInfo.CurrentCulture` - [#1985](https://github.com/fluentassertions/fluentassertions/pull/1985).
* Fixed `BeEquivalentTo` not taking into account any `record` equivalency settings coming from the `AssertionOptions` - [#1984](https://github.com/fluentassertions/fluentassertions/pull/1984)
* Fixed `ExecutionTimeOf` formatting failing when the expression includes {} - [#1994](https://github.com/fluentassertions/fluentassertions/pull/1994)

## 6.7.0

### What's new
* Added `BeDefined` and `NotBeDefined` to assert on existence of an enum value - [#1888](https://github.com/fluentassertions/fluentassertions/pull/1888)
* Added the ability to exclude fields & properties marked as non-browsable in the code editor from structural equality comparisons - [#1807](https://github.com/fluentassertions/fluentassertions/pull/1807) & [#1812](https://github.com/fluentassertions/fluentassertions/pull/1812)
* Assertions on the collection types in System.Data (`DataSet.Tables`, `DataTable.Columns`, `DataTable.Rows`) have been restored - [#1812](https://github.com/fluentassertions/fluentassertions/pull/1812)
* Added `For`/`Exclude` to allow exclusion of members inside a collection - [#1782](https://github.com/fluentassertions/fluentassertions/pull/1782)
* Added overload for `HaveElement` for `XDocument` and `XElement` to assert on number of XML nodes - [#1880](https://github.com/fluentassertions/fluentassertions/pull/1880)

### Fixes
* Fixed the failure message for regex matches (occurrence overload) to include the missing subject - [#1913](https://github.com/fluentassertions/fluentassertions/pull/1913)
* Fixed `WithArgs` matching too many events when at least one argument matched the expected type - [#1920](https://github.com/fluentassertions/fluentassertions/pull/1920)

## 6.6.0

### What's New
* Annotated `[Not]MatchRegex(string)` with `[StringSyntax("Regex")]` which IDEs can use to colorize the regular expression argument - [#1816](https://github.com/fluentassertions/fluentassertions/pull/1816)
* Added support for .NET6 `DateOnly` struct - [#1844](https://github.com/fluentassertions/fluentassertions/pull/1844)
* Added support for .NET6 `TimeOnly` struct - [#1848](https://github.com/fluentassertions/fluentassertions/pull/1848)
* Added `NotBe` for nullable boolean values - [#1865](https://github.com/fluentassertions/fluentassertions/pull/1865)
* Added a new overload to `MatchRegex()` to assert on the number of regex matches - [#1869](https://github.com/fluentassertions/fluentassertions/pull/1869)
* Added difference to numeric assertion failure messages - [#1859](https://github.com/fluentassertions/fluentassertions/pull/1859)

### Fixes
* `EnumAssertions.Be` did not determine the caller name - [#1835](https://github.com/fluentassertions/fluentassertions/pull/1835)
* Ensure `ExcludingMissingMembers` doesn't undo usage of `WithMapping` in `BeEquivalentTo` - [#1838](https://github.com/fluentassertions/fluentassertions/pull/1838)
* Better handling of NaN in various numeric assertions - [#1822](https://github.com/fluentassertions/fluentassertions/pull/1822) & [#1867](https://github.com/fluentassertions/fluentassertions/pull/1867)
* `WithMapping` in `BeEquivalentTo` now also works when the root is a collection - [#1858](https://github.com/fluentassertions/fluentassertions/pull/1858)

## 6.5.1

### Fixes
* Fixed regression introduced in 6.5.0 where `collection.Should().BeInAscendingOrder(x => x)` would fail - [#1802](https://github.com/fluentassertions/fluentassertions/pull/1802)

## 6.5.0

### What's New
* Added `AllSatisfy` for asserting all items in a collection satisfy an inspector - [#1790](https://github.com/fluentassertions/fluentassertions/pull/1790)
* Added `WithMapping` option to `BeEquivalentTo` to map members with different names between the subject and expectation - [#1742](https://github.com/fluentassertions/fluentassertions/pull/1742)

### Fixes
* Improved the documentation on `BeLowerCased` and `BeUpperCased` for strings with non-alphabetic characters - [#1792](https://github.com/fluentassertions/fluentassertions/pull/1792)
* Caller identification does not handle all arguments using `new` - [#1794](https://github.com/fluentassertions/fluentassertions/pull/1794)
* Resolved an issue preventing `HaveAccessModifier` from correctly recognizing internal interfaces and enums - [#1793](https://github.com/fluentassertions/fluentassertions/issues/1793)
* Improved tracing for nested `AssertionScope`s - [#1797](https://github.com/fluentassertions/fluentassertions/pull/1797)

### Fixes (Extensibility)
* Fixed a continuation issue when using `ClearExpectation` - [#1791](https://github.com/fluentassertions/fluentassertions/pull/1791)

## 6.4.0

### What's New
* Added `ThatAreStatic()` and `ThatAreNotStatic()` for filtering in method assertions - [#1740](https://github.com/fluentassertions/fluentassertions/pull/1740)
* Added new assertions for the `HttpStatusCode` of an `HttpResponseMessage` - [#1737](https://github.com/fluentassertions/fluentassertions/pull/1737)
* Added non-generic overloads for `WithInnerExceptionExactly` and `WithInnerException` - [#1769](https://github.com/fluentassertions/fluentassertions/pull/1769)

### Fixes
* `ContainItemsAssignableTo` now expects at least one item assignable to `T` - [#1765](https://github.com/fluentassertions/fluentassertions/pull/1765)
* Querying methods on classes, e.g. `typeof(MyController).Methods()`, now also includes static methods - [#1740](https://github.com/fluentassertions/fluentassertions/pull/1740)
* Variable name is not captured after await assertion - [#1770](https://github.com/fluentassertions/fluentassertions/pull/1770)
* `OccurredEvent` ordering on monitored object is now done via thread-safe counter - [#1773](https://github.com/fluentassertions/fluentassertions/pull/1773)
* Avoid a `NullReferenceException` when testing an application compiled with .NET Native - [#1776](https://github.com/fluentassertions/fluentassertions/pull/1776)
* `[Not]Contain(key, value)` for dictionary-like enumerables incorrectly checked if the key was present - [#1786](https://github.com/fluentassertions/fluentassertions/pull/1786)
* Avoid throwing a `FormatException` when caller name determination returns an unformattable string - [#1788](https://github.com/fluentassertions/fluentassertions/pull/1788)

## 6.3.0

### What's New
* Added `ThatAreAsync()` and `ThatAreNotAsync()` for filtering in method assertions - [#1725](https://github.com/fluentassertions/fluentassertions/pull/1725)
* Added `ThatAreVirtual()` and `ThatAreNotVirtual()` for filtering in method assertions - [#1744](https://github.com/fluentassertions/fluentassertions/pull/1744)
* Added collection content to assertion messages for `HaveCountGreaterThan()`, `HaveCountGreaterThanOrEqualTo()`, `HaveCountLessThan()` and `HaveCountLessThanOrEqualTo()` - [#1760](https://github.com/fluentassertions/fluentassertions/pull/1760)

### Fixes
* Prevent multiple enumeration of `IEnumerable`s in parameter-less `ContainSingle()` - [#1753](https://github.com/fluentassertions/fluentassertions/pull/1753)
* Changed `HaveCount()` assertion message order to state expected and actual collection count before dumping its content` - [#1760](https://github.com/fluentassertions/fluentassertions/pull/1760)
* `CompleteWithinAsync` did not take initial sync computation into account when measuring execution time - [1762](https://github.com/fluentassertions/fluentassertions/pull/1762).

## 6.2.0

### What's New

* Added new overloads to all `GreaterOrEqualTo` and `LessOrEqualTo` assertions, adding the word `Than` - [#1673](https://github.com/fluentassertions/fluentassertions/pull/1673)
* `BeAsync()` and `NotBeAsync()` are now also available on `MethodInfoSelectorAssertions` - [#1700](https://github.com/fluentassertions/fluentassertions/pull/1700)

### Fixes

* Prevent exceptions when asserting on `ImmutableArray<T>` - [#1668](https://github.com/fluentassertions/fluentassertions/pull/1668)
* `At` now retains the `DateTimeKind` and keeps sub-second precision when using a `TimeSpan` - [#1687](https://github.com/fluentassertions/fluentassertions/pull/1687).
* Removed iteration over enumerable when generating the `BeEmpty` assertion failure message - [#1692](https://github.com/fluentassertions/fluentassertions/pull/1692).
* Prevent `ArgumentNullException` when formatting a lambda expression containing an extension method - [#1696](https://github.com/fluentassertions/fluentassertions/pull/1696)
* `IgnoringCyclicReferences` in `BeEquivalentTo` now works while comparing value types using `ComparingByMembers` - [#1708](https://github.com/fluentassertions/fluentassertions/pull/1708)
* Using `BeEquivalentTo` on a collection with nested collections would complain about missing members - [#1713](https://github.com/fluentassertions/fluentassertions/pull/1713)
* Formatting a lambda expression containing lifted operators - [#1714](https://github.com/fluentassertions/fluentassertions/pull/1714).
* Performance improvements in `BeEquivalentTo` by caching expensive Reflection operations - [#1719](https://github.com/fluentassertions/fluentassertions/pull/1719)

## 6.1.0

### What's New

* Prevent asserting directly on `AndConstraint` - [#1649](https://github.com/fluentassertions/fluentassertions/pull/1649)
* Added `WithInnerExceptionExactly` extension method on `Task<ExceptionAssertions<T>>` for easier use with `ThrowAsync` - [#1658](https://github.com/fluentassertions/fluentassertions/pull/1658)

### Fixes

* Resolved a significant performance degradation in `BeEquivalentTo` - [#1660](https://github.com/fluentassertions/fluentassertions/pull/1660)

## 6.0.0

### What's New

* Added official support for .NET Core 3.0 - [#1227](https://github.com/fluentassertions/fluentassertions/pull/1227).
* Added `WithOffset` extension method on `DateTime` for easier creation of `DateTimeOffset` objects - [#1235](https://github.com/fluentassertions/fluentassertions/pull/1235).
* Added `collectionOfStrings.Should().NotContainMatch()` to assert that the collection does not contain a string that matches a wildcard pattern - [#1246](https://github.com/fluentassertions/fluentassertions/pull/1246).
* The `Using`/`When` option on `BeEquivalentTo` will now use the conversion rules when trying to match the predicate - [#1257](https://github.com/fluentassertions/fluentassertions/pull/1257).
* Added `NotBeWritable` to `PropertyInfoSelectorAssertions` to be able to assert that properties are not writable - [#1269](https://github.com/fluentassertions/fluentassertions/pull/1269).
* Added extension to assert `TaskCompletionSource<T>` - [#1267](https://github.com/fluentassertions/fluentassertions/pull/1267).
* Added the ability to pass an `IEqualityComparer<T>` through `BeEquivalentTo(x => x.Using<MyComparer>())` - [#1284](https://github.com/fluentassertions/fluentassertions/pull/1284).
* Added `NotBe` to `BooleanAssertions` to be able to assert that a boolean is not the expected value - [#1290](https://github.com/fluentassertions/fluentassertions/pull/1290).
* Make `DefaultValueFormatter` and `EnumerableValueFormatter` suitable for inheritance - [#1295](https://github.com/fluentassertions/fluentassertions/pull/1295).
* Added support for dictionary assertions on `IReadOnlyDictionary<TKey, TValue>` - [#1298](https://github.com/fluentassertions/fluentassertions/pull/1298).
* `GenericAsyncFunctionAssertions` now has `AndWhichConstraint` overloads for `NotThrow[Async]` and `NotThrowAfter[Async]` - [#1289](https://github.com/fluentassertions/fluentassertions/pull/1289).
* Added `ReturnTypes` to `MethodInfoSelector` to get all return types from all the methods selected
* Added `[Not]Be` to `MethodInfoSelector` to check that methods [don't] have specified access modifier
* Added `ThatAre[Not]Classes`, `ThatAre[Not]Static` selectors to `TypeSelector`
* Added `ThatSatisfy` to `TypeSelector` to filter types with specified predicate
* Added `UnwrapEnumerableTypes` to `TypeSelector` to get the `T` type from types implementing `IEnumerable<T>`
* Added `UnwrapTaskTypes` to `TypeSelector` to get the `T` type for any type that are `Task<T>`  or `ValueTask<T>`
* Added `[Not]BeSealed` to `TypeSelectorAssertions`
* Added `collection.Should().NotContainEquivalentOf` to use object graph comparison rules to assert absence of an element in the collection - [#1318](https://github.com/fluentassertions/fluentassertions/pull/1318).
* Added `[Not]BeInNamespace` and `[NotBeUnderNamespace]` to `TypeSelectorAssertions` - [#1329](https://github.com/fluentassertions/fluentassertions/pull/1329).
* The `Using` option on `BeEquivalentTo` and on `AssertionOptions.AssertEquivalencyUsing` now supports custom `IOrderingRule` implementations [#1337](https://github.com/fluentassertions/fluentassertions/pull/1337).
* Added `AllBe` to `StringCollectionAssertions` to be able to assert that all strings in collection are equal to the specified string - [#1332](https://github.com/fluentassertions/fluentassertions/pull/1332).
* Added `ForConstraint` method to `AssertionScope` to open up `OccurenceConstraint` for usage in custom assertion extensions - [#1341](https://github.com/fluentassertions/fluentassertions/pull/1341).
* Added `NotContainInOrder` to `CollectionAssertions` and `StringCollectionAssertions` to be able to assert that the collection does not contain the specified elements in the exact same order, not necessarily consecutive - [#1339](https://github.com/fluentassertions/fluentassertions/pull/1339).
* Added async version of `Where` extension method to `ExceptionAssertions` to be able to check asynchronously thrown exception - [#1352](https://github.com/fluentassertions/fluentassertions/pull/1352).
* Added `[Not]BeUpperCased` and `[Not]BeLowerCased` to `StringAssertions` to be able to assert that a string is in upper or lower casing or not - [#1357](https://github.com/fluentassertions/fluentassertions/pull/1357).
* Added `ObjectAssertions<TSubject, TAssertions>` to ease creation of custom assertion classes - [#1371](https://github.com/fluentassertions/fluentassertions/pull/1371).
* Added `ComparingBy{Members,Value}(Type)` to allow specifying open generic types - [#1389](https://github.com/fluentassertions/fluentassertions/pull/1389).
* Added overload of `CollectionAssertions.NotBeEquivalentTo` that takes a `config` parameter` - [#1408](https://github.com/fluentassertions/fluentassertions/pull/1408).
* Changed `StringAssertions.StartWith`, `StringAssertions.EndWith` and their `EquivalentOf` versions to allow empty strings - [#1413](https://github.com/fluentassertions/fluentassertions/pull/1413).
* The equivalency assertions will now include the type of the member and whether it involves a field or property - [#1379](https://github.com/fluentassertions/fluentassertions/pull/1379)
* Changed AttributeBasedFormatter to allow custom formatter selection based on the parent type - [#1418](https://github.com/fluentassertions/fluentassertions/pull/1418).
* Added nullable overload for `Be` and `NotBe` methods of `DateTimeAssertions` and `DateTimeOffsetAssertions` - [#1427](https://github.com/fluentassertions/fluentassertions/issues/1427).
* Added overload of `Enumerating` extension method to be able to force the enumeration of an object member -[#1433](https://github.com/fluentassertions/fluentassertions/pull/1433)
* Add overloads of `MatchRegex` and `NotMatchRegex` that take `System.Text.RegularExpressions.Regex` -[#1436](https://github.com/fluentassertions/fluentassertions/pull/1436)
* Added support for equivalency tests on System.Data types (`DataSet`, `DataTable`, `DataColumn`, `DataRow`, `DataRelation`, `Constraint`) - [#1419](https://github.com/fluentassertions/fluentassertions/pull/1419).
* Added `WithParameterName` extension to ease asserting on the parameter name for a thrown `ArgumentException` - [#1466](https://github.com/fluentassertions/fluentassertions/pull/1466).
* Added `BeExactly` assertions to verify a `DateTimeOffset` exactly, i.e both its date/time and its offset - [#1609](https://github.com/fluentassertions/fluentassertions/pull/1609).
* Added `NotCompleteWithinAsync` to `TaskCompletionSourceAssertions` - [#1474](https://github.com/fluentassertions/fluentassertions/pull/1474).
* Added `HaveValue(decimal)`, `HaveSameValueAs` and `HaveSameNameAs` to `EnumAssertions` - [#1479](https://github.com/fluentassertions/fluentassertions/pull/1479).
* Added `WithResult` extension method to `CompleteWithinAsync` assertions for `Task<T>` and `TaskCompletionSource<T>` - [#1478](https://github.com/fluentassertions/fluentassertions/pull/1478).
* Added `Satisfy` to be able to compare a collection with a set of predicates in any order - [#1500](https://github.com/fluentassertions/fluentassertions/pull/1500).
* Added milliseconds formatting for error messages including `TimeSpan` - [#1504](https://github.com/fluentassertions/fluentassertions/pull/1504).
* Added `AddReportable` overload to `AssertionScope` for deferring reportable value calculation only on a test failure - [#1515](https://github.com/fluentassertions/fluentassertions/pull/1515).
* Added the possibility to set the maximum depth and other formatting settings either globally or per `AssertionScope` - [#1469](https://github.com/fluentassertions/fluentassertions/pull/1469).
* Added `BeInAscendingOrder` and `BeInDescendingOrder` for collections taking a lambda expression - [#1526](https://github.com/fluentassertions/fluentassertions/pull/1526)
* Added `[Not]BeWritable`, `[Not]BeSeekable`, `[Not]BeReadable`, `[Not]BeReadOnly`, `[Not]BeWriteOnly`, `[Not]HaveLength` , `[Not]HavePosition` for Stream and `[Not]HaveBufferSize` for BufferedStream - [#1543](https://github.com/fluentassertions/fluentassertions/pull/1543)
* Added native support for `XDocument`, `XElement` and `XAttribute` properties and fields to `BeEquivalentTo` - [#1572](https://github.com/fluentassertions/fluentassertions/pull/1572)
* The equivalency failure message will include information on how tuples, anonymous types, records and other types are compared - [#1571](https://github.com/fluentassertions/fluentassertions/pull/1571)
* Improved formatting a dictionary when key or value is a complex type - [#1577](https://github.com/fluentassertions/fluentassertions/pull/1577).
* Added `NotBe(string)` for symmetry with `Be(string)` for GuidAssertions - [#1597](https://github.com/fluentassertions/fluentassertions/pull/1597).
* Added `BeOneOf` for enum assertions - [#1637](https://github.com/fluentassertions/fluentassertions/pull/1637)
* Homogenized assertion message formatting of predicate expressions - [#1619](https://github.com/fluentassertions/fluentassertions/pull/1619).

### Fixes

* Reported actual value when it contained {% raw %}`{{{{` or `}}}}`{% endraw %} - [#1234](https://github.com/fluentassertions/fluentassertions/pull/1234).
* Changed dictionary assertion `NotContainKeys` to honour the key comparer if applicable - [#1233](https://github.com/fluentassertions/fluentassertions/pull/1233).
* Ensures that date time assertions like "a is less than an hour after b" don't succeed when `a - b == -30.Minutes()` [#1313](https://github.com/fluentassertions/fluentassertions/pull/1313).
* Event raising assertions like `WithSender` and `WithArgs` will only return the events that match the constraints - [#1321](https://github.com/fluentassertions/fluentassertions/pull/1321)
* Fixed an `InvalidCastException` that `BeEquivalentTo` could throw while debugging - [#1325](https://github.com/fluentassertions/fluentassertions/pull/1325)
* Ensured that `Given` will no longer evaluate its predicate if the preceding `FailWith` raised an assertion failure - [#1325](https://github.com/fluentassertions/fluentassertions/pull/1325)
* Improved the message that `RaisePropertyChangeFor` throws when the wrong property was detected - [#1333](https://github.com/fluentassertions/fluentassertions/pull/1333)
* Guard against negative precision arguments for `BeCloseTo` and `BeApproximately` - [#1386](https://github.com/fluentassertions/fluentassertions/pull/1386)
* Guard against implicitly or explicitly trying to compare primitive types by members - [#1394](https://github.com/fluentassertions/fluentassertions/pull/1394).
* Fixed formatting of brackets in expressions passed to ContainSingle(...) - [#1406](https://github.com/fluentassertions/fluentassertions/pull/1406).
* Fixed `Contain`, `NotContain` and `OnlyContain` to avoid multiple enumerations when the condition is false - [#1421](https://github.com/fluentassertions/fluentassertions/pull/1421).
* Added variable name and other useful and consistent information to XML assertions - [#1440](https://github.com/fluentassertions/fluentassertions/pull/1440).
* Sometimes `BeEquivalentTo` reported an incorrect message when a dictionary was missing a key - [#1454](https://github.com/fluentassertions/fluentassertions/pull/1454)
* Some dictionary failures did not honor the user-provided reason - [#1456](https://github.com/fluentassertions/fluentassertions/pull/1456)
* Restrict what types `WhenTypeIs<T>` can use and how `Using<T>` handles non-nullable types, see the [Migration Guide](/upgradingtov6#using) for more details - [#1494](https://github.com/fluentassertions/fluentassertions/pull/1494).
* `HaveElement` did not ignore the xml namespace - [#1541](https://github.com/fluentassertions/fluentassertions/pull/1541)
* Better parameter checking of `TypeAssertions` - [#1550](https://github.com/fluentassertions/fluentassertions/pull/1550)
* `[Not]HaveExplicitMethod` did not mention the parameters in the failure message - [#1550](https://github.com/fluentassertions/fluentassertions/pull/1550)
* Better parameter checking of `PropertyInfoAssertions` - [#1558](https://github.com/fluentassertions/fluentassertions/pull/1558)
* Better parameter checking of `MethodBaseAssertions` and `MethodInfoAssertions` - [#1559](https://github.com/fluentassertions/fluentassertions/pull/1559)
* Better parameter checking of `AssemblyAssertions` - [#1561](https://github.com/fluentassertions/fluentassertions/pull/1561)
* Better parameter checking of `PropertyInfoSelectorAssertions` - [#1565](https://github.com/fluentassertions/fluentassertions/pull/1565)
* Better parameter checking of `MethodInfoSelectorAssertions` - [#1569](https://github.com/fluentassertions/fluentassertions/pull/1569)
* Better parameter checking of `XDocumentAssertions`, `XElementAssertions` and `XAttributeAssertions` - [#1564](https://github.com/fluentassertions/fluentassertions/pull/1564)
* In a chained assertion API call, a second call to `ForCondition` should not even evaluate its lambda when the previous assertion failed - [#1587](https://github.com/fluentassertions/fluentassertions/pull/1587)
* Added parameter checking for `Be(string)` for GuidAssertions - [#1597](https://github.com/fluentassertions/fluentassertions/pull/1597).
* Improved consistency of XML documentation on `AssertionScope`, `ContinuedAssertionScope`, and `GivenSelector<T>` methods. - [#1606](https://github.com/fluentassertions/fluentassertions/pull/1606).
* Handle `WithDefaultIdentifier` and `WithExpectation` correctly when an `AssertionScope` continues - [#1610](https://github.com/fluentassertions/fluentassertions/pull/1610).
* Improved stack trace when a property of an element of a generic collection throws an exception during `GenericEnumerableEquivalencyStep` in `GenericCollectionAssertions` - [#1615](https://github.com/fluentassertions/fluentassertions/pull/1615).
* Removed the type info from the failure message in equivalency checks - [#1621](https://github.com/fluentassertions/fluentassertions/pull/1621).
* Fixed a regression so that collections of similarly typed key-value pairs should be equivalent to a dictionary - [#1603](https://github.com/fluentassertions/fluentassertions/pull/1603).
* Fixed a regression where a nested class without suitable members inside a collection raised an exception - [#1627](https://github.com/fluentassertions/fluentassertions/pull/1627).
* Fixed support for covariant and inherited property exclusion and inclusion in `BeEquivalentTo` - [#1631](https://github.com/fluentassertions/fluentassertions/pull/1631).
* Reintroduced `Match` for enum assertions - [#1637](https://github.com/fluentassertions/fluentassertions/pull/1637)
* Improved caller name determination by supporting multiple lines, comments and semicolons - [#1435](https://github.com/fluentassertions/fluentassertions/pull/1435).

### Breaking Changes

* Dropped support for .NET Framework 4.5, .NET Standard 1.3 and 1.6 - [#1227](https://github.com/fluentassertions/fluentassertions/pull/1227).
* Dropped support for older test frameworks such as MSTest v1, NSpec v1 and v2, XUnit v1, Gallio and MBUnit - [#1227](https://github.com/fluentassertions/fluentassertions/pull/1227).
* Removed `[Not]Have{Im,Ex}plictConversionOperator` (they had typos) - [#1221](https://github.com/fluentassertions/fluentassertions/pull/1221).
  * Use the equivalent assertions without the typo "plict" instead.
* Removed `NotBeAscendingInOrder`/`NotBeDescendingInOrder` - [#1221](https://github.com/fluentassertions/fluentassertions/pull/1221).
  * Use `NotBeInAscendingOrder`/`NotBeInDescendingOrder` instead.
* Removed `HasAttribute`, `HasMatchingAttribute` and `IsDecoratedWith(Type, bool)` `Type` extensions - [#1221](https://github.com/fluentassertions/fluentassertions/pull/1221).
  * Use `IsDecoratedWith`/`IsDecoratedWithOrInherits` instead.
* Made `EquivalencyAssertionOptionsExtentions` `internal` (and fixed a typo in the type name) - [#1221](https://github.com/fluentassertions/fluentassertions/pull/1221).
* Changed `ReferenceTypeAssertions.Subject` to be `readonly` - [#1229](https://github.com/fluentassertions/fluentassertions/pull/1229).
  * Set the `Subject` through the constructor instead.
* Changed `TypeAssertions.HaveAccessModifier` return type from `AndConstraint<Type>` to `AndConstraint<TypeAssertions>` - [#1159](https://github.com/fluentassertions/fluentassertions/pull/1159).
* Changed `TypeAssertions.NotHaveAccessModifier` return type from `AndConstraint<Type>` to `AndConstraint<TypeAssertions>` - [#1159](https://github.com/fluentassertions/fluentassertions/pull/1159).
* Changed `AllBeAssignableTo<T>` and `AllBeOfType<T>` return type from `AndConstraint<TAssertions>` to `AndWhichConstraint<TAssertions, IEnumerable<T>>` - [#1265](https://github.com/fluentassertions/fluentassertions/pull/1265).
* The new extension on `TaskCompletionSource<T>` overlays the previously used assertions based on `ObjectAssertions`.
* Removed `[Not]BeCloseTo` for `DateTime[Offset]` and `TimeSpan` that took an `int precision` - [#1278](https://github.com/fluentassertions/fluentassertions/pull/1278).
  * Use the overloads that take a `TimeSpan precision` instead.
* Aligned strings to be compared using `Ordinal[Ignorecase]` - [#1283](https://github.com/fluentassertions/fluentassertions/pull/1283).
* Changed `AutoConversion` to convert using `CultureInfo.InvariantCulture` instead of `CultureInfo.CurrentCulture` - [#1283](https://github.com/fluentassertions/fluentassertions/pull/1283).
* Renamed `StartWithEquivalent` and `EndWithEquivalent` to `StartWithEquivalentOf` and `EndWithEquivalentOf` to make the API for Equivalent methods consistent - [#1292](https://github.com/fluentassertions/fluentassertions/pull/1292).
* Several event raising assertion APIs will return an `IEventRecording` instead of `IEventRecorder` - [#1321](https://github.com/fluentassertions/fluentassertions/pull/1321)
* The classes `EventMonitor` and `RecordedEvent` are now treated as `internal` code - [#1321](https://github.com/fluentassertions/fluentassertions/pull/1321)
* Renamed method `GetEventRecorder` of interface `IMonitor` to `GetRecordingFor` and will return `IEventRecording` - [#1321](https://github.com/fluentassertions/fluentassertions/pull/1321)
* Removed synchronous assertions on asynchronous operations (`Throw`, `NotThrow`, `CompleteWithin`, ...) [#1324](https://github.com/fluentassertions/fluentassertions/pull/1324)
* Do not modify `SynchronizationContext.Current` while asserting asynchronous operations. In case hitting deadlocks in your tests please check whether you mix synchronous and asynchronous operations. [#1324](https://github.com/fluentassertions/fluentassertions/pull/1324)
* Moved `[Not]HaveFlag` from `ObjectAssertions` to `EnumAssertions` - [#1375](https://github.com/fluentassertions/fluentassertions/pull/1375).
* Requesting an unsupported test framework via `Services.Configuration.TestFrameworkName` or `"FluentAssertions.TestFramework"` now throws an exception instead of using the fallback - [#1366](https://github.com/fluentassertions/fluentassertions/pull/1366).
* Guard `[Not]Match`, `[Not]MatchEquivalentOf`, `[Not]MatchRegex` and `[Not]ContainMatch` against `null` or empty patterns - [#1401](https://github.com/fluentassertions/fluentassertions/pull/1401).
* Renamed `WhichValue` to `WhoseValue` - [#1581](https://github.com/fluentassertions/fluentassertions/pull/1581)
* By default, records are now compared by their members. Can be overridden using `ComparingRecordsByValue` - [#1571](https://github.com/fluentassertions/fluentassertions/pull/1571)
* Changed `becauseArgs` of `[Not]Reference(Assembly)` from `string[]` to `object[]` - [#1459](https://github.com/fluentassertions/fluentassertions/pull/1459)
* Major overhaul on how enums are handled, see the [Migration Guide](/upgradingtov6#enums) for more details - [#1479](https://github.com/fluentassertions/fluentassertions/pull/1479).
* Removed support for non-generic collections, see the [Migration Guide](/upgradingtov6#collections) for more details - [#1529](https://github.com/fluentassertions/fluentassertions/pull/1529).
* Removed `[Not]Contain(IEnumerable<T>, params T[])` - [#1529](https://github.com/fluentassertions/fluentassertions/pull/1529).
* Removed `BeEquivalentTo(params object[])` - [#1529](https://github.com/fluentassertions/fluentassertions/pull/1529).
* Renamed `EquivalencyStepCollection` to `EquivalencyPlan` and `AssertionOptions.EquivalencyCollection` to `EquivalencyPlan` - [#1539](https://github.com/fluentassertions/fluentassertions/pull/1539).
* `BeEquivalentTo` will no longer include `internal` properties and fields, unless `IncludingInternalProperties` or `IncludingInternalFields` is used - [#1575](https://github.com/fluentassertions/fluentassertions/pull/1575).
* Renamed `protected` methods on `DelegateAssertionsBase` such as `Throw` and `NotThrow` to avoid confusion - [#1642](https://github.com/fluentassertions/fluentassertions/pull/1642)
* Moved the properties of `IMemberInfo.SelectedMemberInfo` to `IMemberInfo`. Renamed `IMemberInfo.SelectedMemberPath` to `Path` and replaced `SelectedMemberDescription` by a combination of `Path` and `Name` - [#1379](https://github.com/fluentassertions/fluentassertions/pull/1379)
* Changed `[Not]Be` on an `IComparable<T>` to use `Equals(object)` instead of `CompareTo(T)` and added `[Not]BeRankedEquallyTo` to use `CompareTo(T)` - [#1177](https://github.com/fluentassertions/fluentassertions/pull/1177).

### Breaking Changes (Extensibility)

* Removed parameterless constructors from: `CollectionAssertions`, `ReferenceTypeAssertions`, `MemberInfoAssertions`, `MethodBaseAssertions` and `MethodInfoAssertions` - [#1229](https://github.com/fluentassertions/fluentassertions/pull/1229).
  * Use the constructors taking a `subject` instead.
* Restrict generic constraints on `[Nullable]NumericAssertions<T>` to `IComparable<T>` - [#1266](https://github.com/fluentassertions/fluentassertions/pull/1266).
* Changed return type of `[Nullable]NumericAssertions.Subject` from `IComparable` to `T?` and `T`, respectively - [#1266](https://github.com/fluentassertions/fluentassertions/pull/1266).
* Removed `Succeeded` and `SourceSucceeded` from `Continuation` and `IAssertionScope` - [#1325](https://github.com/fluentassertions/fluentassertions/pull/1325)
* Made the extension methods under the `FluentAssertions.Common` namespace `internal` - [#1376](https://github.com/fluentassertions/fluentassertions/pull/1376)
* The `IEquivalencyValidationContext` has undergone significant refactorings where its properties have been moved into the `INode` hierarchy and the two `Reason` and `Tracer` classes - [#1379](https://github.com/fluentassertions/fluentassertions/pull/1379)
* The `IMemberMatchingRule`, `IMemberSelectionRule` and `IOrderingRule` have been changed to replace their dependency on `SelectedMemberInfo` to `INode` and its derivatives `IMember` - [#1379](https://github.com/fluentassertions/fluentassertions/pull/1379)
* The `SelectedMemberInfo` class has been removed, since it main user, `IMemberInfo` has been flattened - [#1379](https://github.com/fluentassertions/fluentassertions/pull/1379)
* Several methods that took an `IMemberInfo`, but could also act on other objects than a property or field now take an `IObjectInfo` - [#1379](https://github.com/fluentassertions/fluentassertions/pull/1379)
* Pascal cased `CallerIdentifier.logger` - [#1458](https://github.com/fluentassertions/fluentassertions/pull/1458).
* Pascal cased `SelfReferenceEquivalencyAssertionOptions.orderingRules` - [#1458](https://github.com/fluentassertions/fluentassertions/pull/1458).
* Moved extension methods on `ExceptionAssertions` from `AssertionExtensions` to `ExceptionAssertionsExtensions` - [#1471](https://github.com/fluentassertions/fluentassertions/pull/1471).
* Moved `Including(Expression<Func<IMemberInfo, bool>> predicate)` from `EquivalencyAssertionOptions<T>` to `SelfReferenceEquivalencyAssertionOptions<T>` - [#1495](https://github.com/fluentassertions/fluentassertions/pull/1495).
* `Formatter.ToString()` and `IValueFormatter` now work with a `FormattingOptions` object that wraps the `UseLineBreaks` option - [#1469](https://github.com/fluentassertions/fluentassertions/pull/1469).
* Split-up the subject and expectation from `IEquivalencyValidationContext` into a new type `Comparands`. This affected `IEquivalencyStep` - [#1539](https://github.com/fluentassertions/fluentassertions/pull/1539).
* Moved the responsibility of `IEquivalencyStep.CanHandle` into `Handle` and replaced `Handle`'s return value with a more clearer `EquivalencyResult` - [#1539](https://github.com/fluentassertions/fluentassertions/pull/1539)
* Simplified `MemberSelectionContext` which is used by `IMemberSelectionRule` to remove the need for extensions to understand the difference between run-time and compile-time types - [#1539](https://github.com/fluentassertions/fluentassertions/pull/1539).
* The implementations of `IEquivalencyStep` have moved to the `FluentAssertions.Equivalency.Steps` namespace - [#1588](https://github.com/fluentassertions/fluentassertions/pull/1588).

## [5.10.3](https://www.nuget.org/packages/FluentAssertions/5.10.3)

### Fixes

* Fixed XPath index calculation in XML comparison when child node names are repeated in repeated parent nodes - [#1273](https://github.com/fluentassertions/fluentassertions/pull/1273)

## [5.10.2](https://www.nuget.org/packages/FluentAssertions/5.10.2)

### Fixes

* Added missing dependency on System.Xml - [#79](https://github.com/fluentassertions/fluentassertions/issues/79)

## 5.10.1

This version was skipped.

## [5.10.0](https://www.nuget.org/packages/FluentAssertions/5.10.0)

### What's New

* Added `string.Should().NotBeEquivalentTo` - [#1134](https://github.com/fluentassertions/fluentassertions/pull/1134)
* Added `collectionOfStrings.Should().ContainMatch()` to assert that the collection contains at least one string that matches a wildcard pattern - [#1138](https://github.com/fluentassertions/fluentassertions/pull/1138)
* Added overloads of `NotBeInAscendingOrder` and `NotBeInDescendingOrder` that take a property expression - [#1140](https://github.com/fluentassertions/fluentassertions/pull/1140)
* Include the index in the XPath information reported while comparing `XDocument`s for equivalence - [#1181](https://github.com/fluentassertions/fluentassertions/pull/1181)
* Added `NotHaveSameCount` and `HaveSameCount` for dictionary assertions - [#1178](https://github.com/fluentassertions/fluentassertions/pull/1178)
* Extended `string.Should().Contain()` and `string.Should().ContainEquivalentOf()` to test the number of times a phrase exists in a string using `theString.Should().Contain("is a", MoreThan.Thrice())` - [#1145](https://github.com/fluentassertions/fluentassertions/pull/1145)
* Added support for methods returning `ValueTask` and `ValueTask<T>` - [#1158](https://github.com/fluentassertions/fluentassertions/pull/1158)
* Added support for using `SatisfyRespectively` on all collections, including collections of `string` - [#1201](https://github.com/fluentassertions/fluentassertions/pull/1201)

### Fixes

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

## [5.9.0](https://www.nuget.org/packages/FluentAssertions/5.9.0)

### What's New

* Added `Match` method to (nullable) numeric assertions - [#1112](https://github.com/fluentassertions/fluentassertions/pull/1112)

### Fixes

* Using a custom `IAssertionStrategy` with an `AssertionScope` did not always capture all assertion failures - [#1118](https://github.com/fluentassertions/fluentassertions/issues/1118)
* Ensured `null` arguments are handled with clearer exception messages - [#1117](https://github.com/fluentassertions/fluentassertions/pull/1117)

Special thanks to contributors [@liklainy](https://github.com/Liklainy) and [Amaury Levé](https://github.com/Evangelink).

## [5.8.0](https://www.nuget.org/packages/FluentAssertions/5.8.0)

### What's New

* Added thread-safety to tests using `AssertionScope` while running many `async` tests concurrently - [#1091](https://github.com/fluentassertions/fluentassertions/pull/1091)
* Allow users to specify a custom `IAssertionStrategy` for the assertion scope, for instance, to create screenshots when a test fails - [#1094](https://github.com/fluentassertions/fluentassertions/pull/1094) & [#906](https://github.com/fluentassertions/fluentassertions/issues/906).
* Supports .NET Core 3.0 Preview 7 - [#1107](https://github.com/fluentassertions/fluentassertions/pull/1107)

### Fixes

* `Excluding` with `BeEquivalentTo` did not always work well with overridden properties - [#1087](https://github.com/fluentassertions/fluentassertions/pull/1087) & [#1077](https://github.com/fluentassertions/fluentassertions/issues/1077)
* Failure message formatting threw a `FormatException` when a nested message contains braces - [#1092](https://github.com/fluentassertions/fluentassertions/pull/1092)
* Fixed confusing `(Not)BeAssignableTo` failure messages - [#1104](https://github.com/fluentassertions/fluentassertions/pull/1104) & [#1103](https://github.com/fluentassertions/fluentassertions/issues/1103)
* Fixed a potential leakage of failure messages between multiple assertions - [#1105](https://github.com/fluentassertions/fluentassertions/pull/1105)

Kudos to [conklinb](https://github.com/conklinb) and [Amaury Levé](https://github.com/Evangelink) for notable contributions and my partner-in-crime [Jonas Nyrup](https://github.com/jnyrup) for some of the fixes, a lot of (internal) quality improvements and his critical eye.

## [5.7.0](https://www.nuget.org/packages/FluentAssertions/5.7.0)

### What's New

* Added official support for .NET Core 3 (Preview 5 or later) - [#1057](https://github.com/fluentassertions/fluentassertions/pull/1057)
* Introduced `SatisfyRespectively` on collections - [#1043](https://github.com/fluentassertions/fluentassertions/pull/1043)
* Added an alternative fluent syntax for evaluating/invoking actions - [#1017](https://github.com/fluentassertions/fluentassertions/pull/1017)
* Added overloads of `Invoking` and `Awaiting` for different sets of generic parameters - [#1051](https://github.com/fluentassertions/fluentassertions/pull/1051)
* `NotThrowAfter` is now also available for .NET Standard 1.3 and 1.6 - [#1050](https://github.com/fluentassertions/fluentassertions/pull/1050)
* Added `CompleteWithin` to assert asynchronous operations complete within a time span - [#1013](https://github.com/fluentassertions/fluentassertions/pull/1013)/[#1048](https://github.com/fluentassertions/fluentassertions/pull/1048)
* Added `NotBeEquivalent` for objects - [#1071](https://github.com/fluentassertions/fluentassertions/pull/1071)
* Added `WithMessage()` for `async` exception assertions - [#1052](https://github.com/fluentassertions/fluentassertions/pull/1052)
* Added extension methods like `100.Nanosecond()` and `20.Microsecond()` to represent time spans - [#1069](https://github.com/fluentassertions/fluentassertions/pull/1069)

### Fixes

* `AllBeAssignableTo` and `AllBeOfType` did not work for list of types - [#1007](https://github.com/fluentassertions/fluentassertions/pull/1007)
* Backslashes in subject or expected result were not correctly shown in the message - [#986](https://github.com/fluentassertions/fluentassertions/pull/986)
* `BeOfType` does not attach to the `AssertionScope` correctly - [#1002](https://github.com/fluentassertions/fluentassertions/pull/1002)
* Event monitoring did not detect events on interfaces - [#821](https://github.com/fluentassertions/fluentassertions/pull/821)
* Fix continuation on `NotThrow(After)` in chained `AssertionScope` invocations - [#1031](https://github.com/fluentassertions/fluentassertions/pull/1031)
* Allow nesting equivalency checks in custom assertion rules when using `BeEquivalentTo` - [#1033](https://github.com/fluentassertions/fluentassertions/pull/1033)
* Removed redundant use of the thread pool in async assertions - [#1020](https://github.com/fluentassertions/fluentassertions/pull/1020)
* Improved formatting of multidimensional arrays - [#1044](https://github.com/fluentassertions/fluentassertions/pull/1044)
* Better handling of exceptions wrapped in `AggregateException`s - [#1041](https://github.com/fluentassertions/fluentassertions/pull/1041)
* Fix `BadImageFormatException` under the .NET Core 3.0 Preview - [#1057](https://github.com/fluentassertions/fluentassertions/pull/1057)
* `ThrowExactly` and `ThrowExactlyAsync` now support expecting an `AggregateException` - [#1046](https://github.com/fluentassertions/fluentassertions/pull/1057)

Kudos to [Lukas Grützmacher](https://github.com/lg2de), [Matthias Lischka](https://github.com/matthiaslischka), [Christoffer Lette](https://github.com/Lette), [Ed Ball](https://github.com/ejball), [David Omid](https://github.com/davidomid), [mu88](https://github.com/mu88), [Dmitriy Maksimov](https://github.com/DmitriyMaksimov) and [Ivan Shimko](https://github.com/vanashimko) for the contributions and [Jonas Nyrup](https://github.com/jnyrup) to make this release possible again.

## [5.6.0](https://www.nuget.org/packages/FluentAssertions/5.6.0)

### Fixes

* Provide opt-out to `AssertionOptions(o => o.WithStrictOrdering())` -[#974](https://github.com/fluentassertions/fluentassertions/pull/974)
* Add collection assertion `ContainEquivalentOf` - [#950](https://github.com/fluentassertions/fluentassertions/pull/950)
* Add `Should().NotThrowAfter` assertion for actions - [#942](https://github.com/fluentassertions/fluentassertions/pull/942)

Kudos to @BrunoJuchli, @matthiaslischka and @frederik-h for these amazing additions.

## [5.5.3](https://www.nuget.org/packages/FluentAssertions/5.5.3)

### Fixes

* Performance fixes in `BeEquivalenTo` - [#935](https://github.com/fluentassertions/fluentassertions/pull/935)
* Reverted 5.5.0 changes to `AssertionScope` to ensure binary compatibility - [#977](https://github.com/fluentassertions/fluentassertions/pull/977)

## [5.5.2](https://www.nuget.org/packages/FluentAssertions/5.5.2)

### Fixes

* Allows `BeEquivalentTo` to handle a non-generic collection as the SUT - [#975](https://github.com/fluentassertions/fluentassertions/pull/975), [#973](https://github.com/fluentassertions/fluentassertions/issues/973)
* Optimized performance of `IncludeMemberByPathSelectionRule` - [#969](https://github.com/fluentassertions/fluentassertions/pull/969)

## [5.5.1](https://www.nuget.org/packages/FluentAssertions/5.5.1)

### What's New

* Now provides a hint when strings differ in length and contain differences - [#915](https://github.com/fluentassertions/fluentassertions/pull/915), [#907](https://github.com/fluentassertions/fluentassertions/issues/907)
* Added `ThrowAsync`, `ThrowExactlyAsync` and `NotThrowAsync` - [#931](https://github.com/fluentassertions/fluentassertions/pull/931)
* Added support for `Should().Throw` and `Should().NotThrow` for `Func<T>` - [#951](https://github.com/fluentassertions/fluentassertions/pull/951)
* Added support for `private protected` access modifier - [#932](https://github.com/fluentassertions/fluentassertions/pull/932)
* Updated `BeApproximately` to support nullable types for both the subject and the expectation nullable - [#934](https://github.com/fluentassertions/fluentassertions/pull/934)
* Added `async` version of `ExecutionTime` to - [#938](https://github.com/fluentassertions/fluentassertions/pull/938)
* Updated `NotBeApproximately` to accepting nullable subject and expectation - [#939](https://github.com/fluentassertions/fluentassertions/pull/939)
* `type.Should().Be(type)` now support open generics - [#954](https://github.com/fluentassertions/fluentassertions/issues/954), [#955](https://github.com/fluentassertions/fluentassertions/pull/955)

### Fixes

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
