---
title: Dictionaries
permalink: /dictionaries/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

You can apply Fluent Assertions to your generic dictionaries as well.
Of course you can assert any dictionary to be null or not null, and empty or not empty.
Like this:

```csharp
Dictionary<int, string> dictionary;
dictionary.Should().BeNull();

dictionary = new Dictionary<int, string>();
dictionary.Should().NotBeNull();
dictionary.Should().BeEmpty();
dictionary.Add(1, "first element");
dictionary.Should().NotBeEmpty();
```

You can also assert the equality of the entire dictionary, where the equality of the keys and values will be validated using their Equals implementation.
Like this:

```csharp
var dictionary1 = new Dictionary<int, string>
{
    { 1, "One" },
    { 2, "Two" }
};

var dictionary2 = new Dictionary<int, string>
{
    { 1, "One" },
    { 2, "Two" }
};

var dictionary3 = new Dictionary<int, string>
{
    { 3, "Three" },
};

dictionary1.Should().Equal(dictionary2);
dictionary1.Should().NotEqual(dictionary3);
```

Or you can assert that the dictionary contains a certain key or value:

```csharp
dictionary.Should().ContainKey(1);
dictionary.Should().ContainKeys(1, 2);
dictionary.Should().NotContainKey(9);
dictionary.Should().NotContainKeys(9, 10);
dictionary.Should().ContainValue("One");
dictionary.Should().ContainValues("One", "Two");
dictionary.Should().NotContainValue("Nine");
dictionary.Should().NotContainValues("Nine", "Ten");
```

You can also assert that the dictionary has a certain number of items:

```csharp
dictionary.Should().HaveCount(2);
dictionary.Should().NotHaveCount(3);
```

And finally you can assert that the dictionary contains a specific key/value pair or not:

```csharp
KeyValuePair<int, string> item1 = new KeyValuePair<int, string>(1, "One");
KeyValuePair<int, string> item2 = new KeyValuePair<int, string>(2, "Two");

dictionary.Should().Contain(item1);
dictionary.Should().Contain(item1, item2);
dictionary.Should().Contain(2, "Two");
dictionary.Should().NotContain(item1);
dictionary.Should().NotContain(item1, item2);
dictionary.Should().NotContain(9, "Nine");
```

Chaining additional assertion is supported as well.

```csharp
dictionary.Should().ContainValue(myClass)
  .Which.SomeProperty.Should().BeGreaterThan(0);
```
