---
title: Tips
permalink: /tips/
layout: single
toc: true
sidebar:
  nav: "sidebar"

---

## General tips

* If your assertion ends with `Should().BeTrue()`, there is most likely a better way to write it.
* By having `Should()` as early as possible in the assertion, we are able to include more information in the failure messages.

## Improved assertions

The examples below show how you might improve your existing assertions to both get more readable assertions and much more informative failure messages.

If you see something missing, please consider submitting a pull request.


{% include assertion-comparison.html header1="Assertion" header2="Improvement" caption="Collections"               examples=site.data.tips.collections %}
{% include assertion-comparison.html header1="Assertion" header2="Improvement" caption="Comparable and Numerics"   examples=site.data.tips.comparable %}
{% include assertion-comparison.html header1="Assertion" header2="Improvement" caption="DateTimes"                 examples=site.data.tips.datetimes %}
{% include assertion-comparison.html header1="Assertion" header2="Improvement" caption="Dictionaries"              examples=site.data.tips.dictionaries %}
{% include assertion-comparison.html header1="Assertion" header2="Improvement" caption="Exceptions"                examples=site.data.tips.exceptions %}
{% include assertion-comparison.html header1="Assertion" header2="Improvement" caption="Nullables"                 examples=site.data.tips.nullables %}
{% include assertion-comparison.html header1="Assertion" header2="Improvement" caption="Strings"                   examples=site.data.tips.strings %}
{% include assertion-comparison.html header1="Assertion" header2="Improvement" caption="Types"                     examples=site.data.tips.types %}

## MSTest Migration

The examples below show how you might write equivalent MSTest assertions using Fluent Assertions including the failure message from each case.
We think this is both a useful migration guide and a convincing argument for switching.

If you see something missing, please consider submitting a pull request.

{% include assertion-comparison.html header1="MSTest" header2="Fluent Assertions" idPrefix="mstest-" caption="Assert"            examples=site.data.mstest-migration.assert %}
{% include assertion-comparison.html header1="MSTest" header2="Fluent Assertions" idPrefix="mstest-" caption="CollectionAssert"  examples=site.data.mstest-migration.collectionAssert %}
{% include assertion-comparison.html header1="MSTest" header2="Fluent Assertions" idPrefix="mstest-" caption="StringAssert"      examples=site.data.mstest-migration.stringAssert %}
{% include assertion-comparison.html header1="MSTest" header2="Fluent Assertions" idPrefix="mstest-" caption="Exceptions"        examples=site.data.mstest-migration.exceptions %}

## Using global AssertionOptions

The `AssertionOptions` class allows you to globally configure how `Should().BeEquivalentTo()` works, see also [Object graph comparison](objectgraphs.md). Setting up the global configuration multiple times can lead to multi-threading issues when tests are run in parallel.

In order to ensure the global AssertionOptions are configured exactly once, a test framework specific solution is required.

### xUnit.net

Create a custom [xUnit.net test framework](https://xunit.net/docs/running-tests-in-parallel#runners-and-test-frameworks) where you configure equivalency assertions. This class can be shared between multiple test projects using assembly references.

```csharp
namespace MyNamespace
{
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public class MyFramework: XunitTestFramework
    {
        public MyFramework(IMessageSink messageSink)
            : base(messageSink)
        {
            AssertionOptions.AssertEquivalencyUsing(
                options => { <configure here> });
        }
    }
}
```

Add the assembly level attribute so that xUnit.net picks up your custom test framework. This is required for *every* test assembly that should use your custom test framework.

```csharp
[assembly: Xunit.TestFramework("MyNamespace.MyFramework", "MyAssembly.Facts")]
```

Note:
* The `nameof` operator cannot be used to reference the `MyFramework` class. If your global configuration doesn't work, ensure there is no typo in the assembly level attribute declaration and that the assembly containing the `MyFramework` class is referenced by the test assembly and gets copied to the output folder.
* Because you have to add the assembly level attribute per assembly you can define different `AssertionOptions` per test assembly if required.
