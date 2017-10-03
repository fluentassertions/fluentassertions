---
title: Tips
layout: page
---

General tips:
* If your assertion ends with `Should().BeTrue()`, there is most likely a better way to write it.
* By having `Should()` as early as possible in the assertion, we are able to include more information in the failure messages.

## Improved assertions
{:.no_toc}

The examples below show how you might improve your existing assertions to both get more readable assertions and much more informative failure messages.

If you see something missing, please consider submitting a pull request.

* TOC
{:toc}


{% include assertion-comparison.html header1="Assertion" header2="Improvement" caption="Collections"               examples=site.data.tips.collections %}
{% include assertion-comparison.html header1="Assertion" header2="Improvement" caption="Comparable and Numerics"   examples=site.data.tips.comparable %}
{% include assertion-comparison.html header1="Assertion" header2="Improvement" caption="Dictionaries"              examples=site.data.tips.dictionaries %}
{% include assertion-comparison.html header1="Assertion" header2="Improvement" caption="Exceptions"                examples=site.data.tips.exceptions %}
{% include assertion-comparison.html header1="Assertion" header2="Improvement" caption="Nullables"                 examples=site.data.tips.nullables %}
{% include assertion-comparison.html header1="Assertion" header2="Improvement" caption="Strings"                   examples=site.data.tips.strings %}
{% include assertion-comparison.html header1="Assertion" header2="Improvement" caption="Types"                     examples=site.data.tips.types %}