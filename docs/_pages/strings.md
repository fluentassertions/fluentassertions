---
title: Strings
permalink: /strings/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

For asserting whether a string is null, empty or contains whitespace only, you have a wide range of methods to your disposal.

```csharp
string theString = "";
theString.Should().NotBeNull();
theString.Should().BeNull();
theString.Should().BeEmpty();
theString.Should().NotBeEmpty("because the string is not empty");
theString.Should().HaveLength(0);
theString.Should().BeNullOrWhiteSpace(); // either null, empty or whitespace only
theString.Should().NotBeNullOrWhiteSpace();
```

Obviously you’ll find all the methods you would expect for string assertions.

```csharp
theString = "This is a String";
theString.Should().Be("This is a String");
theString.Should().NotBe("This is another String");
theString.Should().BeEquivalentTo("THIS IS A STRING");
theString.Should().NotBeEquivalentTo("THIS IS ANOTHER STRING");

theString.Should().BeOneOf(
    "That is a String",
    "This is a String",
);

theString.Should().Contain("is a");
theString.Should().Contain("is a", Exactly.Once());
theString.Should().Contain("is a", AtLeast.Twice());
theString.Should().Contain("is a", MoreThan.Thrice());
theString.Should().Contain("is a", AtMost.Times(5));
theString.Should().Contain("is a", LessThan.Twice());
theString.Should().ContainAll("should", "contain", "all", "of", "these");
theString.Should().ContainAny("any", "of", "these", "will", "do");
theString.Should().NotContain("is a");
theString.Should().NotContainAll("can", "contain", "some", "but", "not", "all");
theString.Should().NotContainAny("can't", "contain", "any", "of", "these");
theString.Should().ContainEquivalentOf("WE DONT CARE ABOUT THE CASING");
theString.Should().ContainEquivalentOf("WE DONT CARE ABOUT THE CASING", Exactly.Once());
theString.Should().ContainEquivalentOf("WE DONT CARE ABOUT THE CASING", AtLeast.Twice());
theString.Should().ContainEquivalentOf("WE DONT CARE ABOUT THE CASING", MoreThan.Thrice());
theString.Should().ContainEquivalentOf("WE DONT CARE ABOUT THE CASING", AtMost.Times(5));
theString.Should().ContainEquivalentOf("WE DONT CARE ABOUT THE CASING", LessThan.Twice());
theString.Should().NotContainEquivalentOf("HeRe ThE CaSiNg Is IgNoReD As WeLl");

theString.Should().StartWith("This");
theString.Should().NotStartWith("This");
theString.Should().StartWithEquivalentOf("this");
theString.Should().NotStartWithEquivalentOf("this");

theString.Should().EndWith("a String");
theString.Should().NotEndWith("a String");
theString.Should().EndWithEquivalentOf("a string");
theString.Should().NotEndWithEquivalentOf("a string");
```

We even support wildcards.
For instance, if you would like to assert that some email address is correct, use this:

```csharp
emailAddress.Should().Match("*@*.com");
homeAddress.Should().NotMatch("*@*.com");
```

If the casing of the input string is irrelevant, use this:

```csharp
emailAddress.Should().MatchEquivalentOf("*@*.COM");
emailAddress.Should().NotMatchEquivalentOf("*@*.COM");
```

And if wildcards aren't enough for you, you can always use some regular expression magic:

```csharp
someString.Should().MatchRegex("h.*\\sworld.$");
subject.Should().NotMatchRegex(".*earth.*");
```
