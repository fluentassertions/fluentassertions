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